using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.AlipayPayAPI;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Controllers;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Models.Admin;
using Sk_B2BAPI.TianTaPay;
using Sk_B2BAPI.Tool;
using Sk_B2BAPI.WxPayAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Sk_B2BAPI.Admin.article.ashx
{
    /// <summary>
    /// ReturnJson 的摘要说明
    /// </summary>
    public class ReturnJson : IHttpHandler, IRequiresSessionState
    {
        [ValidateInput(false)]
        [HttpPost]
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string returnJson;
            try
            {
                Dt_User user = RoleFuns.IsLoginAdmin(context.Session["user"]);
                if (user != null)//登录检测，权限检测 context.Session["user"]
                {
                    string type = context.Request.QueryString["type"];//请求类型
                    string json = context.Request.QueryString["json"];//请求参数(json类型)
                    string proc = context.Request.QueryString["proc"];//存储过程名称
                    JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                    if (type != null)
                    {
                        List<string> rolestr = context.Session["role"] != null ? (List<string>)context.Session["role"] : null;
                        RoleFuns.SetAdminLog(user.username, obj["type"].ToString(), rolestr);
                        //if (/*rolestr == null || RoleFuns.SetAdminLog(user.username, obj["type"].ToString(), rolestr) == 0*/false)
                        //{
                        //    returnJson = JsonMethod.GetError(4, "抱歉您没有权限进入！");
                        //}
                    }
                    switch (type)
                    {
                        case "ReturnList":
                            returnJson = GetReturnJson(json, proc, user.userId, user.entId);
                            break;
                        case "ReturnNumber":
                            returnJson = GetReturnJsonInt(json, proc, user.userId, user.entId);
                            break;
                        case "ReturnListNumber":
                            returnJson = GetReturnDs(json, proc, user.userId, user.entId);
                            break;
                        case "QueryArticle":
                            if (user.entId != "superintendent")
                            {
                                returnJson = JsonMethod.GetError(1, "非顶级管理员！");
                                break;
                            }
                            returnJson = QueryArticle(json, proc, user.userId, user.entId);
                            break;
                        case "UpdateArticle":
                            returnJson = UpdateArticle(json, proc, user.userId, user.entId);
                            break;
                        case "QueryArticleMt":
                            returnJson = QueryArticleMt(json, proc, user.userId, user.entId);
                            break;
                        case "UpdateArticleList":
                            returnJson = UpdateArticleList(json, proc, user.userId, user.entId);
                            break;
                        default:
                            returnJson = JsonMethod.GetError(4, "参数错误！");
                            break;
                    }
                }
                else
                {
                    returnJson = JsonMethod.GetError(2, "登陆超时,请重新登陆！");
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                returnJson = JsonMethod.GetError(4, msg);
            }
            context.Response.Write(returnJson);
        }
        /// <summary>
        /// 获取数据返回数据列表
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string GetReturnJson(string json, string proc, string userId, string entid)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entid)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            string r_json;
            if (ds.Tables.Count >= 3)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    int recordCount = Convert.ToInt32(ds.Tables[2].Rows[0]["recordCount"].ToString().Trim());
                    int pageCount = Convert.ToInt32(ds.Tables[2].Rows[0]["pageCount"].ToString().Trim());
                    r_json = JsonMethod.GetDataTable(0, recordCount, pageCount, ds.Tables[1]);
                }
                else
                {
                    string error = "无数据";
                    r_json = JsonMethod.GetError(1, error);
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int recordCount = 0;
                    int pageCount = 0;
                    r_json = JsonMethod.GetDataTable(0, recordCount, pageCount, ds.Tables[0]);
                }
                else
                {
                    string error = "无数据";
                    r_json = JsonMethod.GetError(1, error);
                }
            }
            return r_json;
        }
        /// <summary>
        /// 更新数据目录
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <returns></returns>
        protected string GetReturnJsonInt(string json, string proc, string userId, string entid)
        {
            string r_json = "";
            var msg = "";
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entid)).ToArray();//动态解析json参数
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int n = 0, m = 0;
            if (obj["type"].ToString() == "Pc_SetReturnMoney")
            {
                var model = sql.RtDataTable($"select top 1 Amount from dt_order_refund where OrderNo='{obj["strwhere"]}'");
                if (model.Rows.Count > 0)
                {
                    if (obj["source"].ToString() == "甜塔")
                    {
                        if (RefundTianTa(obj["strwhere"].ToString(), entid, decimal.Parse(model.Rows[0]["Amount"].ToString()), out string message, true))
                        {
                            n++;
                        }
                        else
                        {
                            msg = message;
                        }
                    }
                    else if(obj["source"].ToString() == "官方")
                    {
                        if (OfficialRefund(obj["strwhere"].ToString(), entid, decimal.Parse(model.Rows[0]["Amount"].ToString()), out string message, true))
                        {
                            n++;
                        }
                        else
                        {
                            msg = message;
                        }
                    }
                    else
                    {
                        msg = "意外的支付方式（非甜塔、官方）";
                    }
                }
            }
            else if (obj["type"].ToString() == "Pc_SetReturnPro" && obj["uptype"].ToString() != "-1")
            {
                //var model = sql.RtDataTable($" select rr.OrderId,Convert(decimal(15,2),(ot.hshj*ot.shl*ot.Discount)) as Amount from dt_order_ReturnRemark rr join dt_users uu on uu.userid=rr.Userid join dt_orders oo on oo.order_no=rr.OrderId join dt_order_out ot on oo.order_no=ot.OrderNo and ot.Status=2 join dt_businessdoc bb on bb.businessid=uu.businessid join dt_order_goods og on og.order_no=oo.order_no and ot.articleId=og.article_id  and og.status not in(1,3) join dt_article_attribute aa on aa.article_id=og.article_id where rr.Id='{obj["strwhere"]}'");
                var model = sql.RtDataTable($" " +
                    $" select rr.OrderId,Convert(decimal(15,2),(sum(ot.hshj*ot.shl*ot.Discount))) as Amount " +
                    $" from dt_order_ReturnRemark rr " +
                    $" join dt_users uu on uu.userid=rr.Userid " +
                    $" join dt_orders oo on oo.order_no=rr.OrderId " +
                    $" join dt_order_out ot on oo.order_no=ot.OrderNo and ot.Status=2 	and rr.timestamp=ot.timestamp " +
                    $" join dt_businessdoc bb on bb.businessid=uu.businessid " +
                    $" join dt_order_goods og on og.order_no=oo.order_no and ot.articleId=og.article_id and rr.timestamp=og.timestamp and og.status not in(0,1,3,6,7)  " +
                    $" join dt_article_attribute aa on aa.article_id=og.article_id " +
                    $" where rr.Id='{obj["strwhere"]}'  " +
                    $" group by rr.OrderId ");
                if (model.Rows.Count > 0)
                {
                    if (obj["source"].ToString() == "甜塔")
                    {
                        if (RefundTianTa(model.Rows[0]["OrderId"].ToString(), entid, decimal.Parse(model.Rows[0]["Amount"].ToString()), out string message, true))
                        {
                            n++;
                        }
                        else
                        {
                            msg = message;
                        }
                    }
                    else if (obj["source"].ToString() == "官方")
                    {
                        if (OfficialRefund(model.Rows[0]["OrderId"].ToString(), entid, decimal.Parse(model.Rows[0]["Amount"].ToString()), out string message, true))
                        {
                            n++;
                        }
                        else
                        {
                            msg = message;
                        }
                    }
                    else
                    {
                        msg = "意外的支付方式（非甜塔、官方）";
                    }
                }
                else
                {
                    msg = "无退货中的商品";
                }
            }
            else if (obj["type"].ToString() == "ActiveRefundWriteBack")
            {
                var model = sql.RtDataTable($"select top 1 order_no,(real_amount-refund_fee) as amount from dt_orders where order_no='{obj["strwhere"]}'");
                if (model.Rows.Count > 0)
                {
                    if (obj["source"].ToString() == "甜塔")
                    {
                        if (RefundTianTa(obj["strwhere"].ToString(), entid, decimal.Parse(model.Rows[0]["amount"].ToString()), out string message, true))
                        {
                            n++;
                        }
                        else
                        {
                            msg = message;
                        }
                    }
                    else if (obj["source"].ToString() == "官方")
                    {
                        if (OfficialRefund(obj["strwhere"].ToString(), entid, decimal.Parse(model.Rows[0]["amount"].ToString()), out string message, true))
                        {
                            n++;
                        }
                        else
                        {
                            msg = message;
                        }
                    }
                    else
                    {
                        msg = "意外的支付方式（非甜塔、官方）";
                    }
                }
            }
            else if (obj["type"].ToString() == "CustomRefund")
            {
                var model = sql.RtDataTable($"" +
                    $" select a.order_no,case when a.refund_fee>=a.real_amount then 0 else (a.real_amount-a.refund_fee) end as fee,b.source " +
                    $" from dt_orders a(nolock) " +
                    $" left join dt_orders_payment b(nolock) on a.order_no=b.orderNo  and 	b.type='在线支付'	 and paystatus=2 and a.payment_id=100009 " +
                    $" where a.order_no='{obj["strwhere"]}' ");

                if (model.Rows.Count > 0)
                {
                    var parameterFee = decimal.Parse(obj["fee"].ToString());
                    var fee = decimal.Parse(obj["fee"].ToString());
                    if (parameterFee < fee)
                    {
                        if (model.Rows[0]["source"].ToString() == "甜塔")
                        {
                            if (RefundTianTa(model.Rows[0]["order_no"].ToString(), entid, fee, out string message, true))
                            {
                                n++;
                            }
                            else
                            {
                                msg = message;
                            }
                        }
                        else if (model.Rows[0]["source"].ToString() == "官方")
                        {
                            if (OfficialRefund(model.Rows[0]["order_no"].ToString(), entid, fee, out string message, true))
                            {
                                n++;
                            }
                            else
                            {
                                msg = message;
                            }
                        }
                        else
                        {
                            msg = "意外的支付方式（非甜塔、官方）";
                        }
                    }
                    else
                    {
                        msg = $"退款金额超出可退额度:{parameterFee},无法进行退款";
                    }
                }
                else
                {
                    msg = "订单不存在或非线上支付或未付款";
                }
            }
            else
            {
                n++;
            }
            if (n > 0 && proc == "proc_OrderQuery")
            {
                m = sql.ExecuteNonQuery(proc, param);
                if (m > 0)
                {
                    msg = obj["uptype"].ToString() == "-1"? "数据更新成功！" : "线上退款发起成功，数据回写成功！";
                    r_json = JsonMethod.GetError(0, msg);
                }
                else
                {
                    msg = obj["uptype"].ToString() == "-1" ? "数据更新失败！" : "线上退款发起成功，数据回写失败！";
                    r_json = JsonMethod.GetError(1, msg);
                }
            }
            else if (n > 0 && proc != "proc_OrderQuery")
            {
                m = sql.ExecuteNonQuery(proc, param);
                if (m>0)
                {
                    msg = "数据更新成功！";
                    r_json = JsonMethod.GetError(0, msg);
                }
                else
                {
                    msg = "数据更新失败！";
                    r_json = JsonMethod.GetError(1, msg);
                }
            }
            else
            {
                msg = "更新失败！";
                r_json = JsonMethod.GetError(1, msg);
            }
            return r_json;
        }

        public bool RefundTianTa(string orderNo, string entId, decimal money, out string message,bool druge = false)
        {
            message = "";
            if (string.IsNullOrEmpty(orderNo) || string.IsNullOrEmpty(entId))
            {
                message = "空参异常";
                return false;
            }
            else
            {
                string generate = "";
                if (druge)
                {
                    //查询退款订单最近有没有退款失败记录
                    DAL.OrderInfoDal dal = new DAL.OrderInfoDal();
                    List<PayRecord> payRecords = dal.GetPayRecords(orderNo, entId, "在线退款", 0);
                    if (payRecords.Count > 0)
                    {
                        generate = payRecords[0].Generate;
                        money = payRecords[0].Fee;
                    }
                }
                message = Refund(orderNo, generate, entId, money, out bool judge);
                Log.Info("退款详细信息", message);
                return judge;
            }
        }

        #region 官方线上退款
        /// <summary>
        /// 官方线上退款
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="entId">企业</param>
        /// <param name="refundAmount">退款金额</param>
        /// <returns></returns>
        public bool OfficialRefund(string orderNo, string entId, decimal money, out string message, bool druge = false)
        {
            message = "";
            //LogQueue.Write(LogType.Debug, "官方线上退款", "进入");
            if (string.IsNullOrEmpty(orderNo) || string.IsNullOrEmpty(entId))
            {
                message = "存在异常空参!";
                return false;
            }
            //通过真实订单号获取订单信息
            DAL.OrderInfoDal odal = new DAL.OrderInfoDal();
            List<Orders> orders = odal.GetOrderInfo(orderNo/*, entId*/);
            if (orders.Count != 1)
            {
                message = "获取单据信息错误";
                return false;
            }
            else if (orders[0].PaymentName != "线上支付")
            {
                message = "该订单支付方式不是线上支付,无法线上退款";
                return false;
            }
            else if (orders[0].PaymentName == "线上支付" && orders[0].PaymentStatus != 2)
            {
                message = "该订单未线上支付,无法线上退款";
                return false;
            }
            else
            {
                //LogQueue.Write(LogType.Debug, "官方线上退款", "验证true");
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
                string refId;
                bool judge;
                switch (payType)
                {
                    case "支付宝":
                        message = RefundApliay(orderNo, entId, generate, money, dal, orders, out judge, out refId);
                        //LogQueue.Write(LogType.Debug, "官方线上退款支付宝", $"{judge}");
                        break;
                    case "微信":
                        message = WeixinRefundApliay(orderNo, entId, generate, money, dal, orders, out judge, out refId);
                        //LogQueue.Write(LogType.Debug, "官方线上退款微信", $"{judge}");
                        break;
                    default:
                        message = "支付方式异常";
                        //LogQueue.Write(LogType.Debug, "官方线上退款", $"支付方式异常");
                        judge = false;
                        break;
                }
                return judge;
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
            data.SetValue("out_trade_no", orderGenerate);
            data.SetValue("total_fee", (int)orderFee);//订单总金额
            data.SetValue("refund_fee", (int)fee);//退款金额
            data.SetValue("out_refund_no", WxPayApi.GenerateOutTradeNo());//随机生成商户退款单号
            data.SetValue("op_user_id", WxPayConfig.GetConfig().GetMchID());//操作员，默认为商户号
            var result = WxPayApi.Refund(data);//提交退款申请给API，接收返回数据
            try
            {
                if (result.GetValue("return_code").ToString() == "SUCCESS" && result.GetValue("result_code").ToString() != "FAIL")
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
                else if (result.GetValue("return_code").ToString() == "SUCCESS" && result.GetValue("result_code").ToString() == "FAIL")
                {
                    //订单在线退款成功记录
                    int number = dal.OrderRefundUpdate(orderNo, generate, "", "", "Fail", payWay);
                    if (number > 0)
                    {
                        judge = false;
                        msg = $"Code:{result.GetValue("err_code")},详细信息:{result.GetValue("err_code_des")},在线退款响应失败，状态已回写";
                        return msg;
                    }
                    else
                    {
                        judge = false;
                        msg = $"Code:{result.GetValue("err_code")},详细信息:{result.GetValue("err_code_des")},在线退款响应失败，状态未回写";
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
                        msg = $"Code:{result.GetValue("return_code")},详细信息:{result.GetValue("return_msg")},在线退款响应失败，状态已回写";
                        return msg;
                    }
                    else
                    {
                        judge = false;
                        msg = $"Code:{result.GetValue("return_code")},详细信息:{result.GetValue("return_msg")},在线退款响应失败，状态未回写";
                        return msg;
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

        #region 支付宝线上退款
        /// <summary>
        /// 支付宝线上退款
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="entId">企业</param>
        /// <param name="refundAmount">退款金额</param>
        /// <returns></returns>
        public string RefundApliay(string orderNo, string entId, string generate, decimal fee, DAL.OrderInfoDal dal, List<Orders> orders, out bool judge, out string refId)
        {
            string msg, nowTime = "";
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
                refId = response.TradeNo;
                if (response.Code == "10000" && response.Msg == "Success")
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
                else
                {

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

        #region 支付宝初始化配置
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

        #region 甜塔支付退款
        /// <summary>
        /// 甜塔支付退款
        /// </summary>
        /// <param name="orderNo">商户订单号</param>
        /// <param name="generate">商户退款临时订单号:临时单号为空时表示新退款单，临时单号不为空表示退款失败，重新发起退款</param>
        /// <param name="entId">机构</param>
        /// <param name="fee">在线退款金额</param>
        /// <returns></returns>
        public static string Refund(string orderNo, string generate, string entId, decimal fee, out bool judge)
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
                    msg = "在线支付流水号为空，请确认订单";
                }
                else if (string.IsNullOrEmpty(orderThirdparty))
                {
                    judge = false;
                    msg = "三方支付交易号为空，请确认订单";
                }
                else if (payName != "线上支付")
                {
                    judge = false;
                    msg = "订单不属于线上支付无法发起退款";
                }
                if (fee > (orderFee - refundFee))
                {
                    judge = false;
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
                        num = odal.OrderRefundUpdate(orderNo, generate, "", "", "Retry",payWay);
                    }
                    else
                    {
                        generate = GenerateOrderNo(orderNo);
                        nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //记录退款信息
                        num = odal.OrderRefundRecord(orderNo, generate, entId, nowTime, fee, payWay,"甜塔","PC");
                    }
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
                msg = ex.Message.ToString();
            }
            return msg;
        }
        #endregion


        #region 随机订单号、时间戳
        /// <summary>
        /// 根据当前系统时间加随机序列来生成订单号
        /// </summary>
        /// <returns>订单号</returns>
        public static string GenerateOrderNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}{2}", "", DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }
        public static string GenerateOrderNo(string orderNo)
        {
            var ran = new Random();
            return string.Format("{0}A{1}", orderNo,GenerateTimeStamp());
        }
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion

        /// <summary>
        /// 更新数据库的数据，返回一个表
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <returns></returns>
        protected string GetReturnDs(string json, string proc, string UserId, string entid)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, UserId, entid)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR(proc, param);
            string r_json;
            if (dt.Rows.Count > 0)
            {
                int flag = int.Parse(dt.Rows[0]["flag"].ToString() ?? "1");
                string msg = dt.Rows[0]["msg"].ToString();
                r_json = JsonMethod.GetError(flag, msg);
            }
            else
            {
                string msg = dt.Rows[0]["msg"].ToString();
                r_json = JsonMethod.GetError(1, msg);
            }
            return r_json;
        }
        
        /// <summary>
        /// 新闻资讯查询
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="UserId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string QueryArticle(string json, string proc, string UserId, string entid)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.id,a.status,a.title,a.zhaiyao,a.sort_id,a.add_time,b.title as typeName,a.click,c.name ");
            strSql.Append("from dt_article_news a(nolock)  join dt_article_category b(nolock) on a.category_id=b.id ");
            strSql.Append(" join dt_status c(nolock) on a.status=c.status");
            strSql.Append(" where a.channel_id=6 ");
            //strSql.Append(" and left(add_time,10) between ''" + obj["dateMin"] + "'' and ''" + obj["dateMax"] + "''");
            if (obj["status"].ToString() != "")
            {
                strSql.Append("and  a.status like ''%" + obj["status"] + "%''");
            }
            if (obj["dateMin"].ToString() != "")
            {
                strSql.Append(" and add_time>''"+ obj["dateMin"]+"'' ");
            }
            if (obj["dateMax"].ToString() != "")
            {
                strSql.Append(" and add_time<''" + obj["dateMax"] + "'' ");
            }
            if (obj["strWhere"].ToString() != "")
            {
                strSql.Append(" and (a.id like ''%" + obj["strWhere"] + "%'' or a.tags like ''%" + obj["strWhere"] + "%'' or a.title like ''%" + obj["strWhere"] + "%'')");
            }
            strSql.Append(" order by add_time desc");//and b.channel_id=6 and a.status<>0
            StringBuilder strProc = new StringBuilder();
            strProc.Append("   declare @PageCount int,@RecordCount int,@SearchTime int ");
            strProc.Append(" EXEC [dbo].[Paging] @sql ='" + strSql.ToString() + "',@page =" + int.Parse(obj["PageIndex"].ToString()) + ",");
            strProc.Append("@pageSize =" + int.Parse(obj["PageSize"].ToString()) + ",@pageCount = @pageCount OUTPUT,");
            strProc.Append("@recordCount = @recordCount OUTPUT,@SearchTime = @SearchTime OUTPUT ");
            strProc.Append("SELECT	@pageCount as N'pageCount',@recordCount as N'recordCount',@SearchTime as N'SearchTime' ");
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RtDataSet(strProc.ToString());
            string r_json;
            if (ds.Tables[1].Rows.Count > 0)
            {
                int recordCount = Convert.ToInt32(ds.Tables[2].Rows[0]["recordCount"].ToString().Trim());
                int pageCount = Convert.ToInt32(ds.Tables[2].Rows[0]["pageCount"].ToString().Trim());
                r_json = JsonMethod.GetDataTable(0, recordCount, pageCount, ds.Tables[1]);
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }
            return r_json;
        }
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="UserId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string UpdateArticle(string json, string proc, string UserId, string entid)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_article_news set status=" + int.Parse(obj["status"].ToString()) + " where id='" + obj["id"] + "'");
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            bool flag = sql.ExecuteSql(strSql.ToString());
            if (flag)
            {
                return JsonMethod.GetError(0, "操作成功");
            }
            else
            {
                return JsonMethod.GetError(1, "操作成功");
            }
        }
        /// <summary>
        /// 获取文章详情信息
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="UserId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        protected string QueryArticleMt(string json, string proc, string UserId, string entid)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,channel_id,category_id,tags,title,zhaiyao,content,sort_id,add_time from dt_article_news(nolock) where id='" + obj["id"] + "'");
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RtDataTable(strSql.ToString());
            if (dt.Rows.Count > 0)
            {
                return JsonMethod.GetDataTable(0, dt.Rows.Count, 1, dt);
            }
            else
            {
                return JsonMethod.GetError(1, "未找到资源！");
            }
        }
        /// <summary>
        /// 批量删除新闻
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="UserId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        public string UpdateArticleList(string json, string proc, string UserId, string entid)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            StringBuilder strSql = new StringBuilder();
            string[] list = (obj["list"].ToString()).Split(',');
            if (list.Length > 0)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] != "")
                    {
                        strSql.Append("update dt_article_news set status=0 where id='" + list[i] + "'");
                    }
                }
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                bool flag = sql.ExecuteSql(strSql.ToString());
                if (flag)
                {
                    return JsonMethod.GetError(0, "操作成功");
                }
                else
                {
                    return JsonMethod.GetError(1, "操作成功");
                }
            }
            else
            {
                return JsonMethod.GetError(1, "请选择");
            }

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}