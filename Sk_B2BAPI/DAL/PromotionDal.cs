using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sk_B2BAPI.DAL
{
    public class PromotionDal
    {
        #region 组合专区
        /// <summary>
        /// 获取组合促销信息
        /// </summary>
        /// <returns></returns>
        public List<PromMt> GetGroupInfo(string entId, string userId, string promType, string jgjb, string khType,bool staleDated,bool landing)
        {
            ///获取购物车商品列表
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@Type","GetGroup"),
              new SqlParameter("@UserId",userId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@Jgjb",jgjb),
              new SqlParameter("@promType",promType),
              new SqlParameter("@khlb",khType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_PromotionInfo", param);
            List<PromMt> list = new List<PromMt>();
            if (ds.Tables.Count == 2 && ds.Tables[0].Rows.Count>0)
            {
                list = SetGroupInfo(ds.Tables[0], ds.Tables[1], staleDated, landing);
            }
            return list;
        }
        /// <summary>
        ///获取单个组合信息
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="userId">用户</param>
        /// <param name="fabh">活动编号</param>
        /// <returns></returns>
        public DataTable GetGroupInfo(string entId, string userId,string fabh)
        {
            ///获取购物车商品列表
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@Type","GetGroupOne"),
              new SqlParameter("@UserId",userId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@fabh",fabh)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("Proc_PromotionInfo", param);
            return dt;
        }
        /// <summary>
        /// 填充数据(PromMt)
        /// </summary>
        /// <param name="promMt"></param>
        /// <param name="promDt"></param>
        /// <returns></returns>
        protected List<PromMt> SetGroupInfo(DataTable dtMt, DataTable dtDt, bool staleDated, bool landing)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            List<PromMt> mtList = new List<PromMt>();
            foreach (DataRow drMt in dtMt.Rows)
            {
                PromMt mt = new PromMt();
                mt.EntId = drMt["entid"].ToString();
                mt.Fabh = drMt["fabh"].ToString();
                mt.FaTitle = drMt["faTitle"].ToString();
                mt.Fabs = drMt["fabs"].ToString();
                mt.StartDate = drMt["startDate"].ToString();
                mt.EndDate = drMt["endDate"].ToString();
                mt.Amount = BasisConfig.ObjToDecimal(drMt["amount"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                mt.YAmount = BasisConfig.ObjToDecimal(drMt["yamount"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                mt.KhAmount = BasisConfig.ObjToDecimal(drMt["khAmount"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                mt.Describe = drMt["describe"].ToString();
                DataRow[] drDt_1 = dtDt.Select("fabh='" + mt.Fabh + "' and entid='" + mt.EntId + "'", "fa_sn asc");
                List<PromDt> dtList = new List<PromDt>();
                foreach (DataRow drDt in drDt_1)
                {
                    PromDt dt = new PromDt();
                    dt.Fabh = drDt["fabh"].ToString();
                    dt.Fa_Sn =int.Parse(drDt["fa_sn"].ToString());
                    dt.Article_Id = drDt["article_id"].ToString();
                    dt.Sub_Title = drDt["sub_title"].ToString();
                    dt.Drug_Factory = drDt["drug_factory"].ToString();
                    dt.Drug_Spec = drDt["drug_spec"].ToString();
                    dt.Big_Package = BasisConfig.ObjToDecimal(drDt["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    //大包装控制 Y-取大包装 N-不取大包装
                    string packControl = drDt["packControl"].ToString().Trim();
                    //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                    string scattered = drDt["scattered"].ToString().Trim();
                    if (packControl == "Y")
                    {
                        dt.Min_Package = BasisConfig.ObjToDecimal(drDt["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                    else
                    {
                        if (scattered == "Y")
                        {
                            dt.Min_Package = BasisConfig.ObjToDecimal(drDt["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                        }
                        else
                        {
                            dt.Min_Package = BasisConfig.ObjToDecimal(drDt["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                        }
                    }
                    dt.Stock_Quantity = BasisConfig.ObjToDecimal(drDt["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                    dt.GoodsLimit= drDt["goodslimit"].ToString();
                    if (landing && !staleDated)
                    {
                        dt.Price = BasisConfig.ObjToDecimal(drDt["price"].ToString(), BaseConfiguration.PricePlace, 0.00M).ToString();
                        dt.PromPrice = BasisConfig.ObjToDecimal(drDt["giftprice"].ToString(), BaseConfiguration.PricePlace, 0.00M).ToString();
                        dt.Limit= drDt["limit"].ToString();
                    }
                    else
                    {
                        dt.Price = "会员可见";
                        dt.PromPrice = "会员可见";
                        dt.Limit = "Y";
                    }
                    dt.MeetCount = decimal.Parse(drDt["meetCount"].ToString());
                    dt.Discount = decimal.Parse(drDt["Discount"].ToString());
                    dt.GiftId = drDt["GiftId"].ToString();
                    dt.Quantity = BasisConfig.ObjToDecimal(drDt["giftquantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                    dt.MaxQuantity = BasisConfig.ObjToDecimal(drDt["giftquantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                    dt.Package_Unit = drDt["package_unit"].ToString();
                    if (drDt["Img_Url"].ToString() != "")
                    {
                        dt.Img_Url = web_url+drDt["img_url"].ToString();
                    }
                    else
                    {
                        dt.Img_Url = "";
                    }
                    dtList.Add(dt);
                }
                mt.PromDt = dtList;
                mtList.Add(mt);
            }
            return mtList;
        }
        #endregion

        #region 促销专区
        /// <summary>
        /// 获取促销专区信息
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="jgjb"></param>
        /// <param name="khType"></param>
        /// <returns></returns>
        public List<PromList> GetTopPromSingle(string entId, string userId, string promType, string jgjb, string khType, /*int num*/int pageIndex, int pageSize,bool landing, bool staleDated, out int recordCount, out int pageCount)
        {
            recordCount = 0;
            pageCount = 0;
            ///获取单品促销信息
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@Type","GetTopPromSingle"),
              new SqlParameter("@UserId",userId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@Jgjb",jgjb),
              new SqlParameter("@promType",promType),
              new SqlParameter("@khlb",khType),
              new SqlParameter("@pageIndex",pageIndex),
              new SqlParameter("@pageSize",pageSize),
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var ds = sql.RunProDataSet("Proc_PromotionInfo", param);
            List<PromList> list = new List<PromList>();
            if (ds.Tables.Count > 0)
            {
                list = SetPromSingle(ds.Tables[1], landing,staleDated);
                recordCount = int.Parse(ds.Tables[2].Rows[0]["recordCount"].ToString());
                pageCount = int.Parse(ds.Tables[2].Rows[0]["pageCount"].ToString());
            }
            return list;
        }
        protected List<PromList> SetPromSingle(DataTable dt,bool landing,bool staleDated)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            List<PromList> list = new List<PromList>();
            foreach (DataRow dr in dt.Rows)
            {
                PromList prom = new PromList
                {
                    EntId = dr["entid"].ToString(),
                    Fabh = dr["fabh"].ToString(),
                    FaTitle = dr["faTitle"].ToString(),
                    Fabs = dr["fabs"].ToString(),
                    StartDate = dr["startDate"].ToString(),
                    EndDate = dr["endDate"].ToString(),
                    Amount = BasisConfig.ObjToDecimal(dr["Amount"].ToString(), BaseConfiguration.InventoryPlace,0.00M),
                    YAmount = BasisConfig.ObjToDecimal(dr["yAmount"].ToString(), BaseConfiguration.InventoryPlace,0.00M),
                    KhAmount = BasisConfig.ObjToDecimal(dr["KhAmount"].ToString(), BaseConfiguration.InventoryPlace,0.00M),
                    Describe = dr["describe"].ToString(),
                    Article_Id = dr["article_id"].ToString(),
                    Sub_Title = dr["sub_title"].ToString(),
                    Drug_Spec = dr["drug_spec"].ToString(),
                    Drug_Factory = dr["drug_factory"].ToString(),
                    Big_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M),
                    Stock_Quantity = BasisConfig.ObjToDecimal(dr["Stock_Quantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M),
                    GoodsLimit= dr["goodslimit"].ToString()
                };
                //大包装控制 Y-取大包装 N-不取大包装
                string packControl = dr["packControl"].ToString().Trim();
                //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                string scattered = dr["scattered"].ToString().Trim();
                if (packControl == "Y")
                {
                    prom.Min_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                }
                else
                {
                    if (scattered == "Y")
                    {
                        prom.Min_Package = BasisConfig.ObjToDecimal(dr["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                    else
                    {
                        prom.Min_Package = BasisConfig.ObjToDecimal(dr["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                }
                //价格
                if (landing && !staleDated)
                {
                    prom.Price = BasisConfig.ObjToDecimal(dr["price"].ToString(), BaseConfiguration.PricePlace, 0.00M).ToString();
                    prom.Limit = dr["limit"].ToString();
                }
                else
                {
                    prom.Price = "会员可见";
                    prom.Limit = "Y";
                }
                if (dr["img_url"].ToString()!="")
                {
                    prom.Img_Url = web_url + dr["img_url"].ToString();
                }
                else
                {
                    prom.Img_Url = "";
                }
                list.Add(prom);
            }
            return list;
        }
        #endregion

        #region 限时抢购
        /// <summary>
        /// 限时抢购
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="userId">用户</param>
        /// <param name="promType">活动类型</param>
        /// <param name="jgjb">价格级别</param>
        /// <param name="khType">客户类型</param>
        /// <param name="staleDated">证书是否过期</param>
        /// <param name="landing">是否登录</param>
        /// <returns></returns>
        public List<PromFlashSale> GetPromFlashSale(string entId, string userId, string promType, string jgjb, string khType,bool landing,bool staleDated)
        {
            ///获取单品促销信息
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@Type","GetFlashSale"),
              new SqlParameter("@UserId",userId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@Jgjb",jgjb),
              new SqlParameter("@promType",promType),
              new SqlParameter("@khlb",khType),
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("Proc_PromotionInfo", param);//源数据

            #region 重写
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //获取正在进行的商品组
            DataRow[] begin = dt.Select(" startDate <= '" + now + "' and endDate > '" + now + "' ");
            //LogQueue.Write(LogType.Debug, "正在进行", $"{begin.Length},now:{now}");
            DataTable dtBegin = dt.Clone();
            foreach (DataRow item in begin)
            {
                dtBegin.ImportRow(item);
            }
            //还未开始的商品组
            DataRow[] after = dt.Select(" startDate > '"+now+"'");
            //LogQueue.Write(LogType.Debug, "还未开始", $"{after.Length},now:{now}");
            DataTable dtAfter = dt.Clone();
            foreach (DataRow item in after)
            {
                dtAfter.ImportRow(item);
            }
            List<PromFlashSale> list = new List<PromFlashSale>();
            if (dt.Rows.Count>0)
            {
                list =  SetDate(dtBegin, dtAfter, landing,staleDated);
            }
            return list;
            #endregion
        }

        protected List<PromFlashSale> SetDate(DataTable tBegin, DataTable tAfter,bool landing,bool staleDated)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            List<PromFlashSale> pList = new List<PromFlashSale>();

            /*正在进行的活动商品*/
            if (tBegin.Rows.Count>0)
            {
                PromFlashSale mt = new PromFlashSale();
                mt.Title = "正在进行中";
                mt.Start = 2;
                mt.StartDate = tBegin.Rows[0]["startDate"].ToString();
                mt.EndDat = tBegin.Rows[0]["endDate"].ToString();
                List<PromList> beginList = new List<PromList>();
                foreach (DataRow dr in tBegin.Rows)
                {
                    PromList prom = new PromList();
                    prom.EntId = dr["entid"].ToString();
                    prom.EntId= dr["entname"].ToString();
                    prom.Fabh = dr["fabh"].ToString();
                    prom.FaTitle = dr["faTitle"].ToString();
                    prom.Fabs = dr["fabs"].ToString();
                    prom.StartDate = dr["startDate"].ToString();
                    prom.EndDate = dr["endDate"].ToString();
                    prom.Amount = BasisConfig.ObjToDecimal(dr["Amount"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                    prom.YAmount = BasisConfig.ObjToDecimal(dr["yAmount"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                    prom.KhAmount = BasisConfig.ObjToDecimal(dr["KhAmount"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                    prom.Describe = dr["describe"].ToString();
                    prom.Article_Id = dr["article_id"].ToString();
                    prom.Sub_Title = dr["sub_title"].ToString();
                    prom.Drug_Spec = dr["drug_spec"].ToString();
                    prom.Drug_Factory = dr["drug_factory"].ToString();
                    prom.Big_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    prom.GoodsLimit= dr["goodslimit"].ToString();
                    //大包装控制 Y-取大包装 N-不取大包装
                    string packControl = dr["packControl"].ToString().Trim();
                    //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                    string scattered = dr["scattered"].ToString().Trim();
                    if (packControl == "Y")
                    {
                        prom.Min_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                    else
                    {
                        if (scattered == "Y")
                        {
                            prom.Min_Package = BasisConfig.ObjToDecimal(dr["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                        }
                        else
                        {
                            prom.Min_Package = BasisConfig.ObjToDecimal(dr["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                        }
                    }
                    prom.Stock_Quantity = BasisConfig.ObjToDecimal(dr["Stock_Quantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                    prom.Quantity = BasisConfig.ObjToDecimal(dr["giftquantity"].ToString(), BaseConfiguration.InventoryPlace, 0.000M);
                    if (landing && !staleDated)
                    {
                        prom.Price = BasisConfig.ObjToDecimal(dr["price"].ToString(), BaseConfiguration.PricePlace, 0.00M).ToString();
                        prom.CPrice = BasisConfig.ObjToDecimal(dr["cprice"].ToString(), BaseConfiguration.PricePlace, 0.00M).ToString();
                        prom.Limit= dr["limit"].ToString();
                    }
                    else
                    {
                        prom.Price = "会员可见";
                        prom.CPrice = "会员可见";
                        prom.Limit = "Y";
                    }
                    if (dr["img_url"].ToString() != "")
                    {
                        prom.Img_Url = web_url + dr["img_url"].ToString();
                    }
                    else
                    {
                        prom.Img_Url = "";
                    }
                    beginList.Add(prom);
                }
                mt.list = beginList;
                pList.Add(mt);
            }

            /*未开始的活动商品*/
            if (tAfter.Rows.Count>0)
            {
                var query = from q in tAfter.AsEnumerable()
                            group q by new { t1 = q.Field<string>("StartDate"), t2 = q.Field<string>("endDate") } into active
                            select new { start = active.Key.t1, end = active.Key.t2 };
                foreach (var item in query)
                {
                    DataRow[] row = tAfter.Select(" StartDate='" + item.start.ToString() + "'  ");
                    DataTable table = tAfter.Clone();
                    foreach (var r in row)
                    {
                        table.ImportRow(r);
                    }
                    PromFlashSale prom = new PromFlashSale();
                    prom.Title = item.start.ToString() + "——" + item.end.ToString();
                    //prom.Title = item.start.ToString().Substring(11,5) +"-"+ item.end.ToString().Substring(11, 5);
                    prom.StartDate = item.start.ToString();
                    prom.EndDat = item.end.ToString();
                    prom.Start = 1;
                    List<PromList> afterList = new List<PromList>();
                    
                    foreach (DataRow dr in table.Rows)
                    {
                        PromList prom1 = new PromList();
                        prom1.EntId = dr["entname"].ToString();
                        prom1.EntId = dr["entid"].ToString();
                        prom1.Fabh = dr["fabh"].ToString();
                        prom1.FaTitle = dr["faTitle"].ToString();
                        prom1.Fabs = dr["fabs"].ToString();
                        prom1.StartDate = dr["startDate"].ToString();
                        prom1.EndDate = dr["endDate"].ToString();
                        prom1.Amount = BasisConfig.ObjToDecimal(dr["Amount"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                        prom1.YAmount = BasisConfig.ObjToDecimal(dr["yAmount"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                        prom1.KhAmount = BasisConfig.ObjToDecimal(dr["KhAmount"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                        prom1.Describe = dr["describe"].ToString();
                        prom1.Article_Id = dr["article_id"].ToString();
                        prom1.Sub_Title = dr["sub_title"].ToString();
                        prom1.Drug_Spec = dr["drug_spec"].ToString();
                        prom1.Drug_Factory = dr["drug_factory"].ToString();
                        prom1.Big_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                        prom1.GoodsLimit= dr["goodslimit"].ToString();
                        //大包装控制 Y-取大包装 N-不取大包装
                        string packControl = dr["packControl"].ToString().Trim();
                        //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                        string scattered = dr["scattered"].ToString().Trim();
                        if (packControl == "Y")
                        {
                            prom1.Min_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                        }
                        else
                        {
                            if (scattered == "Y")
                            {
                                prom1.Min_Package = BasisConfig.ObjToDecimal(dr["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                            }
                            else
                            {
                                prom1.Min_Package = BasisConfig.ObjToDecimal(dr["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                            }
                        }
                        prom1.Stock_Quantity = BasisConfig.ObjToDecimal(dr["Stock_Quantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                        prom1.Quantity = BasisConfig.ObjToDecimal(dr["giftquantity"].ToString(), BaseConfiguration.InventoryPlace, 0.000M);
                        if (landing && !staleDated)
                        {
                            prom1.Price = BasisConfig.ObjToDecimal(dr["price"].ToString(), BaseConfiguration.PricePlace, 0.00M).ToString();
                            prom1.CPrice = BasisConfig.ObjToDecimal(dr["cprice"].ToString(), BaseConfiguration.PricePlace, 0.00M).ToString();
                            prom1.Limit= dr["limit"].ToString();
                        }
                        else
                        {
                            prom1.Price = "会员可见";
                            prom1.CPrice = "会员可见";
                            prom1.Limit = "Y";
                        }
                        if (dr["img_url"].ToString() != "")
                        {
                            prom1.Img_Url = web_url + dr["img_url"].ToString();
                        }
                        else
                        {
                            prom1.Img_Url = "";
                        }
                        afterList.Add(prom1);
                    }
                    prom.list = afterList;
                    pList.Add(prom);
                }
            }
            return pList;
        }
        #endregion
    }
}