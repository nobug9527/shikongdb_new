using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.AlipayPayAPI;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
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
using System.Threading;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;

namespace Sk_B2BAPI.Controllers
{
    /// <summary>
    /// 充值
    /// </summary>
    public class RechargeController : Controller
    {
        /*
         * 充值选项
         */
        #region 充值选项
        /// <summary>
        /// 充值选项
        /// </summary>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RechargeOptions(string entId, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId.ToString();
                }
                List<RechargeGoods> rechargeGoods = new List<RechargeGoods>();
                RechargeDal rechargeDal = new RechargeDal();
                rechargeGoods = rechargeDal.RechargeOptions(entId, userId);
                return Json(new { success = true, message = "充值选项获取成功", list = rechargeGoods });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/RechargeOptions", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 充值方式
        /// <summary>
        /// 充值方式
        /// </summary>
        /// <param name="entId">机构</param>
        /// <returns></returns>
        public JsonResult RechargeType(string entId)
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId.ToString();
                }
                PayApiDal payApiDal = new PayApiDal();
                List<Payment> payments = payApiDal.PayTypes(entId);
                return Json(new { success = true, message = "充值方式获取成功", list = payments });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/RechargeType", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        /*
         * 充值退款记录
         */
        #region 充值退款记录
        /// <summary>
        /// 充值退款记录
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="payment">支付方式 微信/支付宝/All</param>
        /// <param name="operation">操作类型 1充值/0退款/99全部</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">单页条目数</param>
        /// <returns></returns>
        public JsonResult RechargeRecord(string userId, string entId, string payment = "All", int operation = 99, int pageIndex = 1, int pageSize = 30)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                RechargeDal rechargeDal = new RechargeDal();
                List<RechargeOrders> rechargeOrders = rechargeDal.RechargeRecord(userId, entId, payment, operation, pageIndex, pageSize, out int pageCount, out int recordCount);
                return Json(new { success = true, message = "充值记录获取成功", pageCount = pageCount, recordCount = recordCount, list = rechargeOrders });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/RechargeRecord", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        /*
         * 测试
         */
        #region 签名测试
        public JsonResult TestSign()
        {
            IDictionary<string, string> paramsMap = new Dictionary<string, string>();
            paramsMap.Add("app_id", "2018052161078888");
            //签名
            string sign = AlipaySignature.RSASign(paramsMap, AlipayConfigHelper.privateKey, "utf-8", "RSA2", false);
            paramsMap.Add("sign", sign);
            //验证签名
            bool checkSign = AlipaySignature.RSACheckV2(paramsMap, AlipayConfigHelper.alipayPublicKey, "utf-8", "RSA2", false);
            return Json(new { success = checkSign, sing = sign });
        }
        #endregion

        #region 充值规则测试
        public JsonResult ce(string ruleId)
        {
            /*生成充值订单*/
            RechargeDal rechargeDal = new RechargeDal();
            List<RechargeRule> rechargeRules = rechargeDal.RechargeRules(ruleId);
            return Json(new { list = rechargeRules });
        }
        #endregion

        #region 微信沙箱测试验签测试密钥
        public JsonResult GetSignKey()
        {
            WxPayData data = new WxPayData();
            data.SetValue("mch_id", WxPayConfig.GetConfig().GetMchID());
            WxPayData payData = WxPayApi.GetSignKey(data);
            if (payData.GetValue("return_code").ToString() == "SUCCESS")
            {
                return Json(new { return_code = payData.GetValue("return_code").ToString(), mchId = payData.GetValue("mch_id").ToString(), signKey = payData.GetValue("sandbox_signkey").ToString() });
            }
            else
            {
                return Json(new { return_code = payData.GetValue("return_code").ToString(), return_msg = payData.GetValue("return_msg").ToString() });
            }
        }
        #endregion

        #region xml读取测试
        public ActionResult Rxml()
        {
            string content = AlipayConfigHelper.privateKey;
            return Content(content, "text");
        }
        #endregion

        /*
         * 甜塔充值
         */
        #region 甜塔扫码充值
        /// <summary>
        /// 甜塔扫码充值
        /// </summary>
        /// <param name="userId">支付用户</param>
        /// <param name="fee">支付金额,单位分</param>
        /// <param name="entId">机构Id</param>
        /// <param name="productId">支付商品Id</param>
        /// <param name="ruleId">规则Id</param>
        /// <param name="rechargeType">充值类型 1定额/2不定额</param>
        /// <param name="payType">支付渠道【微信/支付宝】</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RechargePayUrlTianTa(string userId, string entId, string productId, decimal fee, string ruleId, int rechargeType, string payType)
        {
            string orderNo = "", rQCode = "", payUrl = "", payWay = "", message = "";
            bool flag;
            payWay = payType;
            switch (payType)
            {
                case "微信": payType = "WECHAT"; break;
                default: payType = "ALIPAY"; break;
            }
            try
            {
                //商户订单号
                var outTradeNo = WxPayApi.GenerateOutTradeNo();
                if (!string.IsNullOrWhiteSpace(productId))
                {
                    /*生成充值订单*/
                    RechargeDal rechargeDal = new RechargeDal();
                    bool bl = rechargeDal.IncreaseRecharge(userId, entId, outTradeNo, productId, fee, "", payWay, 1, "" /*WxPayConfig.GetConfig().GetAppID()*/, ruleId, rechargeType, out string msg);
                    if (bl)
                    {
                        decimal realAmount = Convert.ToDecimal(fee.ToString("0.00"));
                        payUrl = GetPayUrlTianTa(outTradeNo, int.Parse((realAmount * 100).ToString("0")), payType, out bool judge);
                        if (judge)
                        {
                            flag = bl;
                            message = "商户充值订单生成成功";
                            rQCode = Convert.ToBase64String(CreateRQCode(payUrl).ToArray());
                        }
                        else
                        {
                            flag = !bl;
                            message = payUrl;
                            rQCode = "";
                        }
                        orderNo = outTradeNo;
                    }
                    else
                    {
                        flag = bl;
                        message = "商户充值订单生成失败,请重新发起充值请求";
                    }
                }
                else
                {
                    flag = false;
                    message = "订单商品不能为空";
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/RechargePayUrlTianTa", ex.Message.ToString());
                flag = false;
                message = ex.Message.ToString();
            }
            return Json(new { success = flag, message = message, rQCode = rQCode, orderNo = orderNo, payUrl = payUrl, fee = fee });
        }
        /// <summary>
        /// 返回甜塔支付二维码
        /// </summary>
        /// <param name="outTradeNo">随机支付单号</param>
        /// <param name="fee">订单金额</param>
        /// <param name="payType">支付渠道</param>
        /// <param name="judge">二维码获取成功返回true,否则返回false</param>
        /// <returns>支付二维码</returns>
        public string GetPayUrlTianTa(string outTradeNo, int fee, string payType, out bool judge)
        {
            string webUrl = BaseConfiguration.SercerIp.ToString();
            TianTaData tianTa = new TianTaData();
            //tianTa.SetValue("money", "1");//测试金额
            tianTa.SetValue("money", fee.ToString());//订单金额
            tianTa.SetValue("body", outTradeNo); //商品描述
            tianTa.SetValue("orderNo", outTradeNo);//订单号
            tianTa.SetValue("type", payType);//支付渠道
            if (payType == "WECHAT")//微信
            {
                tianTa.SetValue("notifyUrl", webUrl + "/Recharge/NotifyReturnTianTaWechat");//异步回调Url
            }
            else//支付宝
            {
                tianTa.SetValue("notifyUrl", webUrl + "/Recharge/NotifyReturnTianTaAlipay");//异步回调Url
            }

            TianTaData result = TianTaApi.ScanPay(tianTa);//调用甜塔扫码支付接口

            string url = "";
            judge = false;
            var content = result.GetValue("content").ToString();
            JObject jo = (JObject)JsonConvert.DeserializeObject(content);

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
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            TianTaData tianTa = new TianTaData();
            tianTa.FromUrl(builder.ToString());

            //LogQueue.Write(LogType.Debug, "充值return_code", tianTa.GetValue("return_code").ToString());
            if (tianTa.GetValue("return_code").ToString() == "SUCCESS")
            {
                //LogQueue.Write(LogType.Debug, "充值返回参数", $"transaction_id:{tianTa.GetValue("transaction_id").ToString()},out_trade_no:{tianTa.GetValue("out_trade_no").ToString()},total_fee:{tianTa.GetValue("total_fee").ToString()},result_code:{tianTa.GetValue("result_code").ToString()}");
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

                /*查询充值订单*/
                RechargeDal rechargeDal = new RechargeDal();
                List<RechargeOrders> rechargeOrders = rechargeDal.InquireRecharge(outTradeNo);
                //LogQueue.Write(LogType.Debug, "充值订单状态", $"Status:{rechargeOrders[0].Status},Fee:{(rechargeOrders[0].Fee * 100).ToString()}");
                if (rechargeOrders[0].Status == 2)//判断商城支付订单状态
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                    return Content(res.ToXml(), "text/xml");
                }
                else if (Convert.ToDecimal((rechargeOrders[0].Fee * 100).ToString("0")) != totalFee)//判断商城支付订单金额
                {
                    LogQueue.Write(LogType.Error, "微信在线支付回调", $"订单金额不符，商城在线支付订单{outTradeNo}金额：{rechargeOrders[0].Fee},微信回调订单金额数据：{totalFee}！");
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
                    //业务结果判断
                    if (resultCode == "SUCCESS")
                    {
                        bool bl = rechargeDal.UpdateRecharge(outTradeNo, transactionId, 2, 0, "", "", out string message);
                        if (!bl)
                        {
                            LogQueue.Write(LogType.Error, "微信线上充值回调", $"商户线上充值订单：{outTradeNo}，支付成功-状态回写失败！");
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
                        bool bl = rechargeDal.UpdateRecharge(outTradeNo, "", 0, 0, "", "", out string message);
                        if (!bl)
                        {
                            LogQueue.Write(LogType.Error, "微信线上充值回调", $"商户线上充值订单：{outTradeNo}，支付失败-状态回写失败！");
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

            /*查询充值订单*/
            RechargeDal rechargeDal = new RechargeDal();
            List<RechargeOrders> rechargeOrders = rechargeDal.InquireRecharge(outTradeNo);
            //验证订单是否为商户订单
            if (rechargeOrders.Count > 0)
            {
                //验证支付金额与订单金额是否一致
                if (rechargeOrders[0].Fee == Convert.ToDecimal(totalAmount))
                {
                    //状态TRADE_SUCCESS的通知触发条件是买家付款成功
                    if (map["trade_status"] == "TRADE_FINISHED" || map["trade_status"] == "TRADE_SUCCESS")
                    {
                        //判断订单支付状态 1未支付2已支付
                        if (rechargeOrders[0].Status == 1)
                        {
                            //通过商户真实单号【orderNo】、支付临时订单号【outTradeNo】更新
                            bool bl = rechargeDal.UpdateRecharge(outTradeNo, tradeNo, 2, 0, "", "", out string message);
                            if (!bl)
                            {
                                LogQueue.Write(LogType.Error, "支付宝网站充值回调", $"商城充值订单{outTradeNo}支付成功，状态回写失败！");
                                return Content("支付信息回写失败!");
                            }
                            else
                            {
                                return Content("success");
                            }
                        }
                        else
                        {
                            return Content("success");
                        }
                    }
                    else
                    {
                        bool bl = rechargeDal.UpdateRecharge(outTradeNo, tradeNo, 0, 0, "", "", out string message);
                        if (!bl)
                        {
                            LogQueue.Write(LogType.Error, "支付宝网站充值回调", $"商城充值订单{outTradeNo}支付失败，状态回写失败！");
                            return Content("failure");
                        }
                        else
                        {
                            return Content("success");
                        }
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
        #endregion

        #region 甜塔App充值
        /// <summary>
        /// 甜塔App充值
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="productId">商品</param>
        /// <param name="fee">金额</param>
        /// <param name="ruleId">规则</param>
        /// <param name="rechargeType">充值类型 1定额/2不定额</param>
        /// <param name="payType">支付渠道【微信/支付宝】</param>
        /// <returns></returns>
        public JsonResult RechargeAppPayTianTa(string userId, string entId, string productId, int fee, string ruleId, int rechargeType, string payType)
        {
            bool flag;
            var apliayResult = new Models.ApliayResult<object>();
            if (payType != "微信")
            {
                apliayResult.Success = false;
                apliayResult.Message = "除微信外暂不支持其它支付方式";
            }
            try
            {
                string payWay = payType;
                switch (payType)
                {
                    case "微信": payType = "WECHAT"; break;
                    default: payType = "ALIPAY"; break;
                }
                //商户订单号
                var out_trade_no = WxPayApi.GenerateOutTradeNo();
                if (!string.IsNullOrWhiteSpace(productId))
                {
                    /*生成订单*/
                    RechargeDal rechargeDal = new RechargeDal();
                    bool bl = rechargeDal.IncreaseRecharge(userId, entId, out_trade_no, productId, fee, "", payWay, 1, ""/*WxPayConfig.GetConfig().GetAppID()*/, ruleId, rechargeType, out string msg);
                    if (bl)
                    {
                        flag = bl;
                        string orderNo = out_trade_no;
                        List<PayApp> payApps = new List<PayApp>();
                        payApps = GetTianTaAppPay(orderNo, fee, payType, out string code, out string mag);

                        if (code == "SUCCESS")
                        {
                            string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            apliayResult.Success = true;
                            apliayResult.Message = "商户充值订单生成成功";
                            apliayResult.orderNo = orderNo;
                            apliayResult.fee = fee;
                            apliayResult.initiationTime = nowTime;
                            apliayResult.Source = payApps;
                        }
                        else
                        {
                            apliayResult.Success = false;
                            apliayResult.Message = msg;
                            apliayResult.orderNo = orderNo;
                            apliayResult.fee = fee;
                        }
                    }
                    else
                    {
                        apliayResult.Success = bl;
                        apliayResult.Message = "商户充值订单生成失败,请重新发起充值请求";
                    }
                }
                else
                {
                    apliayResult.Success = false;
                    apliayResult.Message = "订单商品不能为空";
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/RechargeAppPayWeChat", ex.Message.ToString());
                apliayResult.Success = false;
                apliayResult.Message = ex.Message.ToString();
            }
            return Json(apliayResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 甜塔App支付
        /// </summary>
        /// <param name="outTradeNo">支付订单临时单号</param>
        /// <param name="fee">支付金额</param>
        /// <param name="payType">支付方式</param>
        /// <param name="code">返回结果 Success【成功】/FAILED【失败】</param>
        /// <param name="msg">返回结果描述</param>
        /// <returns></returns>
        public List<PayApp> GetTianTaAppPay(string outTradeNo, int fee, string payType, out string code, out string msg)
        {
            try
            {
                TianTaData tianTa = new TianTaData();
                tianTa.SetValue("type", payType);
                tianTa.SetValue("money", fee);
                tianTa.SetValue("body", outTradeNo);
                tianTa.SetValue("orderNo", outTradeNo);
                string webUrl = BaseConfiguration.SercerIp.ToString();
                if (payType == "WECHAT")//微信
                {
                    tianTa.SetValue("notifyUrl", webUrl + "/Recharge/NotifyReturnTianTaWechat");//异步回调Url
                }
                else//支付宝
                {
                    tianTa.SetValue("notifyUrl", webUrl + "/Recharge/NotifyReturnTianTaAlipay");//异步回调Url
                }
                TianTaData result = TianTaApi.AppPay(tianTa);//调用甜塔App支付接口
                //生成带签名的App支付信息
                TianTaData data = new TianTaData();
                List<PayApp> payApps = new List<PayApp>();
                if (result.GetValue("code").ToString() == "1" && result.GetValue("respType").ToString() == "Success")
                {
                    code = result.GetValue("respType").ToString();
                    msg = result.GetValue("msg").ToString();

                    //App支付签名
                    data.SetValue("appid", result.GetValue("sub_appid").ToString());
                    data.SetValue("partnerid", result.GetValue("sub_mch_id").ToString());
                    data.SetValue("prepayid", result.GetValue("prepay_id").ToString());
                    data.SetValue("noncestr", result.GetValue("nonce_str").ToString());
                    data.SetValue("timestamp", TianTaData.GenerateTimeStamp());
                    data.SetValue("package", "Sign=WXPay");
                    string sign;
                    try
                    {
                        sign = data.MakeSign();
                        data.SetValue("sign", sign);//签名
                        code = "SUCCESS";
                        msg = "预支付交易会话完成";

                        PayApp payApp = new PayApp()
                        {
                            Appid = data.GetValue("appid").ToString(),
                            Partnerid = data.GetValue("partnerid").ToString(),
                            Prepayid = data.GetValue("prepayid").ToString(),
                            Noncestr = data.GetValue("noncestr").ToString(),
                            Package = data.GetValue("package").ToString(),
                            Timestamp = data.GetValue("timestamp").ToString(),
                            Sign = data.GetValue("sign").ToString()
                        };
                        payApps.Add(payApp);
                    }
                    catch (Exception ex)
                    {
                        code = "FAILED";
                        msg = ex.Message.ToString();
                    }
                }
                else
                {
                    code = result.GetValue("respType").ToString();
                    msg = result.GetValue("msg").ToString();
                }
                return payApps;
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Apliay/GetTianTaAppPay：", ex.ToString());
                code = "FAILED";
                msg = ex.ToString();
                return null;
            }
        }
        #endregion

        #region 甜塔退款
        /// <summary>
        /// 甜塔退款
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="productId">产品</param>
        /// <param name="transactionId">官方交易流水号</param>
        /// <param name="outTradeNo">商户订单号</param>
        /// <param name="refundFee">退款金额</param>
        /// <param name="payType">支付渠道【微信/支付宝】</param>
        /// <returns></returns>
        public ActionResult RechargeRefundTianTa(string userId, string entId, string productId, string transactionId, string outTradeNo, decimal refundFee, string payType)
        {
            string payWay = payType;
            switch (payType)
            {
                case "微信": payType = "WECHAT"; break;
                default: payType = "ALIPAY"; break;
            }
            //退款临时单号
            string outRefundNo = WxPayApi.GenerateOutTradeNo();
            /*生成退款订单*/
            RechargeDal rechargeDal = new RechargeDal();
            bool bl = rechargeDal.IncreaseRecharge(userId, entId, outRefundNo, productId, refundFee, "", payWay, 0, ""/*AlipayConfigHelper.appId*/, "", 0, out string msg);
            if (bl)
            {
                TianTaData tianTa = new TianTaData();
                tianTa.SetValue("pay_no", outTradeNo);//商户订单号
                tianTa.SetValue("trade_no", transactionId);//第三方订单号
                //退款金额保留两位
                var refee = Convert.ToDecimal(refundFee.ToString("0.00"));
                tianTa.SetValue("type", payType);//支付渠道/退款渠道
                tianTa.SetValue("money", int.Parse((refee * 100).ToString("0")));//在线退款金额 单位分
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
                    /*回写退款订单*/
                    RechargeDal rechargeDal1 = new RechargeDal();
                    bool flag = rechargeDal1.UpdateRecharge(outRefundNo, refundId, 2, refee, transactionId, outTradeNo, out string message);
                    if (!flag)
                    {
                        LogQueue.Write(LogType.Error, "支付宝退款", $"商城退款订单{outRefundNo} 退款成功，成功状态回写失败！");
                    }
                    return Json(new { success = true, message = "退款成功" });
                }
                else
                {
                    /*回写退款订单*/
                    RechargeDal rechargeDal1 = new RechargeDal();
                    bool flag = rechargeDal1.UpdateRecharge(outRefundNo, "", 0, refee, transactionId, outTradeNo, out string message);
                    if (!flag)
                    {
                        LogQueue.Write(LogType.Error, "支付宝退款", $"商城退款订单{outRefundNo} 退款成功，成功状态回写失败！");
                    }
                    return Json(new { success = true, message = jo["msg"].ToString() });
                }
            }
            else
            {
                return Json(new { success = true, message = "退款订单生成失败" });
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
            var content = result.GetValue("content").ToString();
            JObject jo = (JObject)JsonConvert.DeserializeObject(content);
            if (jo["code"].ToString() == "1" && jo["respType"].ToString() == "Success")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 甜塔充值订单状态查询
        /// <summary>
        /// 甜塔充值订单状态查询
        /// </summary>
        /// <param name="orderNo">商户订单号</param>
        /// <param name="tradeNo">官方交易号</param>
        /// <param name="payType">充值渠道</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryTianTa(string orderNo, string tradeNo, string payType)
        {
            if (string.IsNullOrEmpty(orderNo) && string.IsNullOrEmpty(tradeNo))
            {
                return Json(new { success = true, message = "商户订单号与第三方订单号不能同时为空" });
            }
            else
            {
                string message;
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
                        message = jo["status_msg"].ToString();
                    }
                    else
                    {
                        message = jo["msg"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message.ToString();
                }
                return Json(new { success = true, message = message });
            }
        }
        #endregion
        /*
        * 微信扫码支付
        */
        #region 微信扫码支付
        /// <summary>
        /// 微信扫描支付模式二Url
        /// </summary>
        /// <param name="userId">支付用户</param>
        /// <param name="fee">支付金额,单位分</param>
        /// <param name="entId">机构Id</param>
        /// <param name="productId">支付商品Id</param>
        /// <param name="ruleId">规则Id</param>
        /// <param name="rechargeType">充值类型 1定额/2不定额</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RechargePayUrlWeChat(string userId, string entId, string productId, int fee, string ruleId, int rechargeType)
        {
            string orderNo = "", rQCode = "", payUrl = "";
            bool flag;
            string message;
            try
            {
                //商户订单号
                var outTradeNo = WxPayApi.GenerateOutTradeNo();
                if (!string.IsNullOrWhiteSpace(productId))
                {
                    /*生成充值订单*/
                    RechargeDal rechargeDal = new RechargeDal();
                    bool bl = rechargeDal.IncreaseRecharge(userId, entId, outTradeNo, productId, fee, "", "微信", 1, WxPayConfig.GetConfig().GetAppID(), ruleId, rechargeType, out string msg);
                    if (bl)
                    {
                        payUrl = GetPayUrlWeChat(productId, outTradeNo, fee, out bool judge);
                        if (judge)
                        {
                            flag = bl;
                            message = "商户充值订单生成成功";
                            rQCode = Convert.ToBase64String(DoWaitProcess(payUrl, outTradeNo, "WeChat").ToArray());
                        }
                        else
                        {
                            flag = !bl;
                            message = payUrl;
                            rQCode = "";
                        }
                        orderNo = outTradeNo;
                    }
                    else
                    {
                        flag = bl;
                        message = "商户充值订单生成失败,请重新发起充值请求";
                    }
                }
                else
                {
                    flag = false;
                    message = "订单商品不能为空";
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/RechargePayUrlWeChat", ex.Message.ToString());
                flag = false;
                message = ex.Message.ToString();
            }
            return Json(new { success = flag, message = message, rQCode = rQCode, orderNo = orderNo, payUrl = payUrl, fee = fee/*, rule = rechargeRules*/ });
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
            data.SetValue("body", "商城充值");//商品描述
            data.SetValue("attach", "附加信息,用于后台或者存入数据库,做自己的判断");//附加数据
            data.SetValue("out_trade_no", out_trade_no);//随机字符串
            data.SetValue("total_fee", fee);//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(30).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", "商品的备忘,可以自定义");//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", productId);//商品ID
            data.SetValue("notify_url", webUrl + "/Recharge/RechargeResultWeChat");/*异步通知url*/
            WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
            string url;
            if (result.GetValue("return_code").ToString() == "FAIL")
            {
                judge = false;
                url = result.GetValue("retmsg").ToString();
            }
            else
            {
                judge = true;
                url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接
            }
            return url;
        }
        #endregion

        #region 微信支付轮询
        public void LoopQueryWechat(object o)
        {
            try
            {
                int count = 40;
                int interval = 3000;
                string out_trade_no = o.ToString();
                WxPayData data = new WxPayData();
                data.SetValue("transaction_id", "");/*微信订单号*/
                data.SetValue("out_trade_no", out_trade_no);/*商户订单号*/
                WxPayData payData = new WxPayData();
                for (int i = 1; i <= count; i++)
                {
                    Thread.Sleep(interval);
                    payData = WxPayApi.OrderQuery(data);
                    if (payData != null)
                    {
                        if (payData.GetValue("return_code").ToString() == "SUCCESS" && payData.GetValue("result_code").ToString() == "SUCCESS" && payData.GetValue("trade_state").ToString() == "SUCCESS")
                        {
                            DoSuccessProcessWeChat(payData);
                            return;
                        }
                    }
                }
                DoFailedProcessWeChat(data);
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/LoopQueryWechat", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 请添加微信支付成功后的处理
        /// </summary>
        private void DoSuccessProcessWeChat(WxPayData payData)
        {
            //支付成功，请更新相应单据
            /*查询充值订单*/
            RechargeDal rechargeDal = new RechargeDal();
            string outTradeNo = payData.GetValue("out_trade_no").ToString();
            string transactionId = payData.GetValue("transaction_id").ToString();
            List<RechargeOrders> rechargeOrders = rechargeDal.InquireRecharge(outTradeNo);
            if (rechargeOrders[0].Status == 1)//判断商城充值订单状态 状态0【失败，订单关闭】1【已下单】2【订单交易成功】
            {
                bool bl = rechargeDal.UpdateRecharge(outTradeNo, transactionId, 2, 0, "", "", out string message);
                if (!bl)
                {
                    LogQueue.Write(LogType.Error, "微信轮询-支付成功", $"商城充值订单{outTradeNo}回写失败！");
                }
            }
        }

        /// <summary>
        /// 请添加微信支付失败后的处理
        /// </summary>
        private void DoFailedProcessWeChat(WxPayData payData)
        {
            string outTradeNo = payData.GetValue("out_trade_no").ToString();
            //LogQueue.Write(LogType.Debug, "", $"支付失败");
            //支付失败，请更新相应单据
            WxPayData data = new WxPayData();
            data.SetValue("out_trade_no", outTradeNo);/*商户订单号*/
            WxPayData wxPay = WxPayApi.CloseOrder(data);

            if (wxPay.GetValue("return_code").ToString() == "SUCCESS" && wxPay.GetValue("result_code").ToString() == "SUCCESS")
            {
                RechargeDal rechargeDal = new RechargeDal();
                bool bl = rechargeDal.UpdateRecharge(outTradeNo, "", 0, 0, "", "", out string message);
                if (!bl)
                {
                    LogQueue.Write(LogType.Error, "微信轮询-支付失败", $"商城充值订单{outTradeNo},支付失败，失败状态回写失败！");
                }
            }
            else
            {
                LogQueue.Write(LogType.Error, "微信轮询-支付失败", $"商城充值订单{outTradeNo}，支付失败，撤销原订单失败！");
            }
        }
        #endregion 

        #region 微信支付回调
        /// <summary>
        /// 微信支付回调
        /// </summary>
        /// <returns></returns>
        public ActionResult RechargeResultWeChat()
        {
            string strData = ProcessPayNotify();
            return this.Content(strData, "text/xml");
        }

        /// <summary>
        /// 获取微信支付回调数据
        /// </summary>
        /// <returns></returns>
        public string ProcessPayNotify()
        {
            string strXml = "";
            WxPayData notifyData = GetNotifyData();
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                strXml = res.ToXml();
                return strXml;
            }
            else if (!notifyData.IsSet("out_trade_no"))
            {
                //若out_trade_no不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中商户订单号不存在");
                strXml = res.ToXml();
                return strXml;
            }

            string transactionId = notifyData.GetValue("transaction_id").ToString();//微信支付订单号
            string outTradeNo = notifyData.GetValue("out_trade_no").ToString();//商户充值订单号
            decimal totalFee = decimal.Parse(notifyData.GetValue("total_fee").ToString());//订单金额
            string resultCode = notifyData.GetValue("result_code").ToString();//业务结果
            /*查询充值订单*/
            RechargeDal rechargeDal = new RechargeDal();
            List<RechargeOrders> rechargeOrders = rechargeDal.InquireRecharge(outTradeNo);
            if (rechargeOrders[0].Status != 1)//判断商城充值订单状态
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                strXml = res.ToXml();
                return strXml;
            }
            else if (rechargeOrders[0].Fee != totalFee)//判断商城充值订单金额
            {
                LogQueue.Write(LogType.Error, "微信支付回调", $"订单金额不符，商城充值订单{outTradeNo}金额：{rechargeOrders[0].Fee},微信回调订单金额数据：{totalFee}！");
                //若订单金额不符，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单金额错误");
                strXml = res.ToXml();
                return strXml;
            }
            else if (!QueryOrder(transactionId))//查询订单，判断订单真实性
            {
                LogQueue.Write(LogType.Error, "微信支付回调", $"根据微信回调数据微信支付订单号{transactionId}查询订单失败！");
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                strXml = res.ToXml();
                return strXml;
            }
            else//查询订单成功
            {
                //业务结果判断
                if (resultCode == "SUCCESS")
                {
                    /*充值成功回写充值订单*/
                    bool bl = rechargeDal.UpdateRecharge(outTradeNo, transactionId, 2, 0, "", "", out string message);
                    if (!bl)
                    {
                        LogQueue.Write(LogType.Error, "微信支付回调", $"商户充值订单：{outTradeNo}，支付成功-状态回写失败！");
                        return "";
                    }
                }
                else
                {
                    string errCodeDes = notifyData.GetValue("err_code_des").ToString();//错误代码描述
                    LogQueue.Write(LogType.Error, "微信支付回调", $"商户充值订单：{outTradeNo}，支付失败-原因：{errCodeDes}！");

                    /*订单支付失败关闭原单*/
                    WxPayData data = new WxPayData();
                    data.SetValue("out_trade_no", outTradeNo);/*商户订单号*/
                    WxPayData payData = WxPayApi.CloseOrder(data);
                    if (payData.GetValue("return_code").ToString() == "SUCCESS" && payData.GetValue("result_code").ToString() == "SUCCESS")
                    {
                        /*订单支付失败回写商户订单*/
                        bool bl = rechargeDal.UpdateRecharge(outTradeNo, transactionId, 0, 0, "", "", out string message);
                        if (!bl)
                        {
                            LogQueue.Write(LogType.Error, "微信支付回调", $"商户充值订单：{outTradeNo}，支付失败-状态回写失败！");
                            return "";
                        }
                    }
                    else
                    {
                        LogQueue.Write(LogType.Error, "微信支付回调", $"商户充值订单：{outTradeNo}，支付失败-关闭原单失败！");
                        return "";
                    }
                }

                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                strXml = res.ToXml();
                return strXml;
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
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
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
        /// <param name="transaction_id">微信支付订单号</param>
        /// <returns>是否存在（true/false）</returns>
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);/*微信支付订单号*/
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 微信支付状态查询
        /// <summary>
        /// 充值订单支付状态查询(微信接口查询)
        /// </summary>
        /// <param name="transactionId">微信订单号</param>
        /// <param name="outTradeNo">商户订单号</param>
        /// <returns></returns>
        public JsonResult RechargeStatusWeChat(/*string transactionId,*/ string outTradeNo)
        {
            WxPayData data = new WxPayData();
            if (/*string.IsNullOrEmpty(transactionId)&&*/string.IsNullOrEmpty(outTradeNo))
            {
                return Json(new { success = false, message = "商户订单号不能全部为空" });
            }
            RechargeDal recharge = new RechargeDal();
            List<RechargeOrders> recharges = recharge.InquireRecharge(outTradeNo);
            data.SetValue("transaction_id", recharges[0].TransactionId);/*微信订单号*/
            data.SetValue("out_trade_no", recharges[0].OrderNo);/*商户订单号*/
            //if (!string.IsNullOrEmpty(transactionId))
            //{
            //    data.SetValue("transaction_id", transactionId);/*微信订单号*/
            //}
            //if (!string.IsNullOrEmpty(outTradeNo))
            //{
            //    data.SetValue("out_trade_no", outTradeNo);/*商户订单号*/
            //}
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
                switch (tradeState)
                {
                    case "SUCCESS": code = 2; message = "支付成功"; break;
                    case "REFUND": code = 2; message = "转入退款"; break;
                    case "NOTPAY": code = 0; message = "未支付"; break;
                    case "CLOSED": code = 0; message = "已关闭"; break;
                    case "REVOKED": code = 0; message = "已撤销"; break;
                    case "USERPAYING": code = 1; message = "用户支付中"; break;
                    case "PAYERROR": code = 1; message = "支付失败"; break;
                    default: code = 99; message = "未知结果"; break;
                }
                return Json(new { success = true, code = code, message = message });
            }
            else
            {
                return Json(new { success = false, message = "未知的异常" });
            }
        }
        #endregion

        #region 微信关闭订单
        /// <summary>
        /// 关闭充值订单
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public JsonResult CloseRechargeWeChat(string orderNo)
        {
            WxPayData data = new WxPayData();
            data.SetValue("out_trade_no", orderNo);/*商户订单号*/
            WxPayData payData = WxPayApi.CloseOrder(data);
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
                /*回写充值订单*/
                RechargeDal rechargeDal = new RechargeDal();
                bool bl = rechargeDal.UpdateRecharge(orderNo, "", 0, 0, "", "", out string message);
                if (bl)
                {
                    return Json(new { success = true, message = payData.GetValue("result_msg").ToString() });
                }
                else
                {
                    return Json(new { success = true, message = "订单关闭失败" });
                }
            }
            else
            {
                return Json(new { success = false, message = "未知的异常" });
            }

        }
        #endregion

        #region 微信退款申请
        /// <summary>
        /// 微信退款申请
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="productId">商品</param>
        /// <param name="transaction_id">微信订单号</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="total_fee">订单金额</param>
        /// <param name="refund_fee">退款金额</param>
        /// <param name="refundNo">退款失败后再次发起退款须传原退款单号</param>
        /// <returns></returns>
        public JsonResult RechargeRefundWeChat(string userId, string entId, string productId, string transaction_id, string out_trade_no, decimal total_fee, decimal refund_fee, string refundNo = "")
        {
            string webUrl = BaseConfiguration.SercerIp.ToString();
            string outRefundNo;
            if (refundNo == "")
            {
                outRefundNo = WxPayApi.GenerateOutTradeNo();
                /*生成订单*/
                RechargeDal rechargeDal = new RechargeDal();
                bool bl = rechargeDal.IncreaseRecharge(userId, entId, outRefundNo, productId, refund_fee, "", "微信", 0, WxPayConfig.GetConfig().GetAppID(), "", 0, out string msg);
                if (bl)
                {
                    WxPayData data = new WxPayData();
                    if (!string.IsNullOrEmpty(transaction_id))
                    {
                        data.SetValue("transaction_id", transaction_id);/*微信订单号*/
                    }
                    else if (!string.IsNullOrEmpty(out_trade_no))
                    {
                        data.SetValue("out_trade_no", out_trade_no);/*商户订单号*/
                    }
                    else
                    {
                        return Json(new { success = false, message = "微信订单号或商户订单号不能全部为空" });
                    }
                    data.SetValue("out_refund_no", outRefundNo);/*商户订单号*/
                    data.SetValue("total_fee", total_fee);/*订单金额*/
                    data.SetValue("refund_fee", refund_fee);/*退款金额*/
                    data.SetValue("notify_url", webUrl + "/Recharge/RefundResultWeChat");/*异步通知url*/
                    WxPayData payData = WxPayApi.Refund(data);
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
                        /*还缺更新订单状态，已发起订单退款*/
                        return Json(new { success = true, message = "退款申请接受成功" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "未知的异常" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "退款单生成失败" });
                }
            }
            else
            {
                outRefundNo = refundNo;
                WxPayData data = new WxPayData();
                if (!string.IsNullOrEmpty(transaction_id))
                {
                    data.SetValue("transaction_id", transaction_id);/*微信订单号*/
                }
                else if (!string.IsNullOrEmpty(out_trade_no))
                {
                    data.SetValue("out_trade_no", out_trade_no);/*商户订单号*/
                }
                else
                {
                    return Json(new { success = false, message = "微信订单号或商户订单号不能全部为空" });
                }
                data.SetValue("out_refund_no", outRefundNo);/*商户订单号*/
                data.SetValue("total_fee", total_fee);/*订单金额*/
                data.SetValue("refund_fee", refund_fee);/*退款金额*/
                data.SetValue("notify_url", webUrl + "/Recharge/RefundResultWeChat");/*异步通知url*/
                WxPayData payData = WxPayApi.Refund(data);
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
                    return Json(new { success = true, message = "退款申请接受成功" });
                }
                else
                {
                    return Json(new { success = false, message = "未知的异常" });
                }
            }
        }
        #endregion

        #region 微信退款申请回调
        /// <summary>
        /// 微信退款申请回调
        /// </summary>
        /// <returns></returns>
        public ActionResult RefundResultWeChat()
        {
            string strData = ProcessRefundNotify();
            return this.Content(strData, "text/xml");
        }

        /// <summary>
        /// 获取微信退款回调数据
        /// </summary>
        /// <returns></returns>
        public string ProcessRefundNotify()
        {
            string strXml;
            WxPayData notifyData = GetNotifyData();
            /*判断加密信息是否存在*/
            if (!notifyData.IsSet("return_code") || notifyData.GetValue("return_code").ToString() != "SUCCESS" || !notifyData.IsSet("req_info"))
            {
                //若req_info不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "未返回正确微信支付退款解密信息");
                strXml = res.ToXml();
                return strXml;
            }
            var reqInfo = WxPayApi.DecodeBase64("utf-8", notifyData.GetValue("req_info").ToString());/*解码后的加密串*/
            var key = WxPayApi.GenerateMD5(WxPayConfig.GetConfig().GetKey());/*对商户key做md5，得到32位小写key* */
            var data = WxPayApi.DecryptAES(reqInfo, key);
            WxPayData payData = new WxPayData();
            payData.FromXml(data, false);/*xml转为对象*/
            if (!payData.IsSet("refund_id"))
            {
                //若refund_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "退款结果中微信退款单号不存在");
                strXml = res.ToXml();
                return strXml;
            }
            else if (!payData.IsSet("out_refund_no"))
            {
                //若out_refund_no不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "退款结果中商户退款单号不存在");
                strXml = res.ToXml();
                return strXml;
            }
            else
            {
                var outTradeNo = payData.GetValue("out_trade_no").ToString();/*商户订单号*/
                var transactionId = payData.GetValue("transaction_id").ToString();/*微信订单号*/
                var outRefundNo = payData.GetValue("out_refund_no").ToString();/*商户退款单号*/
                var refundId = payData.GetValue("refund_id").ToString();/*微信退款单号*/
                var totalFee = decimal.Parse(payData.GetValue("total_fee").ToString());/*订单充值金额*/
                var settlementRefundFee = decimal.Parse(payData.GetValue("settlement_refund_fee").ToString());/*退款金额*/
                /*回写退款订单*/
                RechargeDal rechargeDal = new RechargeDal();
                if (payData.GetValue("refund_status").ToString() == "SUCCESS")
                {
                    bool bl = rechargeDal.UpdateRecharge(outRefundNo, refundId, 2, totalFee, transactionId, outTradeNo, out string message);
                    if (!bl)
                    {
                        LogQueue.Write(LogType.Error, "微信支付回调", $"商城退款订单：{outRefundNo} 退款成功，成功状态回写失败！");
                        strXml = "";
                        return strXml;
                    }
                }
                else
                {
                    bool bl = rechargeDal.UpdateRecharge(outRefundNo, refundId, 0, totalFee, transactionId, outTradeNo, out string message);
                    if (!bl)
                    {
                        LogQueue.Write(LogType.Error, "微信支付回调", $"商城退款订单{outRefundNo} 退款失败，失败状态回写失败！");
                        strXml = "";
                        return strXml;
                    }
                }
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                strXml = res.ToXml();
                return strXml;
            }
        }
        #endregion

        #region 微信退款查询
        /// <summary>
        /// 微信退款查询
        /// </summary>
        /// <param name="refund_id">微信退款单号</param>
        /// <param name="out_refund_no">商户退款单号</param>
        /// <param name="transaction_id">微信订单号</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <returns></returns>
        public JsonResult RechargeRefundStatusWeChat(string refund_id, string out_refund_no, string transaction_id, string out_trade_no)
        {
            WxPayData data = new WxPayData();
            if (string.IsNullOrEmpty(refund_id) && string.IsNullOrEmpty(out_refund_no) && string.IsNullOrEmpty(transaction_id) && string.IsNullOrEmpty(out_trade_no))
            {
                return Json(new { success = false, message = "微信退款单号、商户退款单号、微信订单号与商户订单号不能全部为空" });
            }
            if (!string.IsNullOrEmpty(refund_id))
            {
                data.SetValue("refund_id", refund_id);/*微信退款单号*/
            }
            if (!string.IsNullOrEmpty(out_refund_no))
            {
                data.SetValue("out_refund_no", out_refund_no);/*商户退款单号*/
            }
            if (!string.IsNullOrEmpty(transaction_id))
            {
                data.SetValue("transaction_id", transaction_id);/*微信订单号*/
            }
            if (!string.IsNullOrEmpty(out_trade_no))
            {
                data.SetValue("out_trade_no", out_trade_no);/*商户订单号*/
            }
            WxPayData payData = WxPayApi.RefundQuery(data);
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
                var refundStatus = payData.GetValue("refund_status_$n").ToString();/*退款状态*/
                string message;
                int code;
                switch (refundStatus)
                {
                    case "SUCCESS": code = 2; message = "退款成功"; break;
                    case "REFUNDCLOSE": code = 0; message = "退款关闭"; break;
                    case "PROCESSING": code = 1; message = "退款处理中"; break;
                    case "CHANGE": code = 0; message = "退款异常"; break;
                    default: code = 99; message = "未知结果"; break;
                }
                return Json(new { success = true, code = code, message = message });
            }
            else
            {
                return Json(new { success = false, message = "未知的异常" });
            }

        }
        #endregion

        /*
         * 微信App支付
        */

        #region 微信支付App
        /// <summary>
        /// 微信支付App
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="productId">商品</param>
        /// <param name="fee">金额</param>
        /// <param name="ruleId">规则</param>
        /// <param name="rechargeType">充值类型 1定额/2不定额</param>
        /// <returns></returns>
        public JsonResult RechargeAppPayWeChat(string userId, string entId, string productId, int fee, string ruleId, int rechargeType)
        {
            string orderNo = "";
            object data = new object();
            bool flag;
            //List<RechargeRule> rechargeRules = null;
            string message;
            try
            {
                //商户订单号
                var out_trade_no = WxPayApi.GenerateOutTradeNo();
                if (!string.IsNullOrWhiteSpace(productId))
                {
                    /*生成订单*/
                    RechargeDal rechargeDal = new RechargeDal();
                    bool bl = rechargeDal.IncreaseRecharge(userId, entId, out_trade_no, productId, fee, "", "微信", 1, WxPayConfig.GetConfig().GetAppID(), ruleId, rechargeType, out string msg);
                    if (bl)
                    {
                        flag = bl;
                        orderNo = out_trade_no;
                        WxPayData payData = GetPayWeChat(productId, out_trade_no, fee, out string code, out string mag);
                        if (code == "SUCCESS")
                        {
                            data = payData.ToXml();
                            message = "商户充值订单生成成功";
                        }
                        else
                        {
                            data = "";
                            message = mag;
                        }

                        //data = new { appid = payData.GetValue("appid").ToString(), partnerid = payData.GetValue("partnerid").ToString(), prepayid = payData.GetValue("prepayid").ToString(), noncestr = payData.GetValue("noncestr").ToString(), package = payData.GetValue("package").ToString(), timestamp = payData.GetValue("timestamp").ToString(), sign = payData.GetValue("sign").ToString() };
                        //if (!string.IsNullOrEmpty(ruleId))
                        //{
                        //    rechargeRules = rechargeDal.RechargeRules(ruleId);
                        //}

                    }
                    else
                    {
                        flag = bl;
                        message = "商户充值订单生成失败,请重新发起充值请求";
                    }
                }
                else
                {
                    flag = false;
                    message = "订单商品不能为空";
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/RechargeAppPayWeChat", ex.Message.ToString());
                flag = false;
                message = ex.Message.ToString();
            }
            return Json(new { success = flag, message = message, data = data, orderNo = orderNo, fee = fee/*, rule = rechargeRules*/ });
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
                WxPayData data = new WxPayData();
                data.SetValue("body", "商城充值");//商品描述
                data.SetValue("attach", "附加信息,用于后台或者存入数据库,做自己的判断");//附加数据
                data.SetValue("out_trade_no", out_trade_no);//随机字符串
                data.SetValue("total_fee", fee);//总金额
                data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
                data.SetValue("time_expire", DateTime.Now.AddMinutes(30).ToString("yyyyMMddHHmmss"));//交易结束时间
                data.SetValue("goods_tag", "商品的备忘,可以自定义");//商品标记
                data.SetValue("trade_type", "NATIVE");//交易类型
                data.SetValue("product_id", productId);//商品ID
                data.SetValue("notify_url", webUrl + "/Recharge/RechargeResultWeChat");/*异步通知url*/
                WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
                                                               //生成带签名的客户端支付信息
                WxPayData payData = new WxPayData();
                if (result.GetValue("return_code").ToString() == "SUCCESS" && result.GetValue("result_code").ToString() == "SUCCESS")
                {
                    string prepayid = result.GetValue("prepay_id").ToString();//预支付交易会话标识

                    payData.SetValue("appid", WxPayConfig.GetConfig().GetAppID());//公众账号ID
                    payData.SetValue("partnerid", WxPayConfig.GetConfig().GetMchID());//商户号
                    payData.SetValue("prepayid", prepayid);//预支付交易会话标识
                    payData.SetValue("noncestr", WxPayApi.GenerateNonceStr());//随机串
                    payData.SetValue("package", "Sign=WXPay");
                    payData.SetValue("timestamp", WxPayApi.GenerateTimeStamp());//时间戳
                    payData.SetValue("sign_type", WxPayData.SIGN_TYPE_HMAC_SHA256);//签名类型   
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
                LogQueue.Write(LogType.Error, "Recharge/GetPayWeChat：", ex.ToString());
                code = "FAIL";
                msg = ex.ToString();
                return null;
            }
        }
        #endregion

        /*
         * 支付宝扫码支付
        */

        #region 支付宝扫码支付---已弃用
        ///// <summary>
        ///// 生成订单返回支付宝扫码Url、商户单号
        ///// </summary>
        ///// <param name="userId">用户</param>
        ///// <param name="entId">机构</param>
        ///// <param name="productId">订单商品</param>
        ///// <param name="fee">订单金额</param>
        ///// <returns></returns>
        //public JsonResult RechargePayUrlAlipay(string userId, string entId, string productId, decimal fee)
        //{
        //    try
        //    {
        //        var out_trade_no = WxPayApi.GenerateOutTradeNo();
        //        string payUrl = GetPayUrlAlipay(productId, out_trade_no, fee);
        //        if (!string.IsNullOrEmpty(productId))
        //        {
        //            /*生成订单*/
        //            RechargeDal rechargeDal = new RechargeDal();
        //            bool bl = rechargeDal.IncreaseRecharge(userId, entId, out_trade_no, productId, fee, "", "支付宝", out string message);
        //            if (bl)
        //            {
        //                return Json(new { success = true, message = message, Url = payUrl, orderNo = out_trade_no });
        //            }
        //            else
        //            {
        //                return Json(new { success = false, message = message });
        //            }
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "订单商品不能为空！" });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LogQueue.Write(LogType.Error, "Recharge/RechargePayUrlAlipay", ex.Message.ToString());
        //        return Json(new { success = false, message = "支付宝充值失败！" });
        //    }
        //}

        ///// <summary>
        ///// 获取支付宝扫码Url
        ///// </summary>
        ///// <param name="productId">商品</param>
        ///// <param name="out_trade_no">商户订单号</param>
        ///// <param name="fee">订单金额</param>
        ///// <returns></returns>
        //public string GetPayUrlAlipay(string productId,string out_trade_no,decimal fee)
        //{
        //    IAopClient client = GetAlipayClient();
        //    AlipayTradeWapPayModel model = new AlipayTradeWapPayModel();
        //    model.OutTradeNo = out_trade_no;
        //    model.Body = "商城充值";
        //    model.TotalAmount = fee.ToString();
        //    model.Subject = "商城充值";
        //    model.TimeoutExpress = "2h";
        //    model.ProductCode = "FACE_TO_FACE_PAYMENT";

        //    string url="";
        //    try
        //    {
        //        AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();
        //        //在公共参数中设置回跳和通知地址
        //        request.SetReturnUrl(AlipayConfigHelper.return_url);
        //        request.SetNotifyUrl(AlipayConfigHelper.notify_url);
        //        request.SetBizModel(model);
        //        AlipayTradePrecreateResponse response = client.Execute(request);
        //        url = response.QrCode;
        //        //string a= "{\"alipay_trade_precreate_response\": {\"code\": \"10000\",\"msg\": \"Success\",\"out_trade_no\": \"050823252214942\",\"qr_code\": \"https://qr.alipay.com/bax04344igml342yrywa004f\"},\"sign\": \"DIeOExAIIF4RhPsHLp4tZtQP15SU4k/CUsJ//Z782HOpTe/BYMK/YjiPv9I2JF6GHSIU+qotprO/unkwf1qN41d9g4qErx7oLpRLxGZgSOMo4js8wBcauN4osXmPRfmDvlCo6cO9s4rj3976223h/8kzBlRp3lpgOnD+uxyQrqqEsb6ddQosbEiqYW3XADu3X9GRTuDaJFsk4tkc1Uly6Mx17SXJs7c49CA0ObLJZ5RsV8Wd55qDWc93FqOKClNikOnFc0QwvAPuLXyW4DFpfdqk4LlegGT33jdfzTuPFoXXnab/P6/dRRGd4ZW82lVAx4pjGhia4Ad+Wb+sNf5QVw==\"}";
        //        //JObject obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(response.Body.ToString());
        //        //url =obj["alipay_trade_precreate_response"]["qr_code"].ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        LogQueue.Write(LogType.Error, "Recharge/GetPayUrlAlipay", ex.Message.ToString());
        //    }
        //    return url;
        //}

        ///// <summary>
        ///// 支付宝同步回调
        ///// 功能：页面跳转同步通知页面
        ///// </summary>
        //public ActionResult RechargeSyncResultAlipay()
        //{
        //    // http://localhost:10231/College/ReturnUrl?
        //    // total_amount =0.01
        //    // ×tamp=2017-07-24+17%3A57%3A08
        //    // &sign=********
        //    // &trade_no=2017072421001004930200313969
        //    // &sign_type=RSA2
        //    // &auth_app_id=2016080500169628
        //    // &charset=utf-8
        //    // &seller_id=2088102169996595
        //    // &method=alipay.trade.page.pay.return
        //    // &app_id=2016080500169628
        //    // &out_trade_no=GM201707241756580000000001
        //    // &version=1.0 
        //    //将同步通知中收到的所有参数都存放到map中
        //    IDictionary<string, string> map = GetRequestGet();
        //    if (map.Count > 0) //判断是否有带返回参数
        //    {
        //        try
        //        {
        //            //支付宝的公钥
        //            string alipayPublicKey = AlipayConfigHelper.alipayPublicKey;
        //            string signType = AlipayConfigHelper.signType;
        //            string charset = AlipayConfigHelper.charset;
        //            bool keyFromFile = false;
        //            // 获取支付宝GET过来反馈信息  
        //            bool verify_result = AlipaySignature.RSACheckV1(map, alipayPublicKey, charset, signType, keyFromFile);
        //            if (verify_result)
        //            {
        //                // 验证成功 
        //                //Response.Redirect("/College/Index?id=1");
        //                return Content("ok");
        //            }
        //            else
        //            {
        //                return Content("验证失败");
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw new Exception(e.Message);
        //        }
        //    }
        //    else
        //    {
        //        return Content("无返回参数");
        //    }
        //}

        ///// <summary>
        ///// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        ///// </summary>
        ///// <returns>request回来的信息组成的数组</returns>
        //public IDictionary<string, string> GetRequestGet()
        //{
        //    int i = 0;
        //    IDictionary<string, string> sArray = new Dictionary<string, string>();
        //    NameValueCollection coll;
        //    //Load Form variables into NameValueCollection variable.
        //    coll = Request.QueryString;

        //    // Get names of all forms into a string array.
        //    String[] requestItem = coll.AllKeys;

        //    for (i = 0; i < requestItem.Length; i++)
        //    {
        //        sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
        //    }
        //    return sArray;
        //}

        ///// <summary>
        ///// 支付异步回调
        ///// 功能：服务器异步通知页面
        ///// 创建该页面文件时，请留心该页面文件中无任何HTML代码及空格
        ///// 该页面不能在本机电脑测试，请到服务器上做测试。请确保外部可以访问该页面。
        ///// 该页面调试工具请使用写文本函数logResult。
        ///// 如果没有收到该页面返回的 success 信息，支付宝会在24小时内按一定的时间策略重发通知
        ///// GmkCollege
        ///// </summary>
        //public ActionResult RechargeAsynchResultAlipay()
        //{
        //    // 获取支付宝Post过来反馈信息
        //    IDictionary<string, string> map = GetRequestPost();
        //    if (map.Count > 0) //判断是否有带返回参数
        //    {
        //        try
        //        {
        //            //支付宝的公钥
        //            string alipayPublicKey = AlipayConfigHelper.alipayPublicKey;
        //            string signType = AlipayConfigHelper.signType;
        //            string charset = AlipayConfigHelper.charset;
        //            bool keyFromFile = false;

        //            bool verify_result = AlipaySignature.RSACheckV1(map, alipayPublicKey, charset, signType, keyFromFile);
        //            // 验签成功后，按照支付结果异步通知中的描述，对支付结果中的业务内容进行二次校验，校验成功后在response中返回success并继续商户自身业务处理，校验失败返回failure
        //            if (verify_result)
        //            {
        //                //商户订单号
        //                string out_trade_no = map["out_trade_no"];
        //                //支付宝交易号
        //                string trade_no = map["trade_no"];
        //                //交易创建时间
        //                string gmt_create = map["gmt_create"];
        //                //交易付款时间
        //                string gmt_payment = map["gmt_payment"];
        //                //通知时间
        //                string notify_time = map["notify_time"];
        //                //通知类型  trade_status_sync
        //                string notify_type = map["notify_type"];
        //                //通知校验ID
        //                string notify_id = map["notify_id"];
        //                //开发者的app_id
        //                string app_id = map["app_id"];
        //                //卖家支付宝用户号
        //                string seller_id = map["seller_id"];
        //                //买家支付宝用户号
        //                string buyer_id = map["buyer_id"];
        //                //实收金额
        //                string receipt_amount = map["receipt_amount"];
        //                //交易状态
        //                //交易状态TRADE_FINISHED的通知触发条件是商户签约的产品不支持退款功能的前提下，买家付款成功；
        //                //或者，商户签约的产品支持退款功能的前提下，交易已经成功并且已经超过可退款期限
        //                //状态TRADE_SUCCESS的通知触发条件是商户签约的产品支持退款功能的前提下，买家付款成功
        //                if (map["trade_status"] == "TRADE_FINISHED" || map["trade_status"] == "TRADE_SUCCESS")
        //                {
        //                    /*查询充值订单*/
        //                    RechargeDal rechargeDal = new RechargeDal();
        //                    List<RechargeOrders> rechargeOrders = rechargeDal.InquireRecharge(out_trade_no);
        //                    if (rechargeOrders[0].Status == 2)//判断商城充值订单状态
        //                    {
        //                        return Content("success");
        //                    }
        //                    else
        //                    {
        //                        bool bl = rechargeDal.UpdateRecharge(out_trade_no, trade_no, 2, out string message, 0, "");
        //                        if (bl)
        //                        {
        //                            return Content("success");
        //                        }
        //                        else
        //                        {
        //                            LogQueue.Write(LogType.Error, "支付宝支付回调", $"商城充值订单{out_trade_no}回写失败！");
        //                            return Content("支付信息回写失败!");
        //                        }
        //                    }
        //                }
        //                //else if(map["trade_status"] == "WAIT_BUYER_PAY")//交易创建，等待买家付款
        //                //{
        //                //    //轮询订单结果
        //                //    //根据业务需要，选择是否新起线程进行轮询
        //                //    ParameterizedThreadStart parStart = new ParameterizedThreadStart(LoopQuery);
        //                //    Thread myThread = new Thread(parStart);
        //                //    LoopParameter parameter = new LoopParameter { Out_trade_no = out_trade_no, Trade_no = trade_no };
        //                //    myThread.Start(parameter);
        //                //    //return Content("");
        //                //}
        //                else
        //                {
        //                    return Content("未支付成功!");
        //                }
        //            }
        //            // 验签失败则记录异常日志，并在response中返回failure.
        //            else
        //            {
        //                //Response.Write("验证失败");
        //                return Content("验证失败");
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw new Exception(e.Message);
        //        }
        //    }
        //    else
        //    {
        //        //Response.Write("无返回参数");
        //        return Content("无返回参数");
        //    }
        //}

        ///// <summary>
        ///// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        ///// </summary>
        ///// <returns>request回来的信息组成的数组</returns>
        //public IDictionary<string, string> GetRequestPost()
        //{
        //    int i = 0;
        //    IDictionary<string, string> sArray = new Dictionary<string, string>();
        //    NameValueCollection coll;
        //    coll = Request.Form;

        //    // Get names of all forms into a string array.
        //    String[] requestItem = coll.AllKeys;

        //    for (i = 0; i < requestItem.Length; i++)
        //    {
        //        sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
        //    }
        //    return sArray;
        //}



        ///// <summary>
        ///// 线程参数
        ///// </summary>
        //public class LoopParameter
        //{
        //    public string Out_trade_no { get; set; }
        //    public string Trade_no { get; set; }

        //}

        ///// <summary>
        ///// 轮询支付结果
        ///// </summary>
        ///// <param name="obj"></param>
        //public void LoopQuery(object lp)
        //{
        //    IAopClient client = GetAlipayClient();
        //    AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
        //    int count = 10;
        //    int interval = 3000;
        //    LoopParameter parameter = (LoopParameter)lp;
        //    AlipayTradeQueryResponse response=null;
        //    for (int i = 1; i <= count; i++)
        //    {
        //        Thread.Sleep(interval);

        //        request.BizContent = "{" +
        //        "\"out_trade_no\":\""+ parameter.Out_trade_no + "\"," +
        //        "\"trade_no\":\""+ parameter.Trade_no+"\"" +
        //        "  }";

        //        response = client.Execute(request);
        //        if (response.Code== "10000")
        //        {
        //            //JObject obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(response.Body.ToString());
        //            //var status = obj["alipay_trade_query_response"]["trade_status"].ToString();
        //            //if (status == "TRADE_SUCCESS" || status == "TRADE_FINISHED")
        //            //{
        //            //    //支付成功处理
        //            //    DoSuccessProcess(queryResult);
        //            //    return;
        //            //}
        //            if (response.TradeStatus == "TRADE_FINISHED" || response.TradeStatus == "TRADE_SUCCESS" || response.TradeStatus == "TRADE_CLOSED")
        //            {
        //                break;
        //            }
        //        }

        //    }
        //    ////支付失败处理
        //    //DoFailedProcess(queryResult);
        //    //未付款交易超时或等待超时。
        //    if (response.Code != "10000" || response.TradeStatus == "TRADE_CLOSED" || response.TradeStatus == "WAIT_BUYER_PAY")
        //    {
        //        //撤销订单
        //        //StringBuilder param = new StringBuilder();
        //        //param.Append("{\"out_trade_no\":\"" + payResponse.OutTradeNo + "\"}");
        //        //biz_content = param.ToString();
        //        //Cancel(biz_content);
        //    }

        //    return response;
        //}
        #endregion

        #region 支付宝初始化配置文件信息
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
        #endregion

        #region 支付宝扫码支付
        /// <summary>
        /// 支付宝扫码支付
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="productId">商品</param>
        /// <param name="fee">金额</param>
        /// <param name="ruleId">规则</param>
        /// <param name="rechargeType">充值类型 1定额/2不定额</param>
        /// <returns></returns>
        public JsonResult RechargePayUrlAlipay(string userId, string entId, string productId, decimal fee, string ruleId, int rechargeType)
        {
            string result = "", rQCode = "", orderNo = "";
            bool flag;
            var out_trade_no = WxPayApi.GenerateOutTradeNo();
            /*生成订单*/
            RechargeDal rechargeDal = new RechargeDal();
            bool bl = rechargeDal.IncreaseRecharge(userId, entId, out_trade_no, productId, fee, "", "支付宝", 1, AlipayConfigHelper.appId, ruleId, rechargeType, out string message);
            if (bl)
            {
                IAopClient client = GetAlipayClient();

                AlipayTradePrecreateModel model = new AlipayTradePrecreateModel
                {
                    OutTradeNo = out_trade_no,
                    Body = "商城充值",
                    TotalAmount = fee.ToString(),
                    Subject = "商城充值",
                    TimeoutExpress = "30m",
                    ProductCode = "FACE_TO_FACE_PAYMENT"
                };
                AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();
                request.SetBizModel(model);

                AlipayTradePrecreateResponse response = client.Execute(request);


                if (response != null)
                {
                    switch (response.Code)
                    {
                        case ResultCode.SUCCESS:
                            flag = true;
                            rQCode = Convert.ToBase64String(DoWaitProcess(response.QrCode, response.OutTradeNo, "Alipay").ToArray());
                            orderNo = response.OutTradeNo;
                            result = "商户充值订单生成成功";
                            break;
                        case ResultCode.ERROR:
                            if (response == null)
                            {
                                flag = false;
                                result = "配置或网络异常，请检查后重试";
                            }
                            else
                            {
                                flag = false;
                                result = "系统异常，请更新外部订单后重新发起请求";
                            }
                            break;
                        default:
                            flag = false;
                            result = response.Body;
                            break;
                    }
                }
                else
                {
                    if (response == null)
                    {
                        flag = false;
                        result = "配置或网络异常，请检查后重试";
                    }
                    else
                    {
                        flag = false;
                        result = "系统异常，请更新外部订单后重新发起请求";
                    }
                }
            }
            else
            {
                flag = bl;
                result = "商户充值订单生成失败,请更新外部订单后重新发起请求";
            }

            return Json(new { success = flag, message = result, rQCode = rQCode, orderNo = orderNo, fee = fee/*, rule = rechargeRules*/ });
        }


        #endregion

        #region 支付宝轮询
        /// <summary>
        /// 轮询
        /// </summary>
        /// <param name="o">订单号</param>
        public void LoopQueryAlipay(object o)
        {
            try
            {
                //LogQueue.Write(LogType.Debug, "", "进入轮询方法");
                int count = 40;
                int interval = 3000;
                string out_trade_no = o.ToString();
                IAopClient client = GetAlipayClient();
                AlipayTradeQueryModel model = new AlipayTradeQueryModel();
                model.OutTradeNo = out_trade_no;
                model.TradeNo = "";
                AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
                request.SetBizModel(model);
                AlipayTradeQueryResponse response = new AlipayTradeQueryResponse();
                for (int i = 1; i <= count; i++)
                {
                    //LogQueue.Write(LogType.Debug, "", $"第{i}次轮询");
                    Thread.Sleep(interval);
                    response = client.Execute(request);
                    if (response != null)
                    {
                        if (response.Code == ResultCode.SUCCESS && (response.TradeStatus == "TRADE_SUCCESS" || response.TradeStatus == "TRADE_FINISHED"))
                        {
                            DoSuccessProcessAlipay(response);
                            return;
                        }
                    }
                }
                DoFailedProcessAlipay(response);
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/LoopQueryAlipay", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 请添加支付宝支付成功后的处理
        /// </summary>
        private void DoSuccessProcessAlipay(AlipayTradeQueryResponse response)
        {
            //LogQueue.Write(LogType.Debug, "", $"支付成功");
            //支付成功，请更新相应单据
            /*查询充值订单*/
            RechargeDal rechargeDal = new RechargeDal();
            List<RechargeOrders> rechargeOrders = rechargeDal.InquireRecharge(response.OutTradeNo);
            if (rechargeOrders[0].Status == 1)//判断商城充值订单状态 状态0【失败，订单关闭】1【已下单】2【订单交易成功】
            {
                bool bl = rechargeDal.UpdateRecharge(response.OutTradeNo, response.TradeNo, 2, 0, "", "", out string message);
                if (!bl)
                {
                    LogQueue.Write(LogType.Error, "支付宝轮询-支付成功", $"商城充值订单{response.OutTradeNo}回写失败！");
                }
            }
        }

        /// <summary>
        /// 请添加支付宝支付失败后的处理
        /// </summary>
        private void DoFailedProcessAlipay(AlipayTradeQueryResponse response)
        {
            //LogQueue.Write(LogType.Debug, "", $"支付失败");
            //支付失败，请更新相应单据
            IAopClient client = GetAlipayClient();
            AlipayTradeCancelModel model = new AlipayTradeCancelModel();
            model.OutTradeNo = response.OutTradeNo;
            model.TradeNo = response.TradeNo;
            AlipayTradeCancelRequest request = new AlipayTradeCancelRequest();
            request.SetBizModel(model);
            AlipayTradeCancelResponse cancelResponse = client.Execute(request);
            if (cancelResponse.Code == ResultCode.SUCCESS)
            {
                RechargeDal rechargeDal = new RechargeDal();
                bool bl = rechargeDal.UpdateRecharge(cancelResponse.OutTradeNo, cancelResponse.TradeNo, 0, 0, "", "", out string message);
                if (!bl)
                {
                    LogQueue.Write(LogType.Error, "支付宝轮询-支付失败", $"商城充值订单{cancelResponse.OutTradeNo},支付失败，失败状态回写失败！");
                }
            }
            else
            {
                LogQueue.Write(LogType.Error, "支付宝轮询-支付失败", $"商城充值订单{cancelResponse.OutTradeNo}，支付失败，撤销原订单失败！");
            }
        }
        #endregion

        #region 支付宝退款
        /// <summary>
        /// 支付宝退款
        /// 当交易发生之后一段时间内，由于买家或者卖家的原因需要退款时，卖家可以通过退款接口将支付款退还给买家，支付宝将在收到退款请求并且验证成功之后，按照退款规则将支付款按原路退到买家帐号上。 
        /// 交易超过约定时间（签约时设置的可退款时间）的订单无法进行退款 
        /// 支付宝退款支持单笔交易分多次退款，多次退款需要提交原支付订单的商户订单号和设置不同的退款单号。
        /// 一笔退款失败后重新提交，要采用原来的退款单号。
        /// 总退款金额不能超过用户实际支付金额
        /// </summary>
        /// <returns></returns>
        public ActionResult RechargeRefundAlipay(string userId, string entId, string productId, string transactionId, string outTradeNo, /*decimal totalFee,*/ decimal refundFee)
        {
            string outRefundNo = WxPayApi.GenerateOutTradeNo();
            /*生成退款订单*/
            RechargeDal rechargeDal = new RechargeDal();
            bool bl = rechargeDal.IncreaseRecharge(userId, entId, outRefundNo, productId, refundFee, "", "支付宝", 0, AlipayConfigHelper.appId, "", 0, out string msg);
            if (bl)
            {
                IAopClient client = GetAlipayClient();
                AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
                AlipayTradeRefundModel model = new AlipayTradeRefundModel();
                model.OutTradeNo = outTradeNo;
                model.TradeNo = transactionId;
                model.RefundAmount = refundFee.ToString();
                model.RefundReason = "正常退款";
                request.SetBizModel(model);
                AlipayTradeRefundResponse response = client.Execute(request);
                string Info = response.Body;

                if (response.Code == ResultCode.SUCCESS)
                {
                    /*回写退款订单*/
                    RechargeDal rechargeDal1 = new RechargeDal();
                    bool flag = rechargeDal1.UpdateRecharge(response.OutTradeNo, response.TradeNo, 2, refundFee, transactionId, outTradeNo, out string message);
                    if (!flag)
                    {
                        LogQueue.Write(LogType.Error, "支付宝退款", $"商城退款订单{response.OutTradeNo} 退款成功，成功状态回写失败！");
                    }
                    return Json(new { success = true, message = "退款成功" });
                }
                else
                {
                    /*回写退款订单*/
                    RechargeDal rechargeDal1 = new RechargeDal();
                    bool flag = rechargeDal1.UpdateRecharge(response.OutTradeNo, response.TradeNo, 0, refundFee, transactionId, outTradeNo, out string message);
                    if (!flag)
                    {
                        LogQueue.Write(LogType.Error, "支付宝退款", $"商城退款订单{response.OutTradeNo} 退款成功，成功状态回写失败！");
                    }
                    return Json(new { success = true, message = response.Msg });
                }
            }
            else
            {
                return Json(new { success = true, message = "退款订单生成失败" });
            }
        }
        #endregion

        #region 支付宝关闭订单
        /// <summary>
        /// 支付宝关闭订单
        /// 用于交易创建后，用户在一定时间内未进行支付，可调用该接口直接将未付款的交易进行关闭。
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult RechargeCloseAlipay(string orderNo)
        {
            //查询要关闭的订单信息
            RechargeDal recharge = new RechargeDal();
            List<RechargeOrders> recharges = recharge.InquireRecharge(orderNo);
            IAopClient client = GetAlipayClient();
            AlipayTradeCloseRequest request = new AlipayTradeCloseRequest();
            AlipayTradeCloseModel model = new AlipayTradeCloseModel();
            model.OutTradeNo = recharges[0].OrderNo;
            model.TradeNo = recharges[0].TransactionId;
            request.SetBizModel(model);
            AlipayTradeCloseResponse response = client.Execute(request);
            string Info = response.Body;

            if (response.Code == "10000")
            {
                /*回写充值订单*/
                RechargeDal rechargeDal = new RechargeDal();
                bool bl = rechargeDal.UpdateRecharge(response.OutTradeNo, response.TradeNo, 0, 0, "", "", out string message);
                if (!bl)
                {
                    LogQueue.Write(LogType.Error, "支付宝关闭订单", $"商城充值订单{response.OutTradeNo} 关闭成功，成功状态回写失败！");
                }
                return Json(new { success = true, message = "订单关闭成功" });
            }
            else
            {
                return Json(new { success = true, message = response.Msg });
            }
        }
        #endregion

        #region 支付宝撤销订单
        /// <summary>
        /// 支付宝撤销订单
        /// 支付交易返回失败或支付系统超时，调用该接口撤销交易。
        /// 如果此订单用户支付失败，支付宝系统会将此订单关闭；如果用户支付成功，支付宝系统会将此订单资金退还给用户。 
        /// 注意：只有发生支付系统超时或者支付结果未知时可调用撤销，其他正常支付的单如需实现相同功能请调用申请退款API。
        /// 提交支付交易后调用【查询订单API】，没有明确的支付结果再调用【撤销订单API】。
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public ActionResult RechargeCancelAlipay(string OrderNo)
        {
            //查询要撤销的订单信息
            RechargeDal recharge = new RechargeDal();
            List<RechargeOrders> recharges = recharge.InquireRecharge(OrderNo);
            IAopClient client = GetAlipayClient();
            AlipayTradeCancelModel model = new AlipayTradeCancelModel();
            model.OutTradeNo = recharges[0].OrderNo;
            model.TradeNo = recharges[0].TransactionId;
            AlipayTradeCancelRequest request = new AlipayTradeCancelRequest();
            request.SetBizModel(model);
            AlipayTradeCancelResponse response = client.Execute(request);
            if (response.Code == "10000")
            {
                return Json(new { success = true, message = "订单撤销成功" });
            }
            else
            {
                return Json(new { success = true, message = response.Msg });
            }
        }
        #endregion

        #region 支付宝查询订单
        /// <summary>
        /// 支付宝查询订单
        /// 该接口提供所有支付宝支付订单的查询，商户可以通过该接口主动查询订单状态，完成下一步的业务逻辑。 
        /// 需要调用查询接口的情况： 当商户后台、网络、服务器等出现异常，商户系统最终未接收到支付通知； 
        /// 调用支付接口后，返回系统错误或未知交易状态情况； 
        /// 调用alipay.trade.pay，返回INPROCESS的状态； 
        /// 调用alipay.trade.cancel之前，需确认支付状态；
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult RechargeQueryAlipay(string orderNo)
        {
            //查询要撤销的订单信息
            RechargeDal recharge = new RechargeDal();
            List<RechargeOrders> recharges = recharge.InquireRecharge(orderNo);

            IAopClient client = GetAlipayClient();
            AlipayTradeQueryModel model = new AlipayTradeQueryModel();
            model.OutTradeNo = recharges[0].OrderNo;
            model.TradeNo = recharges[0].TransactionId;
            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            request.SetBizModel(model);
            AlipayTradeQueryResponse response = client.Execute(request);
            string message;
            int code;
            if (response.Code == "10000")
            {
                switch (response.TradeStatus)
                {
                    case "WAIT_BUYER_PAY": code = 1; message = "交易创建，等待买家付款"; break;
                    case "TRADE_CLOSED": code = 0; message = "未付款交易超时关闭，或支付完成后全额退款"; break;
                    case "TRADE_SUCCESS": code = 2; message = "交易支付成功"; break;
                    case "TRADE_FINISHED": code = 2; message = "交易结束，不可退款"; break;
                    default: code = 99; message = "未知状态"; break;
                }
                return Json(new { success = true, code = response.Code, status = code, msg = message, subMsg = response.SubMsg });
            }
            else if (response.Code == "40004")
            {
                switch (response.SubCode)
                {
                    case "ACQ.SYSTEM_ERROR": code = 0; message = "系统错误,请联系商户确认"; break;
                    case "ACQ.INVALID_PARAMETER": code = 0; message = "参数无效,请联系商户确认"; break;
                    case "ACQ.TRADE_NOT_EXIST": code = 1; message = "交易不存在或已撤销,若要继续请重新发起支付"; break;
                    default: code = 99; message = "未知异常"; break;
                }
                return Json(new { success = true, code = response.Code, status = code, msg = message, subMsg = response.SubMsg });
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
                /*返回status=0表示失效*/
                LogQueue.Write(LogType.Error, "", $"网关返回码:{response.Code},网关返回码描述:{response.Msg},业务返回码:{response.SubCode},业务返回码描述:{response.SubMsg}");
                return Json(new { success = false, code = response.Code, status = 0, msg = message, subMsg = response.SubMsg });
            }
        }
        #endregion

        /*
         * 支付宝App支付
        */
        #region 支付宝App
        /// <summary>
        /// 支付宝App
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="entId">机构Id</param>
        /// <param name="productId">商品Id</param>
        /// <param name="fee">金额</param>
        /// <param name="ruleId">规则</param>
        /// <param name="rechargeType">充值类型 1定额/2不定额</param>
        /// <returns></returns>
        public JsonResult RechargeAppPayAlipay(string userId, string entId, string productId, decimal fee, string ruleId, int rechargeType)
        {
            string result = "", orderNo = "";
            object data = new object();
            bool flag;
            //List<RechargeRule> rechargeRules=null;
            var outTradeNo = WxPayApi.GenerateOutTradeNo();
            orderNo = outTradeNo;
            /*生成订单*/
            RechargeDal rechargeDal = new RechargeDal();
            bool bl = rechargeDal.IncreaseRecharge(userId, entId, outTradeNo, productId, fee, "", "支付宝", 1, AlipayConfigHelper.appId, ruleId, rechargeType, out string message);
            if (bl)
            {
                IAopClient client = GetAlipayClient();
                AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
                model.OutTradeNo = outTradeNo;
                model.Body = "商城充值";
                model.TotalAmount = fee.ToString();
                model.Subject = "商城充值";
                model.TimeoutExpress = "30m";
                model.ProductCode = "QUICK_MSECURITY_PAY";
                string webUrl = BaseConfiguration.SercerIp.ToString();
                AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
                request.SetBizModel(model);
                //request.SetNotifyUrl(AlipayConfigHelper.notify_url);
                //request.SetNotifyUrl(webUrl+ "/Recharge/ReturnResultAppPayAlipay");
                request.SetNotifyUrl(webUrl + "/Recharge/NotifyResultAppPayAlipay");

                AlipayTradeAppPayResponse response = client.SdkExecute(request);
                if (!response.IsError)
                {
                    data = response.Body;
                    flag = true;
                    result = "签名后订单信息获取成功";
                }
                else
                {
                    flag = false;
                    result = response.SubMsg.ToString();
                }


                //if (!string.IsNullOrEmpty(ruleId))
                //{
                //    rechargeRules = rechargeDal.RechargeRules(ruleId);
                //}


            }
            else
            {
                flag = bl;
                result = "商户充值订单生成失败,请更新外部订单后重新发起请求";
            }
            return Json(new { success = flag, message = result, data = data, orderNo = orderNo, fee = fee/*,rule=rechargeRules*/ });
        }
        #endregion

        #region 支付宝App同步支付结果校验
        /// <summary>
        /// 支付宝App同步支付结果校验
        /// </summary>
        /// <param name="resultStatus">前台返回支付状态</param>
        /// <returns></returns>
        public JsonResult ReturnResultAppPayAlipay(string resultStatus)
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
                return Json(new { success = true, message = message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 支付宝App支付异步回调
        /// <summary>
        /// 支付宝App支付异步回调
        /// </summary>
        /// <returns></returns>
        public ActionResult NotifyResultAppPayAlipay()
        {
            // 获取支付宝Post过来反馈信息
            IDictionary<string, string> map = GetRequestPost();
            bool flag = AlipaySignature.RSACheckV1(map, AlipayConfigHelper.alipayPublicKey, AlipayConfigHelper.charset, AlipayConfigHelper.signType, false);
            if (flag)
            {
                //商户订单号
                string outTradeNo = map["out_trade_no"];
                //支付宝交易号
                string tradeNo = map["trade_no"];
                //交易创建时间
                string gmtCreate = map["gmt_create"];
                //交易付款时间
                string gmtPayment = map["gmt_payment"];
                //通知时间
                string notifyTime = map["notify_time"];
                //通知类型  trade_status_sync
                string notifyType = map["notify_type"];
                //通知校验ID
                string notifyId = map["notify_id"];
                //开发者的app_id
                string appId = map["app_id"];
                //卖家支付宝用户号
                string sellerId = map["seller_id"];
                //买家支付宝用户号
                string buyerId = map["buyer_id"];
                //实收金额
                string receiptAmount = map["receipt_amount"];
                //订单金额
                string totalAmount = map["total_amount"];


                RechargeDal rechargeDal = new RechargeDal();
                /*查询充值订单*/
                List<RechargeOrders> rechargeOrders = rechargeDal.InquireRecharge(outTradeNo);
                //商户需要验证该通知数据中的 out_trade_no 是否为商户系统中创建的订单号
                if (rechargeOrders != null)
                {
                    //LogQueue.Write(LogType.Debug, "支付宝App支付异步回调", "该订单是商户系统中创建的订单");
                    //判断 total_amount 是否确实为该订单的实际金额（即商户订单创建时的金额）；
                    if (rechargeOrders[0].Fee.ToString() == totalAmount)
                    {
                        //LogQueue.Write(LogType.Debug, "支付宝App支付异步回调", "该订单金额正常");
                        //校验通知中的 seller_id（或者 seller_email) 是否为 out_trade_no 这笔单据对应的操作方
                        //（有的时候，一个商户可能有多个 seller_id/seller_email）；
                        //当前客户拥有一个seller_id/seller_email，故生成订单时未保存至数据库，省略该步校验

                        //验证 app_id 是否为该商户本身。
                        if (appId == AlipayConfigHelper.appId)
                        {
                            //LogQueue.Write(LogType.Debug, "支付宝App支付异步回调", "app_id正确 为该商户本身");
                            //交易状态
                            //交易状态TRADE_FINISHED的通知触发条件是商户签约的产品不支持退款功能的前提下，买家付款成功；
                            //或者，商户签约的产品支持退款功能的前提下，交易已经成功并且已经超过可退款期限
                            //状态TRADE_SUCCESS的通知触发条件是商户签约的产品支持退款功能的前提下，买家付款成功
                            if (map["trade_status"] == "TRADE_FINISHED" || map["trade_status"] == "TRADE_SUCCESS")
                            {
                                //LogQueue.Write(LogType.Debug, "支付宝App支付异步回调", "付款成功");
                                if (rechargeOrders[0].Status == 1)//判断商城充值订单状态
                                {
                                    bool bl = rechargeDal.UpdateRecharge(outTradeNo, tradeNo, 2, 0, "", "", out string message);
                                    if (!bl)
                                    {
                                        LogQueue.Write(LogType.Error, "支付宝App支付回调", $"商城充值订单{outTradeNo}充值成功，状态回写失败！");
                                        return Content("支付信息回写失败!");
                                    }
                                    else
                                    {
                                        return Content("success");
                                    }
                                }
                                else
                                {
                                    return Content("success");
                                }
                            }
                            else if (map["trade_status"] == "TRADE_CLOSED")//未付款交易超时关闭，或支付完成后全额退款
                            {
                                //LogQueue.Write(LogType.Debug, "支付宝App支付异步回调", "未付款交易超时关闭");
                                bool bl = rechargeDal.UpdateRecharge(outTradeNo, tradeNo, 0, 0, "", "", out string message);
                                if (!bl)
                                {
                                    LogQueue.Write(LogType.Error, "支付宝App支付回调", $"商城充值订单{outTradeNo}充值失败，状态回写失败！");
                                    return Content("支付信息回写失败!");
                                }
                                else
                                {
                                    return Content("success");
                                }
                            }
                            else
                            {
                                LogQueue.Write(LogType.Error, "支付宝App支付异步回调", "付款交易失败");
                                return Content("failure");
                            }
                        }
                        else
                        {
                            LogQueue.Write(LogType.Error, "支付宝App支付异步回调", "app_id错误 不为该商户本身");
                            return Content("failure");
                        }
                    }
                    else
                    {
                        LogQueue.Write(LogType.Error, "支付宝App支付异步回调", "该订单金额不对");
                        return Content("failure");
                    }
                }
                else
                {
                    LogQueue.Write(LogType.Error, "支付宝App支付异步回调", "该订单不是商户系统中创建的订单");
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
            int i = 0;
            IDictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            coll = Request.Form;
            String[] requestItem = coll.AllKeys;
            string str = "";
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Params[requestItem[i]]);
                str += requestItem[i].ToString() + "=" + Request.Params[requestItem[i]].ToString() + "&";
            }
            //LogQueue.Write(LogType.Debug, "数组转字符串1：", str.ToString());
            string wwww = "";
            for (int j = 0; j < Request.Form.Count; j++)
            {
                if (Request.Form.Keys[j].ToString().Substring(0, 1) != "_")
                {
                    wwww += " " + Request.Form.Keys[j].ToString() + " = " + Request.Form[j].ToString();
                }
            }
            //LogQueue.Write(LogType.Debug, "数组转字符串2：", wwww.ToString());
            return sArray;
        }
        #endregion

        #region 返回二维码并启动轮询
        /// <summary>
        /// 返回二维码并启动轮询
        /// </summary>
        /// <param name="QrCode">二维码串</param>
        /// <param name="OutTradeNo">商户订单号</param>
        /// <param name="druge">微信调用/支付宝调用</param>
        private MemoryStream DoWaitProcess(string QrCode, string OutTradeNo, string druge)
        {
            ////轮询订单结果
            ////根据业务需要，选择是否新起线程进行轮询
            //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(LoopQuery);
            //Thread myThread = new Thread(ParStart);
            object o = OutTradeNo;
            //myThread.Start(o);
            //LogQueue.Write(LogType.Debug, "", "开始轮询");
            ////根据业务需要，选择是否线程池进行轮询ThreadPool
            //WaitCallback waitCallback = new WaitCallback(LoopQuery);
            ThreadPool.SetMaxThreads(4, 200);
            if (druge == "WeChat")
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoopQueryWechat), o);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoopQueryAlipay), o);
            }

            return CreateRQCode(QrCode);
        }
        #endregion

        #region 支付url生成二维码
        private MemoryStream CreateRQCode(string QrCode)
        {
            //显示出 preResponse.QrCode 对应的条码
            Bitmap bt;
            string enCodeString = QrCode;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeVersion = 8;
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
        #endregion

        #region 商城内部查询订单支付状态
        /// <summary>
        /// 充值订单支付状态查询(商城内部查询)
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public JsonResult RechargeStatusInterior(string orderNo)
        {
            try
            {
                /*查询充值订单*/
                RechargeDal rechargeDal = new RechargeDal();
                List<RechargeOrders> rechargeOrders = rechargeDal.InquireRecharge(orderNo);
                if (rechargeOrders == null)
                {
                    return Json(new { success = false, code = 0, message = "未查询到充值订单" });
                }
                else if (rechargeOrders[0].Status == 1)
                {
                    return Json(new { success = true, code = 1, message = "未完成支付，正在充值中" });
                }
                else if (rechargeOrders[0].Status == 2)
                {
                    return Json(new { success = true, code = 2, message = "支付成功" });
                }
                else if (rechargeOrders[0].Status == 0)
                {
                    return Json(new { success = true, code = 0, message = "支付失败，订单已关闭" });
                }
                else
                {
                    return Json(new { success = false, code = 0, message = "未知的异常" });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Recharge/RechargeStatus", ex.Message.ToString());
                return Json(new { success = false, code = 0, message = "充值状态查询失败！" });
            }
        }
        #endregion
    }
}