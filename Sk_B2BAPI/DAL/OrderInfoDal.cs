using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.DAL
{
    /// <summary>
    /// 订单数据访问层
    /// </summary>
    public class OrderInfoDal
    {
        #region 订单存盘
        /// <summary>
        /// 订单存盘
        /// </summary>
        /// <param name="entId">企业Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="businessId">客户Id</param>
        /// <param name="userId">客户级别</param>
        /// <param name="payId">支付方式Id</param>
        /// <param name="contact">联系人</param>
        /// <param name="phone">联系方式</param>
        /// <param name="address">地址</param>
        /// <param name="remarks">备注</param>
        /// <param name="couponId">优惠券Id</param>
        /// <param name="orderSource">订单来源PC/APP</param>
        /// <param name="goodsList">选中商品</param>  
        /// <returns></returns>
        public List<UserCoupon> OrderSave(string entId, string userId, string businessId, string jgjb,string clientType , string payId, string contact, string phone, string address, string remarks, string orderSource, string couponId,string bonusId, string goodsList,decimal ordersAmount, decimal realAmount, decimal discountAmount,decimal daAmount,decimal lineDiscount,string ywyId, string fpshtx, string psfs,string fhfs, out bool flag,out string message/*,out float point,out string lucky*/)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","save"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@ywyId",ywyId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@jgjb",jgjb),
                new SqlParameter("@clientType",clientType),
                new SqlParameter("@payId",payId),
                new SqlParameter("@contact",contact),
                new SqlParameter("@phone",phone),
                new SqlParameter("@address",address),
                new SqlParameter("@remarks",remarks),
                new SqlParameter("@orderSource",orderSource),
                new SqlParameter("@couponId",couponId),
                new SqlParameter("@bonusId",bonusId),
                new SqlParameter("@businessId",businessId),
                new SqlParameter("@goodsList",goodsList),
                new SqlParameter("@ordersAmount",ordersAmount),
                new SqlParameter("@realAmount",realAmount),
                new SqlParameter("@discountAmount",discountAmount),
                new SqlParameter("@daAmount",daAmount),
                new SqlParameter("@lineDiscount",lineDiscount),
                new SqlParameter("@fpshtx",fpshtx),
                new SqlParameter("@psfs",psfs),
                new SqlParameter("@fhfs",fhfs)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_OrderSave", param);
            message = "订单存盘失败";
            //lucky = "N";
            //point = 0;
            var list = new List<UserCoupon>();
            flag = false;
            if (ds.Tables.Count > 0)
            {
                flag = true;
                message = ds.Tables[0].Rows[0]["order_no"].ToString();
                //point=float.Parse(ds.Tables[0].Rows[0]["point"].ToString());
                //lucky = ds.Tables[0].Rows[0]["lucky"].ToString();
                //CouponDal couponDal = new CouponDal();
                //list = couponDal.FillUableList(ds.Tables[1]);
            }
            return list;
        }
        #endregion

        #region 下单送优惠券/积分
        /// <summary>
        /// 下单送优惠券/积分
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="entId">机构</param>
        /// <param name="orderNo">订单单号</param>
        /// <param name="realAmount">实付金额</param>
        /// <param name="point">积分</param>
        /// <param name="lucky">抽奖机会</param>
        /// <returns></returns>
        public List<UserCoupon> CouponGive(string userId, string entId, string orderNo,decimal realAmount, out float point, out string lucky)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","Give"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@orderNo",orderNo),
                new SqlParameter("@realAmount",realAmount)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_CouponGiveUser", param);
            var list = new List<UserCoupon>();
            lucky = "N";
            point = 0;
            if (ds.Tables.Count > 0)
            {
                point = float.Parse(ds.Tables[0].Rows[0]["point"].ToString());
                lucky = ds.Tables[0].Rows[0]["lucky"].ToString();
                CouponDal couponDal = new CouponDal();
                if (ds.Tables[1].Rows.Count>0)
                {
                    list = couponDal.FillUableList(ds.Tables[1]);
                }
            }
            return list;
        }
        #endregion

        #region 个人中心订单信息
        /// <summary>
        /// 个人中心获取订单信息
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="days"></param>
        /// <param name="strWhere"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<OrdersMt> OrderQueryMt(string entId, string userId, string startDates, string endDates, string strWhere, int status, int pageIndex, int pageSize,string ywyId, out int recordCount, out int pageCount)
        {
            recordCount = 0;//总条目数
            pageCount = 0;//总页数
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","OrderQuery_Mt"),
                new SqlParameter("@entId",entId),
                new SqlParameter("@userId",userId),
                new SqlParameter("@ywyId",ywyId),
                new SqlParameter("@startDates",startDates),
                new SqlParameter("@endDates",endDates),
                new SqlParameter("@strWhere",strWhere),
                new SqlParameter("@status",status),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("proc_OrderQuery", param);
            List<OrdersMt> list = new List<OrdersMt>();
            if (ds.Tables.Count >= 3)
            {
                recordCount = int.Parse(ds.Tables[2].Rows[0]["recordCount"].ToString().Trim());
                pageCount = int.Parse(ds.Tables[2].Rows[0]["pageCount"].ToString().Trim());
                list = SetOrderQuery(ds.Tables[0],ds.Tables[1]);
            }
            return list;
        }


        /// <summary>
        /// 填充OrdersMt
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<OrdersMt> SetOrderQuery(DataTable dt,DataTable table=null)
        {
            List<OrdersMt> OList = new List<OrdersMt>();
            foreach (DataRow dr in dt.Rows)
            {
                OrdersMt mt = new OrdersMt
                {
                    BillNo = int.Parse(dr["billno"].ToString().Trim()),
                    Order_No = dr["order_no"].ToString().Trim(),
                    Generate = dr["generate"].ToString().Trim(),
                    Thirdparty = dr["thirdparty"].ToString().Trim(),
                    UserId = dr["user_id"].ToString().Trim(),
                    BusinessId = dr["businessId"].ToString().Trim(),
                    PaymentId = dr["payment_id"].ToString().Trim(),
                    PaymentName = dr["paytype"].ToString().Trim(),
                    PaymentFee = decimal.Parse(dr["payment_fee"].ToString().Trim()),
                    PaymentStatus = int.Parse(dr["payment_status"].ToString().Trim()),
                    PaymentTime = dr["payment_time"].ToString().Trim(),
                    PayType = dr["payWay"].ToString().Trim(),
                    Accept_Name = dr["accept_name"].ToString().Trim(),
                    Telphone = dr["telphone"].ToString().Trim(),
                    Address = dr["address"].ToString().Trim(),
                    Real_Amount = decimal.Parse(dr["real_amount"].ToString().Trim()),
                    Order_Amount = decimal.Parse(dr["order_amount"].ToString().Trim()),
                    Discount_Amount = decimal.Parse(dr["discount_amount"].ToString().Trim()),
                    Point = int.Parse(dr["point"].ToString().Trim()),
                    AddTime = dr["add_time"].ToString().Trim(),
                    Status = int.Parse(dr["status"].ToString().Trim()),
                    IsCriticism = dr["IsCriticism"].ToString().Trim(),
                    DeliveryTime = dr["ShipmentsTime"].ToString().Trim(),
                    ExpressCode = dr["Code"].ToString().Trim(),
                    ExpressNum = dr["ExpressNum"].ToString().Trim()
                };
                if (dr["payment_initiationtime"].ToString() == "")
                {
                    mt.Initiationtime = "";
                }
                else
                {
                    mt.Initiationtime = dr["payment_initiationtime"].ToString().Trim();
                }
                if (mt.PaymentId == "100000")
                {
                    mt.PaymentStatusName = "";
                }
                else
                {
                    switch (mt.PaymentStatus)
                    {
                        case 1:
                            mt.PaymentStatusName = "未支付";
                            break;
                        case 2:
                            mt.PaymentStatusName = "已支付";
                            break;
                        default:
                            mt.PaymentStatusName = "";
                            break;
                    }
                }
                mt.StatusName = dr["StatusName"].ToString().Trim();
                List<OrderDt> orderDts = new List<OrderDt>();
                if (table!=null)
                {
                    var webUrl = BaseConfiguration.SercerIp;
                    DataRow[] rows = table.AsEnumerable().Where(r => r.Field<string>("order_no") == dr["order_no"].ToString()).ToArray();
                    foreach (DataRow row in rows)
                    {
                        OrderDt orderDt = new OrderDt
                        {
                            Id = int.Parse(row["id"].ToString().Trim()),
                            Order_No = row["order_no"].ToString().Trim(),
                            Goods_Title = row["sub_title"].ToString().Trim(),
                            Article_Id = row["article_id"].ToString().Trim(),
                            Goods_Price = decimal.Parse(row["goods_price"].ToString().Trim()),
                            Real_Price = decimal.Parse(row["real_price"].ToString().Trim()),
                            Quantity = decimal.Parse(row["quantity"].ToString().Trim()),
                            TaxAmount = decimal.Parse(row["taxAmount"].ToString().Trim()),
                            cxbs = row["cxbs"].ToString().Trim(),
                            Drug_Factory = row["drug_factory"].ToString().Trim(),
                            Drug_Spec = row["drug_spec"].ToString().Trim(),
                            Package_Unit = row["package_unit"].ToString().Trim(),
                            Valdate = row["valdate"].ToString().Trim(),
                            Tag = row["tag"].ToString(),
                            IsCriticism = row["IsCriticism"].ToString().Trim(),
                            Status = int.Parse(row["Status"].ToString().Trim())
                        };
                        //大包装控制 Y-取大包装 N-不取大包装
                        string packControl = row["packControl"].ToString().Trim();
                        //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                        string scattered = row["scattered"].ToString().Trim();
                        //Min_Package = BasisConfig.ObjToDecimal(dtr["min_package"].ToString().Trim(), 0.00M);
                        if (packControl == "Y")
                        {
                            orderDt.Min_Package = BasisConfig.ObjToDecimal(row["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                        }
                        else
                        {
                            if (scattered == "Y")
                            {
                                orderDt.Min_Package = BasisConfig.ObjToDecimal(row["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                            }
                            else
                            {
                                orderDt.Min_Package = BasisConfig.ObjToDecimal(row["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                            }
                        }
                        if (row["img_url"].ToString().Trim() != "")
                        {
                            orderDt.ImgUrl = webUrl + row["img_url"].ToString().Trim();
                        }
                        else
                        {
                            orderDt.ImgUrl = "";
                        }
                        orderDts.Add(orderDt);
                    }
                }
                mt.orderDt = orderDts;
                OList.Add(mt);
            }
            return OList;
        }
        #endregion

        #region 订单汇总信息
        /// <summary>
        /// 订单汇总信息
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="userId">用户</param>
        /// <param name="num">前n条</param>
        /// <returns></returns>
        public List<OrdersMt> OrderQueryMt(string entId, string userId, int num)
        {

            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@type","UserCenter_Mt"),
                new SqlParameter("@entId",entId),
                new SqlParameter("@userId",userId),
                new SqlParameter("@num",num)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("proc_OrderQuery", parameter);
            List<OrdersMt> list = new List<OrdersMt>();
            if (dt.Rows.Count > 0)
            {
                list = SetOrderQuery(dt);
            }
            return list;
        }
        #endregion

        #region 订单详情
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="entId">企业Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="order_No">单据编号</param>
        /// <returns></returns>
        public List<OrdersMt> OrderQueryDt(string entId, string userId, string order_No)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","OrderQuery_Dt"),
                new SqlParameter("@entId",entId),
                new SqlParameter("@userId",userId),
                new SqlParameter("@strWhere",order_No)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("proc_OrderQuery", param);
            List<OrdersMt> list = new List<OrdersMt>();
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                list = SetOrderQueryDt(ds.Tables[0], ds.Tables[1]);
            }
            return list;
        }

        /// <summary>
        /// 填充订单详情
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<OrdersMt> SetOrderQueryDt(DataTable mt, DataTable dt)
        {
            var webUrl = BaseConfiguration.SercerIp;
            List<OrdersMt> list = new List<OrdersMt>();
            ///加载汇总信息
            DataRow mtr = mt.Rows[0];
            OrdersMt omt = new OrdersMt
            {
                BillNo = int.Parse(mtr["billno"].ToString().Trim()),
                Order_No = mtr["order_no"].ToString().Trim(),
                PaymentFee = decimal.Parse(mtr["payment_fee"].ToString().Trim()),
                PaymentTime = mtr["payment_time"].ToString().Trim(),
                PaymentName = mtr["PayType"].ToString().Trim(),
                DeliveryTime = mtr["ShipmentsTime"].ToString().Trim(),
                PayType = mtr["payWay"].ToString().Trim(),
                Accept_Name = mtr["accept_name"].ToString().Trim(),
                Telphone = mtr["telphone"].ToString().Trim(),
                Address = mtr["address"].ToString().Trim(),
                Discount_Amount = decimal.Parse(mtr["discount_amount"].ToString().Trim()),
                Order_Amount = decimal.Parse(mtr["order_amount"].ToString().Trim()),
                Real_Amount = decimal.Parse(mtr["real_amount"].ToString().Trim()),
                BonusAmount = decimal.Parse(mtr["BonusAmount"].ToString().Trim()),
                Postage = decimal.Parse(mtr["postage"].ToString().Trim()),
                Point = decimal.Parse(mtr["point"].ToString().Trim()),
                AddTime = mtr["add_time"].ToString().Trim(),
                Status = int.Parse(mtr["status"].ToString().Trim()),
                StatusName = mtr["StatusName"].ToString().Trim(),
                IsCriticism = mtr["isCriticism"].ToString().Trim(),
                ExpressCode=mtr["Code"].ToString().Trim(),
                ExpressNum=mtr["ExpressNum"].ToString().Trim(),
                ExpressName=mtr["ExpressName"].ToString().Trim(),
                Generate=mtr["generate"].ToString().Trim(),
                Thirdparty=mtr["thirdparty"].ToString().Trim()
            };

            List<OrderDt> listdt = new List<OrderDt>();
            foreach (DataRow dtr in dt.Rows)
            {
                OrderDt odt = new OrderDt
                {
                    Id = int.Parse(dtr["id"].ToString().Trim()),
                    Order_No = dtr["order_no"].ToString().Trim(),
                    Goods_Title = dtr["sub_title"].ToString().Trim(),
                    Article_Id = dtr["article_id"].ToString().Trim(),
                    Goods_Price = BasisConfig.ObjToDecimal(dtr["goods_price"].ToString(),BaseConfiguration.PricePlace,0.00M),
                    Real_Price = BasisConfig.ObjToDecimal(dtr["real_price"].ToString(), BaseConfiguration.PricePlace, 0.00M),
                    Quantity = BasisConfig.ObjToDecimal(dtr["quantity"].ToString(),BaseConfiguration.InventoryPlace,0.00M),
                    NetNumber = BasisConfig.ObjToDecimal(dtr["NetAmount"].ToString(), BaseConfiguration.InventoryPlace, 0.00M),
                    ReturnNumber = BasisConfig.ObjToDecimal(dtr["returnNum"].ToString(), BaseConfiguration.InventoryPlace, 0.00M),
                    TaxAmount = decimal.Parse(dtr["taxAmount"].ToString().Trim()),
                    cxbs = dtr["cxbs"].ToString().Trim(),
                    Drug_Factory = dtr["drug_factory"].ToString().Trim(),
                    Drug_Spec = dtr["drug_spec"].ToString().Trim(),
                    Package_Unit = dtr["package_unit"].ToString().Trim(),
                    Valdate = dtr["valdate"].ToString().Trim(),
                    Tag = dtr["tag"].ToString(),
                    IsCriticism = dtr["IsCriticism"].ToString().Trim(),
                    Status = int.Parse(dtr["Status"].ToString().Trim()),
                    StatusName = dtr["StatusName"].ToString()
                };
                //大包装控制 Y-取大包装 N-不取大包装
                string packControl = dtr["packControl"].ToString().Trim();
                //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                string scattered = dtr["scattered"].ToString().Trim();
                //Min_Package = BasisConfig.ObjToDecimal(dtr["min_package"].ToString().Trim(), 0.00M);
                if (packControl == "Y")
                {
                    odt.Min_Package = BasisConfig.ObjToDecimal(dtr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                }
                else
                {
                    if (scattered == "Y")
                    {
                        odt.Min_Package = BasisConfig.ObjToDecimal(dtr["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                    else
                    {
                        odt.Min_Package = BasisConfig.ObjToDecimal(dtr["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                }
                if (dtr["img_url"].ToString().Trim() != "")
                {
                    odt.ImgUrl = webUrl + dtr["img_url"].ToString().Trim();
                }
                else
                {
                    odt.ImgUrl = "";
                }
                listdt.Add(odt);
            }
            omt.orderDt = listdt;
            list.Add(omt);
            return list;
        }
        #endregion

        #region 退货订单详情
        /// <summary>
        /// 获取退货订单详情
        /// </summary>
        /// <param name="entId">企业Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="order_No">单据编号</param>
        /// <returns></returns>
        public List<OrdersMt> ReturnOrderQueryDt(string entId, string userId, string order_No, string status)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","ReturnOrderQuery_Dt"),
                new SqlParameter("@entId",entId),
                new SqlParameter("@userId",userId),
                new SqlParameter("@status",status),
                new SqlParameter("@strWhere",order_No)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("proc_OrderQuery", param);
            List<OrdersMt> list = new List<OrdersMt>();
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                list = SetReturnOrderQueryDt(ds.Tables[0], ds.Tables[1]);
            }
            return list;
        }

        /// <summary>
        /// 填充退货单详情
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<OrdersMt> SetReturnOrderQueryDt(DataTable mt, DataTable dt)
        {
            var webUrl = BaseConfiguration.SercerIp;
            List<OrdersMt> list = new List<OrdersMt>();
            ///加载汇总信息
            DataRow mtr = mt.Rows[0];
            OrdersMt omt = new OrdersMt
            {
                BillNo = int.Parse(mtr["billno"].ToString().Trim()),
                Order_No = mtr["order_no"].ToString().Trim(),
                PaymentFee = decimal.Parse(mtr["payment_fee"].ToString().Trim()),
                PaymentTime = mtr["payment_time"].ToString().Trim(),
                PaymentName = mtr["PayType"].ToString().Trim(),
                Accept_Name = mtr["accept_name"].ToString().Trim(),
                Telphone = mtr["telphone"].ToString().Trim(),
                Address = mtr["address"].ToString().Trim(),
                Discount_Amount = decimal.Parse(mtr["discount_amount"].ToString().Trim()),
                Order_Amount = decimal.Parse(mtr["order_amount"].ToString().Trim()),
                Real_Amount = decimal.Parse(mtr["real_amount"].ToString().Trim()),
                BonusAmount = decimal.Parse(mtr["BonusAmount"].ToString().Trim()),
                AddTime = mtr["add_time"].ToString().Trim(),
                Status = int.Parse(mtr["status"].ToString().Trim()),
                StatusName = mtr["StatusName"].ToString().Trim(),
                IsCriticism = mtr["isCriticism"].ToString().Trim()
            };
            List<OrderDt> listdt = new List<OrderDt>();
            foreach (DataRow dtr in dt.Rows)
            {
                OrderDt odt = new OrderDt
                {
                    Id = int.Parse(dtr["id"].ToString().Trim()),
                    Order_No = dtr["order_no"].ToString().Trim(),
                    Goods_Title = dtr["sub_title"].ToString().Trim(),
                    Article_Id = dtr["article_id"].ToString().Trim(),
                    Goods_Price = BasisConfig.ObjToDecimal(dtr["goods_price"].ToString(), BaseConfiguration.PricePlace, 0.00M),
                    Real_Price = BasisConfig.ObjToDecimal(dtr["real_price"].ToString(), BaseConfiguration.PricePlace, 0.00M),
                    Quantity = BasisConfig.ObjToDecimal(dtr["quantity"].ToString(), BaseConfiguration.InventoryPlace, 0.00M),
                    ReturnNumber = BasisConfig.ObjToDecimal(dtr["returnNum"].ToString(), BaseConfiguration.InventoryPlace, 0.00M),
                    TaxAmount = decimal.Parse(dtr["taxAmount"].ToString().Trim()),
                    cxbs = dtr["cxbs"].ToString().Trim(),
                    Drug_Factory = dtr["drug_factory"].ToString().Trim(),
                    Drug_Spec = dtr["drug_spec"].ToString().Trim(),
                    Package_Unit = dtr["package_unit"].ToString().Trim(),
                    Valdate = dtr["valdate"].ToString().Trim(),
                    Tag = dtr["tag"].ToString(),
                    IsCriticism = dtr["IsCriticism"].ToString().Trim(),
                    pihao = dtr["pihao"].ToString().Trim(),
                    shl = BasisConfig.ObjToDecimal(dtr["shl"].ToString(), BaseConfiguration.InventoryPlace, 0.00M),
                    hshj = BasisConfig.ObjToDecimal(dtr["hshj"].ToString(), BaseConfiguration.PricePlace, 0.00M),
                    hsje = decimal.Parse(dtr["hsje"].ToString().Trim()),
                    Status = int.Parse(dtr["Status"].ToString().Trim())
                };
                //大包装控制 Y-取大包装 N-不取大包装
                string packControl = dtr["packControl"].ToString().Trim();
                //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                string scattered = dtr["scattered"].ToString().Trim();
                if (packControl == "Y")
                {
                    odt.Min_Package = BasisConfig.ObjToDecimal(dtr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                }
                else
                {
                    if (scattered == "Y")
                    {
                        odt.Min_Package = BasisConfig.ObjToDecimal(dtr["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                    else
                    {
                        odt.Min_Package = BasisConfig.ObjToDecimal(dtr["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                }
                if (dtr["img_url"].ToString().Trim() != "")
                {
                    odt.ImgUrl = webUrl + dtr["img_url"].ToString().Trim();
                }
                else
                {
                    odt.ImgUrl = "";
                }
                listdt.Add(odt);
            }
            omt.orderDt = listdt;
            list.Add(omt);
            return list;
        }
        #endregion

        #region 退货申请
        /// <summary>
        /// 进行退货申请
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="detailsid"></param>
        /// <param name="imgurl"></param>
        /// <param name="remak"></param>
        /// <returns></returns>
        public bool UpOrderProStatus(string orderNo, string userId, string entId, string detailsid, string imgurl, string remak, out string message)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("@type","SaleReturn"),
                    new SqlParameter("@entId",entId),
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@orderNo",orderNo),
                    new SqlParameter("@detailsid",detailsid),
                    new SqlParameter("@imgurl",imgurl),
                    new SqlParameter("@remak",remak)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                DataTable dt = sql.RunProcedureDR("Proc_OrderStatus", param);
                if (dt.Rows.Count > 0)
                {
                    message = dt.Rows[0]["msg"].ToString();
                    return true;
                }
                else
                {
                    message = "申请退货失败,请刷新重试";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
                LogQueue.Write(LogType.Error, "OrderInfoDal/UpOrderProStatus", message);
                return false;
            }
        }
        #endregion

        #region 通过商户真实订单编号获取订单信息
        /// <summary>
        /// 通过商户真实订单编号获取订单信息
        /// </summary>
        /// <param name="orderNo">商户真实订单编号</param>
        /// <param name="entId">企业Id</param>
        /// <returns></returns>
        public List<Orders> GetOrderInfo(string orderNo/*, string entId*/)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","RealityOrder"),
                new SqlParameter("@orderNo",orderNo)//,
                //new SqlParameter("@Entid",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("proc_OrderQuery", param);
            List<Orders> OList = new List<Orders>();
            if (dt.Rows.Count > 0)
            {
                OList = SetOrderInfo(dt, null);
            }
            return OList;
        }
        /// <summary>
        /// 填充订单信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<Orders> SetOrderInfo(DataTable dt, DataTable GoodsDt)
        {
            List<Orders> OList = new List<Orders>();
            foreach (DataRow dr in dt.Rows)
            {
                Orders o = new Orders
                {
                    BillNo = int.Parse(dr["billno"].ToString().Trim()),
                    Order_No = dr["order_no"].ToString().Trim(),
                    UserId = dr["user_id"].ToString().Trim(),
                    BusinessId = dr["businessId"].ToString().Trim(),
                    PaymentId = dr["payment_id"].ToString().Trim(),
                    Initiationtime = dr["payment_initiationtime"].ToString().Trim(),
                    Generate = dr["generate"].ToString().Trim(),
                    Thirdparty = dr["thirdparty"].ToString().Trim(),
                    RefundFee = decimal.Parse(dr["refund_fee"].ToString()),
                    Url = dr["url"].ToString(),
                    PaymentName = dr["paytype"].ToString().Trim(),
                    PaymentFee = decimal.Parse(dr["payment_fee"].ToString().Trim()),
                    PaymentStatus = int.Parse(dr["payment_status"].ToString().Trim()),
                    PaymentTime = dr["payment_time"].ToString().Trim(),
                    PayType = dr["payWay"].ToString().Trim(),
                    Telphone = dr["telphone"].ToString().Trim(),
                    Address = dr["address"].ToString().Trim(),
                    Real_Amount = decimal.Parse(dr["real_amount"].ToString().Trim()),
                    Order_Amount = decimal.Parse(dr["order_amount"].ToString().Trim()),
                    Point = int.Parse(dr["point"].ToString().Trim()),
                    AddTime = dr["add_time"].ToString().Trim(),
                    Source = dr["Source"].ToString().Trim()
                };
                OList.Add(o);
            }
            return OList;
        }
        #endregion

        #region 通过支付临时订单号获取订单信息
        /// <summary>
        /// 通过支付临时订单号获取订单信息
        /// </summary>
        /// <param name="generate">支付临时订单号</param>
        /// <returns></returns>
        public List<Orders> GetOrderByNumber(string generate/*,string entId*/)
        {
            DataTable dt = new DataTable();
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select a.billno,a.order_no,a.user_id,a.accept_name,a.businessId,a.payment_id,a.payment_fee,a.payment_initiationtime,a.payment_status,a.payment_time,");
            //strSql.Append(" b.paytype,a.address,a.real_amount,a.order_amount,a.point,a.status,a.add_time,a.Source,a.telphone,a.address ");
            //strSql.Append(" from dt_orders a(nolock) join dt_payapi b(nolock) on a.payment_id=b.payid and a.entid=b.entid ");
            //strSql.Append(" join dt_orders_payment c(nolock) on a.order_no=c.orderNo and a.entid=c.entId ");
            //strSql.Append(" where c.generate=@generate /*and a.entid=@Entid*/ ");

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","TemporaryOrder"),
                new SqlParameter("@generate",generate)//,
                //new SqlParameter("@entid",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            //dt = sql.RunSqlDataTable(strSql.ToString(), param);
            dt = sql.RunProcedureDR("proc_OrderQuery", param);
            List<Orders> OList = new List<Orders>();
            if (dt.Rows.Count > 0)
            {
                OList = SetOrderInfo(dt, null);
            }
            return OList;
        }
        #endregion

        #region 在线支付成功订单回写
        /// <summary>
        /// 在线支付成功订单回写
        /// </summary>
        /// <param name="orderNo">商户真实支付单据编号</param>
        /// <param name="generate">商户支付临时单据编号</param>
        /// <param name="transactionId">微信/支付宝交易订单号</param>
        /// <param name="payFee">支付金额</param>
        /// <returns></returns>
        public int OrderPayUpdate(string orderNo, string generate, string transactionId, string payFee)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","OrderPayUpdate"),
                new SqlParameter("@Order_No",orderNo),
                new SqlParameter("@generate",generate),
                new SqlParameter("@transactionId",transactionId),
                new SqlParameter("@pay_fee",payFee)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            return sql.ExecuteNonQuery("Proc_OrderPay", param);
        }
        #endregion

        #region 在线支付失败订单回写
        /// <summary>
        /// 在线支付失败订单回写
        /// </summary>
        /// <param name="orderNo">商户真实支付单据编号</param>
        /// <param name="generate">商户支付临时单据编号</param>
        /// <returns></returns>
        public int OrderPayFailUpdate(string orderNo, string generate)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","OrderPayUpdate"),
                new SqlParameter("@Order_No",orderNo),
                new SqlParameter("@generate",generate)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            return sql.ExecuteNonQuery("Proc_OrderPay", param);
        }
        #endregion

        #region 订单在线支付发起时间
        /// <summary>
        /// 订单在线支付发起时间
        /// </summary>
        /// <param name="orderNo">商户真实订单单号</param>
        /// <param name="generate">商户支付临时单号</param>
        /// <param name="time">支付发起时间</param>
        /// <param name="entId">机构</param>
        /// <param name="fee">发起支付金额</param>
        /// <param name="payType">发起支付方式</param>
        /// <param name="url">二维码</param>
        /// <param name="equipment">发起支付的设备</param>
        /// <returns></returns>
        public int OrderPayTimeUpdate(string orderNo, string generate, string entId, string time, decimal fee, string payType,string url, string source, string equipment)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","OrderPayTimeUpdate"),
                new SqlParameter("@Order_No",orderNo),
                new SqlParameter("@time",time),
                new SqlParameter("@generate",generate),
                new SqlParameter("@entid",entId),
                new SqlParameter("@pay_fee",fee),
                new SqlParameter("@payType",payType),
                new SqlParameter("@url",url),
                new SqlParameter("@source",source),
                new SqlParameter("@equipment",equipment)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            return sql.ExecuteNonQuery("Proc_OrderPay", param);
        }
        #endregion

        #region 修正订单汇总支付发起时间及发起设备【不再修正时间只更改发起设备】
        /// <summary>
        /// 修正订单汇总支付发起时间及发起设备【不再修正时间只更改发起设备】
        /// </summary>
        /// <param name="orderNo">商户真实订单号</param>
        /// <param name="generate">商户支付流水号</param>
        /// <param name="time">修正时间</param>
        /// <param name="equipment">发起支付设备</param>
        /// <returns></returns>
        public int PayInitiationTimeCorrect(string orderNo,string generate, string time,string equipment)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","Correct"),
                new SqlParameter("@order_no",orderNo),
                new SqlParameter("@generate",generate),
                new SqlParameter("@time",time),
                new SqlParameter("@equipment",equipment)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            return sql.ExecuteNonQuery("Proc_OrderPay", param);
        }
        #endregion

        #region 订单在线退款记录
        /// <summary>
        /// 订单在线退款记录
        /// </summary>
        /// <param name="orderNo">商户真实订单单号</param>
        /// <param name="refundNo">商户退款临时单号</param>
        /// <param name="entId">机构</param>
        /// <param name="time">退款发起时间</param>
        /// <param name="fee">发起退款金额</param>
        /// <param name="payType">发起退款方式</param>
        /// <param name="equipment">发起支付设备</param>
        /// <returns></returns>
        public int OrderRefundRecord(string orderNo, string refundNo, string entId, string time, decimal fee, string payType,string source,string equipment)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","OrderRefundInitiate"),
                new SqlParameter("@order_no",orderNo),
                new SqlParameter("@time",time),
                new SqlParameter("@refundNo",refundNo),
                new SqlParameter("@entid",entId),
                new SqlParameter("@refund_fee",fee),
                new SqlParameter("@payType",payType),
                new SqlParameter("@source",source),
                new SqlParameter("@equipment",equipment)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            return sql.ExecuteNonQuery("Proc_OrderPay", param);
        }
        #endregion

        #region 订单在线退款回写
        /// <summary>
        /// 订单在线退款回写
        /// </summary>
        /// <param name="orderNo">商户真实退款单据编号</param>
        /// <param name="refundNo">商户退款临时单据编号</param>
        /// <param name="refundId">微信/支付宝交易订单号</param>
        /// <param name="payFee">退款金额</param>
        /// <param name="druge">Success【退款成功】Fail【退款失败】</param>
        /// <returns></returns>
        public int OrderRefundUpdate(string orderNo, string refundNo, string refundId, string payFee,string druge, string payType)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","OrderRefundResult"),
                new SqlParameter("@order_no",orderNo),
                new SqlParameter("@refundNo",refundNo),
                new SqlParameter("@refundId",refundId),
                new SqlParameter("@refund_fee",payFee),
                new SqlParameter("@druge",druge),
                new SqlParameter("@payType",payType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            return sql.ExecuteNonQuery("Proc_OrderPay", param);
        }
        #endregion

        #region 订单在线支付发起时间
        /// <summary>
        /// 订单在线支付发起时间
        /// </summary>
        /// <param name="orderNo">商户真实订单单号</param>
        /// <param name="generate">商户支付临时单号</param>
        /// <param name="time">支付发起时间</param>
        /// <param name="entId">机构</param>
        /// <param name="fee">发起支付金额</param>
        /// <param name="payType">发起支付方式</param>
        /// <param name="equipment">发起支付的设备</param>
        /// <returns></returns>
        public int OrderPayTimeUpdate(string orderNo, string generate, string entId, string time, decimal fee, string payType,string equipment)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","OrderPayTimeUpdate"),
                new SqlParameter("@Order_No",orderNo),
                new SqlParameter("@time",time),
                new SqlParameter("@generate",generate),
                new SqlParameter("@entid",entId),
                new SqlParameter("@pay_fee",fee),
                new SqlParameter("@payType",payType),
                new SqlParameter("@equipment",equipment)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            return sql.ExecuteNonQuery("Proc_OrderPay", param);
        }
        #endregion

        #region 指定订单最近支付流水记录
        /// <summary>
        /// 指定订单最近支付流水记录
        /// </summary>
        /// <param name="orderNo">商户真实订单号</param>
        /// <param name="payType">支付方式</param>
        /// <returns></returns>
        public List<PayRecord> InTwoHoursRecords(string orderNo/*, string entId*/,string payType)
        {
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","InTwoHours"),
                new SqlParameter("@order_no",orderNo),
                new SqlParameter("@payType",payType)
                //new SqlParameter("@Entid",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            dt = sql.RunProcedureDR("Proc_OrderPay", param);
            List<PayRecord> OList = new List<PayRecord>();
            if (dt.Rows.Count > 0)
            {
                OList = PayRecords(dt);
            }
            return OList;
        }

        #endregion

        #region 订单支付/退款 交易记录
        /// <summary>
        /// 订单支付/退款 交易记录
        /// </summary>
        /// <param name="orderNo">商户订单编号</param>
        /// <param name="entId">机构</param>
        /// <param name="type">交易类型 【在线支付】【在线退款】</param>
        /// <param name="status">交易状态 0【失败】1【发起】2【成功】</param>
        /// <returns></returns>
        public List<PayRecord> GetPayRecords(string orderNo,string entId,string type,int status)
        {
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type","PayRecord"),
                new SqlParameter("@order_no",orderNo),
                new SqlParameter("@paymenttype",type),
                new SqlParameter("@status",status)//,
                //new SqlParameter("@Entid",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            dt = sql.RunProcedureDR("Proc_OrderPay", param);
            List<PayRecord> OList = new List<PayRecord>();
            //LogQueue.Write(LogType.Debug, "条目数", OList.Count.ToString());
            if (dt.Rows.Count > 0)
            {
                OList = PayRecords(dt);
            }
            return OList;
        }

        /// <summary>
        /// 订单支付/退款 交易记录数据填充
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <returns></returns>
        public List<PayRecord> PayRecords(DataTable dt)
        {
            List<PayRecord> payRecords = new List<PayRecord>();
            foreach (DataRow item in dt.Rows)
            {
                PayRecord payRecord = new PayRecord()
                {
                    OrderNo = item["orderNo"].ToString(),
                    Generate = item["generate"].ToString(),
                    ThirdParty = item["thirdparty"].ToString(),
                    Fee = Convert.ToDecimal(item["fee"].ToString()),
                    PayStatus = int.Parse(item["paystatus"].ToString()),
                    PayType=item["payType"].ToString(),
                    Type = item["type"].ToString(),
                    EntId=item["entId"].ToString(),
                    Url=item["url"].ToString(),
                    AddTime=item["addtime"].ToString(),
                    LastModifyTime=item["lastmodifytime"].ToString()
                };
                payRecords.Add(payRecord);
            }
            return payRecords;
        }
        #endregion

        #region 订单状态
        /// <summary>
        /// 订单状态
        /// </summary>
        /// <returns></returns>
        public List<OrderStatus> OrderStatuses()
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","OrderStatus")
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_OrderStatus", parameters);
            List<OrderStatus> orders = new List<OrderStatus>();
            if (table.Rows.Count > 0)
            {
                orders = SetOrderStatus(table);
            }
            return orders;
        }
        /// <summary>
        /// 填充订单状态
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<OrderStatus> SetOrderStatus(DataTable table)
        {
            List<OrderStatus> orders = new List<OrderStatus>();
            foreach (DataRow item in table.Rows)
            {
                OrderStatus orderStatus = new OrderStatus
                {
                    Name = item["Name"].ToString().Trim(),
                    Status = item["Status"].ToString().Trim()
                };
                orders.Add(orderStatus);
            }
            return orders;
        }
        #endregion

        #region 订单重算
        /// <summary>
        /// 订单重算(账期付款98折，线上付款97折)
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="userId">用户</param>
        /// <param name="goodsList">购物车Id</param>
        /// <param name="payType">支付方式</param>
        /// <param name="couponId">优惠券Id</param>
        /// <param name="bonusId">红包Id</param>
        /// <param name="clientType">客户类型</param>
        /// <param name="Pricelevel">价格等级</param>
        /// <param name="isDis">是否计算折扣（98、97折）</param>
        /// <returns></returns>
        public List<OrderAmount> OrderCalculate(string entId, string userId, string goodsList,string Pricelevel,string clientType, string payType, string couponId,string bonusId,bool isDis,string ywyId="")
        {
            ///获取购物车信息
            ///获取购物车商品列表
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@UserId",userId),
              new SqlParameter("@ywyId",ywyId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@Jgjb",Pricelevel),
              new SqlParameter("@List",goodsList),
              new SqlParameter("@clientType",clientType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable cart = sql.RunProcedureDR("Proc_CalculatePretreatment", param);
            string couponCode = "", SceneCoding = "", types = "", SceneId = "", ProductCode = "", IsFree = "N";
            decimal deduction = 0, discount = 0, bonusAmount = 0;

            #region 优惠券信息
            if (couponId != "")
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@type","SingleCoupon"),
                    new SqlParameter("@entId",entId),
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@couponId",couponId)
                };
                DataTable coup = sql.RunProcedureDR("Proc_OperationCoupon", parameters);
                couponCode = coup.Rows[0]["couponCode"].ToString();//优惠券编号
                SceneCoding = coup.Rows[0]["SceneCoding"].ToString();//优惠券范围
                types = coup.Rows[0]["types"].ToString();//优惠券类型
                SceneId = coup.Rows[0]["SceneId"].ToString();//范围主键
                ProductCode = coup.Rows[0]["ProductCode"].ToString();//优惠券赠品
                deduction = decimal.Parse(coup.Rows[0]["deduction"].ToString());//满减
                discount = decimal.Parse(coup.Rows[0]["discount"].ToString());//折扣
            }
            #endregion

            #region 红包信息
            if (bonusId != "")
            {
                SqlParameter[] sqls = new SqlParameter[] {
                    new SqlParameter("@type","BonusDetial"),
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@entId",entId),
                    new SqlParameter("@bonusId",bonusId)
                };
                DataTable bonus = sql.RunProcedureDR("Proc_OperationRecharge", sqls);
                if (bonus.Rows.Count > 0)
                {
                    bonusAmount = decimal.Parse(bonus.Rows[0]["bonusAmount"].ToString());
                }
            }
            #endregion

            //*o 金额 *z 折扣 *j 减额 
            decimal pto = 0, ptz = 0, ptj = 0, dpo = 0, dpz = 0, dpj = 0, ppo = 0, ppz = 0, ppj = 0, flo = 0, flz = 0, flj = 0, qco = 0, qcz = 0, qcj = 0, zho = 0, zhz = 0, zhj = 0;

            #region 商品分组
            //普通
            DataTable ptt = cart.Clone();
            DataRow[] ptr = cart.Select("GoodsType='SP'");
            foreach (DataRow item in ptr)
            {
                ptt.ImportRow(item);
            }
            //单品
            DataTable dpt = cart.Clone();
            DataRow[] dpr = cart.Select("PromScenario='3'and Fabh<>'' and GoodsType<>'SP' ");
            foreach (DataRow item in dpr)
            {
                dpt.ImportRow(item);
            }
            //品牌
            DataTable ppt = cart.Clone();
            DataRow[] ppr = cart.Select("PromScenario='2'and Fabh<>'' and GoodsType<>'SP' ");
            foreach (DataRow item in ppr)
            {
                ppt.ImportRow(item);
            }
            //分类
            DataTable flt = cart.Clone();
            DataRow[] flr = cart.Select("PromScenario='1'and Fabh<>'' and GoodsType<>'SP' ");
            foreach (DataRow item in flr)
            {
                flt.ImportRow(item);
            }
            //组合
            DataTable zht = cart.Clone();
            DataRow[] zhr = cart.Select("PromScenario='0' and Fabh<>'' and GoodsType='GZH' ");
            foreach (DataRow item in zhr)
            {
                zht.ImportRow(item);
            }
            //全场
            DataTable qct = cart.Clone();
            DataRow[] qcr = cart.Select("PromScenario='0' and Fabh<>'' and GoodsType not in('GZH','SP') ");
            foreach (DataRow item in qcr)
            {
                qct.ImportRow(item);
            }
            #endregion

            //普通
            if (ptr.Length > 0)
            {
                pto = ptt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                ptz = 0;
                ptj = 0;
                if (SceneCoding == "0")//全部商品
                {
                    if (types == "0" || types == "1" || types == "7" || types == "8")//减额
                    {
                        if (pto < deduction)
                        {
                            ptj = pto;
                        }
                        else
                        {
                            ptj = deduction;
                        }
                    }
                    else if (types == "2" || types == "6" || types == "9")//折扣
                    {
                        ptz += (pto - ptz) - ((pto - ptz) * discount);
                    }
                    else if (types == "5")//免邮
                    {
                        IsFree = "Y";
                    }
                }
                else if (SceneCoding == "1")//分类商品
                {
                    DataTable flsp = cart.Clone();
                    DataRow[] row = ptt.Select("classify='" + SceneId.ToString() + "'");
                    foreach (DataRow item in row)
                    {
                        flsp.ImportRow(item);
                    }
                    decimal a = flsp.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                    foreach (var r in row)
                    {
                        flsp.ImportRow(r);
                    }
                    if (types == "0" || types == "1" || types == "7" || types == "8")//减额
                    {
                        if (pto < deduction)
                        {
                            ptj += pto;
                        }
                        else
                        {
                            ptj += deduction;
                        }
                    }
                    else if (types == "2" || types == "6" || types == "9")//折扣
                    {
                        ptz += a - (a * discount);
                    }
                    else if (types == "5")//免邮
                    {
                        IsFree = "Y";
                    }
                }
                else if (SceneCoding == "2")//品牌商品
                {
                    DataTable flsp = cart.Clone();
                    DataRow[] row = ptt.Select("brand='" + SceneId.ToString() + "'");
                    foreach (DataRow item in row)
                    {
                        flsp.ImportRow(item);
                    }
                    decimal a = flsp.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                    if (types == "0" || types == "1" || types == "7" || types == "8")//减额
                    {
                        if (pto < deduction)
                        {
                            ptj += pto;
                        }
                        else
                        {
                            ptj += deduction;
                        }
                    }
                    else if (types == "2" || types == "6" || types == "9")//折扣
                    {
                        ptz += a - (a * discount);
                    }
                    else if (types == "5")//免邮
                    {
                        IsFree = "Y";
                    }
                }
                else if (SceneCoding == "3")//独立商品
                {
                    DataTable flsp = cart.Clone();
                    DataRow[] row = ptt.Select("article_id='" + SceneId.ToString() + "'");
                    foreach (DataRow item in row)
                    {
                        flsp.ImportRow(item);
                    }
                    decimal a = flsp.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                    if (types == "0" || types == "1" || types == "7" || types == "8")//减额
                    {
                        if (pto < deduction)
                        {
                            ptj += pto;
                        }
                        else
                        {
                            ptj += deduction;
                        }
                    }
                    else if (types == "2" || types == "6" || types == "9")//折扣
                    {
                        ptz += a - (a * discount);
                    }
                    else if (types == "5")//免邮
                    {
                        IsFree = "Y";
                    }
                }
            }
            //单品
            if (dpr.Length > 0)
            {
                //单品原金额
                dpo = dpt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                //单品折扣金额
                dpz = dpo-dpt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price")) * (Convert.ToDecimal(d.Field<int>("discount"))/100)).Sum();
                //单品减额
                dpj = dpt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("derate"))).Sum();
                if ((dpo-dpz)<dpj)
                {
                    dpj = dpo-dpz;
                }
            }
            //品牌
            if (ppr.Length > 0)
            {
                var query = from p in ppt.AsEnumerable()
                            group p by new { t1 = p.Field<string>("Fabh"), t2 = p.Field<int>("discount"),t3=p.Field<decimal>("derate") } into brands
                            select new { Fabh = brands.Key.t1, Discount = brands.Key.t2,Derate=brands.Key.t3 };

                foreach (var brand in query)
                {
                    DataTable dt = cart.Clone();
                    DataRow[] rows = ppt.Select("Fabh='"+ brand.Fabh.ToString() + "'");
                    
                    foreach (DataRow item in rows)
                    {
                        dt.ImportRow(item);
                    }
                    decimal o, z, j;
                    
                    //品牌原金额
                    o = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                    //品牌折扣金额
                    z = o - (o * brand.Discount/100);
                    //品牌减额
                    j = brand.Derate;
                    if ((o-z)<j)
                    {
                        j = (o-z);
                    }
                    ppo += o;
                    ppz += z;
                    ppj += j;
                }
            }
            //分类
            if (flr.Length > 0)
            {
                var query = from p in flt.AsEnumerable()
                            group p by new { t1 = p.Field<string>("Fabh"), t2 = p.Field<int>("discount"), t3 = p.Field<decimal>("derate") } into classify
                            select new { Fabh = classify.Key.t1, Discount = classify.Key.t2, Derate = classify.Key.t3 };

                foreach (var classifty in query)
                {
                    DataTable table = cart.Clone();
                    DataRow[] rows = flt.Select("Fabh='" + classifty.Fabh.ToString() + "'");
                    foreach (DataRow item in rows)
                    {
                        table.ImportRow(item);
                    }
                    decimal o, z, j;
                    //品牌原金额
                    o = table.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                    //品牌折扣金额
                    z = o - (o * Convert.ToDecimal(classifty.Discount.ToString())/100);
                    //品牌减额
                    j = Convert.ToDecimal(classifty.Derate.ToString());
                    if ((o-z)<j)
                    {
                        j = (o-z);
                    }
                    flo += o;
                    flz += z;
                    flj += j;
                }
            }
            //组合
            if (zhr.Length > 0)
            {
                zho = zht.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                zhz = 0;
                zhj = zht.AsEnumerable().Select(d=>Convert.ToDecimal(d.Field<decimal>("derate"))).Sum();
                if (zho<zhj)
                {
                    zhj = zho;
                }
            }
            //全场
            if (qcr.Length > 0)
            {
                var query = from p in qct.AsEnumerable()
                            group p by new { t1 = p.Field<string>("Fabh"), t2 = p.Field<int>("discount"), t3 = p.Field<decimal>("derate") } into overall
                            select new { Fabh = overall.Key.t1, Discount = overall.Key.t2, Derate = overall.Key.t3 };

                foreach (var overall in query)
                {
                    DataTable table = cart.Clone();
                    DataRow[] rows = qct.Select("Fabh='" + overall.Fabh.ToString() + "'");
                    foreach (DataRow item in rows)
                    {
                        table.ImportRow(item);
                    }
                    decimal o, z, j;
                    //全场原金额
                    o = table.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                    //全场折扣金额
                    z = o - (o * Convert.ToDecimal(overall.Discount.ToString())/100);
                    //全场减额
                    j = Convert.ToDecimal(overall.Derate.ToString());
                    if ((o-z)<j)
                    {
                        j = (o-z);
                    }
                    qco += o;
                    qcz += z;
                    qcj += j;
                }
            }

            var zo = pto + dpo + ppo + flo + qco + zho;//总金额
            var zz = ptz + dpz + ppz + flz + qcz + zhz;//总折扣金额
            var zj = ptj + dpj + ppj + flj + qcj + zhj;//总减免金额
            //线下付款折扣、线上付款折扣
            string lineDiscount = "100";
            //折扣门槛
            decimal threshold = Convert.ToDecimal(BaseConfiguration.Threshold);
            var realAmount = zo - zz - zj - bonusAmount;
            GainDiscount(entId, realAmount, out decimal offLine, out decimal onLine);
            if (payType == "100000" && isDis)
            {
                //if ((pto - ptz - ptj) >= threshold)
                    lineDiscount = offLine.ToString();// BaseConfiguration.OffLine.ToString();//线下付款折扣
            }
            else if ((payType == "100009" || payType == "100003") && isDis)
            {
                //if ((pto - ptz - ptj) >= threshold)
                    lineDiscount = onLine.ToString();//BaseConfiguration.OnLine.ToString();//线上付款折扣 余额支付折扣
            }
            else
            {
                lineDiscount = "100";
            }
            //普通商品进行线下付款折扣、线上付款折扣
            zz += realAmount - (realAmount * Convert.ToDecimal(lineDiscount) / 100);

            OrderAmount amount = new OrderAmount
            {
                OrdersAmount = Math.Round(zo,2),
                DiscountAmount = Math.Round(zz + zj,2),
                RealAmount = Math.Round(zo - zz - zj - bonusAmount,2),
                PtAmount = Math.Round(pto - ptz - ptj,2),
                BonusAmount = Math.Round(bonusAmount,2),
                DAAmount = Math.Round(ptz+ ptj, 2),
                Free = IsFree,
                lineDiscount=decimal.Parse(lineDiscount)
            };
            if (couponCode!="")
            {
                amount.CouponCode = int.Parse(couponCode);
            }
            else
            {
                amount.CouponCode = 0;
            }
            List<OrderAmount> list = new List<OrderAmount>
            {
                amount
            };
            return list;
        }

        /// <summary>
        /// 获取折扣
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void GainDiscount(string entId,decimal realAmount, out decimal offLine, out decimal onLine)
        {
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@type","FirstRules"),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@realAmount",realAmount)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("Proc_Admin_Rules", param);
            if (dt.Rows.Count > 0)
            {
                offLine = Convert.ToDecimal(dt.Rows[0]["offLine"].ToString());
                onLine = Convert.ToDecimal(dt.Rows[0]["onLine"].ToString());
            }
            else
            {
                offLine = Convert.ToDecimal(BaseConfiguration.OffLine.ToString());
                onLine = Convert.ToDecimal(BaseConfiguration.OnLine.ToString());
            }
        }

        /// <summary>
        /// 订单原始金额
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="userId">用户</param>
        /// <param name="goodsList">购物车Id</param>
        /// <param name="couponId">优惠券Id</param>
        /// <param name="bonusId">红包Id</param>
        /// <returns></returns>
        public List<OrderAmount> OrderOriginalAmount(string entId, string userId, string goodsList, string Pricelevel, string clientType, string couponId, string bonusId,string ywyId)
        {
            ///获取购物车信息
            ///获取购物车商品列表
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@UserId",userId),
              new SqlParameter("@ywyId",ywyId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@Jgjb",Pricelevel),
              new SqlParameter("@List",goodsList),
              new SqlParameter("@clientType",clientType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable cart = sql.RunProcedureDR("Proc_CalculatePretreatment", param);
            string couponCode = "", SceneCoding = "", types = "", SceneId = "", ProductCode = "", IsFree = "N";
            decimal deduction = 0, discount = 0, bonusAmount = 0;

            #region 优惠券信息
            if (couponId != "")
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@type","SingleCoupon"),
                    new SqlParameter("@entId",entId),
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@couponId",couponId)
                };
                DataTable coup = sql.RunProcedureDR("Proc_OperationCoupon", parameters);
                couponCode = coup.Rows[0]["couponCode"].ToString();//优惠券编号
                SceneCoding = coup.Rows[0]["SceneCoding"].ToString();//优惠券范围
                types = coup.Rows[0]["types"].ToString();//优惠券类型
                SceneId = coup.Rows[0]["SceneId"].ToString();//范围主键
                ProductCode = coup.Rows[0]["ProductCode"].ToString();//优惠券赠品
                deduction = decimal.Parse(coup.Rows[0]["deduction"].ToString());//满减
                discount = decimal.Parse(coup.Rows[0]["discount"].ToString());//折扣
            }
            #endregion

            #region 红包信息
            if (bonusId != "")
            {
                SqlParameter[] sqls = new SqlParameter[] {
                    new SqlParameter("@type","BonusDetial"),
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@entId",entId),
                    new SqlParameter("@bonusId",bonusId)
                };
                DataTable bonus = sql.RunProcedureDR("Proc_OperationRecharge", sqls);
                if (bonus.Rows.Count > 0)
                {
                    bonusAmount = decimal.Parse(bonus.Rows[0]["bonusAmount"].ToString());
                }
            }
            #endregion

            //*o 金额 *z 折扣 *j 减额 
            decimal pto = 0, ptz = 0, ptj = 0, dpo = 0, dpz = 0, dpj = 0, ppo = 0, ppz = 0, ppj = 0, flo = 0, flz = 0, flj = 0, qco = 0, qcz = 0, qcj = 0, zho = 0, zhz = 0, zhj = 0;

            //普通
            DataTable ptt = cart.Clone();
            DataRow[] ptr = cart.Select("GoodsType='SP'");
            foreach (DataRow item in ptr)
            {
                ptt.ImportRow(item);
            }
            //单品
            DataTable dpt = cart.Clone();
            DataRow[] dpr = cart.Select("PromScenario='3'and Fabh<>'' and GoodsType<>'SP' ");
            foreach (DataRow item in dpr)
            {
                dpt.ImportRow(item);
            }
            //品牌
            DataTable ppt = cart.Clone();
            DataRow[] ppr = cart.Select("PromScenario='2'and Fabh<>'' and GoodsType<>'SP' ");
            foreach (DataRow item in ppr)
            {
                ppt.ImportRow(item);
            }
            //分类
            DataTable flt = cart.Clone();
            DataRow[] flr = cart.Select("PromScenario='1'and Fabh<>'' and GoodsType<>'SP' ");
            foreach (DataRow item in flr)
            {
                flt.ImportRow(item);
            }
            //组合
            DataTable zht = cart.Clone();
            DataRow[] zhr = cart.Select("PromScenario='0' and Fabh<>'' and GoodsType='GZH' ");
            foreach (DataRow item in zhr)
            {
                zht.ImportRow(item);
            }
            //全场
            DataTable qct = cart.Clone();
            DataRow[] qcr = cart.Select("PromScenario='0' and Fabh<>'' and GoodsType not in('GZH','SP') ");
            foreach (DataRow item in qcr)
            {
                qct.ImportRow(item);
            }

            if (ptr.Length > 0)//普通
            {
                pto = ptt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                ptz = 0;
                ptj = 0;
                if (couponId != "")
                {
                    if (SceneCoding == "0")//全部商品
                    {
                        if (types == "0" || types == "1" || types == "7" || types == "8")//减额
                        {
                            if (pto < deduction)
                            {
                                ptj = pto;
                            }
                            else
                            {
                                ptj = deduction;
                            }
                        }
                        else if (types == "2" || types == "6" || types == "9")//折扣
                        {
                            ptz = pto - (pto * discount);
                        }
                        else if (types == "5")//免邮
                        {
                            IsFree = "Y";
                        }
                    }
                    else if (SceneCoding == "1")//分类商品
                    {
                        DataTable flsp = cart.Clone();
                        DataRow[] row = ptt.Select("classify='" + SceneId.ToString() + "'");
                        foreach (DataRow item in row)
                        {
                            flsp.ImportRow(item);
                        }
                        decimal a = flsp.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                        foreach (var r in row)
                        {
                            flsp.ImportRow(r);
                        }
                        if (types == "0" || types == "1" || types == "7" || types == "8")//减额
                        {
                            if (pto < deduction)
                            {
                                ptj = pto;
                            }
                            else
                            {
                                ptj = deduction;
                            }
                        }
                        else if (types == "2" || types == "6" || types == "9")//折扣
                        {
                            ptz = a - (a * discount);
                        }
                        else if (types == "5")//免邮
                        {
                            IsFree = "Y";
                        }
                    }
                    else if (SceneCoding == "2")//品牌商品
                    {
                        DataTable flsp = cart.Clone();
                        DataRow[] row = ptt.Select("brand='" + SceneId.ToString() + "'");
                        foreach (DataRow item in row)
                        {
                            flsp.ImportRow(item);
                        }
                        decimal a = flsp.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                        if (types == "0" || types == "1" || types == "7" || types == "8")//减额
                        {
                            if (pto < deduction)
                            {
                                ptj = pto;
                            }
                            else
                            {
                                ptj = deduction;
                            }
                        }
                        else if (types == "2" || types == "6" || types == "9")//折扣
                        {
                            ptz = a - (a * discount);
                        }
                        else if (types == "5")//免邮
                        {
                            IsFree = "Y";
                        }
                    }
                    else if (SceneCoding == "3")//独立商品
                    {
                        DataTable flsp = cart.Clone();
                        DataRow[] row = ptt.Select("article_id='" + SceneId.ToString() + "'");
                        foreach (DataRow item in row)
                        {
                            flsp.ImportRow(item);
                        }
                        decimal a = flsp.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                        if (types == "0" || types == "1" || types == "7" || types == "8")//减额
                        {
                            if (pto < deduction)
                            {
                                ptj = pto;
                            }
                            else
                            {
                                ptj = deduction;
                            }
                        }
                        else if (types == "2" || types == "6" || types == "9")//折扣
                        {
                            ptz = a - (a * discount);
                        }
                        else if (types == "5")//免邮
                        {
                            IsFree = "Y";
                        }
                    }
                }
            }
            if (dpr.Length > 0)//单品
            {
                //单品原金额
                dpo = dpt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                //单品折扣金额
                dpz = dpo - dpt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price")) * (Convert.ToDecimal(d.Field<int>("discount")) / 100)).Sum();
                //单品减额
                dpj = dpt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("derate"))).Sum();
                if ((dpo - dpz) < dpj)
                {
                    dpj = dpo - dpz;
                }
            }
            if (ppr.Length > 0)//品牌
            {
                var query = from p in ppt.AsEnumerable()
                            group p by new { t1 = p.Field<string>("Fabh"), t2 = p.Field<int>("discount"), t3 = p.Field<decimal>("derate") } into brands
                            select new { Fabh = brands.Key.t1, Discount = brands.Key.t2, Derate = brands.Key.t3 };

                foreach (var brand in query)
                {
                    DataTable dt = cart.Clone();
                    DataRow[] rows = ppt.Select("Fabh='" + brand.Fabh.ToString() + "'");

                    foreach (DataRow item in rows)
                    {
                        dt.ImportRow(item);
                    }
                    decimal o, z, j;

                    //品牌原金额
                    o = dt.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                    //品牌折扣金额
                    z = o - (o * brand.Discount / 100);
                    //品牌减额
                    j = brand.Derate;
                    if ((o - z) < j)
                    {
                        j = (o - z);
                    }
                    ppo += o;
                    ppz += z;
                    ppj += j;
                }
            }
            if (flr.Length > 0)//分类
            {
                var query = from p in flt.AsEnumerable()
                            group p by new { t1 = p.Field<string>("Fabh"), t2 = p.Field<int>("discount"), t3 = p.Field<decimal>("derate") } into classify
                            select new { Fabh = classify.Key.t1, Discount = classify.Key.t2, Derate = classify.Key.t3 };

                foreach (var classifty in query)
                {
                    DataTable table = cart.Clone();
                    DataRow[] rows = flt.Select("Fabh='" + classifty.Fabh.ToString() + "'");
                    foreach (DataRow item in rows)
                    {
                        table.ImportRow(item);
                    }
                    decimal o, z, j;
                    //品牌原金额
                    o = table.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                    //品牌折扣金额
                    z = o - (o * Convert.ToDecimal(classifty.Discount.ToString()) / 100);
                    //品牌减额
                    j = Convert.ToDecimal(classifty.Derate.ToString());
                    if ((o - z) < j)
                    {
                        j = (o - z);
                    }
                    flo += o;
                    flz += z;
                    flj += j;
                }
            }
            if (zhr.Length > 0)//组合
            {
                zho = zht.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                zhz = 0;
                zhj = zht.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("derate"))).Sum();
                if (zho < zhj)
                {
                    zhj = zho;
                }
            }
            if (qcr.Length > 0)//全场
            {
                var query = from p in qct.AsEnumerable()
                            group p by new { t1 = p.Field<string>("Fabh"), t2 = p.Field<int>("discount"), t3 = p.Field<decimal>("derate") } into overall
                            select new { Fabh = overall.Key.t1, Discount = overall.Key.t2, Derate = overall.Key.t3 };

                foreach (var overall in query)
                {
                    DataTable table = cart.Clone();
                    DataRow[] rows = qct.Select("Fabh='" + overall.Fabh.ToString() + "'");
                    //LogQueue.Write(LogType.Debug, "全场", overall.Fabh.ToString());
                    //LogQueue.Write(LogType.Debug, "全场discount", overall.Discount.ToString());
                    foreach (DataRow item in rows)
                    {
                        table.ImportRow(item);
                    }
                    decimal o, z, j;
                    //全场原金额
                    o = table.AsEnumerable().Select(d => Convert.ToDecimal(d.Field<decimal>("quantity")) * Convert.ToDecimal(d.Field<decimal>("price"))).Sum();
                    //LogQueue.Write(LogType.Debug, "全场o", o.ToString());
                    //全场折扣金额
                    z = o - (o * Convert.ToDecimal(overall.Discount.ToString()) / 100);

                    //LogQueue.Write(LogType.Debug, "全场z", z.ToString());
                    //全场减额
                    j = Convert.ToDecimal(overall.Derate.ToString());
                    if ((o - z) < j)
                    {
                        j = (o - z);
                    }
                    qco += o;
                    qcz += z;
                    qcj += j;
                }
                //LogQueue.Write(LogType.Debug, "全场qcz", qcz.ToString());
            }


            var zo = pto + dpo + ppo + flo + qco + zho;//总金额
            var zz = ptz + dpz + ppz + flz + qcz + zhz;//总折扣金额
            var zj = ptj + dpj + ppj + flj + qcj + zhj;//总减免金额

            OrderAmount amount = new OrderAmount
            {
                OrdersAmount = zo,
                DiscountAmount = zz + zj,
                RealAmount = zo - zz - zj - bonusAmount,
                PtAmount = pto - ptz - ptj,
                BonusAmount = bonusAmount,
                DAAmount = ptz + ppz + flz + qcz + zhz + ptj + ppj + flj + qcj + zhj,
                Free = IsFree
            };
            if (couponCode != "")
            {
                amount.CouponCode = int.Parse(couponCode);
            }
            else
            {
                amount.CouponCode = 0;
            }
            List<OrderAmount> list = new List<OrderAmount>
            {
                amount
            };
            return list;
        }

        /// <summary>
        /// 订单重算数据填充
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<OrderAmount> FillAmount(DataTable table)
        {
            List<OrderAmount> list = new List<OrderAmount>();
            foreach (DataRow item in table.Rows)
            {
                OrderAmount amount = new OrderAmount
                {
                    OrdersAmount = decimal.Parse(item["OrdersAmount"].ToString()),
                    RealAmount = decimal.Parse(item["RealAmount"].ToString()),
                    DiscountAmount = decimal.Parse(item["DiscountAmount"].ToString()),
                    Free = item["Free"].ToString().Trim(),
                    CouponCode = int.Parse(item["CouponCode"].ToString())
                };
                list.Add(amount);
            }
            return list;
        }
        #endregion

        #region 优惠券赠品
        /// <summary>
        /// 优惠券赠品数据
        /// </summary>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        public List<Gift> GetGifts(int couponCode)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","CouponGift"),
                new SqlParameter("@couponCode",couponCode)
            };

            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_OperationCoupon", parameters);
            var list = new List<Gift>();
            if (table.Rows.Count > 0)
            {
                list = FillGift(table);
            }
            return list;
        }
        /// <summary>
        /// 填充优惠券赠品数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Gift> FillGift(DataTable table)
        {
            var list = new List<Gift>();
            foreach (DataRow item in table.Rows)
            {
                Gift gift = new Gift
                {
                    GoodsId = item["goodsid"].ToString().Trim(),
                    GoodsName = item["goodsname"].ToString().Trim(),
                    DrugSpec = item["drug_spec"].ToString().Trim(),
                    DrugFactory = item["drug_factory"].ToString().Trim(),
                    Price = int.Parse(item["Price"].ToString()),
                    Quantity = int.Parse(item["quantity"].ToString())
                };
                list.Add(gift);
            }
            return list;
        }
        #endregion

        #region 支付方式
        /// <summary>
        /// 支付方式
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="rank">级别</param>
        /// <returns></returns>
        public List<Payment> PaymentType(string entId, string rank)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PayId,PayType,LogoUrl from dt_payapi where EntId=@entId and Rank=@rank and IsPayment='Y'");
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@entId",entId),
                new SqlParameter("@rank",rank)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunSqlDataTable(strSql.ToString(), sqls);
            List<Payment> Payments;
            if (table.Rows.Count > 0)
            {
                return Payments = FillPaymentApi(table);
            }
            else
            {
                return Payments = null;
            }
        }
        /// <summary>
        /// 填充充值方式
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static List<Payment> FillPaymentApi(DataTable table)
        {
            string webUrl = BaseConfiguration.SercerIp;
            List<Payment> payments = new List<Payment>();
            foreach (DataRow item in table.Rows)
            {
                Payment payment = new Payment
                {
                    PayId = item["PayId"].ToString(),
                    PayType = item["PayType"].ToString()
                };
                if (item["PayType"].ToString() == "")
                {
                    payment.LogoUrl = "";
                }
                else
                {
                    payment.LogoUrl = webUrl + item["LogoUrl"].ToString();
                }
                payments.Add(payment);
            }
            return payments;
        }
        #endregion

        #region 消费流水
        /// <summary>
        /// 消费流水
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <returns></returns>
        public List<ExpenseCalendar> ExpenseCalendar(string userId, string entId, int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","ExpenseCalendar"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("proc_OrderQuery", sqls);
            List<ExpenseCalendar> expenseCalendars = new List<ExpenseCalendar>();
            if (set.Tables.Count > 0)
            {
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
                return expenseCalendars = FillExpenseCalendar(set.Tables[1]);
            }
            else
            {
                pageCount = 1;
                recordCount = 0;
                return expenseCalendars = null;
            }
        }

        /// <summary>
        /// 消费流水数据填充
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<ExpenseCalendar> FillExpenseCalendar(DataTable table)
        {
            List<ExpenseCalendar> expenseCalendars = new List<ExpenseCalendar>();

            foreach (DataRow item in table.Rows)
            {
                ExpenseCalendar expenseCalendar = new ExpenseCalendar
                {
                    RecordNo = item["RecordNo"].ToString(),
                    Addtime = item["Addtime"].ToString(),
                    Balance = decimal.Parse(item["Balance"].ToString()),
                    Money = decimal.Parse(item["Money"].ToString()),
                    PayType = item["PayType"].ToString(),
                    Platform = item["Platform"].ToString(),
                    Remake = item["Remake"].ToString()
                };
                if (item["Type"].ToString() == "-1")
                {
                    expenseCalendar.Type = "支出";
                }
                else
                {
                    expenseCalendar.Type = "收入";
                }
                expenseCalendars.Add(expenseCalendar);
            }
            return expenseCalendars;
        }
        #endregion

        #region 订单状态分组条目统计
        /// <summary>
        /// 订单状态分组条目统计
        /// </summary>
        /// <param name="entId">企业</param>
        /// <param name="userId">用户</param>
        public List<OrderStatus> ItemsStatistics(string entId,string userId)
        {
            SqlParameter[] sqls = new SqlParameter[]
            {
                new SqlParameter("@type","ItemsStatistics"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@entid",entId)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunSqlDataTable("proc_OrderQuery", sqls);
            List<OrderStatus> statuses = new List<OrderStatus>();
            if (table.Rows.Count>0)
            {

            }
            return statuses;
        }
        #endregion
    }
}