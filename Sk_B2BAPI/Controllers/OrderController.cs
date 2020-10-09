using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.DAL;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Models.Admin;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace Sk_B2BAPI.Controllers
{
    /// <summary>
    /// 订单处理
    /// </summary>
    public class OrderController : Controller
    {

        #region 订单存盘
        /// <summary>
        /// 订单存盘
        /// </summary>
        /// <param name="entId">企业Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="payId">支付方式Id</param>
        /// <param name="contact">联系人</param>
        /// <param name="phone">联系方式</param>
        /// <param name="address">地址</param>
        /// <param name="remarks">备注</param>
        /// <param name="couponId">优惠券Id</param>
        /// <param name="bonusId">红包Id</param>
        /// <param name="orderSource">订单来源PC/APP</param>
        /// <param name="goodsList">选中商品</param>  
        public JsonResult OrderSave(string entId, string userId, string payId, string contact, string phone, string address, string remarks, string orderSource, string goodsList, string couponId = "", string bonusId = "", string ywyId = "",string fpshtx="",string psfs= "",string fhfs="")
        {
            try
            {
                ///获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = dal.GetUserInfo(userId ?? "", entId);

                if (user.Count <= 0)
                {
                    return Json(new { success = false, message = "用户信息获取失败" });
                }
                ////支付方式
                else if (string.IsNullOrEmpty(payId))
                {
                    return Json(new { success = false, message = "请选择支付方式" });
                }
                ///是否选中商品
                else if (goodsList == null)
                {
                    return Json(new { success = false, message = "请选择商品" });
                }
                else if (user[0].Status == 1)
                {
                    return Json(new { success = false, message = "该账号未通过审核，无法购买商品" });
                }
                ///获取金额信息
                OrderInfoDal cdal = new OrderInfoDal();

                OrderAmount list = new OrderAmount();
                list = cdal.OrderCalculate(user[0].EntId, userId, goodsList, user[0].Pricelevel, user[0].KhType, payId, couponId, bonusId,true, ywyId)[0];

                decimal ordersAmount = list.OrdersAmount;
                decimal realAmount = list.RealAmount;
                decimal discountAmount = list.DiscountAmount;
                decimal bonusAmount = list.BonusAmount;
                decimal daAmount = list.DAAmount;
                decimal lineDiscount = list.lineDiscount;
                decimal limitAmount = Convert.ToDecimal(BaseConfiguration.OrderAmount);
                if (realAmount >= limitAmount)
                {
                    ////商品库存/客户资质拦截拦截
                    OrderInfoDal oDal = new OrderInfoDal();
                    List<UserCoupon> clist = oDal.OrderSave(user[0].EntId, userId, user[0].BusinessId, user[0].Pricelevel, user[0].KhType, payId, contact, phone, address, remarks, orderSource, couponId, bonusId, goodsList, ordersAmount, realAmount, discountAmount, daAmount, lineDiscount, ywyId, fpshtx, psfs, fhfs, out bool flag, out string message);
                    return Json(new { success = flag, message});
                }
                else
                {
                    return Json(new { success = false, message = "订单金额小于" + limitAmount + ",订单无法提交" });
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/OrderSave", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region 下单送优惠券/积分
        /// <summary>
        /// 下单送优惠券/积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="entId">机构</param>
        /// <param name="orderNo">订单单号</param>
        /// <param name="realAmount">实际支付金额</param>
        /// <returns></returns>
        public JsonResult CouponGive(string userId, string entId, string orderNo, decimal realAmount)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请重新登录" });
                }
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.SercerIp;
                }
                OrderInfoDal oDal = new OrderInfoDal();
                List<UserCoupon> clist = oDal.CouponGive(userId, entId, orderNo, realAmount, out float point, out string lucky);
                return Json(new { success = true, message = "优惠券获取成功", point, lucky, list = clist });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/CouponGive", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region 个人中心订单查询
        /// <summary>
        /// 个人中心订单查询
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="days">天数</param>
        /// <param name="strWhere">搜索条件</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        public JsonResult OrderQuery(string entId, string userId, int days, string strWhere, int status, int pageIndex, int pageSize, string ywyId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                ///日期计算
                string endDates = DateTime.Now.ToString("yyyy-MM-dd");
                string startDates = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");
                OrderInfoDal dal = new OrderInfoDal();
                List<OrdersMt> list = dal.OrderQueryMt(entId, userId, startDates, endDates, strWhere, status, pageIndex, pageSize, ywyId, out int recordCount, out int pageCount);
                return Json(new { success = true, list, RecordCount = recordCount, PageCount = pageCount, serverTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/OrderQuery", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 订单详情
        /// <summary>
        /// 查询订单详情
        /// </summary>
        /// <param name="entId">企业Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="orderNo">单据编号</param>
        /// <returns></returns>
        public JsonResult OrderQueryDt(string entId, string userId, string orderNo)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                OrderInfoDal dal = new OrderInfoDal();
                List<OrdersMt> list = dal.OrderQueryDt(entId, userId, orderNo);
                return Json(new { success = true, list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/OrderQueryDt", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 订单状态
        /// <summary>
        /// 查询订单状态
        /// </summary>
        /// <returns></returns>
        public JsonResult OrderStatus()
        {
            try
            {
                OrderInfoDal dal = new OrderInfoDal();
                List<OrderStatus> orderStatuses = dal.OrderStatuses();
                return Json(new { success = true, list = orderStatuses });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/OrderStatus", ex.Message.ToString());
                return Json(new { success = false, message = "E005" });
            }
        }
        #endregion

        #region 订单计算
        /// <summary>
        /// 选择优惠券后订单重算
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="userId">用户</param>
        /// <param name="goodsList">购物车Id</param>
        /// <param name="payType">支付方式</param>
        /// <param name="couponId">优惠券Id</param>
        /// <param name="bonusId">红包Id</param>
        /// <returns></returns>
        public JsonResult OrderCalculate(string entId, string userId, string goodsList, string payType, string couponId = "", string bonusId = "", string ywyId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                ///获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = dal.GetUserInfo(userId, entId);
                if (user.Count <= 0)
                {
                    return Json(new { success = false, message = "用户信息获取失败" });
                }
                ///获取金额信息
                OrderInfoDal cdal = new OrderInfoDal();
                List<OrderAmount> List = cdal.OrderCalculate(user[0].EntId, userId, goodsList, user[0].Pricelevel, user[0].KhType, payType, couponId, bonusId,true, ywyId);
                int couponCode = List[0].CouponCode;
                List<Gift> glist = new List<Gift>();
                if (couponCode != 0)
                {
                    glist = cdal.GetGifts(couponCode);
                }
                return Json(new { success = true, list = List, gift = glist });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/OrderCalculate", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 订单支付选项
        /// <summary>
        /// 订单支付选项
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="rank">级别</param>
        /// <returns></returns>
        public JsonResult PaymentType(string entId, string rank = "0")
        {
            try
            {
                if (string.IsNullOrEmpty(entId))
                {
                    entId = BaseConfiguration.EntId.ToString();
                }
                OrderInfoDal orderInfoDal = new OrderInfoDal();
                List<Payment> payApis = orderInfoDal.PaymentType(entId, rank);
                return Json(new { success = true, message = "付款方式获取成功", list = payApis });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/PaymentOptions", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 物流信息
        /// <summary>
        /// 物流信息
        /// </summary>
        /// <param name="expressCompany">快递公司</param>
        /// <param name="expressNumber">快递单号</param>
        /// <returns></returns>
        public ActionResult Logistics(string expressCompany, string expressNumber)
        {
            try
            {

                if (string.IsNullOrEmpty(expressCompany) || string.IsNullOrEmpty(expressNumber))
                {
                    return Content(JsonHelper.GetErrorJson(3, 0, "物流公司编码或物流单号不能为空"), "json");
                }
                string expressMsg = Express.ReturnKDDate(expressCompany, expressNumber);
                JObject jobject = (JObject)JsonConvert.DeserializeObject(expressMsg);
                if (jobject.ContainsKey("state"))
                {
                    string json = JsonHelper.ReturnExpressMsg("true", "物流信息获取成功", expressMsg);
                    return Content(json, "json");
                }
                else if (jobject.ContainsKey("result"))
                {
                    string json = JsonHelper.ReturnExpressMsg("false", jobject["message"].ToString(), null);
                    return Content(json, "json");
                }
                else
                {
                    return Content(expressMsg, "json");
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/Logistics", ex.Message.ToString());
                return Content(JsonHelper.GetErrorJson(1, 0, ex.Message.ToString()), "json");
            }
        }
        #endregion

        #region 消费流水
        /// <summary>
        /// 消费流水
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页条目</param>
        /// <returns></returns>
        public ActionResult ExpenseCalendar(string userId, string entId, int pageIndex, int pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(entId))
                {
                    return Json(new { success = false, message = "用户或者机构不能为空" });
                }
                OrderInfoDal orderInfoDal = new OrderInfoDal();
                List<ExpenseCalendar> expenseCalendars = orderInfoDal.ExpenseCalendar(userId, entId, pageIndex, pageSize, out int pageCount, out int recordCount);
                return Json(new { success = true, message = "消费流水获取成功", list = expenseCalendars, pageCount, recordCount });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/ExpenseCalendar", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 退货订单详情
        [HttpPost]
        public JsonResult ReturnOrderProduct(string entId, string userId, string orderNo, string status) {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                OrderInfoDal dal = new OrderInfoDal();
                List<OrdersMt> list = dal.ReturnOrderQueryDt(entId, userId, orderNo, status);
                return Json(new { success = true, list });
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/ReturnOrderProduct", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 退货商品图片上传
        /// <summary>
        /// 退货商品图片上传
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadReturnProImg()
        {
            try
            {
                string dataFile;
                string filePath = string.Empty;
                string fileNewName = string.Empty;
                var file = Request.Files;
                if (file.Count <= 0)
                {
                    return Json(new { success = false, message = "请选择要上传的文件！" });//请选择要上传的文件  
                }
                var ufile = file[0];
                string fileName = ufile.FileName;/*获取文件名： C:\Documents and Settings\Administrator\桌面\123.jpg*/
                //Log.Error("错误",$"上传文件名：{fileName}");//上传文件名：201912261054561.jpg
                string suffix = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();/*获取后缀名并转为小写： jpg*/
                int bytes = ufile.ContentLength;//获取文件的字节大小  
                if (suffix != "jpg" && suffix != "png")
                {
                    return Json(new { success = false, message = "上传文件格式错误！" });//只能上传JPG格式图片  
                }
                else if (bytes > 1024 * 1024 * 10)
                {
                    return Json(new { success = false, message = "图片不能大于10M！" }); //图片不能大于1M 
                }
                /*判断文件夹是否存在*/
                if (Directory.Exists(Server.MapPath("/UploadFile/ReturnProImg/")) == false)
                {
                    //Log.Error("打印", $"文件夹Material不存在，重新创建");
                    Directory.CreateDirectory(Server.MapPath("/UploadFile/ReturnProImg/"));
                }
                string path = Server.MapPath("/UploadFile/ReturnProImg/");
                //Log.Error("打印", $"物理文件路径path：{path}");
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string date = DateTime.Now.ToFileTimeUtc().ToString();
                string newfileName = date + "." + suffix;
                string newfile = year + "_" + month + "/" + newfileName;
                string newPath = path + year + "_" + month;
                //Log.Error("打印", $"上传文件路径newPath：{newPath}");//  /UploadFile/Users/System.Web.HttpPostedFileWrapper
                //Log.Error("打印", $"newPath文件是否存在：{System.IO.Directory.Exists(newPath)}");
                if (false == System.IO.Directory.Exists(newPath))
                {
                    //Log.Error("打印", $"文件夹newPath不存在，重新创建");
                    //创建文件夹
                    System.IO.Directory.CreateDirectory(newPath);
                }
                ufile.SaveAs(newPath + "/" + newfileName);//保存图片  
                                                          //保存图片路径
                dataFile = "/UploadFile/ReturnProImg/" + newfile;
                string webUrl = BaseConfiguration.SercerIp.ToString();
                //if (webUrl.Contains("/b2b_api"))
                //{
                //    var index = webUrl.LastIndexOf('/');
                //    webUrl = webUrl.Substring(0, index);
                //}
                return Json(new { success = true, message = "订单图片上传成功！", imgUrl = webUrl + dataFile });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString(), imgUrl = "" });
            }
        }
        #endregion

        #region 退货商品多图片上传
        /// <summary>
        /// 退货商品多图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadManyReturnProImg()
        {
            try
            {
                List<string> imgurl = new List<string>();
                string dataFile;
                string filePath = string.Empty;
                string fileNewName = string.Empty;
                var file = Request.Files;
                if (file.Count <= 0)
                {
                    return Json(new { success = false, message = "请选择要上传的文件！" });//请选择要上传的文件  
                }
                for (int i = 0; i < file.Count; i++) {
                    var ufile = file[i];
                    string fileName = ufile.FileName;/*获取文件名： C:\Documents and Settings\Administrator\桌面\123.jpg*/
                                                     //Log.Error("错误",$"上传文件名：{fileName}");//上传文件名：201912261054561.jpg
                    string suffix = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();/*获取后缀名并转为小写： jpg*/
                    int bytes = ufile.ContentLength;//获取文件的字节大小  
                    if (suffix != "jpg" && suffix != "png")
                    {
                        return Json(new { success = false, message = "上传文件格式错误！" });//只能上传JPG格式图片  
                    }
                    else if (bytes > 1024 * 1024 * 10)
                    {
                        return Json(new { success = false, message = "图片不能大于10M！" }); //图片不能大于1M 
                    }
                    /*判断文件夹是否存在*/
                    if (Directory.Exists(Server.MapPath("/UploadFile/ReturnProImg/")) == false)
                    {
                        //Log.Error("打印", $"文件夹Material不存在，重新创建");
                        Directory.CreateDirectory(Server.MapPath("/UploadFile/ReturnProImg/"));
                    }
                    string path = Server.MapPath("/UploadFile/ReturnProImg/");
                    //Log.Error("打印", $"物理文件路径path：{path}");
                    string year = DateTime.Now.Year.ToString();
                    string month = DateTime.Now.Month.ToString();
                    string date = DateTime.Now.ToFileTimeUtc().ToString();
                    string newfileName = date + "." + suffix;
                    string newfile = year + "_" + month + "/" + newfileName;
                    string newPath = path + year + "_" + month;
                    //Log.Error("打印", $"上传文件路径newPath：{newPath}");//  /UploadFile/Users/System.Web.HttpPostedFileWrapper
                    //Log.Error("打印", $"newPath文件是否存在：{System.IO.Directory.Exists(newPath)}");
                    if (false == System.IO.Directory.Exists(newPath))
                    {
                        //Log.Error("打印", $"文件夹newPath不存在，重新创建");
                        //创建文件夹
                        System.IO.Directory.CreateDirectory(newPath);
                    }
                    ufile.SaveAs(newPath + "/" + newfileName);//保存图片//保存图片路径
                    dataFile = "/UploadFile/ReturnProImg/" + newfile;
                    string webUrl = BaseConfiguration.SercerIp.ToString();
                    imgurl.Add(webUrl + dataFile);
                }
                return Json(new { success = true, message = "订单图片上传成功！", imgUrl = imgurl });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString(), imgUrl = "" });
            }
        }
        #endregion

        #region 申请退货操作
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult UpOrderProStatus(string orderNo, string detailsid, string remak, string imgurl, string userid, string entid) {
            try
            {
                if (string.IsNullOrEmpty(userid))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                OrderInfoDal dal = new OrderInfoDal();
                if (dal.UpOrderProStatus(orderNo, userid, entid, detailsid, imgurl, remak,out string message))
                {
                    return Json(new { success = true, msg = message });
                }
                else {
                    return Json(new { success = false, msg = message });
                };
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/UpOrderProStatus", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion

        #region 导出退货订单
        [HttpGet]
        public FileResult ExportExcel()
        {

            Dt_User user = RoleFuns.IsLoginAdmin(System.Web.HttpContext.Current.Session["user"]);
            ///获取用户到货提醒
            if (user == null)
            {
                return null;
            }

            string json = HttpContext.Request.QueryString["json"].Trim();//请求参数(json类型)
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            StringBuilder strsql = new StringBuilder();

            strsql.Append("select aa.sub_title,aa.drug_factory,aa.approval_number,aa.drug_spec,og.quantity,og.real_price,og.taxAmount,rr.*,uu.username,uu.name,uu.telphone,oo.order_no,oo.billno,oo.add_time,oo.Source,bb.businessname,bb.address,bb.businesscode from dt_order_ReturnRemark rr join dt_users uu on uu.userid=rr.Userid join dt_orders oo on oo.order_no=rr.OrderId join dt_businessdoc bb on bb.businessid=uu.businessid join dt_order_goods og on og.order_no=oo.order_no join dt_article_attribute aa on aa.article_id=og.article_id where og.ReturnNum!=0 and rr.TypeStatus=1 and og.status in (2,4,5) ");
            if (user.entId != "superintendent")
            {
                strsql.Append($" and a.Entid='{user.entId }' ");
            }
            if (StringHelper.IsNull(obj["startDate"] == null ? "" : obj["startDate"].ToString()))
            {
                strsql.Append($" and substring(rr.AddTime,0,11)>='{obj["startDate"]}'");
            }
            if (StringHelper.IsNull(obj["endDate"] == null ? "" : obj["endDate"].ToString()))
            {
                strsql.Append($" and substring(rr.AddTime,0,11)<='{obj["startDate"]}'");
            }
            strsql.Append(" order by rr.AddTime desc");
            //#region 订单导出
            //strsql.Append("select ent.entname,a.order_no,b.name,c.businessname,c.businesscode,a.accept_name,d.PayType,e.name as status,a.discount_amount,a.real_amount,a.order_amount,a.add_time,a.Source,case when d.PayType!='线下支付' and a.payment_status=2 then '已支付'  when d.PayType='线下支付' and a.Status=7 and a.payment_status=2 then '已支付' else '未支付' end as payment_status,a.payment_status from dt_orders a(nolock) join dt_users b on a.entid=b.entid and a.user_id=b.userid join dt_businessdoc c on b.businessid=c.businessid and b.entid=c.entid join dt_payapi d(nolock) on a.payment_id=d.payid and a.entid=d.EntId join dt_status e(nolock) on a.status=e.status join dt_entdoc ent on ent.entid=a.entid where  1=1 ");
            //if (StringHelper.IsNull(obj["status"] == null ? "" : obj["status"].ToString()))
            //{
            //    strsql.Append($" and a.status={obj["status"].ToString()}");
            //}
            //if (StringHelper.IsNull(obj["startDate"] == null ? "" : obj["startDate"].ToString()))
            //{
            //    strsql.Append($" and substring(a.add_time,0,11)>='{obj["startDate"].ToString()}'");
            //}
            //if (StringHelper.IsNull(obj["endDate"] == null ? "" : obj["endDate"].ToString()))
            //{
            //    strsql.Append($" and substring(a.add_time,0,11)<='{obj["startDate"].ToString()}'");
            //}
            //if (StringHelper.IsNull(obj["strWhere"] == null ? "" : obj["strWhere"].ToString()))
            //{
            //    strsql.Append($" and (a.order_no like '{ obj["strWhere"].ToString()}' or b.name like '%{ obj["strWhere"].ToString()}' or c.businesscode like '{ obj["strWhere"].ToString()}' or c.businessname like '{ obj["strWhere"].ToString()}' or c.logogram like '{ obj["strWhere"].ToString()}')");
            //}
            //strsql.Append(" order by a.add_time desc"); 
            //#endregion
            DataTable ds = sql.RunSqlDataTable(strsql.ToString());
            HSSFWorkbook Fw = new HSSFWorkbook();
            ISheet sheet = Fw.CreateSheet("sheet1");
            IRow row1 = sheet.CreateRow(0);
            //表头
            row1.CreateCell(0).SetCellValue("机构");
            row1.CreateCell(1).SetCellValue("单位名称");
            row1.CreateCell(2).SetCellValue("单位地址");
            row1.CreateCell(3).SetCellValue("账号");
            row1.CreateCell(4).SetCellValue("姓名");
            row1.CreateCell(5).SetCellValue("手机");
            row1.CreateCell(6).SetCellValue("订单号");
            row1.CreateCell(7).SetCellValue("渠道");
            row1.CreateCell(8).SetCellValue("下单时间");
            row1.CreateCell(9).SetCellValue("退单时间");
            row1.CreateCell(10).SetCellValue("退货理由");
            row1.CreateCell(11).SetCellValue("国药准字");
            row1.CreateCell(12).SetCellValue("产品名");
            row1.CreateCell(13).SetCellValue("厂家");
            row1.CreateCell(14).SetCellValue("规格");
            row1.CreateCell(15).SetCellValue("数量");
            row1.CreateCell(16).SetCellValue("单价");
            row1.CreateCell(17).SetCellValue("总价");
            int i = 1;
            foreach (DataRow list in ds.Rows)
            {
                IRow row2 = sheet.CreateRow(i);
                row2.CreateCell(0).SetCellValue(list["Entid"].ToString());
                row2.CreateCell(1).SetCellValue(list["businessname"].ToString());
                row2.CreateCell(2).SetCellValue(list["address"].ToString());
                row2.CreateCell(3).SetCellValue(list["username"].ToString());
                row2.CreateCell(4).SetCellValue(list["name"].ToString());
                row2.CreateCell(5).SetCellValue(list["telphone"].ToString());
                row2.CreateCell(6).SetCellValue(list["order_no"].ToString());
                row2.CreateCell(7).SetCellValue(list["Source"].ToString());
                row2.CreateCell(8).SetCellValue(list["add_time"].ToString());
                row2.CreateCell(9).SetCellValue(list["AddTime"].ToString());
                row2.CreateCell(10).SetCellValue(list["Remark"].ToString());
                row2.CreateCell(11).SetCellValue(list["approval_number"].ToString());
                row2.CreateCell(12).SetCellValue(list["sub_title"].ToString());
                row2.CreateCell(13).SetCellValue(list["drug_factory"].ToString());
                row2.CreateCell(14).SetCellValue(list["drug_spec"].ToString());
                row2.CreateCell(15).SetCellValue(list["quantity"].ToString());
                row2.CreateCell(16).SetCellValue(list["real_price"].ToString());
                row2.CreateCell(17).SetCellValue(list["taxAmount"].ToString());
                i++;
            }
            // 写入到客户端 
            MemoryStream ms = new MemoryStream();
            Fw.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "退货退款信息" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }
        #endregion

        #region 确认收货
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <returns></returns>
        public JsonResult UpOrderStatus(string userId,/*string entId,*/ string orderId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "用户未登录，请先登录" });
                }
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                var num=sql.RunSqlNumber($"Update dt_orders Set Status=7 where Order_No='{orderId}'");
                if (num > 0)
                {
                    var sum = sql.RunSqlNumber($"Update dt_order_goods Set Status=6 where Order_No='{orderId}' and Status=1");
                    if (sum > 0)
                    {
                        return Json(new { success = true });
                    }
                    else {
                        return Json(new { success = false });
                    }
                }
                else {
                    return Json(new { success = false});
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/UpOrderStatus", ex.Message.ToString());
                return Json(new { success = false, message = "E005" });
            }
        }
        #endregion

        #region 导出对账单
        /// <summary>
        /// 导出对账单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileResult ExportReconciliation()
        {
            Dt_User user = RoleFuns.IsLoginAdmin(System.Web.HttpContext.Current.Session["user"]);
            ///获取用户到货提醒
            if (user == null)
            {
                return null;
            }
            string json = HttpContext.Request.QueryString["json"].Trim();//请求参数(json类型)
            string proc = HttpContext.Request.QueryString["proc"];//存储过程名称
            string name = HttpContext.Request.QueryString["name"];//文件名称
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            SqlParameter[] param = (JsonMethod.ListParameter(json, user.userId, user.entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            HSSFWorkbook Fw = new HSSFWorkbook();
            #region 汇总
            ISheet mt = Fw.CreateSheet("汇总");
            IRow mth = mt.CreateRow(0);
            //组织汇总表头
            DataTable mthead = ds.Tables[0];
            foreach (DataColumn col in mthead.Columns)
            {
                if (col.ColumnName.Contains("ROWSTAT"))
                    continue;
                //创建单元格并设置单元格内容
                mth.CreateCell(col.Ordinal).SetCellValue(col.ColumnName.ToString());
            }
            //组织汇总表数据
            DataTable mtbody = ds.Tables[0];
            int i = 1;
            foreach (DataRow list in mtbody.Rows)
            {
                IRow mtb = mt.CreateRow(i);
                foreach (DataColumn item in mtbody.Columns)
                {
                    mtb.CreateCell(item.Ordinal).SetCellValue(list[item.ColumnName].ToString());
                }
                i++;
            }
            #endregion

            #region 在线支付-明细
            ISheet dt = Fw.CreateSheet("在线支付-明细");
            IRow dth = dt.CreateRow(0);
            //组织明细表头
            DataTable dthead = ds.Tables[1];
            foreach (DataColumn col in dthead.Columns)
            {
                if (col.ColumnName.Contains("ROWSTAT"))
                    continue;
                //创建单元格并设置单元格内容
                dth.CreateCell(col.Ordinal).SetCellValue(col.ColumnName.ToString());
            }
            //组织明细表数据
            DataTable dtbody = ds.Tables[1];
            int j = 1;
            foreach (DataRow list in dtbody.Rows)
            {
                IRow dtb = dt.CreateRow(j);
                foreach (DataColumn item in dtbody.Columns)
                {
                    dtb.CreateCell(item.Ordinal).SetCellValue(list[item.ColumnName].ToString());
                }
                j++;
            }
            #endregion

            #region 账期支付-明细
            ISheet dtz = Fw.CreateSheet("账期支付-明细");
            IRow dthz = dtz.CreateRow(0);
            //组织明细表头
            DataTable dtheadz = ds.Tables[2];
            foreach (DataColumn col in dthead.Columns)
            {
                if (col.ColumnName.Contains("ROWSTAT"))
                    continue;
                //创建单元格并设置单元格内容
                dthz.CreateCell(col.Ordinal).SetCellValue(col.ColumnName.ToString());
            }
            //组织明细表数据
            DataTable dtbodyz = ds.Tables[2];
            int k = 1;
            foreach (DataRow list in dtbodyz.Rows)
            {
                IRow dtbz = dtz.CreateRow(k);
                foreach (DataColumn item in dtbodyz.Columns)
                {
                    dtbz.CreateCell(item.Ordinal).SetCellValue(list[item.ColumnName].ToString());
                }
                k++;
            }
            #endregion

            #region 退款明细
            ISheet dtt = Fw.CreateSheet("退款明细");
            IRow dtht = dtt.CreateRow(0);
            //组织明细表头
            DataTable dtheadt = ds.Tables[3];
            foreach (DataColumn col in dtheadt.Columns)
            {
                if (col.ColumnName.Contains("ROWSTAT"))
                    continue;
                //创建单元格并设置单元格内容
                dtht.CreateCell(col.Ordinal).SetCellValue(col.ColumnName.ToString());
            }
            //组织明细表数据
            DataTable dtbodyt = ds.Tables[3];
            int l = 1;
            foreach (DataRow list in dtbodyt.Rows)
            {
                IRow dtbt = dtt.CreateRow(l);
                foreach (DataColumn item in dtbodyt.Columns)
                {
                    dtbt.CreateCell(item.Ordinal).SetCellValue(list[item.ColumnName].ToString());
                }
                l++;
            }
            #endregion

            // 写入到客户端 
            MemoryStream ms = new MemoryStream();
            Fw.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }
        #endregion

        #region 导出订单
        /// <summary>
        /// 导出订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileResult ExportExcelForOrderDetail()
        {
            Dt_User user = RoleFuns.IsLoginAdmin(System.Web.HttpContext.Current.Session["user"]);
            ///获取用户到货提醒
            if (user == null)
            {
                return null;
            }
            string json = HttpContext.Request.QueryString["json"].Trim();//请求参数(json类型)
            string proc = HttpContext.Request.QueryString["proc"];//存储过程名称
            string name = HttpContext.Request.QueryString["name"];//文件名称
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            SqlParameter[] param = (JsonMethod.ListParameter(json, user.userId, user.entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            HSSFWorkbook Fw = new HSSFWorkbook();
            #region 汇总
            ISheet mt = Fw.CreateSheet("汇总");
            IRow mth = mt.CreateRow(0);
            //组织汇总表头
            DataTable mthead = ds.Tables[0];
            foreach (DataColumn col in mthead.Columns)
            {
                if (col.ColumnName.Contains("ROWSTAT"))
                    continue;
                //创建单元格并设置单元格内容
                mth.CreateCell(col.Ordinal).SetCellValue(mthead.Rows[0][col.ColumnName].ToString());
            }
            //组织汇总表数据
            DataTable mtbody = ds.Tables[1];
            int i = 1;
            foreach (DataRow list in mtbody.Rows)
            {
                IRow mtb = mt.CreateRow(i);
                foreach (DataColumn item in mtbody.Columns)
                {
                    mtb.CreateCell(item.Ordinal).SetCellValue(list[item.ColumnName].ToString());
                }
                i++;
            }
            #endregion

            #region 明细
            ISheet dt = Fw.CreateSheet("明细");
            IRow dth = dt.CreateRow(0);
            //组织明细表头
            DataTable dthead = ds.Tables[2];
            foreach (DataColumn col in dthead.Columns)
            {
                if (col.ColumnName.Contains("ROWSTAT"))
                    continue;
                //创建单元格并设置单元格内容
                dth.CreateCell(col.Ordinal).SetCellValue(dthead.Rows[0][col.ColumnName].ToString());
            }
            //组织明细表数据
            DataTable dtbody = ds.Tables[3];
            int j = 1;
            foreach (DataRow list in dtbody.Rows)
            {
                IRow dtb = dt.CreateRow(j);
                foreach (DataColumn item in dtbody.Columns)
                {
                    dtb.CreateCell(item.Ordinal).SetCellValue(list[item.ColumnName].ToString());
                }
                j++;
            }
            #endregion

            // 写入到客户端 
            MemoryStream ms = new MemoryStream();
            Fw.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }
        #endregion

        #region 无图商品导出
        /// <summary>
        /// 无图商品导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileResult NoPIGExprot()
        {
            Dt_User user = RoleFuns.IsLoginAdmin(System.Web.HttpContext.Current.Session["user"]);
            ///获取用户到货提醒
            if (user == null)
            {
                return null;
            }
            string json = HttpContext.Request.QueryString["json"].Trim();//请求参数(json类型)
            string proc = HttpContext.Request.QueryString["proc"];//存储过程名称
            string name = HttpContext.Request.QueryString["name"];//文件名称
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            SqlParameter[] param = (JsonMethod.ListParameter(json, user.userId, user.entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            HSSFWorkbook Fw = new HSSFWorkbook();
            #region 数据
            ISheet mt = Fw.CreateSheet("无图商品");
            IRow mth = mt.CreateRow(0);
            DataTable hz = ds.Tables[0];
            //组织数据表头
            foreach (DataColumn col in hz.Columns)
            {
                if (col.ColumnName.Contains("ROWSTAT"))
                    continue;
                //创建单元格并设置单元格内容
                mth.CreateCell(col.Ordinal).SetCellValue(col.ColumnName.ToString());
            }
            //组织数据
            int i = 1;
            foreach (DataRow list in hz.Rows)
            {
                IRow mtb = mt.CreateRow(i);
                foreach (DataColumn item in hz.Columns)
                {
                    mtb.CreateCell(item.Ordinal).SetCellValue(list[item.ColumnName].ToString());
                }
                i++;
            }
            #endregion

            // 写入到客户端 
            MemoryStream ms = new MemoryStream();
            Fw.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }
        #endregion

        #region 订单状态分类统计数目
        /// <summary>
        /// 订单状态分类统计数目
        /// </summary>
        /// <param name="entId">企业</param>
        /// <param name="userId">机构</param>
        /// <returns></returns>
        public ActionResult StateClassificationItemsStatistics(string entId,string userId)
        {
            try
            {
                OrderInfoDal dal = new OrderInfoDal();
                List<OrderStatus> statuses = dal.ItemsStatistics(entId, userId);
                return Json(new { success=true,message="订单状态分组条目统计成功",statuses});
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "Order/StateClassificationItemsStatistics", ex.Message.ToString());
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }
        #endregion
    }
}
