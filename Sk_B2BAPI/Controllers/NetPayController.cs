using Newtonsoft.Json;
using Sk_B2BAPI.Models.NetPayCScanB;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;
using System.Text;
using Sk_B2BAPI.Models;

namespace Sk_B2BAPI.Controllers
{
    /// <summary>
    /// 银联支付接口
    /// </summary>
    public class NetPayController : Controller
    {
        string PCmid = BaseConfiguration.NetPayPCMid;// PC商户号
        string PCtid = BaseConfiguration.NetPayPCTid; // PC终端号

        string APPmid = BaseConfiguration.NetPayAPPMid;// APP商户号
        string APPtid = BaseConfiguration.NetPayAPPTid; // APP终端号

        string APPInstMid = BaseConfiguration.NetPayAPPInstMid;// APP机构商户号
        string PCInstMid = BaseConfiguration.NetPayPCInstMid;// PC机构商户号
        string msgSrc = BaseConfiguration.NetPayMsgSrc;// 消息来源
        string msgSrcId = BaseConfiguration.NetPayMsgSrcId;// 来源编号
        string md5Key = BaseConfiguration.NetPayKey;//key
        string subAppId = BaseConfiguration.NetPaySubAppId; // APP 使用微信时使用
        string swApiUrl = BaseConfiguration.NetPaySwApiUrl; // 服务窗接口地址
        string apiUrl = BaseConfiguration.NetPayApiUrl; // 接口地址
        string notifyUrl = BaseConfiguration.NetPayNotifyUrl;
        string returnUrl = BaseConfiguration.NetPayReturnUrl;

        #region 网页支付接口
        // GET: NetPay
        /// <summary>
        /// 网页支付接口
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="entId"></param>
        /// <returns></returns>
        public ActionResult ScanWebPay(string OrderNo, string entId, string payType)
        {
            string rQCode = "", generate = "";
            bool flag;
            decimal realAmount = 0;
            string message;
            payType = payType.Trim().ToLower();
            ///通过真实订单号获取订单信息
            DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
            List<Models.Orders> order = odal.GetOrderInfo(OrderNo);
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
                realAmount = Convert.ToDecimal(order[0].Real_Amount.ToString("0.00"));
                string totalAmount = (realAmount * 100).ToString("0");

                string datetime = order[0].Initiationtime == "" ? DateTime.Now.AddDays(-10).ToString() : order[0].Initiationtime;
                DateTime initiationtime = Convert.ToDateTime(datetime);
                DateTime nowtime = DateTime.Now;
                TimeSpan a, b;
                a = new TimeSpan(initiationtime.Ticks);
                b = new TimeSpan(nowtime.Ticks);
                int ticks = (int)b.Subtract(a).TotalMilliseconds;
                //指定订单、指定支付方式 最近交易记录
                List<Models.PayRecord> record = odal.InTwoHoursRecords(OrderNo, "银联");
                string urlRecord = "", typeRecord = "", generateRecord = "", timeRecord = "", source = "";
                if (record.Count > 0)
                {
                    urlRecord = record[0].Url;
                    generateRecord = record[0].Generate;
                    typeRecord = record[0].PayType;
                    timeRecord = record[0].AddTime;
                    source = record[0].Source;
                }

                if (ticks > 7200000 || urlRecord == "" || "银联" != typeRecord || timeRecord == "" || source != "银联")
                {
                    //商户随机支付单号
                    generate = ApliayController.GenerateOrderNo(OrderNo);
                    /* 银联组装参数 开始 */
                    Dictionary<string, object> requestParams = new Dictionary<string, object>();
                    DateTime nowTime = DateTime.Now;

                    string msgType = "acp.jsPay";
                    if (payType == "wepay") // 微信服务窗
                        msgType = "WXPay.jsPay";
                    else if (payType == "alipay") // 阿里服务窗
                        msgType = "trade.jsPay";
                    else if (payType == "netpay") // 云闪付服务窗
                        msgType = "acp.jsPay";

                    requestParams.Add("msgSrc", msgSrc);// 消息来源
                    requestParams.Add("msgType", msgType);// 支付类型
                    requestParams.Add("mid", PCmid);// 商户号
                    requestParams.Add("tid", PCtid);// 终端号
                    requestParams.Add("totalAmount", totalAmount);// 订单金额，单位分
                    requestParams.Add("instMid", PCInstMid);// 机构商户号
                    requestParams.Add("requestTimestamp", nowtime.ToString("yyyy-MM-dd HH:mm:ss"));// 请求时间
                    requestParams.Add("expireTime", nowtime.AddHours(2).ToString("yyyy-MM-dd HH:mm:ss"));// 过期时间
                    requestParams.Add("merOrderId", msgSrcId + generate);
                    requestParams.Add("notifyUrl", notifyUrl);
                    requestParams.Add("returnUrl", returnUrl + "?OrderBh=" + OrderNo + "&Je=￥" + realAmount);
                    requestParams.Add("signType", "SHA256");
                    /* 银联组装参数 结束 */

                    // 请求银联接口，返回数据
                    string requestUrl = NetPayCScanBUtil.MakeOrderRequest(requestParams, swApiUrl, md5Key);
                    flag = true;
                    message = "商户在线支付发起成功";
                    rQCode = requestUrl;
                    int druge;
                    int i = 0;
                    do
                    {
                        i++;
                        //订单在线支付发起时间,临时单号记录
                        druge = odal.OrderPayTimeUpdate(OrderNo, generate, entId, nowtime.ToString("yyyy-MM-dd HH:mm:ss"), order[0].Real_Amount, "银联", requestUrl, "银联", "PC");
                        if (druge <= 0)
                        {
                            LogQueue.Write(LogType.Error, "NetPay/ScanWebPay", $"第{i}次订单在线支付发起时间记录失败");
                        }
                    } while (druge <= 0 && i < 4);
                }
                else
                {
                    flag = true;
                    message = "商户在线支付发起成功";
                    //上次支付流水号
                    generate = generateRecord;
                    rQCode = urlRecord;
                    int druge;
                    int i = 0;
                    do
                    {
                        druge = odal.PayInitiationTimeCorrect(OrderNo, generate, timeRecord, "PC");
                        if (druge <= 0)
                        {
                            LogQueue.Write(LogType.Error, "NetPay/ScanWebPay", $"第{i}次订单汇总支付发起时间修正失败");
                        }
                    } while (druge <= 0 && i < 4);
                }
            }
            return Json(new { success = flag, message, rQCode = ReturnQRCode(rQCode), orderNo = generate, fee = realAmount, initiationTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
        }
        #endregion

        #region APP发起支付
        /// <summary>
        /// APP发起支付
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="entId"></param>
        /// <returns></returns>
        public ActionResult ScanAppPay(string OrderNo, string entId, string payType)
        {
            string snowTime = "", generate = "";
            bool flag;
            decimal realAmount = 0;
            string message;
            string appPayRequest = "";
            payType = payType.Trim().ToLower();
            ///通过真实订单号获取订单信息
            DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
            List<Models.Orders> order = odal.GetOrderInfo(OrderNo);
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
                realAmount = Convert.ToDecimal(order[0].Real_Amount.ToString("0.00"));
                string totalAmount = (realAmount * 100).ToString("0");

                string datetime = order[0].Initiationtime == "" ? DateTime.Now.AddDays(-10).ToString() : order[0].Initiationtime;
                snowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime initiationtime = Convert.ToDateTime(datetime);
                DateTime nowtime = DateTime.Now;
                TimeSpan a, b;
                a = new TimeSpan(initiationtime.Ticks);
                b = new TimeSpan(nowtime.Ticks);
                int ticks = (int)b.Subtract(a).TotalMilliseconds;
                //指定订单、指定支付方式 最近交易记录
                List<Models.PayRecord> record = odal.InTwoHoursRecords(OrderNo, "银联");
                string urlRecord = "", typeRecord = "", generateRecord = "", timeRecord = "", source = "";
                if (record.Count > 0)
                {
                    urlRecord = record[0].Url;
                    generateRecord = record[0].Generate;
                    typeRecord = record[0].PayType;
                    timeRecord = record[0].AddTime;
                    source = record[0].Source;
                }

                if (ticks > 7200000 || urlRecord == "" || "银联" != typeRecord || timeRecord == "" || source != "银联")
                {
                    //商户随机支付单号
                    generate = ApliayController.GenerateOrderNo(OrderNo);
                    /* 银联组装参数 开始 */
                    Dictionary<string, object> requestParams = new Dictionary<string, object>();
                    DateTime nowTime = DateTime.Now;

                    string msgType = "uac.appOrder";
                    if (payType == "wepay") // 微信
                    {
                        msgType = "wx.appPreOrder";
                        requestParams.Add("subAppId", subAppId);
                    }
                    else if (payType == "alipay") // 支付宝
                        msgType = "trade.precreate";
                    else if (payType == "netpay") // 云闪付
                        msgType = "uac.appOrder";

                    requestParams.Add("msgSrc", msgSrc);// 消息来源
                    requestParams.Add("msgType", msgType);// 支付类型
                    requestParams.Add("mid", APPmid);// 商户号
                    requestParams.Add("tid", APPtid);// 终端号
                    requestParams.Add("totalAmount", totalAmount);// 订单金额，单位分
                    requestParams.Add("instMid", APPInstMid);// APP机构商户号
                    requestParams.Add("requestTimestamp", snowTime);// 请求时间戳
                    requestParams.Add("signType", "SHA256");
                    requestParams.Add("merOrderId", msgSrcId + generate);
                    requestParams.Add("expireTime", DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH:mm:ss"));
                    requestParams.Add("notifyUrl", notifyUrl);
                    requestParams.Add("sign", NetPayCScanBUtil.MakeSignSHA256(requestParams, md5Key));// 签名
                    /* 银联组装参数 结束 */

                    // 请求银联接口，返回数据
                    string requestJsonString = JsonConvert.SerializeObject(requestParams);
                    string responseJsonString = NetPayCScanBUtil.CreateHttpPostRequest(requestJsonString, apiUrl);
                    Dictionary<string, object> jsonParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJsonString);

                    // 验证返回数据的签名
                    if (NetPayCScanBUtil.CheckSign(jsonParams, md5Key))
                    {
                        DisplayQRCodeModel getQRCodeResult = JsonConvert.DeserializeObject<DisplayQRCodeModel>(responseJsonString);
                        if (string.Equals(getQRCodeResult.errCode, "SUCCESS"))
                        {
                            flag = true;
                            message = "商户在线支付发起成功";
                            appPayRequest = getQRCodeResult.appPayRequest.ToString().Replace(" ", "").Replace("\r", "").Replace("\n", "");
                            int druge;
                            int i = 0;
                            do
                            {
                                i++;
                                //订单在线支付发起时间,临时单号记录
                                druge = odal.OrderPayTimeUpdate(OrderNo, generate, entId, snowTime, order[0].Real_Amount, "银联", appPayRequest, "银联", "APP");
                                if (druge <= 0)
                                {
                                    LogQueue.Write(LogType.Error, "NetPayCScanB/ScanPay", $"第{i}次订单在线支付发起时间记录失败");
                                }
                            } while (druge <= 0 && i < 4);
                        }
                        else
                        {
                            flag = false;
                            message = getQRCodeResult.errMsg;
                        }
                    }
                    else
                    {
                        flag = false;
                        message = "商户在线支付发起失败";
                    }
                }
                else
                {
                    flag = true;
                    message = "商户在线支付发起成功";
                    //上次支付流水号
                    generate = generateRecord;
                    appPayRequest = urlRecord;
                    int druge;
                    int i = 0;
                    do
                    {
                        druge = odal.PayInitiationTimeCorrect(OrderNo, generate, timeRecord, "APP");
                        if (druge <= 0)
                        {
                            LogQueue.Write(LogType.Error, "NetPay/ScanAppPay", $"第{i}次订单汇总支付发起时间修正失败");
                        }
                    } while (druge <= 0 && i < 4);
                }
            }
            return Json(new { success = flag, message, appPayRequest, orderNo = generate, fee = realAmount, initiationTime = snowTime });
        }
        #endregion

        #region 查询
        // POST: /Home/BillsQuery
        [HttpPost]
        public ActionResult PayQuery(string orderNo, string equipment)
        {
            ViewBag.Message = "账单状态查询";

            DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
            List<Models.Orders> order = odal.GetOrderInfo(orderNo);

            string generate = order[0].Generate;

            equipment = equipment.ToUpper().Trim();

            Dictionary<string, object> requestParams = new Dictionary<string, object>();

            requestParams.Add("msgSrc", msgSrc);
            requestParams.Add("msgType", "query");
            if (equipment == "PC")
            {
                requestParams.Add("mid", PCmid);
                requestParams.Add("tid", PCtid);
                requestParams.Add("instMid", PCInstMid);
            }
            else if (equipment == "APP")
            {
                requestParams.Add("mid", APPmid);
                requestParams.Add("tid", APPtid);
                requestParams.Add("instMid", APPInstMid);
            }
            requestParams.Add("requestTimestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            requestParams.Add("merOrderId", msgSrcId + generate);
            requestParams.Add("signType", "SHA256");
            requestParams.Add("sign", NetPayCScanBUtil.MakeSignSHA256(requestParams, md5Key));

            string requestJsonString = JsonConvert.SerializeObject(requestParams);

            string responseJsonString = NetPayCScanBUtil.CreateHttpPostRequest(requestJsonString, apiUrl);

            Dictionary<string, object> responseJsonParams = NetPayCScanBUtil.ParseMultiJsonObject(responseJsonString);

            // 验证返回数据的签名
            if (NetPayCScanBUtil.CheckSign(responseJsonParams, md5Key))
            {
                DisplayBillsQueryResultModel queryResult = JsonConvert.DeserializeObject<DisplayBillsQueryResultModel>(responseJsonString);

                string status = queryResult.status;
                string message;
                bool succcessstatus = false;
                int code = 0;
                switch (status)
                {
                    case "NEW_ORDER":
                        code = 1;
                        message = "新订单";
                        break;
                    case "TRADE_CLOSED":
                        code = 0;
                        message = "在指定时间段内未支付时关闭；在交易完成全额退款成功时关闭；支付失败。";
                        succcessstatus = true;
                        break;
                    case "WAIT_BUYER_PAY":
                        code = 1;
                        message = "交易创建，等待付款";
                        succcessstatus = false;
                        break;
                    case "TRADE_SUCCESS":
                        code = 2;
                        message = "支付成功";
                        succcessstatus = true;
                        break;
                    case "TRADE_REFUND":
                        code = 2;
                        message = "订单转入退货流程";
                        succcessstatus = true;
                        break;
                    default:
                        code = 99;
                        message = "不明确的交易状态";
                        break;
                }
                return Json(new { success = succcessstatus, code, message });
            }
            else
                return Json(new { success = false, code = 99, message = "未知的异常" });
        }
        #endregion

        #region 退款
        // POST: /Home/BillRefund // 账单退款
        public string PayRefund(string orderNo, string entId, string generate, decimal fee, DAL.OrderInfoDal dal, List<Models.Orders> orders, out bool judge, out string refId)
        {
            string msg, nowTime = "";
            refId = "";
            string orderGenerate = orders[0].Generate.ToString();//在线支付流水号
            string orderThirdparty = orders[0].Thirdparty.ToString();//支付交易号
            var orderFee = orders[0].PaymentFee;//在线支付金额

            var refundFee = orders[0].RefundFee;//在线已退款金额
            string payName = orders[0].PaymentName;//订单支付方式
            string payType = orders[0].PayType;//支付渠道/退款渠道
            string equipment = orders[0].equipment;// 支付发生在APP或PC
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
                generate = ApliayController.GenerateOrderNo();
                nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //记录退款信息
                num = dal.OrderRefundRecord(orderNo, generate, entId, nowTime, fee, payWay, "银联", "PC");
            }
            decimal refundAmount = fee * 100;

            Dictionary<string, object> requestParams = new Dictionary<string, object>();
            requestParams.Add("msgSrc", msgSrc);
            requestParams.Add("msgType", "refund");
            if (equipment == "PC")
            {
                requestParams.Add("mid", PCmid);
                requestParams.Add("tid", PCtid);
                requestParams.Add("instMid", PCInstMid);
            }
            else if (equipment == "APP")
            {
                requestParams.Add("mid", APPmid);
                requestParams.Add("tid", APPtid);
                requestParams.Add("instMid", APPInstMid);
            }
            requestParams.Add("refundAmount", Convert.ToInt32(refundAmount).ToString());
            requestParams.Add("requestTimestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            requestParams.Add("merOrderId", msgSrcId + orderGenerate);
            requestParams.Add("signType", "SHA256");
            requestParams.Add("sign", NetPayCScanBUtil.MakeSignSHA256(requestParams, md5Key));

            string requestJsonString = JsonConvert.SerializeObject(requestParams);

            string responseJsonString = NetPayCScanBUtil.CreateHttpPostRequest(requestJsonString, apiUrl);

            Dictionary<string, object> responseJsonParams = NetPayCScanBUtil.ParseMultiJsonObject(responseJsonString);

            // 验证返回数据的签名
            if (NetPayCScanBUtil.CheckSign(responseJsonParams, md5Key))
            {
                DisplayBillRefundResultModel refundResult = JsonConvert.DeserializeObject<DisplayBillRefundResultModel>(responseJsonString);

                string status = refundResult.refundStatus;
                if (status == "SUCCESS")
                {
                    //订单在线退款成功记录
                    int number = dal.OrderRefundUpdate(orderNo, generate, refundResult.refundOrderId, refundResult.refundAmount.ToString(), "Success", payWay);
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
                else //if (status == "FAIL")
                {
                    //订单在线退款成功记录
                    int number = dal.OrderRefundUpdate(orderNo, generate, "", "", "Fail", payWay);
                    if (number > 0)
                    {
                        judge = false;
                        msg = $"Code:{refundResult.errCode},在线退款响应失败，状态已回写";
                    }
                    else
                    {
                        judge = false;
                        msg = $"Code:{refundResult.errCode},在线退款响应失败，状态未回写";
                    }
                }
            }
            else
            {
                judge = false;
                msg = "在线退款响应失败，参数异常";
            }
            return msg;
        }
        #endregion

        #region 通知地址(银联回调地址)
        /// <summary>
        /// 通知地址(银联回调地址)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult ReceiveNotify()
        {
            ViewBag.Message = "接收通知数据";

            Dictionary<String, object> notifyData = new Dictionary<string, object>();

            // 不要用固定的数据格式处理通知数据，后续系统可能会添加参数
            foreach (var key in Request.Form.AllKeys)
            {
                notifyData.Add(key, Request.Form[key]);
            }
            string generate = notifyData["merOrderId"].ToString();
            string totalAmount = notifyData["totalAmount"].ToString();
            string seqId = notifyData["seqId"].ToString();
            // 获取订单信息
            DAL.OrderInfoDal Odal = new DAL.OrderInfoDal();
            // 通过支付临时订单号获取订单信息
            List<Orders> order = Odal.GetOrderByNumber(generate);
            // 存在数据
            if (order.Count > 0)
            {
                string realAmount = (order[0].Real_Amount * 100).ToString("0");
                // 金额是否对等
                if (realAmount == totalAmount)
                {
                    // 效验银联参数
                    if (NetPayCScanBUtil.CheckSign(notifyData, md5Key))
                    {
                        //判断订单支付状态 1未支付2已支付
                        if (order[0].PaymentStatus == 1)
                        {
                            //获取商户支付真实单号
                            string orderNo = order[0].Order_No;
                            //通过商户真实单号【orderNo】、支付临时订单号【generate】,更新订单状态
                            if (Odal.OrderPayUpdate(orderNo, generate, seqId, order[0].Real_Amount.ToString("0.00")) > 0)
                            {
                                return Content("SUCCESS");
                            }
                            else
                            {
                                LogQueue.Write(LogType.Error, "银联支付回调", $"商城充值订单{generate}充值成功，状态回写失败！");
                                return Content("FAILED");
                            }
                        }
                        else
                        {
                            return Content("SUCCESS");
                        }
                    }
                    else
                    {
                        LogQueue.Write(LogType.Error, "银联支付异步回调", "付款交易失败");
                        return Content("FAILED");
                    }
                }
                else
                {
                    LogQueue.Write(LogType.Error, "银联支付异步回调", "支付金额与订单金额不一致");
                    return Content("FAILED");
                }
            }
            else
            {
                LogQueue.Write(LogType.Error, "银联支付异步回调", "该订单不是商户系统中创建的订单");
                return Content("FAILED");
            }
        }
        #endregion

        #region 返回跳转地址
        // GET:  //Home/ReceiveReturnUrlData
        /// <summary>
        /// 返回跳转地址
        /// </summary>
        /// <returns></returns>
        public string ReceiveReturn()
        {
            ViewBag.Message = "接收返回数据";

            Dictionary<string, object> returnUrlData = new Dictionary<string, object>();
            try
            {
                // 不要用固定的数据格式处理返回数据，后续系统可能会添加参数
                foreach (var key in Request.QueryString.AllKeys)
                {
                    returnUrlData.Add(key, Request.QueryString[key]);
                }
                if (NetPayCScanBUtil.CheckSign(returnUrlData, md5Key))
                {
                    return "returnUrl返回数据验证成功：" + JsonConvert.SerializeObject(returnUrlData);
                }
            }
            catch (Exception)
            {
                return "returnUrl返回数据验证失败：" + JsonConvert.SerializeObject(returnUrlData);
            }
            return "returnUrl返回数据验证失败：" + JsonConvert.SerializeObject(returnUrlData);
        }
        #endregion

        #region 生成二维码base64
        /// <summary>
        ///  生成二维码base64
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ReturnQRCode(string url)
        {
            //初始化二维码生成工具
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 2;

            //将字符串生成二维码图片
            Bitmap image = qrCodeEncoder.Encode(url, Encoding.Default);

            //保存为PNG到内存流  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            image.Dispose();
            String strbaser64 = Convert.ToBase64String(ms.GetBuffer());
            ms.Dispose();
            return "data:image/png;base64," + strbaser64;
        }
        #endregion
    }
}