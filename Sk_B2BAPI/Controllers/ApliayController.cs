using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.AlipayPayAPI;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Construction;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.TianTaPay;
using Sk_B2BAPI.Tool;
using Sk_B2BAPI.WxPayAPI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;

namespace Sk_B2BAPI.Controllers
{
    public class ApliayController : Controller
    {  
        #region 支付宝网站支付
        
        /// <summary>
        /// 支付宝初始化配置文件信息
        /// </summary>
        /// <returns></returns>
        private IAopClient GetAlipayClient()
        {
            //支付宝网关地址 
            string serviceUrl = AlipayConfigHelper.serviceUrl;
            //应用ID,以2088开头由16位纯数字组成的字符串
            string appId = AlipayConfigHelper.appId;
            //商户私钥
            string privateKey = AlipayConfigHelper.privateKey;
            //支付宝的公钥
            string alipayPublicKey = AlipayConfigHelper.alipayPublicKey;

            string format = AlipayConfigHelper.format;
            string version = AlipayConfigHelper.version;
            string signType = AlipayConfigHelper.signType;
            string charset = AlipayConfigHelper.charset;
            bool keyFromFile = false;

            IAopClient client = new DefaultAopClient(serviceUrl, appId, privateKey, format, version, signType, alipayPublicKey, charset, keyFromFile);

            return client;
        }

        /// <summary>
        /// 支付宝电脑网站支付
        /// </summary>
        /// <param name="orderNo">在线支付订单</param>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WebPayApliay(string orderNo, string entId)
        {
            ///通过真实订单号获取订单信息
            DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
            List<Orders> order = odal.GetOrderInfo(orderNo/*, entId*/);
            if (order.Count != 1)
            {
                return Json(new { success = false, message = "错误:获取单据信息错误" });
            }
            else if (order[0].PaymentName != "线上支付")
            {
                return Json(new { success = false, message = "错误:该订单支付方式不是线上支付" });
            }
            else if (order[0].PaymentName == "线上支付" && order[0].PaymentStatus == 2)
            {
                return Json(new { success = false, message = "错误:该订单已支付" });
            }
            else
            {
                IAopClient client = GetAlipayClient();
                AlipayTradePagePayModel payModel = new AlipayTradePagePayModel();
                //商户随机支付单号
                string generate = GenerateOrderNo(orderNo);
                payModel.OutTradeNo = generate;
                
                payModel.ProductCode = "FAST_INSTANT_TRADE_PAY";
                payModel.TotalAmount = order[0].Real_Amount.ToString("0.00");
                payModel.Subject = "B2B商城订单商品";
                AlipayTradePagePayRequest payRequest = new AlipayTradePagePayRequest();
                string webUrl = BaseConfiguration.SercerIp.ToString();
                payRequest.SetNotifyUrl(webUrl + "/Apliay/NotifyResultWebPayApliay");
                payRequest.SetBizModel(payModel);
                AlipayTradePagePayResponse payResponse = client.pageExecute(payRequest);
                if (payResponse!=null)
                {
                    switch (payResponse.Code)
                    {
                        case ResultCode.SUCCESS:
                            int druge;
                            int i = 0;
                            string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            do
                            {
                                i++;
                                //订单在线支付发起时间,临时单号记录
                                druge = odal.OrderPayTimeUpdate(orderNo, generate,entId, nowTime, order[0].Real_Amount,"支付宝", payResponse.Body, "官方","PC");
                                if (druge <= 0)
                                {
                                    LogQueue.Write(LogType.Error, "Apliay/ScanPayApliay", $"第{i}次订单在线支付发起时间记录失败");
                                }
                            } while (druge <= 0 && i < 4);
                            break;
                        default:
                            break;
                    }
                    return Content(payResponse.Body, "text/html");
                }
                else
                {
                    return Json(new { success = false, message = "系统异常，请重新发起支付请求" });
                }
            } 
        }

        /// <summary>
        /// 支付宝网站异步回调
        /// </summary>
        /// <returns></returns>
        public ActionResult NotifyResultWebPayApliay()
        {
            // 获取支付宝Post过来反馈信息
            IDictionary<string, string> map = GetRequestPost();
            // 异步返回结果的验签
            bool flag = AlipaySignature.RSACheckV1(map, AlipayConfigHelper.alipayPublicKey, AlipayConfigHelper.charset, AlipayConfigHelper.signType, false);
            if (flag)
            {
                //商户订单号
                string outTradeNo = map["out_trade_no"];
                //支付宝交易号
                string tradeNo = map["trade_no"];
                //订单金额
                string totalAmount = map["total_amount"];
                //开发者的app_id
                string appId = map["app_id"];
                //公用回传参数
                //string entId = map["passback_params"];
                ///获取订单信息
                DAL.OrderInfoDal Odal = new DAL.OrderInfoDal();
                //通过支付临时订单号获取订单信息
                List<Orders> order = Odal.GetOrderByNumber(outTradeNo/*, HttpUtility.UrlDecode(entId)*/);
                //验证订单是否为商户订单
                if (order.Count>0)
                {
                    //验证支付金额与订单金额是否一致
                    if (order[0].Real_Amount == Convert.ToDecimal(totalAmount))
                    {
                        //验证应用ID是否一致
                        if (appId == AlipayConfigHelper.appId)
                        {
                            //状态TRADE_SUCCESS的通知触发条件是买家付款成功
                            if (map["trade_status"] == "TRADE_FINISHED" || map["trade_status"]== "TRADE_SUCCESS")
                            {
                                //判断订单支付状态 1未支付2已支付
                                if (order[0].PaymentStatus==1)
                                {
                                    //获取商户支付真实单号
                                    string orderNo = order[0].Order_No;
                                    //通过商户真实单号【orderNo】、支付临时订单号【outTradeNo】更新
                                    if (Odal.OrderPayUpdate(orderNo, outTradeNo, tradeNo, totalAmount) > 0)
                                    {
                                        return Content("success");
                                    }
                                    else
                                    {
                                        LogQueue.Write(LogType.Error, "支付宝网站支付回调", $"商城充值订单{outTradeNo}充值成功，状态回写失败！");
                                        return Content("支付信息回写失败!");
                                    }
                                }
                                else
                                {
                                    return Content("success");
                                }
                            }
                            else
                            {
                                LogQueue.Write(LogType.Error, "支付宝网站支付异步回调", "付款交易失败");
                                return Content("failure");
                            }
                        }
                        else
                        {
                            LogQueue.Write(LogType.Error, "支付宝网站支付异步回调", "应用ID不一致");
                            return Content("failure");
                        }
                    }
                    else
                    {
                        LogQueue.Write(LogType.Error, "支付宝网站支付异步回调", "支付金额与订单金额不一致");
                        return Content("failure");
                    }
                }
                else
                {
                    LogQueue.Write(LogType.Error, "支付宝网站支付异步回调", "该订单不是商户系统中创建的订单");
                    return Content("failure");
                }
            }
            else
            {
                return Content("failure");
            }
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组 
        /// request回来的信息组成的数组
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetRequestPost()
        {
            IDictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            coll = Request.Form;
            String[] requestItem = coll.AllKeys;
            string str = "";
            int i;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Params[requestItem[i]]);
                str += requestItem[i].ToString() + "=" + Request.Params[requestItem[i]].ToString() + "&";
            }
            string wwww = "";
            for (int j = 0; j < Request.Form.Count; j++)
            {
                if (Request.Form.Keys[j].ToString().Substring(0, 1) != "_")
                {
                    wwww += " " + Request.Form.Keys[j].ToString() + " = " + Request.Form[j].ToString();
                }
            }
            return sArray;
        }
        #endregion

        #region 支付宝扫码支付
        /// <summary>
        /// 支付宝扫码支付
        /// </summary>
        /// <param name="orderNo">在线支付订单单号</param>
        /// <param name="entId">机构</param>
        /// <param name="payment">支付方式</param>
        /// <returns></returns>
        public ActionResult ScanPayApliay(string orderNo, string entId)
        {
            string rQCode = "";
            bool flag;
            ///通过真实订单号获取订单信息
            DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
            List<Orders> order = odal.GetOrderInfo(orderNo/*, entId*/);
            if (order.Count != 1)
            {
                return Json(new { success = false, message = "获取单据信息错误" });
            }
            else if (order[0].PaymentName != "线上支付")
            {
                return Json(new { success = false, message = "该订单支付方式不是线上支付" });
            }
            else if (order[0].PaymentName == "线上支付" && order[0].PaymentStatus == 2)
            {
                return Json(new { success = false, message = "该订单已支付" });
            }
            else
            {
                //商户随机支付单号
                string generate;
                string datetime = order[0].Initiationtime == "" ? DateTime.Now.AddDays(-10).ToString() : order[0].Initiationtime;
                string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime initiationtime = Convert.ToDateTime(datetime);
                DateTime nowtime = DateTime.Now;
                TimeSpan a, b;
                a = new TimeSpan(initiationtime.Ticks);
                b = new TimeSpan(nowtime.Ticks);
                int ticks = (int)b.Subtract(a).TotalMilliseconds;
                //指定订单、指定支付方式 最近交易记录
                List<PayRecord> record = odal.InTwoHoursRecords(orderNo, "支付宝");
                string urlRecord = "", typeRecord = "", generateRecord = "", timeRecord = "", source = "";
                if (record.Count > 0)
                {
                    urlRecord = record[0].Url;
                    generateRecord = record[0].Generate;
                    typeRecord = record[0].PayType;
                    timeRecord = record[0].AddTime;
                    source = record[0].Source;
                }

                string result;
                if (ticks > 7200000 || urlRecord == "" || "支付宝" != typeRecord || timeRecord == "" || source != "官方")
                {
                    IAopClient client = GetAlipayClient();
                    AlipayTradePrecreateModel payModel = new AlipayTradePrecreateModel();
                    generate = GenerateOrderNo(orderNo);
                    payModel.OutTradeNo = generate;
                    payModel.ProductCode = "FACE_TO_FACE_PAYMENT";
                    payModel.TotalAmount = Convert.ToDecimal(order[0].Real_Amount.ToString("0.00")).ToString();
                    payModel.Subject = "B2B商城订单";
                    AlipayTradePrecreateRequest payRequest = new AlipayTradePrecreateRequest();
                    string webUrl = BaseConfiguration.SercerIp.ToString();
                    payRequest.SetNotifyUrl(webUrl + "/Apliay/NotifyResultScanPayApliay");
                    payRequest.SetBizModel(payModel);
                    AlipayTradePrecreateResponse payResponse = client.Execute(payRequest);
                    if (payResponse != null)
                    {
                        switch (payResponse.Code)
                        {
                            case ResultCode.SUCCESS:
                                flag = true;
                                result = "商户订单支付发起成功";
                                rQCode = Convert.ToBase64String(CreateRQCode(payResponse.QrCode).ToArray());
                                int druge;
                                int i = 0;

                                do
                                {
                                    i++;
                                    //订单在线支付发起时间,临时单号(流水号)记录
                                    druge = odal.OrderPayTimeUpdate(orderNo, generate, entId, nowTime, order[0].Real_Amount, "支付宝", payResponse.QrCode, "官方","PC");
                                    if (druge <= 0)
                                    {
                                        LogQueue.Write(LogType.Error, "Apliay/ScanPayApliay", $"第{i}次订单在线支付发起时间记录失败");
                                    }
                                } while (druge <= 0 && i < 4);

                                break;
                            case ResultCode.ERROR: flag = false; result = "系统异常，请更新外部订单后重新发起请求"; break;
                            case ResultCode.FAIL: flag = false; result = "业务处理失败"; break;
                            case ResultCode.LLLEGALITY: flag = false; result = "非法参数"; break;
                            case ResultCode.PARAMETER: flag = false; result = "缺少必选参数"; break;
                            case ResultCode.AUTHORITY: flag = false; result = "权限不足"; break;
                            default: flag = false; result = payResponse.Body; break;
                        }
                        return Json(new { success = flag, message = result, rQCode, orderNo = generate, fee = order[0].Real_Amount, initiationTime = nowTime });
                    }
                    else
                    {
                        if (payResponse == null)
                        {
                            return Json(new { success = false, message = "配置或网络异常，请检查后重试" });
                        }
                        else
                        {
                            return Json(new { success = false, message = "系统异常，请更新外部订单后重新发起请求" });
                        }
                    }

                }
                else
                {
                    flag = true;
                    result = "商户在线支付发起成功";
                    //上次支付流水号
                    generate = generateRecord;
                    //payUrl = urlRecord;
                    rQCode = Convert.ToBase64String(CreateRQCode(urlRecord).ToArray());
                    int druge;
                    int i = 0;
                    do
                    {
                        druge = odal.PayInitiationTimeCorrect(orderNo,generate, timeRecord,"PC");
                        if (druge <= 0)
                        {
                            LogQueue.Write(LogType.Error, "Apliay/ScanPayApliay", $"第{i}次订单汇总支付发起时间修正失败");
                        }
                    } while (druge <= 0 && i < 4);
                    return Json(new { success = flag, message = result, rQCode, orderNo = generate, fee = order[0].Real_Amount, initiationTime = nowTime });
                }
            }
        }

        /// <summary>
        /// 支付宝扫码支付异步回调
        /// </summary>
        /// <returns></returns>
        public ActionResult NotifyResultScanPayApliay()
        {
            // 获取支付宝Post过来反馈信息
            IDictionary<string, string> map = GetRequestPost();
            // 异步返回结果的验签
            bool flag = AlipaySignature.RSACheckV1(map, AlipayConfigHelper.alipayPublicKey, AlipayConfigHelper.charset, AlipayConfigHelper.signType, false);
            LogQueue.Write(LogType.Debug, "验签", flag.ToString());
            if (flag)
            {
                //商户支付临时订单号(流水号)
                string outTradeNo = map["out_trade_no"];
                //支付宝交易号
                string tradeNo = map["trade_no"];
                //订单金额
                string totalAmount = map["total_amount"];
                //开发者的app_id
                string appId = map["app_id"];
                
                ///获取订单信息
                DAL.OrderInfoDal Odal = new DAL.OrderInfoDal();
                //通过支付临时订单号获取订单信息
                List<Orders> order = Odal.GetOrderByNumber(outTradeNo/*, entId*/);
                LogQueue.Write(LogType.Debug, "信息", $"count:{order.Count},realAmount:{order[0].Real_Amount},appid:{AlipayConfigHelper.appId},map:{map["trade_status"]},status:{order[0].PaymentStatus},outTradeNo:{outTradeNo},tradeNo:{tradeNo},totalAmount:{totalAmount},appId:{appId}");
                //验证订单是否为商户订单
                if (order.Count > 0)
                {
                    //验证支付金额与订单金额是否一致
                    if (order[0].Real_Amount == Convert.ToDecimal(totalAmount))
                    {
                        //验证应用ID是否一致
                        if (appId == AlipayConfigHelper.appId)
                        {
                            //状态TRADE_SUCCESS的通知触发条件是买家付款成功
                            if (map["trade_status"] == "TRADE_FINISHED" || map["trade_status"] == "TRADE_SUCCESS")
                            {
                                //判断订单支付状态 1未支付2已支付
                                if (order[0].PaymentStatus == 1)
                                {
                                    //获取商户支付真实单号
                                    string orderNo = order[0].Order_No;
                                    //通过商户真实单号、支付临时订单号更新
                                    if (Odal.OrderPayUpdate(orderNo, outTradeNo, tradeNo, totalAmount) <= 0)
                                    {
                                        return Content("success");
                                    }
                                    else
                                    {
                                        LogQueue.Write(LogType.Error, "支付宝网站支付回调", $"商城充值订单{outTradeNo}充值成功，状态回写失败！");
                                        return Content("支付信息回写失败!");
                                    }
                                }
                                else
                                {
                                    return Content("success");
                                }
                            }
                            else
                            {
                                LogQueue.Write(LogType.Error, "支付宝网站支付异步回调", "付款交易失败");
                                return Content("failure");
                            }
                        }
                        else
                        {
                            LogQueue.Write(LogType.Error, "支付宝网站支付异步回调", "应用ID不一致");
                            return Content("failure");
                        }
                    }
                    else
                    {
                        LogQueue.Write(LogType.Error, "支付宝网站支付异步回调", "支付金额与订单金额不一致");
                        return Content("failure");
                    }
                }
                else
                {
                    LogQueue.Write(LogType.Error, "支付宝网站支付异步回调", "该订单不是商户系统中创建的订单");
                    return Content("failure");
                }
            }
            else
            {
                return Content("failure");
            }
        }

        /// <summary>
        /// 生成二维码并以流的形式输出
        /// </summary>
        /// <param name="QrCode"></param>
        /// <returns></returns>
        private MemoryStream CreateRQCode(string QrCode)
        {
            //显示出 preResponse.QrCode 对应的条码
            Bitmap bt;
            string enCodeString = QrCode;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder
            {
                QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
                QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H,
                QRCodeScale = 3,
                QRCodeVersion = 8
            };
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            ///*二维码图片*/
            //string filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString()
            // + ".jpg";
            //bt.Save(Server.MapPath("~/images/") + filename);

            //将二维码保存到内存流，并输出
            using (MemoryStream stream = new MemoryStream())
            {
                bt.Save(stream, ImageFormat.Jpeg);
                stream.Flush();
                return stream;
            }
        }

        /// <summary>
        /// 在线支付订单状态
        /// </summary>
        /// <param name="orderNo">在线支付订单临时单号(流水号)</param>
        /// <returns></returns>
        public ActionResult StatusPayApliay(string outTradeNo)
        {
            IAopClient client = GetAlipayClient();
            AlipayTradeQueryModel model = new AlipayTradeQueryModel
            {
                OutTradeNo = outTradeNo
            };
            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            request.SetBizModel(model);
            AlipayTradeQueryResponse response = client.Execute(request);
            string message;
            int tautscode=99;
            if (response.Code == "10000")
            {
                bool succcessstatus = false;
                switch (response.TradeStatus)
                {
                    case "WAIT_BUYER_PAY": tautscode = 1; message = "交易创建，等待买家付款"; break;
                    case "TRADE_CLOSED": tautscode = 0; message = "未付款交易超时关闭，或支付完成后全额退款"; succcessstatus = true; break;
                    case "TRADE_SUCCESS": tautscode = 2; message = "交易支付成功"; succcessstatus = true; break;
                    case "TRADE_FINISHED": tautscode = 2; message = "交易结束，不可退款"; break;
                    default: tautscode = 99; message = "未知状态"; break;
                }
               
                return Json(new { success = succcessstatus, code = tautscode, status = response.Code , msg = message, subMsg = response.SubMsg });
            }
            else if (response.Code == "40004")
            {
                switch (response.SubCode)
                {
                    case "ACQ.SYSTEM_ERROR": tautscode = 0; message = "系统错误,请联系商户确认"; break;
                    case "ACQ.INVALID_PARAMETER": tautscode = 0; message = "参数无效,请联系商户确认"; break;
                    case "ACQ.TRADE_NOT_EXIST": tautscode = 1; message = "交易不存在或已撤销,若要继续请重新发起支付"; break;
                    default: tautscode = 99; message = "未知异常"; break;
                }
                return Json(new { success = false, code = tautscode, status = response.Code, msg = message, subMsg = response.SubMsg });
            }
            else
            {
                switch (response.Code)
                {
                    case "20000": message = "服务不可用"; break;
                    case "20001": message = "授权权限不足"; break;
                    case "40001": message = "缺少必选参数"; break;
                    case "40002": message = "非法的参数"; break;
                    case "40004": message = "业务处理失败"; break;
                    case "40006": message = "权限不足"; break;
                    default: message = "未知异常"; break;
                }
                LogQueue.Write(LogType.Error, "", $"网关返回码:{response.Code},网关返回码描述:{response.Msg},业务返回码:{response.SubCode},业务返回码描述:{response.SubMsg}");
                return Json(new { success = false, code =99, status = response.Code, msg = message, subMsg = response.SubMsg });
            }
        }
        #endregion

        #region 支付宝App在线支付
        /// <summary>
        /// 支付宝App在线支付
        /// </summary>
        /// <param name="orderNo">在线支付订单单号</param>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        public ActionResult AppPayAlipay(string orderNo, string entId)
        {
            ///通过商户真实订单号获取订单信息
            DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
            List<Orders> order = odal.GetOrderInfo(orderNo/*, entId*/);

            if (order.Count != 1)
            {
                return Json(new { success = false, message = "获取单据信息错误" });
            }
            else if (order[0].PaymentName != "线上支付")
            {
                return Json(new { success = false, message = "该订单支付方式不是线上支付" });
            }
            else if (order[0].PaymentName == "线上支付" && order[0].PaymentStatus == 2)
            {
                return Json(new { success = false, message = "该订单已支付" });
            }
            else
            {
                try
                {
                    //商户随机支付单号
                    string generate;
                    string datetime = order[0].Initiationtime == "" ? DateTime.Now.AddDays(-10).ToString() : order[0].Initiationtime;
                    string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime initiationtime = Convert.ToDateTime(datetime);
                    DateTime nt = DateTime.Now;
                    TimeSpan a, b;
                    a = new TimeSpan(initiationtime.Ticks);
                    b = new TimeSpan(nt.Ticks);
                    int ticks = (int)b.Subtract(a).TotalMilliseconds;
                    //指定订单、指定支付方式 最近交易记录
                    List<PayRecord> record = odal.InTwoHoursRecords(orderNo, "支付宝");
                    string urlRecord = "", typeRecord = "", generateRecord = "", timeRecord = "", source = "";
                    if (record.Count > 0)
                    {
                        urlRecord = record[0].Url;
                        generateRecord = record[0].Generate;
                        typeRecord = record[0].PayType;
                        timeRecord = record[0].AddTime;
                        source = record[0].Source;
                    }
                    
                    if (ticks > 7200000 || urlRecord == "" || "支付宝" != typeRecord || timeRecord == "" || source != "官方")
                    {
                        IAopClient client = GetAlipayClient();
                        AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
                        //商户随机支付单号
                        generate = GenerateOrderNo(orderNo);
                        model.OutTradeNo = generate;
                        model.Body = "商城订单";
                        model.TotalAmount = order[0].Real_Amount.ToString("0.00");
                        model.Subject = "商城订单";
                        model.TimeoutExpress = "2h";
                        model.ProductCode = "QUICK_MSECURITY_PAY";
                        string webUrl = BaseConfiguration.SercerIp.ToString();
                        AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
                        request.SetBizModel(model);
                        request.SetNotifyUrl(webUrl + "/Apliay/NotifyResultScanPayApliay");
                        AlipayTradeAppPayResponse response = client.SdkExecute(request);
                        if (!response.IsError)
                        {
                            int druge;
                            int i = 0;
                            do
                            {
                                i++;
                                //订单在线支付发起时间,临时单号记录
                                druge = odal.OrderPayTimeUpdate(orderNo, generate, entId, nowTime, order[0].Real_Amount, "支付宝", response.Body, "官方", "APP");
                                if (druge <= 0)
                                {
                                    LogQueue.Write(LogType.Error, "Apliay/ScanPayApliay", $"第{i}次订单在线支付发起时间记录失败");
                                }
                            } while (druge <= 0 && i < 4);
                            return Json(new { success = true, message = "支付申请已发起", data = response.Body, orderNo = generate, fee = order[0].Real_Amount, initiationTime = nowTime });
                        }
                        else
                        {
                            return Json(new { success = false, message = response.SubMsg, data = response.Body, orderNo = generate, fee = order[0].Real_Amount, initiationTime = "" });
                        }
                    }
                    else
                    {
                        //上次支付流水号
                        generate = generateRecord;
                        int druge;
                        int i = 0;
                        do
                        {
                            druge = odal.PayInitiationTimeCorrect(orderNo, generate, timeRecord, "PC");
                            if (druge <= 0)
                            {
                                LogQueue.Write(LogType.Error, "Apliay/ScanPayApliay", $"第{i}次订单汇总支付发起时间修正失败");
                            }
                        } while (druge <= 0 && i < 4);
                        return Json(new { success = true, message = "支付申请已发起", data = urlRecord, orderNo = generate, fee = order[0].Real_Amount, initiationTime = nowTime });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"{ex.Message}" });
                }
            }
        }

        /// <summary>
        /// 支付宝App同步支付结果校验
        /// </summary>
        /// <param name="resultStatus">前台返回支付状态</param>
        /// <returns></returns>
        public ActionResult ReturnResultAppPayAlipay(string resultStatus)
        {
            try
            {
                string message = "";
                switch (resultStatus)
                {
                    case "9000": message = "订单支付成功"; break;
                    case "8000": message = "正在处理中，支付结果未知（有可能已经支付成功），请查询商户订单列表中订单的支付状态"; break;
                    case "4000": message = "订单支付失败"; break;
                    case "5000": message = "重复请求"; break;
                    case "6001": message = "用户中途取消"; break;
                    case "6002": message = "网络连接出错"; break;
                    case "6004": message = "支付结果未知（有可能已经支付成功），请查询商户订单列表中订单的支付状态"; break;
                    case "其它": message = "其它支付错误"; break;
                    default: message = "未知错误"; break;
                }
                return Json(new { success = true, message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 官方线上退款
        /// <summary>
        /// 官方线上退款
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="entId">企业</param>
        /// <param name="refundAmount">退款金额</param>
        /// <returns></returns>
        public JsonResult OfficialRefund(string orderNo, string entId, decimal money)
        {
            //通过真实订单号获取订单信息
            DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
            List<Orders> orders = odal.GetOrderInfo(orderNo/*, entId*/);
            if (orders.Count != 1)
            {
                return Json(new { success = false, message = "获取单据信息错误" });
            }
            else if (orders[0].PaymentName != "线上支付")
            {
                return Json(new { success = false, message = "该订单支付方式不是线上支付,无法线上退款" });
            }
            else if (orders[0].PaymentName == "线上支付" && orders[0].PaymentStatus != 2)
            {
                return Json(new { success = false, message = "该订单未线上支付,无法线上退款" });
            }
            else
            {
                // 根据订单号获取订单对应支付记录（退款失败）
                string generate = "";
                DAL.OrderInfoDal dal = new DAL.OrderInfoDal();
                List<PayRecord> payRecords = dal.GetPayRecords(orderNo, entId, "在线退款", 0);
                if (payRecords.Count > 0)
                {
                    generate = payRecords[0].Generate;
                    money = payRecords[0].Fee;
                }
                string payType = orders[0].PayType;//支付渠道/退款渠道
                string message, refId;
                bool judge;
                switch (payType)
                {
                    case "支付宝":
                        message = RefundApliay(orderNo, entId, generate, money,dal, orders, out judge,out refId);
                        break;
                    case "微信":
                        message = WeixinRefundApliay(orderNo, entId, generate, money, dal, orders, out judge, out refId);
                        //judge = false;
                        break;
                    case "银联":
                        NetPayController netPayCScanB = new NetPayController();
                        message = netPayCScanB.PayRefund(orderNo, entId, generate, money, dal, orders, out judge, out refId);
                        break;
                    default:
                        message = "支付方式异常";
                        judge = false;
                        break;
                }
                return Json(new { success = judge, message });
            }
        }
        #endregion

        #region 支付宝线上退款
        /// <summary>
        /// 支付宝线上退款
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="entId">企业</param>
        /// <param name="refundAmount">退款金额</param>
        /// <returns></returns>
        public string RefundApliay(string orderNo,string entId,string generate, decimal fee, DAL.OrderInfoDal dal, List<Orders> orders,out bool judge, out string refId)
        {
            string msg,nowTime = "";
            string orderGenerate = orders[0].Generate.ToString();//在线支付流水号
            string orderThirdparty = orders[0].Thirdparty.ToString();//支付交易号
            var orderFee = orders[0].PaymentFee;//在线支付金额
            var refundFee = orders[0].RefundFee;//在线已退款金额
            string payName = orders[0].PaymentName;//订单支付方式
            string payType = orders[0].PayType;//支付渠道/退款渠道
            string payWay = payType;
            if (string.IsNullOrEmpty(orderGenerate))
            {
                judge = false;
                refId = "";
                msg = "在线支付流水号为空，请确认订单";
            }
            else if (string.IsNullOrEmpty(orderThirdparty))
            {
                judge = false;
                refId = "";
                msg = "支付交易号为空，请确认订单";
            }
            //判断退款金额是否超出实际金额（退款金额 < 支付金额 - 退款成功金额）
            if (fee > (orderFee - refundFee))
            {
                judge = false;
                refId = "";
                msg = "当前退款金额数大于允许退款金额数，请确认退款金额";
            }
            //是否存在退款失败记录？存在，获取原退款单号，更新失败记录：不存在，重新生成退款单号，新增退款记录
            int num;
            if (!string.IsNullOrEmpty(generate))
            {
                num = dal.OrderRefundUpdate(orderNo, generate, "", "", "Retry", payWay);
            }
            else
            {
                generate = GenerateOrderNo(orderNo);
                nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //记录退款信息
                num = dal.OrderRefundRecord(orderNo, generate, entId, nowTime, fee, payWay, "官方","PC");
            }

            //组织退款数据，发起退款
            /*缺逻辑处理*/
            IAopClient client = GetAlipayClient();
            // 商户订单号，和交易号不能同时为空
            string out_trade_no = orderGenerate;

            // 支付宝交易号，和商户订单号不能同时为空
            string trade_no = orderThirdparty;

            // 请求退款接口时，传入的退款号，如果在退款时未传入该值，则该值为创建交易时的商户订单号，必填。
            string out_request_no = generate;
            AlipayTradeRefundModel model = new AlipayTradeRefundModel();
            model.OutTradeNo = out_trade_no;
            model.TradeNo = trade_no;
            model.OutRequestNo = out_request_no;
            model.RefundAmount = fee.ToString();
            AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
            request.SetBizModel(model);
            AlipayTradeRefundResponse response = null;
            try
            {
                response = client.Execute(request);
                //判断退款是否成功？更新数据（退款状态、金额），返回详细信息
                refId = response.TradeNo;
                if (response.Code == "10000" && response.Msg== "Success")
                {
                    //订单在线退款成功记录
                    int number = dal.OrderRefundUpdate(orderNo, generate, response.TradeNo, response.RefundFee, "Success", payWay);
                    if (number > 0)
                    {
                        judge = true;
                        msg = "在线退款成功，状态已回写";
                    }
                    else
                    {
                        judge = true;
                        msg = "在线退款成功，状态未回写";
                    }
                }
                else {

                    //订单在线退款成功记录
                    int number = dal.OrderRefundUpdate(orderNo, generate, "", "", "Fail", payWay);
                    if (number > 0)
                    {
                        judge = false;
                        msg = $"Code:{response.Code},在线退款响应失败，状态已回写";
                    }
                    else
                    {
                        judge = false;
                        msg = $"Code:{response.Code},在线退款响应失败，状态未回写";
                    }
                    
                }
               
            }
            catch (Exception exp)
            {
                judge = false;
                refId = "";
                msg = exp.Message.ToString();
            }
            
            return msg;
        }
        #endregion

        #region 微信扫码支付
        /// <summary>
        /// 微信扫码支付
        /// </summary>
        /// <param name="orderNo">在线支付订单单号</param>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        public ActionResult ScanPayWeChat(string orderNo, string entId)
        {
            string rQCode = "", nowTime = "", generate = "";
            bool flag;
            decimal realAmount = 0;
            string message;
            try
            {
                ///通过真实商户订单获取订单信息
                DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
                List<Orders> order = odal.GetOrderInfo(orderNo/*, entId*/);
                if (order.Count != 1)
                {
                    return Json(new { success = false, message = "获取单据信息错误" });
                }
                else if (order[0].PaymentName != "线上支付")
                {
                    return Json(new { success = false, message = "该订单支付方式不是线上支付" });
                }
                else if (order[0].PaymentName == "线上支付" && order[0].PaymentStatus == 2)
                {
                    return Json(new { success = false, message = "该订单已支付" });
                }
                else
                {
                    realAmount = Convert.ToDecimal(order[0].Real_Amount.ToString("0.00"));
                    int fee = int.Parse((realAmount * 100).ToString("0"));

                    string datetime = order[0].Initiationtime == "" ? DateTime.Now.AddDays(-10).ToString() : order[0].Initiationtime;
                    nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime initiationtime = Convert.ToDateTime(datetime);
                    DateTime nowtime = DateTime.Now;
                    TimeSpan a, b;
                    a = new TimeSpan(initiationtime.Ticks);
                    b = new TimeSpan(nowtime.Ticks);
                    int ticks = (int)b.Subtract(a).TotalMilliseconds;

                    //指定订单、指定支付方式 最近交易记录
                    List<PayRecord> record = odal.InTwoHoursRecords(orderNo, "微信");
                    string urlRecord = "", typeRecord = "", generateRecord = "", timeRecord = "", source = "";
                    if (record.Count > 0)
                    {
                        urlRecord = record[0].Url;
                        generateRecord = record[0].Generate;
                        typeRecord = record[0].PayType;
                        timeRecord = record[0].AddTime;
                        source = record[0].Source;
                    }

                    string payUrl;
                    //超过两小时 二维码为空 支付方式不同
                    if (ticks > 7200000 || urlRecord == "" || "微信" != typeRecord || timeRecord == "" || source != "官方")
                    {
                        //商户随机支付单号
                        generate = GenerateOrderNo(orderNo);
                        payUrl = GetPayUrlWeChat(generate, generate, fee, out bool judge);
                        if (judge)
                        {
                            flag = true;
                            message = "商户在线支付发起成功";
                            rQCode = Convert.ToBase64String(CreateRQCode(payUrl).ToArray());
                            int druge;
                            int i = 0;
                            do
                            {
                                i++;
                                //订单在线支付发起时间,临时单号记录
                                druge = odal.OrderPayTimeUpdate(orderNo, generate, entId, nowTime, order[0].Real_Amount, "微信", payUrl, "官方","PC");
                                if (druge <= 0)
                                {
                                    LogQueue.Write(LogType.Error, "Apliay/ScanPayApliay", $"第{i}次订单在线支付发起时间记录失败");
                                }
                            } while (druge <= 0 && i < 4);
                        }
                        else
                        {
                            flag = false;
                            message = payUrl;
                            rQCode = "";
                        }
                    }
                    else
                    {
                        flag = true;
                        message = "商户在线支付发起成功";
                        //上次支付流水号
                        generate = generateRecord;
                        rQCode = Convert.ToBase64String(CreateRQCode(urlRecord).ToArray());
                        int druge;
                        int i = 0;
                        do
                        {
                            druge = odal.PayInitiationTimeCorrect(orderNo,generate, timeRecord,"PC");
                            if (druge <= 0)
                            {
                                LogQueue.Write(LogType.Error, "Apliay/ScanPayApliay", $"第{i}次订单汇总支付发起时间修正失败");
                            }
                        } while (druge <= 0 && i < 4);
                    }
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/RechargePayUrlWeChat", ex.Message.ToString());
                flag = false;
                message = ex.Message.ToString();
            }
            return Json(new { success = flag, message, rQCode, orderNo = generate,  fee = realAmount, initiationTime = nowTime });
        }

        /// <summary>
        ///  生成直接支付url，支付url有效期为2小时,模式二
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="out_trade_no">商户订单id</param>
        /// <param name="fee">商户订单金额</param>
        /// <returns>支付url</returns>
        public string GetPayUrlWeChat(string productId, string out_trade_no, int fee, out bool judge)
        {
            string webUrl = BaseConfiguration.SercerIp.ToString();
            WxPayData data = new WxPayData();
            data.SetValue("body", "商城订单");//商品描述
            data.SetValue("attach", "附加信息,用于后台或者存入数据库,做自己的判断");//附加数据
            data.SetValue("out_trade_no", out_trade_no);//随机字符串
            data.SetValue("total_fee", fee);//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(120).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", "B2B商城订单");//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", productId);//商品ID
            data.SetValue("notify_url", webUrl + "/Apliay/NotifyResultScanPayWeChat");/*异步通知url*/
            WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
            string url;

            if (result.GetValue("return_code").ToString() == "FAIL")
            {
                judge = false;
                url = result.GetValue("return_msg").ToString();
            }
            else
            {
                judge = true;
                url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接
            }
            return url;
        }

        /// <summary>
        /// 微信线上支付异步回调
        /// </summary>
        /// <returns></returns>
        public ActionResult NotifyResultScanPayWeChat()
        {
            WxPayData notifyData = GetNotifyData();
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                return Content(res.ToXml(), "text/xml");
            }
            else if (!notifyData.IsSet("out_trade_no"))
            {
                //若out_trade_no不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中商户订单号不存在");
                return Content(res.ToXml(), "text/xml");
            }
            string transactionId = notifyData.GetValue("transaction_id").ToString();//微信支付订单号
            string outTradeNo = notifyData.GetValue("out_trade_no").ToString();//商户支付临时订单号(流水号)
            decimal totalFee = decimal.Parse(notifyData.GetValue("total_fee").ToString());//订单金额
            string resultCode = notifyData.GetValue("result_code").ToString();//业务结果
            ///获取订单信息
            DAL.OrderInfoDal Odal = new DAL.OrderInfoDal();
            //通过支付临时订单号获取订单信息
            List<Orders> order = Odal.GetOrderByNumber(outTradeNo/*, entId*/);
            if (order[0].status == 2)//判断商城充值订单状态
            {
                Log.Info("信息", "已支付");
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                return Content(res.ToXml(), "text/xml");
            }
            else if (Convert.ToDecimal((order[0].Real_Amount * 100).ToString("0")) != totalFee)//判断商城充值订单金额
            {
                Log.Info("信息", "查询订单失败金额bufu");
                LogQueue.Write(LogType.Error, "微信在线支付回调", $"订单金额不符，商城在线支付订单{outTradeNo}金额：{order[0].Real_Amount},微信回调订单金额数据：{totalFee}！");
                //若订单金额不符，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单金额错误");
                return Content(res.ToXml(), "text/xml");
            }
            else if (!QueryOrder(transactionId))//查询订单，判断订单真实性
            {
                Log.Info("信息", "查询订单失败订单真实性");
                LogQueue.Write(LogType.Error, "微信在线支付回调", $"根据微信回调数据微信在线支付订单号{transactionId}查询订单失败！");
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                return Content(res.ToXml(), "text/xml");
            }
            else//查询订单成功
            {
                Log.Info("信息", "信息查询成功");
                WxPayData res = new WxPayData();
                //业务结果判断
                if (resultCode == "SUCCESS")
                {
                    //获取商户支付真实单号
                    string orderNo = order[0].Order_No;
                    //通过商户真实单号【orderNo】、支付临时订单号【outTradeNo】更新
                    if (Odal.OrderPayUpdate(orderNo, outTradeNo, transactionId, order[0].Real_Amount.ToString()) > 0)
                    {
                        LogQueue.Write(LogType.Error, "微信线上支付回调", $"商户线上支付订单：{outTradeNo}，支付成功-状态回写失败！");
                        res.SetValue("return_code", " ");
                        res.SetValue("return_msg", "客户端状态修改失败");
                    }
                    else
                    {
                        res.SetValue("return_code", "SUCCESS");
                        res.SetValue("return_msg", "OK");
                    }
                }
                else
                {
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                }
                return Content(res.ToXml(), "text/xml");
            }
        }

        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public WxPayData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = Request.InputStream;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            int count;
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                LogQueue.Write(LogType.Error, "微信支付回调，签名校验", res.ToXml().ToString());
                return res;
            }

            return data;
        }

        /// <summary>
        /// 查询订单是否存在
        /// </summary>
        /// <param name="transactionId">微信支付订单号</param>
        /// <returns>是否存在（true/false）</returns>
        private bool QueryOrder(string transactionId)
        {
            Log.Info("流水单号", transactionId);
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transactionId);/*微信支付订单号*/
            WxPayData res = WxPayApi.OrderQuery(req);
            Log.Info("信息", res.ToString());
            if (res.GetValue("return_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 在线支付订单状态
        /// </summary>
        /// <param name="outTradeNo">在线支付订单临时单号(流水号)</param>
        /// <returns></returns>
        public ActionResult StatusPayWeChat(string outTradeNo)
        {
            
            WxPayData data = new WxPayData();
            if (string.IsNullOrEmpty(outTradeNo))
            {
                return Json(new { success = false, message = "商户订单号不能全部为空" });
            }
            data.SetValue("transaction_id", "");/*微信订单号*/
            data.SetValue("out_trade_no", outTradeNo);/*商户订单号*/
            WxPayData payData = WxPayApi.OrderQuery(data);
            if (payData.GetValue("return_code").ToString() == "FAIL")
            {
                return Json(new { success = false, message = payData.GetValue("return_msg").ToString() });
            }
            else if (payData.GetValue("return_code").ToString() == "SUCCESS" && payData.GetValue("result_code").ToString() == "FAIL")
            {
                return Json(new { success = false, message = payData.GetValue("err_code_des").ToString() });
            }
            else if (payData.GetValue("return_code").ToString() == "SUCCESS" && payData.GetValue("result_code").ToString() == "SUCCESS")
            {
                var tradeState = payData.GetValue("trade_state").ToString();/*交易状态*/
                string message;
                int code;
                bool succcessstatus = false;
                switch (tradeState)
                {
                    case "SUCCESS": code = 2; message = "支付成功"; succcessstatus = true; break;
                    case "REFUND": code = 2; message = "转入退款"; succcessstatus = true; break;
                    case "NOTPAY": code = 0; message = "未支付"; break;
                    case "CLOSED": code = 0; message = "已关闭"; succcessstatus = true; break;
                    case "REVOKED": code = 0; message = "已撤销"; succcessstatus = true; break;
                    case "USERPAYING": code = 1; message = "用户支付中"; break;
                    case "PAYERROR": code = 1; message = "支付失败"; succcessstatus = true; break;
                    default: code = 99; message = "未知结果"; break;
                }
                return Json(new { success = succcessstatus, code, message });
            }
            else
            {
                return Json(new { success = false, message = "未知的异常" });
            }
        }
        #endregion

        #region 微信App在线支付
        /// <summary>
        /// 微信App在线支付
        /// </summary>
        /// <param name="orderNo">在线支付订单单号</param>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        public ActionResult AppPayWeChat(string orderNo, string entId)
        {
            try
            {
                decimal realAmount = 0;
                ///通过真实订单号获取订单信息
                DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
                List<Orders> order = odal.GetOrderInfo(orderNo/*, entId*/);
                if (order.Count != 1)
                {
                    return Json(new { success = false, message = "单据信息错误" });
                }
                else if (order[0].PaymentName != "线上支付")
                {
                    return Json(new { success = false, message = "订单支付方式不是线上支付" });
                }
                else if (order[0].PaymentName == "线上支付" && order[0].PaymentStatus == 2)
                {
                    return Json(new { success = false, message = "订单已支付" });
                }
                else
                {
                    //商户随机支付单号
                    string generate;
                    string datetime = order[0].Initiationtime == "" ? DateTime.Now.AddDays(-10).ToString() : order[0].Initiationtime;
                    string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime initiationtime = Convert.ToDateTime(datetime);
                    DateTime nt = DateTime.Now;
                    TimeSpan a, b;
                    a = new TimeSpan(initiationtime.Ticks);
                    b = new TimeSpan(nt.Ticks);
                    int ticks = (int)b.Subtract(a).TotalMilliseconds;
                    //指定订单、指定支付方式 最近交易记录
                    List<PayRecord> record = odal.InTwoHoursRecords(orderNo, "支付宝");
                    string urlRecord = "", typeRecord = "", generateRecord = "", timeRecord = "", source = "";
                    if (record.Count > 0)
                    {
                        urlRecord = record[0].Url;
                        generateRecord = record[0].Generate;
                        typeRecord = record[0].PayType;
                        timeRecord = record[0].AddTime;
                        source = record[0].Source;
                    }

                    if (ticks > 7200000 || urlRecord == "" || "微信" != typeRecord || timeRecord == "" || source != "官方")
                    {
                        realAmount = Convert.ToDecimal(order[0].Real_Amount.ToString("0.00"));
                        int fee = Convert.ToInt32((realAmount * 100).ToString("0"));
                        //商户随机支付单号
                        generate = GenerateOrderNo(orderNo);
                        WxPayData payData = GetPayWeChat(generate, generate, fee, out string code, out string msg);
                        if (code == "SUCCESS")
                        {
                            int druge;
                            int i = 0;
                            do
                            {
                                i++;
                                //订单在线支付发起时间,临时单号记录
                                druge = odal.OrderPayTimeUpdate(orderNo, generate, entId, nowTime, order[0].Real_Amount, "微信", payData.ToXml(), "官方", "APP");
                                if (druge <= 0)
                                {
                                    LogQueue.Write(LogType.Error, "Apliay/ScanPayApliay", $"第{i}次订单在线支付发起时间记录失败");
                                }
                            } while (druge <= 0 && i < 4);
                            return Json(new { success = true, message = "订单在线支付发起成功", data = payData.ToXml(), orderNo = generate, fee = realAmount, initiationTime = nowTime });
                        }
                        else
                        {
                            return Json(new { success = false, message = msg, data = "", orderNo = generate, fee = realAmount, initiationTime = "" });
                        }
                    }
                    else
                    {
                        //上次支付流水号
                        generate = generateRecord;
                        int druge;
                        int i = 0;
                        do
                        {
                            druge = odal.PayInitiationTimeCorrect(orderNo, generate, timeRecord, "PC");
                            if (druge <= 0)
                            {
                                LogQueue.Write(LogType.Error, "Apliay/ScanPayApliay", $"第{i}次订单汇总支付发起时间修正失败");
                            }
                        } while (druge <= 0 && i < 4);
                        return Json(new { success = true, message = "订单在线支付发起成功", data = urlRecord, orderNo = generate, fee = realAmount, initiationTime = nowTime });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString(), data = "", orderNo = orderNo, fee = 0 });
            }
        }
        /// <summary>
        ///  生成微信客户端支付信息
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="out_trade_no">商户订单id</param>
        /// <param name="fee">商户订单金额</param>
        /// <returns>支付url</returns>
        public WxPayData GetPayWeChat(string productId, string out_trade_no, int fee, out string code, out string msg)
        {
            try
            {
                string webUrl = BaseConfiguration.SercerIp.ToString();
                string nonceStr = WxPayApi.GenerateNonceStr();
                WxPayData data = new WxPayData();
                data.SetValue("body", "商城订单");//商品描述
                data.SetValue("attach", "附加信息,用于后台或者存入数据库,做自己的判断");//附加数据
                data.SetValue("out_trade_no", out_trade_no);//随机字符串
                data.SetValue("total_fee", fee);//总金额
                data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
                data.SetValue("time_expire", DateTime.Now.AddMinutes(120).ToString("yyyyMMddHHmmss"));//交易结束时间
                data.SetValue("goods_tag", "商品的备忘,可以自定义");//商品标记
                data.SetValue("trade_type", "NATIVE");//交易类型
                data.SetValue("product_id", productId);//商品ID
                data.SetValue("notify_url", webUrl + "/Apliay/NotifyResultScanPayWeChat");/*异步通知url*/
                data.SetValue("nonce_str", nonceStr);/* 随机字符串*/
                WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
                //生成带签名的客户端支付信息
                WxPayData payData = new WxPayData();
                if (result.GetValue("return_code").ToString() == "SUCCESS" && result.GetValue("result_code").ToString() == "SUCCESS")
                {
                    string prepayid = result.GetValue("prepay_id").ToString();//预支付交易会话标识

                    payData.SetValue("appid", WxPayConfig.GetConfig().GetAppID());//公众账号ID
                    payData.SetValue("partnerid", WxPayConfig.GetConfig().GetMchID());//商户号
                    payData.SetValue("prepayid", prepayid);//预支付交易会话标识
                    payData.SetValue("noncestr", nonceStr);//随机串
                    payData.SetValue("package", "Sign=WXPay");
                    payData.SetValue("timestamp", WxPayApi.GenerateTimeStamp());//时间戳
                    string sign;
                    try
                    {
                        sign = payData.MakeSign();
                        payData.SetValue("sign", sign);//签名
                        code = "SUCCESS";
                        msg = "预支付交易会话完成";
                    }
                    catch (Exception ex)
                    {
                        code = "FAIL";
                        msg = ex.Message.ToString();
                    }
                }
                else if (result.GetValue("return_code").ToString() == "FAIL")
                {
                    code = "FAIL";
                    msg = result.GetValue("return_msg").ToString();
                }
                else if (result.GetValue("result_code").ToString() == "FAIL")
                {
                    code = "FAIL";
                    msg = result.GetValue("err_code_des").ToString();
                }
                else
                {
                    code = "FAIL";
                    msg = "未知异常";
                }
                return payData;
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Debug, "Apliay/GetPayWeChat：", ex.ToString());
                code = "FAIL";
                msg = ex.ToString();
                return null;
            }
        }
        #endregion

        #region 微信线上退款
        /// <summary>
        /// 微信线上退款
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="entId">企业</param>
        /// <param name="refundAmount">退款金额</param>
        /// <returns></returns>
        public string WeixinRefundApliay(string orderNo, string entId, string generate, decimal fee, DAL.OrderInfoDal dal, List<Orders> orders, out bool judge, out string refId)
        {
            string msg, nowTime = "";
            refId = "";
            string orderGenerate = orders[0].Generate.ToString();//在线支付流水号
            string orderThirdparty = orders[0].Thirdparty.ToString();//支付交易号
            var orderFee = orders[0].PaymentFee;//在线支付金额
          
            var refundFee = orders[0].RefundFee;//在线已退款金额
            string payName = orders[0].PaymentName;//订单支付方式
            string payType = orders[0].PayType;//支付渠道/退款渠道
            string payWay = payType;
            if (string.IsNullOrEmpty(orderGenerate))
            {
                judge = false;
                refId = "";
                msg = "在线支付流水号为空，请确认订单";
            }
            else if (string.IsNullOrEmpty(orderThirdparty))
            {
                judge = false;
                refId = "";
                msg = "支付交易号为空，请确认订单";
            }
            //判断退款金额是否超出实际金额（退款金额 < 支付金额 - 退款成功金额）
            if (fee > (orderFee - refundFee))
            {
                judge = false;
                refId = "";
                msg = "当前退款金额数大于允许退款金额数，请确认退款金额";
            }
            //是否存在退款失败记录？存在，获取原退款单号，更新失败记录：不存在，重新生成退款单号，新增退款记录
            int num;
            if (!string.IsNullOrEmpty(generate))
            {
                num = dal.OrderRefundUpdate(orderNo, generate, "", "", "Retry", payWay);
            }
            else
            {
                generate = GenerateOrderNo(orderNo);
                nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //记录退款信息
                num = dal.OrderRefundRecord(orderNo, generate, entId, nowTime, fee, payWay, "官方","PC");
            }
            fee *= 100;
            orderFee *= 100;
            WxPayData data = new WxPayData();
            data.SetValue("transaction_id", orderThirdparty);
            data.SetValue("out_trade_no", orderGenerate );
            data.SetValue("total_fee", (int)orderFee);//订单总金额
            data.SetValue("refund_fee", (int)fee);//退款金额
            data.SetValue("out_refund_no", WxPayApi.GenerateOutTradeNo());//随机生成商户退款单号
            data.SetValue("op_user_id", WxPayConfig.GetConfig().GetMchID());//操作员，默认为商户号
            var result = WxPayApi.Refund(data);//提交退款申请给API，接收返回数据
            try
            {
                if (result.GetValue("return_code").ToString() == "SUCCESS" && result.GetValue("result_code").ToString()!= "FAIL")
                {
                    //订单在线退款成功记录
                    int number = dal.OrderRefundUpdate(orderNo, generate, result.GetValue("out_refund_no").ToString(), result.GetValue("refund_fee").ToString(), "Success", payWay);
                    if (number > 0)
                    {
                        judge = true;
                        msg = "在线退款成功，状态已回写";
                        return msg;
                    }
                    else
                    {
                        judge = true;
                        msg = "在线退款成功，状态未回写";
                        return msg;
                    }
                }
                else
                {

                    //订单在线退款成功记录
                    int number = dal.OrderRefundUpdate(orderNo, generate, "", "", "Fail", payWay);
                    if (number > 0)
                    {
                        judge = false;
                        msg = $"Code:{result.GetValue("Code")},在线退款响应失败，状态已回写";
                        return msg;
                        //judge = false;
                        //msg = "在线退款失败，状态已回写;退款失败原因：" + returnMsg;
                    }
                    else
                    {
                        judge = false;
                        msg = $"Code:{result.GetValue("Code")},在线退款响应失败，状态未回写";
                        return msg;
                        //judge = false;
                        //msg = "在线退款失败，状态未回写;退款失败原因：" + returnMsg;
                    }

                }

            }
            catch (Exception exp)
            {
                judge = false;
                refId = "";
                msg = exp.Message.ToString();
            }

            return msg;
        }
        #endregion

        #region App支付状态回写
        /// <summary>
        /// App支付状态回写
        /// </summary>
        /// <param name="orderNo">在线支付订单真实单号</param>
        /// <param name="generate">在线支付订单临时单号(流水号)</param>
        /// <param name="tradeNo">第三方支付订单号</param>
        /// <param name="fee">在线支付订单金额</param>
        /// <returns></returns>
        public ActionResult PaymentStatus(string orderNo,string generate,string tradeNo, decimal fee)
        {
            ///通过真实订单获取订单信息
            DAL.OrderInfoDal Odal = new DAL.OrderInfoDal();
            List<Orders> order = Odal.GetOrderInfo(orderNo/*, HttpUtility.UrlDecode(entId)*/);
            if (order.Count > 0)
            {
                if (fee == order[0].Real_Amount)
                {
                    if (order[0].PaymentStatus == 0)
                    {
                        //通过商户真实单号、支付临时订单号更新
                        if (Odal.OrderPayUpdate(orderNo, generate, tradeNo, fee.ToString()) <= 0)
                        {
                            return Json(new { success = true, message = "订单支付成功" });
                        }
                        else
                        {
                            LogQueue.Write(LogType.Error, "App支付", $"商城订单{orderNo}在线支付成功，状态回写失败！");
                            return Content("支付信息回写失败!");
                        }
                    }
                    else
                    {
                        return Json(new { success = true, message = "订单支付成功" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "支付金额与订单金额不一致" });
                }
            }
            else
            {
                return Json(new { success = false, message = "获取单据信息错误,不存在订单编号对应的订单信息" });
            }
        }
        #endregion

        #region 随机订单号、时间戳
        /// <summary>
        /// 根据当前系统时间加随机序列来生成订单号
        /// </summary>
        /// <returns>订单号</returns>
        public static string GenerateOrderNo()
        {
            var ran = new Random(Guid.NewGuid().GetHashCode());
            return string.Format("{0}{1}{2}", "", DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(100,999));
        }
        public static string GenerateOrderNo(string orderNo)
        {
            var ran = new Random(Guid.NewGuid().GetHashCode());
            return string.Format("{0}A{1}{2}", orderNo, GenerateTimeStamp(),ran.Next(10,99));
        }
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion

        #region 建行聚合支付
        [HttpPost]
        public JsonResult AggregationPayConstrunction(string orderNo, string entId, string payType)
        {
            string payWay = "聚合";
            bool flag;
            //int fee;
            ///通过商户真实订单编号获取订单信息
            DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
            List<Orders> order = odal.GetOrderInfo(orderNo/*, entId*/);
            if (order.Count != 1)
            {
                return Json(new { success = false, message = "获取单据信息错误" });
            }
            else if (order[0].PaymentName != "线上支付")
            {
                return Json(new { success = false, message = "该订单支付方式不是线上支付" });
            }
            else if (order[0].PaymentName == "线上支付" && order[0].PaymentStatus == 2)
            {
                return Json(new { success = false, message = "该订单已支付" });
            }
            else
            {
                decimal realAmount = Convert.ToDecimal(order[0].Real_Amount.ToString("0.00"));
                //fee = int.Parse((realAmount * 100).ToString("0"));
                decimal fee = realAmount;
                //商户随机支付单号
                string generate;
                string datetime = order[0].Initiationtime == "" ? DateTime.Now.AddDays(-10).ToString() : order[0].Initiationtime;
                string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime initiationtime = Convert.ToDateTime(datetime);
                DateTime nowtime = DateTime.Now;
                TimeSpan a, b;
                a = new TimeSpan(initiationtime.Ticks);
                b = new TimeSpan(nowtime.Ticks);
                int ticks = (int)b.Subtract(a).TotalMilliseconds;

                //指定订单、指定支付方式 最近交易记录
                List<PayRecord> record = odal.InTwoHoursRecords(orderNo, payWay);
                string urlRecord = "", typeRecord = "", generateRecord = "", timeRecord = "", source = "";
                if (record.Count > 0)
                {
                    urlRecord = record[0].Url;
                    generateRecord = record[0].Generate;
                    typeRecord = record[0].PayType;
                    timeRecord = record[0].AddTime;
                    source = record[0].Source;
                }

                string message;
                string payUrl;
                string rQCode;
                //超过15分钟 二维码为空 支付方式不同
                if (ticks > 300000 || urlRecord == "" || payWay != typeRecord || timeRecord == "")
                {
                    generate = GenerateOrderNo(orderNo);
                    payUrl = GetPayUrlConstrunction(orderNo, generate, fee, out bool judge);
                    if (judge)
                    {
                        flag = true;
                        message = "商户在线支付发起成功";
                        rQCode = Convert.ToBase64String(CreateRQCode(payUrl).ToArray());
                        int druge;
                        int i = 0;
                        do
                        {
                            i++;
                            //订单在线支付发起时间,临时单号记录
                            druge = odal.OrderPayTimeUpdate(orderNo, generate, entId, nowTime, realAmount, payType, payUrl, "建行","PC");
                            if (druge <= 0)
                            {
                                LogQueue.Write(LogType.Error, "Apliay/AggregationPayConstrunction", $"第{i}次订单在线支付发起时间记录失败");
                            }
                        } while (druge <= 0 && i < 4);

                    }
                    else
                    {
                        flag = false;
                        message = payUrl;
                        rQCode = "";
                    }
                }
                else
                {
                    flag = true;
                    message = "商户在线支付发起成功";
                    //上次支付流水号
                    generate = generateRecord;
                    payUrl = urlRecord;
                    rQCode = Convert.ToBase64String(CreateRQCode(payUrl).ToArray());
                    int druge;
                    int i = 0;
                    do
                    {
                        druge = odal.PayInitiationTimeCorrect(orderNo, generate,timeRecord,"PC");
                        if (druge <= 0)
                        {
                            LogQueue.Write(LogType.Error, "Apliay/AggregationPayConstrunction", $"第{i}次订单汇总支付发起时间修正失败");
                        }
                    } while (druge <= 0 && i < 4);
                }
                return Json(new { success = flag, message, rQCode, orderNo = generate, payUrl, fee = realAmount, initiationTime = nowTime });
            }
        }
        /// <summary>
        /// 返回建行支付二维码链接
        /// </summary>
        /// <param name="orderNo">商户真实单号</param>
        /// <param name="outTradeNo">支付临时单号</param>
        /// <param name="fee">支付订单金额</param>
        /// <param name="judge">支付是否发起成功</param>
        /// <returns></returns>
        public string GetPayUrlConstrunction(string orderNo, string outTradeNo, decimal fee, out bool judge)
        {
            ConstructionData data = new ConstructionData();
            data.SetValue("MERCHANTID",ConstructionConfig.Initialize().GetMerchantId());
            data.SetValue("POSID", ConstructionConfig.Initialize().GetPosId());
            data.SetValue("BRANCHID", ConstructionConfig.Initialize().GetBranchId());
            data.SetValue("ORDERID", outTradeNo);
            data.SetValue("PAYMENT", fee);
            data.SetValue("CURCODE", "01");
            data.SetValue("REMARK1", orderNo);
            data.SetValue("REMARK2", "");
            data.SetValue("TXCODE", "530550");
            data.SetValue("RETURNTYPE","3");
            data.SetValue("TIMEOUT", "");

            ConstructionData construction = ConstructionPayApi.RQCode(data);
            string url;
            if (construction.GetValue("SUCCESS").ToString()== "true")
            {
                ConstructionData constructionData = ConstructionPayApi.HttpsPost(construction.GetValue("PAYURL").ToString());
                if (constructionData.GetValue("SUCCESS").ToString()== "true")
                {
                    judge = true;
                    url = constructionData.GetValue("QRURL").ToString();
                }
                else
                {
                    judge = false;
                    url = "向建行发送https请求失败";
                }
            }
            else
            {
                judge = false;
                url = "向建行发送http请求失败";
            }
            return url;
        }
        /// <summary>
        /// 建行支付异步回调
        /// </summary>
        /// <returns></returns>
        public void NotifyReturnConstruction()
        {
            Stream stream = Request.InputStream;
            int count = 0;
            byte[] vs = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while (count>stream.Read(vs,0,1024))
            {
                builder.Append(Encoding.UTF8.GetString(vs, 0, count));
            }
            stream.Flush();
            stream.Close();
            stream.Dispose();
            LogQueue.Write(LogType.Debug, "建行支付异步回调response:", builder.ToString());
            ConstructionData construction = new ConstructionData();
            construction.Dictionary(builder.ToString());

            if (construction.IsSet("ORDERID") )
            {
                string orderId = construction.GetValue("ORDERID").ToString();
                string payment = construction.GetValue("PAYMENT").ToString();
                ///获取订单信息
                DAL.OrderInfoDal Odal = new DAL.OrderInfoDal();
                //通过支付临时订单号获取订单信息
                List<Orders> order = Odal.GetOrderByNumber(orderId/*, entId*/);
                if (order[0].PaymentStatus == 2)//判断商城支付订单状态
                {
                    LogQueue.Write(LogType.Error, "Apliay/NotifyReturnConstruction", $"订单:{orderId}已支付,状态：{order[0].PaymentStatus}");
                }
                else if (order[0].Real_Amount.ToString()!= payment)//判断订单金额
                {
                    LogQueue.Write(LogType.Error, "Apliay/NotifyReturnConstruction", $"订单金额不符;回调金额：{payment},订单金额：{order[0].Real_Amount}");
                }
                else
                {
                    //更新回写数据
                    //未完成
                }
            }
            else
            {
                LogQueue.Write(LogType.Error, "Apliay/NotifyReturnConstruction", "支付结果中订单号不存在");
            }

        }
        #endregion

        #region 甜塔扫码支付
        /// <summary>
        /// 甜塔扫码支付
        /// </summary>
        /// <param name="orderNo">商户订单</param>
        /// <param name="entId">机构</param>
        /// <param name="payType">支付渠道【微信/支付宝】</param>
        /// <returns></returns>
        public ActionResult ScanPayTianTa(string orderNo, string entId,string payType)
        {
            bool flag;
            int fee;
            string payWay = payType;
            payType = payType == "微信" ? "WECHAT" : "ALIPAY";
            ///通过商户真实订单编号获取订单信息
            DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
            List<Orders> order = odal.GetOrderInfo(orderNo/*, entId*/);
            if (order.Count != 1)
            {
                return Json(new { success = false, message = "获取单据信息错误" });
            }
            else if (order[0].PaymentName != "线上支付")
            {
                return Json(new { success = false, message = "该订单支付方式不是线上支付" });
            }
            else if (order[0].PaymentName == "线上支付" && order[0].PaymentStatus == 2)
            {
                return Json(new { success = false, message = "该订单已支付" });
            }
            else
            {
                decimal realAmount = Convert.ToDecimal(order[0].Real_Amount.ToString("0.00"));
                fee = int.Parse((realAmount * 100).ToString("0"));
                //商户随机支付单号
                string generate;
                string datetime = order[0].Initiationtime == "" ? DateTime.Now.AddDays(-10).ToString() : order[0].Initiationtime;
                string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime initiationtime = Convert.ToDateTime(datetime);
                DateTime nowtime = DateTime.Now;
                TimeSpan a, b;
                a = new TimeSpan(initiationtime.Ticks);
                b = new TimeSpan(nowtime.Ticks);
                int ticks = (int)b.Subtract(a).TotalMilliseconds;

                //指定订单、指定支付方式 最近交易记录
                List<PayRecord> record = odal.InTwoHoursRecords(orderNo,payWay);
                string urlRecord="", typeRecord="", generateRecord="",timeRecord="", source = "";
                if (record.Count>0)
                {
                    urlRecord= record[0].Url;
                    generateRecord = record[0].Generate;
                    typeRecord = record[0].PayType;
                    timeRecord = record[0].AddTime;
                    source = record[0].Source;
                }

                string payUrl;
                string message;
                string rQCode;
                //超过两小时 二维码为空 支付方式不同
                if (ticks > 7200000 || urlRecord == "" || payWay != typeRecord || timeRecord == "" || source != "甜塔")
                {
                    generate = GenerateOrderNo(orderNo);
                    payUrl = GetPayUrlTianTa(generate, fee, payType, out bool judge);
                    if (judge)
                    {
                        flag = true;
                        message = "商户在线支付发起成功";
                        rQCode = Convert.ToBase64String(CreateRQCode(payUrl).ToArray());
                        int druge;
                        int i = 0;
                        do
                        {
                            i++;
                            //订单在线支付发起时间,临时单号记录
                            druge = odal.OrderPayTimeUpdate(orderNo, generate, entId, nowTime, realAmount, payWay, payUrl, "甜塔","PC");
                            if (druge <= 0)
                            {
                                LogQueue.Write(LogType.Error, "Apliay/ScanPayApliay", $"第{i}次订单在线支付发起时间记录失败");
                            }
                        } while (druge <= 0 && i < 4);

                    }
                    else
                    {
                        flag = false;
                        message = payUrl;
                        rQCode = "";
                    }
                }
                else
                {
                    flag = true;
                    message = "商户在线支付发起成功";
                    //上次支付流水号
                    generate = generateRecord;
                    payUrl = urlRecord;
                    rQCode = Convert.ToBase64String(CreateRQCode(payUrl).ToArray());
                    int druge;
                    int i = 0;
                    do
                    {
                        druge = odal.PayInitiationTimeCorrect(orderNo,generate, timeRecord,"PC");
                        if (druge <= 0)
                        {
                            LogQueue.Write(LogType.Error, "Apliay/ScanPayApliay", $"第{i}次订单汇总支付发起时间修正失败");
                        }
                    } while (druge <= 0 && i < 4);
                }
                return Json(new { success = flag, message, rQCode, orderNo = generate, payUrl, fee = realAmount, initiationTime = nowTime });
            }
        }

        /// <summary>
        /// 返回甜塔支付二维码
        /// </summary>
        /// <param name="outTradeNo">随机支付单号</param>
        /// <param name="fee">订单金额</param>
        /// <param name="payType">支付渠道</param>
        /// <param name="judge">二维码获取成功返回true,否则返回false</param>
        /// <returns>支付二维码</returns>
        public string GetPayUrlTianTa(string outTradeNo, int fee,string payType, out bool judge)
        {
            string webUrl = BaseConfiguration.SercerIp.ToString();
            TianTaData tianTa = new TianTaData();
            //tianTa.SetValue("money", "1");//测试金额
            tianTa.SetValue("money", fee.ToString());//订单金额
            tianTa.SetValue("body", outTradeNo); //商品描述
            tianTa.SetValue("orderNo", outTradeNo);//订单号
            tianTa.SetValue("type", payType);//支付渠道
            if (payType== "WECHAT")//微信
            {
                tianTa.SetValue("notifyUrl", webUrl + "/Apliay/NotifyReturnTianTaWechat");//异步回调Url
            }
            else//支付宝
            {
                tianTa.SetValue("notifyUrl", webUrl + "/Apliay/NotifyReturnTianTaAlipay");//异步回调Url
            }
            
            TianTaData result = TianTaApi.ScanPay(tianTa);//调用甜塔扫码支付接口

            string content = result.GetValue("content").ToString();
            JObject jo = (JObject)JsonConvert.DeserializeObject(content);


            string url;
            if (jo["code"].ToString() != "1")
            {
                judge = false;
                url = jo["msg"].ToString();
            }
            else
            {
                judge = true;
                url = jo["code_url"].ToString();//获得统一下单接口返回的二维码链接
            }
            return url;
        }


        /// <summary>
        /// 甜塔微信支付异步回调
        /// </summary>
        /// <returns></returns>
        public ActionResult NotifyReturnTianTaWechat()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = Request.InputStream;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            int count;
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            TianTaData tianTa = new TianTaData();
            tianTa.FromUrl(builder.ToString());

            if (tianTa.GetValue("return_code").ToString()== "SUCCESS")
            {
                //检查支付结果中transaction_id是否存在
                if (!tianTa.IsSet("transaction_id"))
                {
                    //若transaction_id不存在，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "支付结果中微信订单号不存在");
                    return Content(res.ToXml(), "text/xml");
                }
                else if (!tianTa.IsSet("out_trade_no"))
                {
                    //若out_trade_no不存在，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "支付结果中商户订单号不存在");
                    return Content(res.ToXml(), "text/xml");
                }

                string transactionId = tianTa.GetValue("transaction_id").ToString();//微信支付订单号
                string outTradeNo = tianTa.GetValue("out_trade_no").ToString();//商户支付临时订单号(流水号)
                decimal totalFee = decimal.Parse(tianTa.GetValue("total_fee").ToString());//订单金额
                string resultCode = tianTa.GetValue("result_code").ToString();//业务结果
                ///获取订单信息
                DAL.OrderInfoDal Odal = new DAL.OrderInfoDal();
                //通过支付临时订单号获取订单信息
                List<Orders> order = Odal.GetOrderByNumber(outTradeNo/*, entId*/);

                if (order[0].PaymentStatus == 2)//判断商城支付订单状态
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                    return Content(res.ToXml(), "text/xml");
                }
                else if (Convert.ToDecimal((order[0].Real_Amount * 100).ToString("0")) != totalFee)//判断商城支付订单金额
                {
                    LogQueue.Write(LogType.Error, "微信在线支付回调", $"订单金额不符，商城在线支付订单{outTradeNo}金额：{order[0].Real_Amount},微信回调订单金额数据：{totalFee}！");
                    //若订单金额不符，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "订单金额错误");
                    return Content(res.ToXml(), "text/xml");
                }
                else if (!OrderQueryTianTa(transactionId, "微信"))//查询订单，判断订单真实性
                {
                    LogQueue.Write(LogType.Error, "微信在线支付回调", $"根据微信回调数据微信在线支付订单号{transactionId}查询订单失败！");
                    //若订单查询失败，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "订单查询失败");
                    return Content(res.ToXml(), "text/xml");
                }
                else//查询订单成功
                {
                    WxPayData res = new WxPayData();
                    //获取商户支付真实单号
                    string orderNo = order[0].Order_No;
                    //业务结果判断
                    if (resultCode == "SUCCESS")
                    {
                        //通过商户真实单号、支付临时订单号更新
                        if (Odal.OrderPayUpdate(orderNo, outTradeNo, transactionId, order[0].Real_Amount.ToString()) <= 0)
                        {
                            //LogQueue.Write(LogType.Debug, "商户支付状态回写：", $"失败");
                            LogQueue.Write(LogType.Error, "微信线上支付回调", $"商户线上支付订单：{orderNo}，支付成功-状态回写失败！");
                            res.SetValue("return_code", " ");
                            res.SetValue("return_msg", "客户端状态修改失败");
                        }
                        else
                        {
                            //LogQueue.Write(LogType.Debug, "商户支付状态回写：", $"成功");
                            res.SetValue("return_code", "SUCCESS");
                            res.SetValue("return_msg", "OK");
                        }
                    }
                    else
                    {
                        if (Odal.OrderPayFailUpdate(orderNo,outTradeNo)<=0)
                        {
                            LogQueue.Write(LogType.Error, "微信线上支付回调", $"商户线上支付订单：{orderNo}，支付失败-状态回写失败！");
                            res.SetValue("return_code", " ");
                            res.SetValue("return_msg", "客户端状态修改失败");
                        }
                        else
                        {
                            res.SetValue("return_code", "SUCCESS");
                            res.SetValue("return_msg", "OK");
                        }
                    }
                    return Content(res.ToXml(), "text/xml");
                }
            }
            else
            {
                //通信异常
                WxPayData res = new WxPayData();
                res.SetValue("return_code", " ");
                res.SetValue("return_msg", "通信异常");
                return Content(res.ToXml(), "text/xml");
            }
            
        }

        /// <summary>
        /// 甜塔支付宝支付异步回调
        /// </summary>
        /// <returns></returns>
        public ActionResult NotifyReturnTianTaAlipay()
        {
            // 获取支付宝Post过来反馈信息
            IDictionary<string, string> map = GetRequestPost();
            //商户订单号
            string outTradeNo = map["out_trade_no"];
            //支付宝交易号
            string tradeNo = map["trade_no"];
            //订单金额
            string totalAmount = map["total_amount"];
            ///获取订单信息
            DAL.OrderInfoDal Odal = new DAL.OrderInfoDal();
            //通过支付临时订单号获取订单信息
            List<Orders> order = Odal.GetOrderByNumber(outTradeNo/*, HttpUtility.UrlDecode(entId)*/);
            //验证订单是否为商户订单
            if (order.Count > 0)
            {
                //验证支付金额与订单金额是否一致
                if (order[0].Real_Amount == Convert.ToDecimal(totalAmount))
                {
                    ////验证应用ID是否一致
                    //if (appId == AlipayConfigHelper.appId)
                    //{
                    //获取商户支付真实单号
                    string orderNo = order[0].Order_No;
                    //状态TRADE_SUCCESS的通知触发条件是买家付款成功
                    if (map["trade_status"] == "TRADE_FINISHED" || map["trade_status"] == "TRADE_SUCCESS")
                    {
                        //判断订单支付状态 1未支付2已支付
                        if (order[0].PaymentStatus == 1)
                        {
                            //通过商户真实单号【orderNo】、支付临时订单号【outTradeNo】更新
                            if (Odal.OrderPayUpdate(orderNo, outTradeNo, tradeNo, totalAmount) > 0)
                            {
                                return Content("success");
                            }
                            else
                            {
                                LogQueue.Write(LogType.Error, "支付宝网站支付回调", $"商城充值订单{outTradeNo}支付成功，状态回写失败！");
                                return Content("支付信息回写失败!");
                            }
                        }
                        else
                        {
                            return Content("success");
                        }
                    }
                    else
                    {
                        if (Odal.OrderPayFailUpdate(orderNo,outTradeNo)>0)
                        {
                            return Content("success");
                        }
                        else
                        {
                            LogQueue.Write(LogType.Error, "支付宝网站支付回调", $"商城充值订单{outTradeNo}支付失败，状态回写失败！");
                            return Content("failure");
                        }
                    }
                    //}
                    //else
                    //{
                    //    LogQueue.Write(LogType.Debug, "支付宝网站支付异步回调", "应用ID不一致");
                    //    return Content("failure");
                    //}
                }
                else
                {
                    LogQueue.Write(LogType.Error, "支付宝网站支付异步回调", "支付金额与订单金额不一致");
                    return Content("failure");
                }
            }
            else
            {
                LogQueue.Write(LogType.Error, "支付宝网站支付异步回调", "该订单不是商户系统中创建的订单");
                return Content("failure");
            }
        }
        #endregion

        #region 甜塔支付判断订单是否存在
        /// <summary>
        /// 甜塔支付判断订单是否存在
        /// </summary>
        /// <param name="tradeNo">官方交易流水号</param>
        /// <param name="payType">交易方式</param>
        /// <returns>true【存在】/false【不存在】</returns>
        private bool OrderQueryTianTa(string tradeNo, string payType)
        {
            switch (payType)
            {
                case "微信": payType = "WECHAT"; break;
                default: payType = "ALIPAY"; break;
            }
            TianTaData tianTa = new TianTaData();
            tianTa.SetValue("pay_no", "");
            tianTa.SetValue("trade_no", tradeNo);
            tianTa.SetValue("type", payType);
            TianTaData result = TianTaApi.Query(tianTa);//甜塔查询支付状态接口
            //LogQueue.Write(LogType.Debug, "OrderQueryTianTa", result.ToUrl().Replace("&","\r\n"));
            var content = result.GetValue("content").ToString();
            JObject jo = (JObject)JsonConvert.DeserializeObject(content);
            if (jo["code"].ToString()=="1" && jo["respType"].ToString()== "Success")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 甜塔支付订单状态查询
        /// <summary>
        /// 甜塔支付订单状态查询
        /// </summary>
        /// <param name="orderNo">商户临时订单号</param>
        /// <param name="tradeNo">第三方订单号</param>
        /// <param name="payType">支付渠道</param>
        /// <returns></returns>
        public ActionResult QueryTianTa(string orderNo,string tradeNo,string payType)
        {
            if (string.IsNullOrEmpty(orderNo)&&string.IsNullOrEmpty(tradeNo))
            {
                return Json(new { success = true, code = "Failed", message = "商户订单号与第三方订单号不能同时为空" });
            }
            else
            {
                string message;
                string code;
                try
                {
                    switch (payType)
                    {
                        case "微信": payType = "WECHAT"; break;
                        default: payType = "ALIPAY"; break;
                    }
                    TianTaData tianTa = new TianTaData();
                    if (orderNo != "")
                    {
                        tianTa.SetValue("pay_no", orderNo);
                    }
                    if (tradeNo != "")
                    {
                        tianTa.SetValue("trade_no", tradeNo);
                    }
                    tianTa.SetValue("type", payType);
                    TianTaData result = TianTaApi.Query(tianTa);//甜塔查询支付状态接口
                    var content = result.GetValue("content").ToString();
                    JObject jo = (JObject)JsonConvert.DeserializeObject(content);

                    if (jo.ContainsKey("status_msg"))
                    {
                        code = jo["status_code"].ToString();
                        message = jo["status_msg"].ToString();
                    }
                    else
                    {
                        code = "";
                        message = jo["msg"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    code = "FAIL";
                    message = ex.Message.ToString();
                }
                return Json(new { success = true, code, message });
            }
        }
        #endregion

        #region 甜塔支付退款
        /// <summary>
        /// 甜塔支付退款
        /// </summary>
        /// <param name="orderNo">商户退款订单号</param>
        /// <param name="entId">机构</param>
        /// <param name="money">退款金额</param>
        /// <param name="generate">订单退款临时单号【重新发起退款】</param>
        /// <param name="druge">是否重新发起退款</param>
        /// <returns></returns>
        public ActionResult RefundTianTa(string orderNo, string entId,decimal money,/*string generate="",*/bool druge=false)
        {
            if (string.IsNullOrEmpty(orderNo)||string.IsNullOrEmpty(entId))
            {
                return Json(new { success=false,message="订单编号或者机构不能为空"});
            }
            else
            {
                string generate = "";
                if (druge /*&& string.IsNullOrEmpty(generate)*/)
                {
                    //查询退款订单最近有没有退款失败记录
                    DAL.OrderInfoDal dal = new DAL.OrderInfoDal();
                    List<PayRecord> payRecords = dal.GetPayRecords(orderNo, entId, "在线退款", 0);
                    if (payRecords.Count>0)
                    {
                        generate = payRecords[0].Generate;
                        money = payRecords[0].Fee;
                    }
                    else
                    {
                        return Json(new { success = false, message = "无支付失败记录" });
                    }
                }
                string msg = Refund(orderNo, generate, entId, money, out bool judge,out string refId);
                return Json(new { success= judge, message= msg, refId });
            }
        }

        /// <summary>
        /// 甜塔支付退款
        /// </summary>
        /// <param name="orderNo">商户订单号</param>
        /// <param name="generate">商户退款临时订单号:临时单号为空时表示新退款单，临时单号不为空表示退款失败，重新发起退款</param>
        /// <param name="entId">机构</param>
        /// <param name="fee">在线退款金额</param>
        /// <returns></returns>
        public string Refund(string orderNo,string generate,string entId, decimal fee,out bool judge,out string refId)
        {
            string msg;
            try
            {
                string nowTime = "";
                //通过商户真实订单编号获取订单信息
                DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
                List<Orders> order = odal.GetOrderInfo(orderNo/*, entId*/);
                string orderGenerate = order[0].Generate.ToString();//在线支付流水号
                string orderThirdparty = order[0].Thirdparty.ToString();//三方支付交易号
                var orderFee = order[0].PaymentFee;//在线支付金额
                var refundFee = order[0].RefundFee;//在线已退款金额
                string payName = order[0].PaymentName;//订单支付方式
                string payType = order[0].PayType;//支付渠道/退款渠道
                string payWay = payType;
                if (string.IsNullOrEmpty(orderGenerate))
                {
                    judge = false;
                    refId = "";
                    msg = "在线支付流水号为空，请确认订单";
                }
                else if (string.IsNullOrEmpty(orderThirdparty))
                {
                    judge = false;
                    refId = "";
                    msg = "三方支付交易号为空，请确认订单";
                }
                else if (payName != "线上支付")
                {
                    judge = false;
                    refId = "";
                    msg = "订单不属于线上支付无法发起退款";
                }
                if (fee > (orderFee - refundFee))
                {
                    judge = false;
                    refId = "";
                    msg = "当前退款金额数大于允许退款金额数，请确认退款金额";
                }
                else
                {
                    TianTaData tianTa = new TianTaData();
                    tianTa.SetValue("pay_no", orderGenerate);//商户订单号
                    tianTa.SetValue("trade_no", orderThirdparty);//第三方订单号
                    switch (payType)
                    {
                        case "微信": payType = "WECHAT"; break;
                        default: payType = "ALIPAY"; break;
                    }
                    //退款金额保留两位
                    var refee = Convert.ToDecimal(fee.ToString("0.00"));
                    tianTa.SetValue("type", payType);//支付渠道/退款渠道
                    tianTa.SetValue("money", int.Parse((refee * 100).ToString("0")));//在线退款金额 单位分

                    //没有退款单号，商户重新随机生成退款单号
                    int num;
                    if (!string.IsNullOrEmpty(generate))
                    {
                        num = odal.OrderRefundUpdate(orderNo, generate, "", "", "Retry", payWay);
                    }
                    else
                    {
                        generate = GenerateOrderNo(orderNo);
                        nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //记录退款信息
                        num = odal.OrderRefundRecord(orderNo, generate, entId, nowTime, fee, payWay, "甜塔","PC");
                    }
                    refId = generate;
                    tianTa.SetValue("refund_no", generate);

                    if (num > 0)
                    {
                        TianTaData result = TianTaApi.Refund(tianTa);//甜塔退款接口
                        var content = result.GetValue("content").ToString();
                        JObject jo = (JObject)JsonConvert.DeserializeObject(content);
                        if (jo["code"].ToString() == "1" && jo["respType"].ToString() == "Success")
                        {
                            string refundId, rFee;
                            if (payType == "WECHAT")
                            {
                                refundId = jo["refund_id"].ToString();//微信退款单号
                                rFee = (Convert.ToDecimal(jo["refund_fee"].ToString()) / 100).ToString();//申请退款金额
                                decimal a = Convert.ToDecimal(jo["refund_fee"].ToString());
                                decimal b = a / 100;
                                rFee = b.ToString();
                            }
                            else
                            {
                                refundId = jo["response"]["trade_no"].ToString();//支付宝退款单号
                                rFee = jo["response"]["send_back_fee"].ToString();//申请退款金额 
                            }

                            //订单在线退款成功记录
                            int number = odal.OrderRefundUpdate(orderNo, generate, refundId, rFee, "Success", payWay);
                            if (number > 0)
                            {
                                judge = true;
                                msg = "在线退款成功，状态已回写";
                            }
                            else
                            {
                                judge = true;
                                msg = "在线退款成功，状态未回写";
                            }

                        }
                        else
                        {
                            string returnMsg = "";//错误详情
                            if (jo.ContainsKey("return_msg"))
                            {
                                returnMsg = jo["return_msg"].ToString();
                            }
                            if (jo.ContainsKey("err_code_des"))
                            {
                                returnMsg = jo["err_code_des"].ToString();
                            }
                            if (jo.ContainsKey("sub_msg"))
                            {
                                returnMsg = jo["sub_msg"].ToString();
                            }

                            //订单在线退款成功记录
                            int number = odal.OrderRefundUpdate(orderNo, generate, "", "", "Fail", payWay);
                            if (number > 0)
                            {
                                judge = false;
                                msg = "在线退款失败，状态已回写;退款失败原因：" + returnMsg;
                            }
                            else
                            {
                                judge = false;
                                msg = "在线退款失败，状态未回写;退款失败原因：" + returnMsg;
                            }
                        }
                    }
                    else
                    {
                        judge = false;
                        msg = "在线退款发起失败;退款失败原因；退款信息记录异常或状态充值失败";
                    }

                }
            }
            catch (Exception ex)
            {
                judge = false;
                refId = "";
                msg = ex.Message.ToString();
            }
            return msg;
        }
        #endregion

        #region 甜塔退款查询
        /// <summary>
        /// 甜塔退款查询
        /// </summary>
        /// <param name="orderNo">支付临时订单号</param>
        /// <param name="refundId">支付官方交易号</param>
        /// <param name="payType">支付方式</param>
        /// <returns></returns>
        public ActionResult RefundQuery(string orderNo,string refundId,string payType)
        {
            try
            {
                if (string.IsNullOrEmpty(orderNo) && string.IsNullOrEmpty(refundId))
                {
                    return Json(new { success = false, message = "商户退款单号及三方退款单号不能同时为空" });
                }
                else
                {
                    TianTaData tianTa = new TianTaData();
                    tianTa.SetValue("pay_no", orderNo);//商户订单号
                    tianTa.SetValue("trade_no", refundId);//官方订单号
                    switch (payType)
                    {
                        case "微信": payType = "WECHAT"; break;
                        default: payType = "ALIPAY"; break;
                    }
                    tianTa.SetValue("type", payType);//支付渠道
                    TianTaData result = TianTaApi.RefundQuery(tianTa);//甜塔退款查询接口
                    var content = result.GetValue("content").ToString();
                    JObject jo = (JObject)JsonConvert.DeserializeObject(content);
                    if (jo["code"].ToString() == "1" && jo["resptype"].ToString() == "Success")
                    {
                        return Json(new { success = true, message = jo["msg"].ToString() });
                    }
                    else
                    {
                        return Json(new { success = false, message = jo["msg"].ToString() });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 甜塔JSAPI支付
        /// <summary>
        /// 甜塔JSAPI支付
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="openId">微信公众号OpenId</param>
        /// <param name="orderNo">商户订单编号</param>
        /// <param name="payType">在线支付方式(仅限微信)</param>
        /// <returns></returns>
        public ActionResult JSAPI(string orderNo, string entId, string openId, string payType= "微信")
        {
            var apliayResult = new ApliayResult<object>();
            try
            {
                string nowTime = "";
                int fee;
                decimal realAmount = 0;
                string payWay = payType;
                payType = payType == "微信" ? "WECHAT" : "ALIPAY";
                ///通过商户真实订单编号获取订单信息
                DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
                List<Orders> order = odal.GetOrderInfo(orderNo/*, entId*/);
                if (order.Count != 1)
                {
                    apliayResult.Success = false;
                    apliayResult.Message = "获取单据信息错误";
                }
                else if (order[0].PaymentName != "线上支付")
                {
                    apliayResult.Success = false;
                    apliayResult.Message = "该订单支付方式不是线上支付";
                }
                else if (order[0].PaymentName == "线上支付" && order[0].PaymentStatus == 2)
                {
                    apliayResult.Success = false;
                    apliayResult.Message = "该订单已支付";
                }
                else
                {
                    realAmount = Convert.ToDecimal(order[0].Real_Amount.ToString("0.00"));
                    fee = int.Parse((realAmount * 100).ToString("0"));
                    //商户随机支付单号
                    string generate = GenerateOrderNo(orderNo);
                    List<PayJsapi> data = GetJSAPIPayUrl(generate, fee, openId, payType, out string code, out string msg);
                    if (code == "SUCCESS")
                    {
                        int druge;
                        int i = 0;
                        nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        do
                        {
                            i++;
                            //订单在线支付发起时间,临时单号记录
                            druge = odal.OrderPayTimeUpdate(orderNo, generate, entId, nowTime, realAmount, payWay, "", "甜塔","APP");
                            if (druge <= 0)
                            {
                                LogQueue.Write(LogType.Error, "Apliay/ScanPayApliay", $"第{i}次订单在线支付发起时间记录失败");
                            }
                        } while (druge <= 0 && i < 4);
                        apliayResult.Success = true;
                        apliayResult.Message = "订单在线支付发起成功";
                    }
                    else
                    {
                        apliayResult.Success = false;
                        apliayResult.Message = "订单在线支付发起失败";
                    }
                    apliayResult.orderNo = generate;
                    apliayResult.fee = realAmount;
                    apliayResult.initiationTime = nowTime;
                    apliayResult.Source = data;
                }
            }
            catch (Exception ex)
            {
                apliayResult.Success = true;
                apliayResult.Message = ex.Message.ToString();
            }
            return Json(apliayResult,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 甜塔JSAPI支付跳转微信路径
        /// </summary>
        /// <param name="outTradeNo"></param>
        /// <param name="fee"></param>
        /// <param name="openId"></param>
        /// <param name="payType"></param>
        /// <param name="judge"></param>
        /// <returns></returns>
        public List<PayJsapi> GetJSAPIPayUrl(string outTradeNo, int fee,string openId, string payType,out string code, out string msg)
        {
            try
            {
                string webUrl = BaseConfiguration.SercerIp.ToString();
                TianTaData tianTa = new TianTaData();
                //tianTa.SetValue("money", "1");//测试金额
                tianTa.SetValue("money", fee.ToString());//订单金额
                tianTa.SetValue("body", outTradeNo); //商品描述
                tianTa.SetValue("orderNo", outTradeNo);//订单号
                tianTa.SetValue("type", payType);//支付渠道
                tianTa.SetValue("notifyUrl", webUrl + "/Apliay/NotifyReturnTianTaWechat");//异步回调Url
                tianTa.SetValue("openid", openId);//微信公众号OpenId
                TianTaData result = TianTaApi.JSAPI(tianTa);//调用甜塔扫码支付接口
                //生成带签名的JSAPI支付信息
                TianTaData data = new TianTaData();
                List<PayJsapi> payJsapis = new List<PayJsapi>();
                
                if (result.GetValue("code").ToString() == "1" && result.GetValue("respType").ToString() == "Success")
                {
                    data.SetValue("appId", result.GetValue("appid").ToString());//公众账号ID
                    data.SetValue("nonceStr", result.GetValue("nonce_str").ToString());//随机串
                    data.SetValue("package", "prepay_id=" + result.GetValue("prepay_id").ToString());
                    data.SetValue("timeStamp", TianTaData.GenerateTimeStamp());//时间戳
                    data.SetValue("signType", TianTaData.SIGN_TYPE_MD5);//签名类型   
                    string sign;
                    try
                    {
                        sign = data.MakeSignByKey(result.GetValue("key").ToString());
                        data.SetValue("paySign", sign);//签名
                        code = "SUCCESS";
                        msg = "预支付交易会话完成";
                        PayJsapi payJsapi = new PayJsapi()
                        {
                            AppId= data.GetValue("appId").ToString(),
                            TimeStamp= data.GetValue("timeStamp").ToString(),//时间戳待定
                            NonceStr= data.GetValue("nonceStr").ToString(),
                            Package= data.GetValue("package").ToString(),
                            SignType= data.GetValue("signType").ToString(),
                            PaySign= data.GetValue("paySign").ToString()
                        };
                        payJsapis.Add(payJsapi);
                    }
                    catch (Exception ex)
                    {
                        code = "FAIL";
                        msg = ex.Message.ToString();
                        LogQueue.Write(LogType.Error, "微信在线支付JSAPI：", ex.ToString());
                    }
                }
                else if (result.GetValue("code").ToString()=="0")
                {
                    code = "FAIL";
                    msg = result.GetValue("msg").ToString();
                }
                else if (result.GetValue("code").ToString()=="15")
                {
                    code = "FAIL";
                    msg = result.GetValue("msg").ToString();
                }
                else
                {
                    code = "FAIL";
                    msg = "未知异常";
                }
                return payJsapis;
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Apliay/GetJSAPIPayUrl：", ex.ToString());
                code = "FAIL";
                msg = ex.ToString();
                return null;
            }
        }
        #endregion
    }
}
