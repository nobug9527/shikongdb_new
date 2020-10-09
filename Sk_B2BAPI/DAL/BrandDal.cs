using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.DAL
{
    public class BrandDal
    {
        /// <summary>
        /// 获取品牌信息
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId">用户Id</param>
        /// <param name="jgjb">价格级别</param>
        /// <param name="khType">客户类型</param>
        /// <param name="khType">客户类型</param>
        /// <param name="khType">客户类型</param>
        /// <param name="khType">客户类型</param>
        /// <returns></returns>
        public List<BrandList> GetBrandList(string entId, string userId, string jgjb, string khType, bool landing,bool staleDated, string faType, string billno,int pageIndex,int pageSize,out int ecordCount, out int pageCount)
        {
            ecordCount = 0;
            pageCount = 0;
            ///获取单品促销信息
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@Type","GetBrandList"),
              new SqlParameter("@UserId",userId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@Jgjb",jgjb),
              new SqlParameter("@khlb",khType),
              new SqlParameter("@faType",faType),
              new SqlParameter("@billno",billno),
              new SqlParameter("@pageIndex",pageIndex),
              new SqlParameter("@pageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_BrandList", param);
            List<BrandList> list = new List<BrandList>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                list = SetBrand(ds,landing, staleDated);
                ecordCount = int.Parse(ds.Tables[3].Rows[0]["recordCount"].ToString());
                pageCount = int.Parse(ds.Tables[3].Rows[0]["pageCount"].ToString());
            }
            return list;
        }
        /// <summary>
        /// 填充BrandList
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public List<BrandList> SetBrand(DataSet ds, bool landing,bool staleDated)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            long InventoryShows = long.Parse(BaseConfiguration.InventoryShows);//页面商品库存最大显示数量
            List<BrandList> listMt = new List<BrandList>();
            foreach (DataRow drMt in ds.Tables[0].Rows)
            {
                ///汇总信息
                BrandList mt = new BrandList();
                mt.EntId = drMt["entid"].ToString();
                mt.BillNo = drMt["billno"].ToString();
                mt.Name = drMt["name"].ToString();
                mt.BeiZhu = drMt["beizhu"].ToString();
                mt.Sort_Id =int.Parse(drMt["sort_id"].ToString());
                mt.Fabh = drMt["fabh"].ToString();
                mt.Cxbs = drMt["cxbs"].ToString();
                if (drMt["img_url"].ToString() != "")
                {
                    mt.ImgUrl =web_url+drMt["img_url"].ToString();
                }
                else
                {
                    mt.ImgUrl = "";
                }
                if (drMt["img_url_dt"].ToString() != "")
                {
                    mt.ImgUrlDt = web_url + drMt["img_url_dt"].ToString();
                }
                else
                {
                    mt.ImgUrlDt = "";
                }
                DataRow[] dr = ds.Tables[2].Select("billno='" + drMt["billno"].ToString() + "'", "sort_id asc,dj_sn asc");
                ///详情商品信息
                List<GoodsInfo> listDt = new List<GoodsInfo>();
                
                foreach (DataRow drDt in dr)
                {
                    GoodsInfo gi = new GoodsInfo
                    {
                        Article_Id = drDt["article_id"].ToString(),
                        GoodsCode = drDt["goodscode"].ToString(),
                        Sub_Title = drDt["sub_title"].ToString(),
                        Drug_Spec = drDt["drug_spec"].ToString(),
                        Package_Unit = drDt["package_unit"].ToString(),
                        Drug_Factory = drDt["drug_factory"].ToString(),
                        Big_Package = BasisConfig.ObjToDecimal(drDt["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M),
                        Stock_Quantity = BasisConfig.ObjToDecimal(drDt["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace, 0.00M),
                        ApprovalNumber = drDt["approval_number"].ToString(),
                        ProposalPrice = BasisConfig.ObjToDecimal(drDt["proposalPrice"].ToString(), BaseConfiguration.PricePlace, 0.00M),
                        Valdate = drDt["valdate"].ToString(),
                        Scattered=drDt["scattered"].ToString(),
                        Fabh = drDt["fabh"].ToString(),
                        Cxbs = drDt["cxbs"].ToString(),
                        GoodsLimit=drDt["goodslimit"].ToString()
                    };
                    //大包装控制 Y-取大包装 N-不取大包装
                    string packControl = drDt["packControl"].ToString().Trim();
                    //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                    string scattered = drDt["scattered"].ToString().Trim();
                    if (packControl=="Y")
                    {
                        gi.Min_Package = BasisConfig.ObjToDecimal(drDt["big_package"].ToString(),BaseConfiguration.PackagePlace, 0.00M);
                    }
                    else
                    {
                        if (scattered == "Y")
                        {
                            gi.Min_Package = BasisConfig.ObjToDecimal(drDt["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                        }
                        else
                        {
                            gi.Min_Package = BasisConfig.ObjToDecimal(drDt["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                        }
                    }
                    //是否显示真实库存
                    if (InventoryShows > 0 && gi.Stock_Quantity > InventoryShows)
                    {
                        if (gi.Stock_Quantity>=1000) {
                            gi.Inventory = "充裕";
                        }
                        else {
                            gi.Inventory = "紧张";
                        }
                        
                    }
                    else
                    {
                        gi.Inventory = BasisConfig.ObjToDecimal(drDt["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M).ToString();
                    }
                    //判断用户是否登陆
                    if (landing && !staleDated)
                    {
                        gi.Price = BasisConfig.ObjToDecimal(drDt["price"].ToString(), BaseConfiguration.PricePlace,0.00M).ToString();
                        gi.Limit = drDt["limit"].ToString();
                    }
                    else
                    {
                        gi.Price = "会员可见";
                        gi.Limit = "Y";
                    }
                    if (drDt["img_url"].ToString() != "")
                    {
                        gi.ImgUrl = web_url + drDt["img_url"].ToString().Trim();
                    }
                    else
                    {
                        gi.ImgUrl = "";
                    }
                    listDt.Add(gi);
                }
                mt.GoodsList = listDt;
                listMt.Add(mt);
            }
            return listMt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="ArrondiType">诊所专区0/药店专区1</param>
        /// <param name="num">显示条目数</param>
        /// <param name="entId">机构</param>
        /// <param name="Pricelevel">价格级别</param>
        /// <returns></returns>
        public List<PromList> GetPharmacyList(string entId, string userId, string ArrondiType, string Pricelevel,string KhType, /*int num,*/int pageIndex,int pageSize,bool landing,bool staleDated, out int recordCount, out int pageCount)
        {
            recordCount = 0;
            pageCount = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","SaleArrondi"),
                new SqlParameter("@entId",entId),
                new SqlParameter("@userId",userId),
                new SqlParameter("@jgjb",Pricelevel),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@khlb",KhType),
                new SqlParameter("@ArrondiType",ArrondiType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            var ds = sql.RunProDataSet("Proc_PromotionInfo", parameters);
            List<PromList> lists = new List<PromList>();
            if (ds.Tables.Count>0)
            {
                lists = SetPromSingle(ds.Tables[1], landing, staleDated);
                recordCount = int.Parse(ds.Tables[2].Rows[0]["recordCount"].ToString());
                pageCount = int.Parse(ds.Tables[2].Rows[0]["pageCount"].ToString());
            }
            return lists;
        }

        protected List<PromList> SetPromSingle(DataTable dt,bool landing,bool staleDated)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            List<PromList> list = new List<PromList>();
            foreach (DataRow dr in dt.Rows)
            {
                PromList prom = new PromList();
                prom.EntId = dr["entid"].ToString();
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
                prom.Stock_Quantity = BasisConfig.ObjToDecimal(dr["Stock_Quantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                prom.ProposalPrice = BasisConfig.ObjToDecimal(dr["proposalPrice"].ToString(), BaseConfiguration.PricePlace,0.00M);
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
                if (dr["img_url"].ToString() != "")
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
    }
}