using Aop.Api.Domain;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace Sk_B2BAPI.DAL
{
    public class GoodsInfoDal
    {
        #region  获取商品列表 
        /// <summary>
        /// 搜索页获取商品列表
        /// </summary>
        /// <param name="userId">客户Id</param>
        /// <param name="searchValue">搜索条件</param>
        /// <param name="letter">厂家首字母</param>
        /// <param name="tags">排序类型</param>
        /// <param name="isKc">是否有货</param>
        /// <param name="CategoryId">商品分类Id</param>
        /// <param name="Login_Id">用户Id</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">没页行数</param>
        /// <returns></returns>
        public List<GoodsList> GetGoodsList(string userId, string searchValue, string letter, string tags, string isKc,string CategoryId,string Login_Id,int pageIndex, int pageSize,string entid,string zqType="")
        {
            //获取用户信息
            UserInfoDal dal = new UserInfoDal();
            List<UserInfo> user = new List<UserInfo>();
            if (!string.IsNullOrEmpty(userId))
            {
                user = dal.GetUserInfo(userId,entid);
            }
            string jgjb = "",clientlimit="", KhType = "";
            ///加载查询条件
            StringBuilder StrWhere = new StringBuilder();
            if (searchValue.Contains(" "))
            {
                int length = searchValue.Length;
                int index = searchValue.IndexOf(" ");
                string goodsValue, factoryValue;
                goodsValue = searchValue.Substring(0, index).Trim();
                factoryValue = searchValue.Substring(index, length - index).ToString().Trim();
                StrWhere.Append(" and ((b.mnemonic_code like '%" + goodsValue + "%' or b.sub_title like '%" + goodsValue + "%') and (b.drug_factory like '%" + factoryValue + "%' or b.Origin_mnemonic_code like '%" + factoryValue + "%' or  b.drug_spec like '%" + factoryValue + "%' ) ) ");
            }
            else
            {
                StrWhere.Append(" and(b.goodscode like '%" + searchValue + "%' or b.sub_title like '%" + searchValue + "%' or b.mnemonic_code like '%" + searchValue + "%'");
                StrWhere.Append(" or b.drug_factory like '%" + searchValue + "%' or b.Origin_mnemonic_code like '%" + searchValue + "%' or b.approval_number like '%"+ searchValue + "%' ) ");
            }
            
            
            //是否有货筛选
            if (isKc == "Y")
            {
                StrWhere.Append(" and c.stock_quantity>0 ");
            }
            if (zqType != "")
            {
                StrWhere.Append(" and exists(select 1  from dt_GoodsSaleArrondi zq(nolock) where b.article_id=zq.article_id and zq.ArrondiType='"+zqType+"') ");
            }
            bool landing = false;
            bool staleDated = false;
            //商品价格大于0筛选
            if (user.Count > 0)
            {
                StrWhere.Append(" and d.price>0.0");
                entid = user[0].EntId;
                jgjb = user[0].Pricelevel;
                clientlimit = user[0].ClientLimit;
                KhType = user[0].KhType;
                staleDated = user[0].StaleDated;
                landing = true;
            }
            ///厂家首字母筛选
            if (letter != "")
            {
                StrWhere.Append(" and left(b.Origin_mnemonic_code,1)='" + letter + "'");
            }

            StringBuilder order = new StringBuilder();
            order.Append(" ");
            //商品排序
            switch (tags)
            {
                case "click_u"://点击量升序
                    order.Append(" order by a.click asc,b.goodscode asc");
                    break;
                case "click_d"://点击量降序
                    order.Append(" order by a.click desc,b.goodscode desc");
                    break;
                case "sales_u"://销量升序
                    order.Append(" order by a.sales asc,b.goodscode asc");
                    break;
                case "sales_d"://销量降序
                    order.Append(" order by a.sales desc,b.goodscode desc");
                    break;
                case "price_u"://价格升序
                    order.Append(" order by d.price asc,b.goodscode asc");
                    break;
                case "price_d"://价格降序
                    order.Append(" order by d.price desc,b.goodscode desc");
                    break;
                case "cxbs"://促销排序
                    order.Append(" order by fabh desc,b.goodscode asc");
                    break;
                case "cjpx"://厂家排序
                    order.Append(" order by drug_factory asc");
                    break;
                case "sppx"://商品排序
                    order.Append(" order by sub_title asc");
                    break;
                default:
                    order.Append(" order by b.goodscode asc");
                    break;
            }
            //////统计客户搜索类容
            if (searchValue != "")
            {
                bool flag = StatisticalDal.GoodsStatistical("GoodsSearch", searchValue, entid);
            }
            ///获取商品列表
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@Type","GetList"),
              new SqlParameter("@UserId",userId),
              new SqlParameter("@Entid",entid),
              new SqlParameter("@PageSize",pageSize),
              new SqlParameter("@PageIndex",pageIndex),
              new SqlParameter("@StrWhere",StrWhere.ToString()),
              new SqlParameter("@Order",order.ToString()),
              new SqlParameter("@CategoryId",CategoryId),
              new SqlParameter("@Login_Id",Login_Id),
              new SqlParameter("@Jgjb",jgjb),
              new SqlParameter("@khlb",KhType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_GoodsList", param);
            List<GoodsList> list = new List<GoodsList>();
            if (ds.Tables.Count >0)
            {
                list = SetGoodsList(ds, landing,staleDated, pageIndex, pageSize,clientlimit);
            }
            return list;
        }
        /// <summary>
        /// 获取商品详情信息
        /// </summary>
        /// <param name="articleId">商品Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public List<GoodsList> GetGoodDetail(string userId, string articleId, string entId)
        {
            ///获取用户信息
            UserInfoDal dal = new UserInfoDal();
            List<UserInfo> user = new List<UserInfo>();
            if (!string.IsNullOrEmpty(userId))
            {
                user=dal.GetUserInfo(userId,entId);
            }
            string jgjb = "",clientlimit="";
            bool landing = false;
            bool staleDated = false;
            //获取客户价格级别
            if (user.Count > 0)
            {
                entId = user[0].EntId;
                jgjb = user[0].Pricelevel;
                clientlimit = user[0].ClientLimit;
                staleDated = user[0].StaleDated;
                landing = true;
            }
            //////统计客户搜索类容
            //bool flag = StatisticalDal.GoodsStatistical("GoodsClick", articleId, entId);
            ///获取商品列表
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@Type","GetDetail"),
              new SqlParameter("@UserId",userId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@article_Id",articleId),
              new SqlParameter("@Jgjb",jgjb)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_GoodsList", param);
            List<GoodsList> list = new List<GoodsList>();
            if (ds.Tables.Count>0)
            {
                ///获取商品详情图片
                ImgInfoDal idal = new ImgInfoDal();
                List<ImgInfo> ilist = idal.GetGoodsDetailImg(articleId);
                //加载信息
                list = SetGoodsList(ds, landing,staleDated, 1,30,clientlimit,ilist);
            }
            return list;
        }
        /// <summary>
        /// 填充数据SearchGoods
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private List<GoodsList> SetGoodsList(DataSet ds, bool landing, bool staleDated, int pageIndex=1, int pageSize=30,string clientlimit="", List<ImgInfo> ilist=null)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            List<GoodsList> list = new List<GoodsList>();
            GoodsList sg = new GoodsList();
            DataTable dtg;
            //分页信息
            if (ds.Tables.Count >= 2)
            {
                DataTable dtc = ds.Tables[1];
                sg.PageCount = int.Parse(dtc.Rows[0]["pageCount"].ToString());
                sg.RountCount = int.Parse(dtc.Rows[0]["recordCount"].ToString());
                dtg = ds.Tables[0];
            }
            else
            {
                sg.PageCount = 1;
                sg.RountCount = 1;
                dtg = ds.Tables[0];
            }
            sg.PageIndex = pageIndex;
            sg.PageSize = pageSize;
            ////商品信息
            List<Models.GoodsInfo> gList = new List<Models.GoodsInfo>();
            if (dtg.Rows.Count > 0)
            {
                long InventoryShows = long.Parse(BaseConfiguration.InventoryShows);//页面商品库存最大显示数量
                foreach (DataRow dr in dtg.Rows)
                {
                    Models.GoodsInfo gi = new Models.GoodsInfo();
                    gi.EntName= dr["entname"].ToString();
                    gi.Article_Id = dr["article_id"].ToString();
                    gi.GoodsCode = dr["goodscode"].ToString();
                    gi.Sub_Title = dr["sub_title"].ToString();
                    gi.Drug_Spec = dr["drug_spec"].ToString();
                    gi.Package_Unit = dr["package_unit"].ToString();
                    gi.Drug_Factory = dr["drug_factory"].ToString();
                    try
                    {
                        gi.GoodsOrigin = dr["GoodsOrigin"].ToString();
                    }
                    catch 
                    {
                        gi.GoodsOrigin = "";
                    }
                    gi.Category = dr["category"].ToString();
                    gi.Big_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    gi.Stock_Quantity = BasisConfig.ObjToDecimal(dr["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace, 0.00M);
                    gi.Cxbs = dr["cxbs"].ToString();
                    gi.Fabh = dr["fabh"].ToString();
                    gi.Describe = dr["Describe"].ToString();
                    gi.ProposalPrice = BasisConfig.ObjToDecimal(dr["proposalPrice"].ToString(), BaseConfiguration.PricePlace, 0.00M);
                    gi.CollectId = dr["collectId"].ToString();
                    gi.Valdate = dr["valdate"].ToString();
                    gi.OldValdate = dr["old_valdate"].ToString();
                    gi.Scattered = dr["scattered"].ToString();
                    gi.ApprovalNumber = dr["approval_number"].ToString();
                    gi.GoodsLimit = dr["goodslimit"].ToString().Trim();//gi.GoodsLimit = dr["goodslimit"].ToString().Trim().Replace("<", "\u003c").Replace(">", "\u003e");
                    gi.PassSum = int.Parse(dr["passSum"].ToString());//评论数
                    gi.RaveReviews = BasisConfig.ObjToDecimal(dr["raveReviews"].ToString(), 0.00M);//好评率
                    gi.ImgList = ilist;//商品图片列表】
                    gi.Abstract = dr["zhaiyao"].ToString();
                    //大包装控制 Y-取大包装 N-不取大包装
                    string packControl = dr["packControl"].ToString().Trim();
                    //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                    string scattered = dr["scattered"].ToString().Trim();
                    if (packControl == "Y")
                    {
                        gi.Min_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                    else
                    {
                        if (scattered == "Y")
                        {
                            gi.Min_Package = BasisConfig.ObjToDecimal(dr["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                        }
                        else
                        {
                            gi.Min_Package = BasisConfig.ObjToDecimal(dr["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                        }
                    }
                    try
                    {
                        gi.BatchNumber = dr["BatchNumber"].ToString().ToString();
                    }
                    catch { }
                    gi.Zbz = BasisConfig.ObjToDecimal(dr["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    //是否展示商品真实库存
                    if (InventoryShows > 0 && gi.Stock_Quantity > InventoryShows)
                    {
                        if (gi.Stock_Quantity >= 1000)
                        {
                            gi.Inventory = "充裕";
                        }
                        else
                        {
                            gi.Inventory = "紧张";
                        }
                    }
                    else
                    {
                        gi.Inventory = BasisConfig.ObjToDecimal(dr["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M).ToString();
                    }
                    //判断用户是否登陆
                    if (landing && !staleDated)
                    {
                        gi.Price = BasisConfig.ObjToDecimal(dr["price"].ToString(), BaseConfiguration.PricePlace,0.00M).ToString();
                        //判断用户是否限销
                        //var judge = JudgeLimit(clientlimit, dr["goodslimit"].ToString());
                        gi.Limit = dr["limit"].ToString().Trim();
                    }
                    else
                    {
                        gi.Price = "会员可见";
                        gi.Limit = "Y";
                    }
                    if (dr["img_url"].ToString()!= "")
                    {
                        gi.ImgUrl = web_url + dr["img_url"].ToString().Trim();
                    }
                    else
                    {
                        gi.ImgUrl = "";
                    }
                    if (dr.Table.Columns.Contains("content"))
                    {
                        gi.Content = dr["content"].ToString().Trim();
                    }
                    //加载促销信息
                    
                    gList.Add(gi);
                    
                }
            }
            sg.GoodsInfo = gList;
            list.Add(sg);
            return list;
        }
        #endregion
        
        #region 获取商品排行列表 
        /// <summary>
        /// 获取商品当月销量/点击量排行
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="Category"></param>
        /// <returns></returns>
        public List<GoodsStatistical> GetGoodsRanking(string userId, string entId, string category,int num)
        {
            SqlParameter[] sqls = new SqlParameter[]{
              new SqlParameter("@Type","Commendation"),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@Classify",category),
              new SqlParameter("@Ranking","True"),
              new SqlParameter("@Num",num)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("Proc_GoodsList", sqls);
            List<GoodsStatistical> list = new List<GoodsStatistical>();
            if (dt.Rows.Count > 0)
            {
                list = SetGoodsStatistical(dt);
            }
            return list;
        }
        /// <summary>
        /// 填充Model层（GoodsStatistical）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<GoodsStatistical> SetGoodsStatistical(DataTable dt)
        {
            List<GoodsStatistical> list = new List<GoodsStatistical>();
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            foreach (DataRow dr in dt.Rows)
            {
                GoodsStatistical gs = new GoodsStatistical();
                gs.Article_Id = dr["article_id"].ToString();
                gs.Sub_Title = dr["sub_title"].ToString();
                gs.Drug_Spec = dr["drug_spec"].ToString();
                gs.Num = int.Parse(dr["Num"].ToString());
                if (dr["img_url"].ToString() != "")
                {
                    gs.ImgUrl = web_url + dr["img_url"].ToString();
                }
                else
                {
                    gs.ImgUrl = "";
                }
                list.Add(gs);
            }
            return list;
        }
        #endregion

        #region 同类商品推荐
        /// <summary>
        /// 获取商品当月销量/点击量排行
        /// </summary>
        /// <param name="entId">企业</param>
        /// <param name="category">分类</param>
        /// <param name="num">条目数</param>
        /// <returns></returns>
        public List<Models.GoodsInfo> Commendation(string entId, string category, int num, string jgjb, string KhType, bool landing, bool staleDated)
        {
            SqlParameter[] sqls = new SqlParameter[]{
              new SqlParameter("@Type","Commendation"),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@Classify",category),
              new SqlParameter("@Ranking","False"),
              new SqlParameter("@Num",num),
              new SqlParameter("@Jgjb",jgjb),
              new SqlParameter("@khlb",KhType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("Proc_GoodsList", sqls);

            List<Models.GoodsInfo> list = new List<Models.GoodsInfo>();
            if (dt.Rows.Count > 0)
            {
                list = CommendationFill(dt,landing,staleDated);
            }
            return list;
        }
        public List<Models.GoodsInfo> CommendationFill(DataTable table, bool landing, bool staleDated)
        {
            List<Models.GoodsInfo> infos = new List<Models.GoodsInfo>();
            long InventoryShows = long.Parse(BaseConfiguration.InventoryShows);//页面商品库存最大显示数量
            string web_url = BaseConfiguration.SercerIp;
            foreach (DataRow item in table.Rows)
            {
                Models.GoodsInfo gi = new Models.GoodsInfo
                {
                    Article_Id = item["article_id"].ToString(),
                    GoodsCode = item["goodscode"].ToString(),
                    Sub_Title = item["sub_title"].ToString(),
                    Drug_Spec = item["drug_spec"].ToString(),
                    Package_Unit = item["package_unit"].ToString(),
                    Drug_Factory = item["drug_factory"].ToString(),
                    Category = item["category"].ToString(),
                    Big_Package = BasisConfig.ObjToDecimal(item["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M),
                    Stock_Quantity = BasisConfig.ObjToDecimal(item["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace, 0.00M),
                    Cxbs = item["cxbs"].ToString(),
                    Fabh = item["fabh"].ToString(),
                    Describe = item["Describe"].ToString(),
                    ProposalPrice = BasisConfig.ObjToDecimal(item["proposalPrice"].ToString(), BaseConfiguration.PricePlace, 0.00M),
                    CollectId = item["collectId"].ToString(),
                    Valdate = item["valdate"].ToString(),
                    OldValdate = item["old_valdate"].ToString(),
                    Scattered=item["scattered"].ToString(),
                    ApprovalNumber = item["approval_number"].ToString(),
                    GoodsLimit = item["goodslimit"].ToString().Trim(),//gi.GoodsLimit = item["goodslimit"].ToString().Trim().Replace("<", "\u003c").Replace(">", "\u003e");
                    PassSum = int.Parse(item["passSum"].ToString()),//评论数
                    RaveReviews = BasisConfig.ObjToDecimal(item["raveReviews"].ToString(), 0.00M)//好评率
                };
                //大包装控制 Y-取大包装 N-不取大包装
                string packControl = item["packControl"].ToString().Trim();
                //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                string scattered = item["scattered"].ToString().Trim();
                if (packControl == "Y")
                {
                    gi.Min_Package = BasisConfig.ObjToDecimal(item["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                }
                else
                {
                    if (scattered == "Y")
                    {
                        gi.Min_Package = BasisConfig.ObjToDecimal(item["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                    else
                    {
                        gi.Min_Package = BasisConfig.ObjToDecimal(item["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                }
                //是否展示商品真实库存
                if (InventoryShows > 0 && gi.Stock_Quantity > InventoryShows)
                {
                    if (gi.Stock_Quantity >= 1000)
                    {
                        gi.Inventory = "充裕";
                    }
                    else
                    {
                        gi.Inventory = "紧张";
                    }
                }
                else
                {
                    gi.Inventory = BasisConfig.ObjToDecimal(item["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace, 0.00M).ToString();
                }
                //判断用户是否登陆
                if (landing && !staleDated)
                {
                    gi.Price = BasisConfig.ObjToDecimal(item["price"].ToString(), BaseConfiguration.PricePlace, 0.00M).ToString();
                    //判断用户是否限销
                    //var judge = JudgeLimit(clientlimit, item["goodslimit"].ToString());
                    gi.Limit = item["limit"].ToString().Trim();
                }
                else
                {
                    gi.Price = "会员可见";
                    gi.Limit = "Y";
                }
                if (item["img_url"].ToString() != "")
                {
                    gi.ImgUrl = web_url + item["img_url"].ToString().Trim();
                }
                else
                {
                    gi.ImgUrl = "";
                }
                if (item.Table.Columns.Contains("content"))
                {
                    gi.Content = item["content"].ToString().Trim();
                }
                infos.Add(gi);
            }
            return infos;
        }
        //public List<GoodsStatistical> Commendation(string entId, string category, int num)
        //{
        //    SqlParameter[] sqls = new SqlParameter[]{
        //      new SqlParameter("@Type","Commendation"),
        //      new SqlParameter("@Entid",entId),
        //      new SqlParameter("@Classify",category),
        //      new SqlParameter("@Ranking","False"),
        //      new SqlParameter("@Num",num)
        //    };
        //    SqlRun sql = new SqlRun(SqlRun.sqlstr);
        //    DataTable dt = sql.RunProcedureDR("Proc_GoodsList", sqls);

        //    List<GoodsStatistical> list = new List<GoodsStatistical>();
        //    if (dt.Rows.Count > 0)
        //    {
        //        list = SetGoodsStatistical(dt);
        //    }
        //    return list;
        //}
        #endregion

        #region 促销详情
        /// <summary>
        /// 促销详情
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public List<Promotion> GetActivity(string userId, string entId, string articleId, string fabh)
        {
            ///获取用户信息
            UserInfoDal dal = new UserInfoDal();
            List<UserInfo> user = dal.GetUserInfo(userId, entId);
            string entid = BaseConfiguration.EntId;
            string jgjb = "", KhType = "";
            if (user.Count > 0)
            {
                entId = user[0].EntId;
                jgjb = user[0].Pricelevel;
                KhType = user[0].KhType;
            }
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","GetActivity"),
                new SqlParameter("@UserId",userId),
                new SqlParameter("@Entid",entId),
                new SqlParameter("@article_id",articleId),
                new SqlParameter("@fabh",fabh),
                new SqlParameter("@Jgjb",jgjb),
                new SqlParameter("@khlb",KhType)
            };

            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_GoodsList", parameters);
            var list = new List<Promotion>();
            if (table.Rows.Count>0)
            {
                list = FillList(table);
            }
            return list;
        }
        
        /// <summary>
        /// 促销详情数据填充
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Promotion> FillList(DataTable table)
        {
            List<Promotion> list = new List<Promotion>();
            foreach (DataRow item in table.Rows)
            {
                Promotion promotion = new Promotion()
                {
                    Fabh = item["fabh"].ToString().Trim(),
                    Fabs = item["fabs"].ToString().Trim(),
                    ClientType = item["clienttype"].ToString().Trim(),
                    ContentType = item["contenttype"].ToString().Trim(),
                    Describe = item["describe"].ToString().Trim(),
                    StartDate = item["startDate"].ToString().Trim(),
                    EndDate = item["endDate"].ToString().Trim(),
                    Discount = decimal.Parse(item["discount"].ToString()),
                    MeetCount = decimal.Parse(item["meetCount"].ToString()),
                    Goodsname = item["goodsname"].ToString().Trim(),
                    GiftQuantity = decimal.Parse(item["giftquantity"].ToString()),
                    GiftPrice = decimal.Parse(item["giftprice"].ToString()),
                    FaType = item["faType"].ToString(),
                    WebTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    XgAmount=item["xgAmount"].ToString()
                };
                list.Add(promotion);
            }
            return list;
        }
        #endregion

        #region 判断限销
        /// <summary>
        /// 判断限销
        /// </summary>
        /// <returns></returns>
        public bool JudgeLimit(string clientlimit,string goodslimit)
        {
            Log.Error("限销",$"客户限销:{clientlimit},商品限销：{goodslimit}");
            //客户控销 （单位），（分类），（区域）
            string[] client = Regex.Split(clientlimit, ",", RegexOptions.IgnoreCase);
            //商品控销 （可销单位），（可销分类），（可销区域）<不可销单位>，<可销分类>,<可销区域>
            string[] goods = Regex.Split(goodslimit, ",", RegexOptions.IgnoreCase);

            bool flag = true;
            if (goods[0].Replace("(","").Replace(")","") != "")//商品控销存在可销单位拦截  
            {
                if (!goods[0].Contains(client[0]))//不包含当前客户的单位
                {
                    flag = false;
                }
            }
            if (goods[1].Replace("(", "").Replace(")", "") != "")//商品控销存在可销分类拦截  
            {
                if (!goods[1].Contains(client[1]))//不包含当前客户所属客户分类
                {
                    flag = false;
                }
            }
            if (goods[2].Replace("(", "").Replace(")", "") != "")//商品控销存在可销区域拦截 
            {
                if (!goods[2].Contains(client[2]))//不包含当前客户所在的区域
                {
                    flag = false;
                }
            }
            if (goods[3].Replace("<", "").Replace(">", "") != "")//商品控销存在不可销单位拦截  
            {
                if (goods[3].Contains(client[0]))//包含当前客户的单位
                {
                    flag = false;
                }
            }
            if (goods[4].Replace("<", "").Replace(">", "") != "")//商品控销存在不可销分类拦截  
            {
                if (goods[4].Contains(client[1]))//包含当前客户所属客户分类
                {
                    flag = false;
                }
            }
            if (goods[5].Replace("<", "").Replace(">", "") != "") //商品控销存在不可销区域拦截
            {
                if (goods[5].Contains(client[2]))//包含当前客户所在区域
                {
                    flag = true;
                }
            }
            return flag;
        }
        #endregion

        #region 搜索栏索引
        /// <summary>
        /// 搜索栏索引
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<SearchIndex> SearchIndex(string entId,string parameter)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","SearchIndex"),
                new SqlParameter("@Entid",entId),
                new SqlParameter("@parameter",parameter)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_GoodsList", parameters);
            var list = new List<SearchIndex>();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow item in table.Rows)
                {
                    var index = new SearchIndex()
                    {
                        Index=item["title"].ToString().Trim()
                    };
                    list.Add(index);
                }
            }
            return list;
        }
        #endregion

        #region 搜索快速下单商品
        /// <summary>
        /// 搜索快速下单商品
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="searchValue">检索参数</param>
        /// <param name="jgjb">价格级别</param>
        /// <param name="landing">是否登录</param>
        /// <param name="staleDated">证书是否过期</param>
        /// <returns></returns>
        public List<GoodsList> SearchFastGoods(string entId, string searchValue,string jgjb,string khlb,bool landing, bool staleDated)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","SearchFastGoods"),
                new SqlParameter("@entid",entId),
                new SqlParameter("@strWhere",searchValue),
                new SqlParameter("@jgjb",jgjb),
                new SqlParameter("@khlb",khlb)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_Admin_GoodsList", parameters);
            List<GoodsList> goodsLists = new List<GoodsList>();
            if (table.Rows.Count > 0)
            {
                goodsLists=FillFastGoods(table, landing,staleDated);
            }
            return goodsLists;
        }

        public List<GoodsList> FillFastGoods(DataTable table,bool landing, bool staleDated)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            long InventoryShows = long.Parse(BaseConfiguration.InventoryShows);//页面商品库存最大显示数量
            List<GoodsList> goodsLists = new List<GoodsList>();
            GoodsList goodsList = new GoodsList();
            List<Models.GoodsInfo> gList = new List<Models.GoodsInfo>();
            foreach (DataRow item in table.Rows)
            {
                Models.GoodsInfo goodsInfo = new Models.GoodsInfo()
                {
                    Article_Id=item["article_id"].ToString(),
                    Sub_Title=item["sub_title"].ToString(),
                    Drug_Factory=item["drug_factory"].ToString(),
                    Drug_Spec=item["drug_spec"].ToString(),
                    Package_Unit=item["package_unit"].ToString(),
                    Valdate =item["valdate"].ToString(),
                    Stock_Quantity= BasisConfig.ObjToDecimal(item["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M),
                    ApprovalNumber=item["approval_number"].ToString(),
                    ProposalPrice= BasisConfig.ObjToDecimal(item["LingSPrice"].ToString(), BaseConfiguration.PricePlace,0.00M),
                    DosageForm=item["dosage_form"].ToString()
                };
                //大包装控制 Y-取大包装 N-不取大包装
                string packControl = item["packControl"].ToString().Trim();
                //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                string scattered = item["scattered"].ToString().Trim();
                if (packControl == "Y")
                {
                    goodsInfo.Min_Package = BasisConfig.ObjToDecimal(item["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                }
                else
                {
                    if (scattered == "Y")
                    {
                        goodsInfo.Min_Package = BasisConfig.ObjToDecimal(item["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                    else
                    {
                        goodsInfo.Min_Package = BasisConfig.ObjToDecimal(item["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                }
                //图片
                if (!string.IsNullOrEmpty(item["img_url"].ToString()))
                {
                    goodsInfo.ImgUrl = web_url + item["img_url"].ToString();
                }
                else
                {
                    goodsInfo.ImgUrl = "";
                }
                //是否展示商品真实数量
                if (Convert.ToDecimal(item["stock_quantity"].ToString())>0&& Convert.ToDecimal(item["stock_quantity"].ToString())> InventoryShows)
                {
                    if (Convert.ToDecimal(item["stock_quantity"].ToString()) >= 1000)
                    {
                        goodsInfo.Inventory = "充裕";
                    }
                    else
                    {
                        goodsInfo.Inventory = "紧张";
                    }
                }
                else
                {
                    goodsInfo.Inventory = BasisConfig.ObjToDecimal(item["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M).ToString();
                }
                if (landing && !staleDated)
                {
                    goodsInfo.Price = BasisConfig.ObjToDecimal(item["CaiGPrice"].ToString(), BaseConfiguration.PricePlace,0.00M).ToString();
                }
                else
                {
                    goodsInfo.Price = "会员可见";
                }
                gList.Add(goodsInfo);
            }
            goodsList.GoodsInfo = gList;
            goodsLists.Add(goodsList);
            return goodsLists;
        }
        #endregion

        #region 常购商品
        /// <summary>
        /// 常购商品
        /// </summary>
        /// <param name="entId">机构</param>
        /// <param name="searchValue">检索参数</param>
        /// <param name="jgjb">价格级别</param>
        /// <param name="landing">是否登录</param>
        /// <param name="staleDated">证书是否过期</param>
        /// <returns></returns>
        public List<GoodsList> OftenBuy(string userId,string entId, string searchValue, string jgjb,string khlb, bool landing, bool staleDated, int pageIndex, int pageSize,out int pageCount,out int recordCount)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","OftenBuy"),
                new SqlParameter("@UserId",userId),
                new SqlParameter("@entid",entId),
                new SqlParameter("@strWhere",searchValue),
                new SqlParameter("@jgjb",jgjb),
                new SqlParameter("@khlb",khlb),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pageSize)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("Proc_GoodsList", parameters);
            List<GoodsList> goodsLists = new List<GoodsList>();
            pageCount = 0;
            recordCount = 0;
            if (set.Tables.Count> 0)
            {
                pageCount = int.Parse(set.Tables[2].Rows[0]["pageCount"].ToString());
                recordCount = int.Parse(set.Tables[2].Rows[0]["recordCount"].ToString());
                goodsLists = FillFastGoods(set.Tables[1], landing, staleDated);
            }
            return goodsLists;
        }
        #endregion

        #region 评论总数据
        /// <summary>
        /// 评论数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="articleId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<StairCriticisms> GetCriticisms(string userId, string entId, string articleId, int pageIndex, int pageSize,string orderBy, out int pageCount, out int recordCount,out int passSum,out decimal raveReviews)
        {
            try
            {
                SqlParameter[] sqls = new SqlParameter[]
                {
                    new SqlParameter("@Type","ShowCriticism"),
                    new SqlParameter("@Entid",entId),
                    new SqlParameter("@UserId",userId),
                    new SqlParameter("@PageIndex",pageIndex),
                    new SqlParameter("@PageSize",pageSize),
                    new SqlParameter("@ArticleId",articleId)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                DataSet set = sql.RunProDataSet("Proc_OperationCriticisms", sqls);
                var list = new List<StairCriticisms>();
                pageCount = 1;
                recordCount = 0;
                passSum = 0;
                raveReviews = 0;
                if (set.Tables.Count > 0)
                {
                    if (set.Tables[0].Rows.Count>0)
                    {
                        list = Criticisms(set.Tables[0], orderBy);
                    }
                    else
                    {
                        list = null;
                    }
                    pageCount = int.Parse(set.Tables[1].Rows[0]["pageCount"].ToString());
                    recordCount = int.Parse(set.Tables[1].Rows[0]["recordCount"].ToString());
                    if (set.Tables[2].Rows.Count>0)
                    {
                        passSum = int.Parse(set.Tables[2].Rows[0]["passSum"].ToString());
                        raveReviews = decimal.Parse(set.Tables[2].Rows[0]["raveReviews"].ToString());
                    }
                    else
                    {
                        passSum = 0; raveReviews = 0;
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString().Trim());
            }
        }
        #endregion

        #region 评论数据
        public List<StairCriticisms> GetReplay(string userId, string entId, int criticismsId, int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            try
            {
                SqlParameter[] sqls = new SqlParameter[]
                {
                    new SqlParameter("@Type","MoreRepaly"),
                    new SqlParameter("@Entid",entId),
                    new SqlParameter("@UserId",userId),
                    new SqlParameter("@PageIndex",pageIndex),
                    new SqlParameter("@PageSize",pageSize),
                    new SqlParameter("@CriticismsId",criticismsId)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                DataSet set = sql.RunProDataSet("Proc_OperationCriticisms", sqls);
                var list = new List<StairCriticisms>();
                pageCount = 1;
                recordCount = 0;
                if (set.Tables.Count > 0)
                {
                    list = Criticisms(set.Tables[0],"desc");
                    pageCount = int.Parse(set.Tables[1].Rows[0]["pageCount"].ToString());
                    recordCount = int.Parse(set.Tables[1].Rows[0]["recordCount"].ToString());
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString().Trim());
            }
        }
        /// <summary>
        /// 评论数据填充
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<StairCriticisms> Criticisms(DataTable table,string orderBy)
        {
            //查询一级评论
            DataRow[] stairt;
            if (orderBy=="desc")
            {
                stairt = table.Select("rank=1", "addtime desc");
            }
            else
            {
                stairt = table.Select("rank=1", "addtime asc");
            }
            string webUrl = BaseConfiguration.SercerIp.ToString();
            List<StairCriticisms> criticisms = new List<StairCriticisms>();
            foreach (DataRow item in stairt)
            {
                StairCriticisms stairCriticisms = new StairCriticisms()
                {
                    UserName = item["username"].ToString().Trim(),
                    AddTime=item["addtime"].ToString().Trim(),
                    Content=item["content"].ToString().Trim(),
                    StarLevel= decimal.Parse(item["starLevel"].ToString()),
                    LikeSum =int.Parse(item["likeSum"].ToString().Trim()),
                    IsLike=item["islike"].ToString().Trim(),
                    Rank=int.Parse(item["rank"].ToString().Trim()),
                    Totality=int.Parse(item["totality"].ToString().Trim()),
                    Id= int.Parse(item["Id"].ToString().Trim())
                };
                //评论用户头像
                if (item["img_url"].ToString().Trim()=="")
                {
                    stairCriticisms.ImgUrl = "";
                }
                else
                {
                    stairCriticisms.ImgUrl = webUrl+item["img_url"].ToString().Trim();
                }
                List<Criticisms> Subordinates = new List<Criticisms>();
                if (int.Parse(item["totality"].ToString().Trim()) > 0)
                {
                    DataRow[] rows = table.Select("superiorId=" + item["superiorId"].ToString() + " and id<>" + item["superiorId"].ToString() + "");
                    foreach (DataRow row in rows)
                    {
                        Criticisms Subordinate = new Criticisms()
                        {
                            UserName = row["username"].ToString().Trim(),
                            AddTime = row["addtime"].ToString().Trim(),
                            LikeSum = int.Parse(row["likeSum"].ToString().Trim()),
                            IsLike = row["islike"].ToString().Trim(),
                            Rank = int.Parse(row["rank"].ToString().Trim()),
                            Id = int.Parse(row["Id"].ToString().Trim())
                        };
                        //评论用户头像
                        if (item["img_url"].ToString().Trim() == "")
                        {
                            stairCriticisms.ImgUrl = "";
                        }
                        else
                        {
                            stairCriticisms.ImgUrl = webUrl + item["img_url"].ToString().Trim();
                        }
                        //如果是3级评论，@拼接上级评论用户名：后跟上级评论内容
                        if (int.Parse(row["rank"].ToString().Trim()) == 3)
                        {
                            Subordinate.Content = row["content"].ToString().Trim() + "//@" + row["replayObj"].ToString().Trim() + ":" + row["objContent"].ToString().Trim();
                        }
                        else
                        {
                            Subordinate.Content = row["content"].ToString().Trim();
                        }
                        Subordinates.Add(Subordinate);
                    }
                }
                stairCriticisms.Subordinate = Subordinates;
                criticisms.Add(stairCriticisms);
            }
            return criticisms;
        }
        #endregion

        #region 编辑评论
        /// <summary>
        /// 编辑评论
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="atricleId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public int RedactCriticism(string userId,string entId, string orderNo,int id, string articleId,string content,decimal starLevel,int superiorId,int replayId,out string message)
        {
            try
            {
                SqlParameter[] sqls = new SqlParameter[]
                {
                    new SqlParameter("@Type","RedactCriticism"),
                    new SqlParameter("@Entid",entId),
                    new SqlParameter("@UserId",userId),
                    new SqlParameter("@Content",content),
                    new SqlParameter("@StarLevel",starLevel),
                    new SqlParameter("@OrderNo",orderNo),
                    new SqlParameter("@Id",id),
                    new SqlParameter("@ArticleId",articleId),
                    new SqlParameter("@SuperiorId",superiorId),
                    new SqlParameter("@ReplayId",replayId)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                int num = sql.ExecuteNonQuery("Proc_OperationCriticisms", sqls);
                if (num > 0)
                {
                    message = "评论发布成功！";
                    return 1;
                }
                else
                {
                    message = "评论发布失败！";
                    return 0;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString().Trim();
                return 0;
            }
        }
        #endregion

        #region 点赞评论
        /// <summary>
        /// 点赞评论
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <param name="criticismsId"></param>
        /// <returns></returns>
        public int LikeCriticism(string userId,string entId,int criticismsId,out string message)
        {
            try
            {
                SqlParameter[] sqls = new SqlParameter[]
                {
                    new SqlParameter("@Type","LikeCriticism"),
                    new SqlParameter("@Entid",entId),
                    new SqlParameter("@UserId",userId),
                    new SqlParameter("@CriticismsId",criticismsId)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                int num = sql.ExecuteNonQuery("Proc_OperationCriticisms", sqls);
                if (num > 0)
                {
                    message = "操作成功！";
                    return 1;
                }
                else
                {
                    message = "操作失败！";
                    return 0;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString().Trim();
                return 0;
            }
        }
        #endregion

        #region 求购
        public bool InsertReply(string buyName,string buyTel,string buyGoods,string productName, string buySpec,decimal buyNumber,decimal buyPrice,string userId,string entId,string message,out string msg)
        {
            try
            {
                SqlParameter[] parameter = new SqlParameter[]{
                        new SqlParameter("@type","InsertReply"),                   
                        new SqlParameter("@buyName",buyName),           
                        new SqlParameter("@buyTel",buyTel),              
                        new SqlParameter("@buyGoods",buyGoods),	        
                        new SqlParameter("@productName",productName),     
                        new SqlParameter("@buySpec",buySpec),	        
                        new SqlParameter("@buyNumber",buyNumber),	    
                        new SqlParameter("@buyPrice",buyPrice),	    
                        new SqlParameter("@userId",userId),		          
                        new SqlParameter("@entId",entId),		       
                        new SqlParameter("@message",message)            
                 };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);

                DataSet ds = sql.RunProDataSet("proc_Requrement", parameter);
                if (ds.Tables[0].Rows.Count>0)
                {
                    msg = ds.Tables[0].Rows[0]["msg"].ToString();
                    return true;
                }
                else
                {
                    msg = "未知异常";
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString().Trim();
                LogQueue.Write(LogType.Error, "GoodsInfoDal/Requrement", msg);
                return false;
            }
        }
        public RequrementList QueryReply(string userId,string entId, int pageIndex, int pageSize)
        {
            SqlParameter[] parameter = new SqlParameter[]{
                        new SqlParameter("@type","QueryReply"),                     //（insertReply，queryReply）
                        new SqlParameter("@userId",userId),		            //（账号id）
                        new SqlParameter("@entId",entId),		            //（机构id）
                        new SqlParameter("@pageIndex",pageIndex),
                        new SqlParameter("@pageSize",pageSize)
                 };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("proc_Requrement", parameter);
            int rountCount = 0, pageCount = 0;
            RequrementList requrementList = new RequrementList();
            if (ds.Tables.Count>0)
            {
                rountCount = int.Parse(ds.Tables[2].Rows[0]["recordCount"].ToString());
                pageCount = int.Parse(ds.Tables[2].Rows[0]["pageCount"].ToString());
                requrementList = QueryReplyList(ds.Tables[1], pageIndex, pageSize, rountCount, pageCount);
            }
            return requrementList;
        }

        public RequrementList QueryReplyList(DataTable dt, int pageIndex, int pageSize,int rountCount,int pageCount)
        {
            RequrementList requrementList = new RequrementList()
            {
                PageCount= pageCount,
                PageIndex= pageIndex,
                PageSize= pageSize,
                RountCount= rountCount
            };
            List<Requrement> requrements = new List<Requrement>();
            foreach (DataRow item in dt.Rows)
            {
                Requrement requrement = new Requrement()
                {
                    UserId = item["userId"].ToString(),
                    EntId = item["entId"].ToString(),
                    BuyName = item["buyName"].ToString(),
                    BuyTel = item["buyTel"].ToString(),
                    BuyGoods = item["buyGoods"].ToString(),
                    ProductName = item["productName"].ToString(),
                    BuySpec = item["buySpec"].ToString(),
                    BuyNumber = item["buyNumber"].ToString(),
                    BuyPrice = item["buyPrice"].ToString(),
                    Message = item["message"].ToString(),
                    RecordDate = item["recordDate"].ToString(),
                    Reply = item["reply"].ToString()
                };
                requrements.Add(requrement);
            }
            requrementList.Requrements = requrements;
            return requrementList;
        }
        #endregion

        #region 购物车推荐商品,相关操作
        /// <summary>
        /// 获取购物车推荐的商品列表，带分页
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public DataSet GetGoodsRecommendList(int pageIndex, int pageSize, string soustr,int type,int status, ref int recordCount, ref int pageCount)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            string sql = "[Proc_GetGoodsRecommendList]";
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@type",type),
                new SqlParameter("@status",status),
                new SqlParameter("@soustr",soustr),
                new SqlParameter("@pageindex",pageIndex),
                new SqlParameter("@pagesize",pageSize),
                new SqlParameter("@pageCount",0),//总页数
                new SqlParameter("@recordCount",0),//总记录数
                new SqlParameter("@SearchTime",0)//执行时间 毫秒
            };

            sqlParams[5].Direction = ParameterDirection.Output;
            sqlParams[6].Direction = ParameterDirection.Output;
            sqlParams[7].Direction = ParameterDirection.Output;
            DataSet dataSet = sqlhelper.RunProDataSet(sql, sqlParams);
            pageCount = Convert.ToInt32(sqlParams[5].Value);
            recordCount = Convert.ToInt32(sqlParams[6].Value);
            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                dataSet.Tables[i].Columns.Remove("ROWSTAT");
            }
            return dataSet;
        }
        /// <summary>
        /// 获取推荐商品类型集合
        /// </summary>
        /// <returns></returns>
        public DataTable GetGoodsRecommendType()
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            string sql = "select * from Zzsk_Configuration where TypeCode=504";
            DataTable dt = sqlhelper.RtDataTable(sql);
            return dt;
        }
        /// <summary>
        /// 添加推荐商品
        /// </summary>
        /// <param name="articleid"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool AddGoodsRecommend(int articleid, int type, int status,int sort)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            string sql1 = "select entid from dt_article where [id]=" + articleid;
            DataTable dataTable1 = sqlhelper.RtDataTable(sql1);
            string entid = Convert.ToString(dataTable1.Rows[0]["entid"]);
            string sql2 = "insert into dt_article_recommend (entid,article_id,[type],[sort],[status]) values ('" + entid + "'," + articleid + "," + type + "," + sort + "," + status + ")";
            bool r = sqlhelper.ExecuteSql(sql2);
            return r;
        }
        /// <summary>
        /// 获取单个推荐商品
        /// </summary>
        /// <param name="articleid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable SingleGoodsRecommend(int articleid, int type)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            string sql = "select a.*,b.goodscode,b.sub_title,b.drug_factory,b.approval_number,b.drug_spec,b.package_unit,b.dosage_form,b.mnemonic_code,b.min_package,b.price "
                       + "from dt_article_recommend a inner join dt_article_attribute b on a.article_id=b.article_id "
                       + "where a.article_id=" + articleid + " and a.[type]=" + type;
            DataTable dataTable = sqlhelper.RtDataTable(sql);
            return dataTable;
        }
        /// <summary>
        ///  删除商品推荐
        /// </summary>
        /// <param name="articleid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool DeleteGoodsRecommend(int articleid, int type)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            string sql = "delete from dt_article_recommend where article_id=" + articleid + " and [type]=" + type;
            bool result = sqlhelper.ExecuteSql(sql);
            return result;
        }
        /// <summary>
        /// 上下架
        /// </summary>
        /// <param name="articleid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool StatusGoodsRecommend(int articleid, int type,int status)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            string sql = "update dt_article_recommend set [status]=" + status + " where article_id=" + articleid + " and [type]=" + type;
            bool result = sqlhelper.ExecuteSql(sql);
            return result;
        }
        /// <summary>
        /// 修改推荐商品
        /// </summary>
        /// <param name="articleid"></param>
        /// <param name="old_articleid"></param>
        /// <param name="type"></param>
        /// <param name="old_type"></param>
        /// <param name="status"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public bool UpdateGoodsRecommend(int articleid,int old_articleid, int type,int old_type, int status, int sort)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            string sql1 = "select entid from dt_article where [id]=" + articleid;
            DataTable dataTable1 = sqlhelper.RtDataTable(sql1);
            string entid = Convert.ToString(dataTable1.Rows[0]["entid"]);
            string sql2 = "update dt_article_recommend set entid='" + entid + "',article_id=" + articleid + ",type=" + type + ",sort=" + sort + ",status=" + status
                        + " where article_id=" + old_articleid + " and [type]=" + old_type;
            bool r = sqlhelper.ExecuteSql(sql2);
            return r;
        }
        /// <summary>
        /// 此个商品推荐，是否存在
        /// </summary>
        /// <param name="articleid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ExistGoodsRecommend(int articleid, int type)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            string sql = "select count(1) from dt_article_recommend where article_id=" + articleid + " and [type]=" + type;
            DataTable dataTable = sqlhelper.RtDataTable(sql);
            int count = Convert.ToInt32(dataTable.Rows[0][0]);
            return count > 0 ? true : false;
        }
        /// <summary>
        /// 网站获取推荐商品
        /// </summary>
        /// <param name="Entid"></param>
        /// <param name="Jgjb"></param>
        /// <param name="khlb"></param>
        /// <param name="Num"></param>
        /// <param name="zzsktype"></param>
        /// <returns></returns>
        public DataTable GetGetArticleRecommend(string Entid, string Jgjb,string khlb, int Num, int zzsktype)
        {
            SqlRun sqlhelper = new SqlRun(SqlRun.sqlstr);
            string sql = "Proc_GetArticleRecommend";
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@Entid",Entid),
                new SqlParameter("@Jgjb",Jgjb),
                new SqlParameter("@khlb",khlb),
                new SqlParameter("@Num",Num),
                new SqlParameter("@zzsktype",zzsktype)
            };

            DataTable dt = sqlhelper.RunProcedureDR(sql, sqlParams);
            return dt;
        }
        #endregion


        #region======================获取专区商品=======================
        /// <summary>
        /// 专区商品
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="zqType"></param>
        /// <returns></returns>
        public DataTable GetZqGoods(string entId,string userId,string zqType)
        {
            ///获取商品列表
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@Type","GetZqGoodsList"),
              new SqlParameter("@UserId",userId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@ArrondiType",zqType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            return  sql.RunProcedureDR("Proc_Admin_Brand", param);
        }
        #endregion
    }
}