using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using System.Text;

namespace Sk_B2BAPI.DAL
{
    public class ValetOrderDal
    {
        /// <summary>
        /// 代客下单获取会员信息
        /// </summary>
        /// <param name="userId">业务员Id</param>
        /// <param name="strWhere">条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容</param>
        public ValetOrderModel<ValetOrder_MemberList> GetMemberList(string ywyId, string strWhere, int pageIndex, int pageSize)
        {
            ValetOrderModel<ValetOrder_MemberList> mode = new ValetOrderModel<ValetOrder_MemberList>();
            mode.PageIndex = pageIndex;
            mode.PageSize = pageSize;
            try
            {
                SqlParameter[] param = new SqlParameter[]{
                 new SqlParameter("@type","GetSalesManList"),
                 new SqlParameter("@ywyId",ywyId),
                 new SqlParameter("@pageIndex",pageIndex),
                 new SqlParameter("@pageSize",pageSize),
                 new SqlParameter("@strWhere",strWhere)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                DataSet ds = sql.RunProDataSet("proc_dkxd_GetInfo", param);
                int pageCount = 0, recordCount = 0;
                DataTable dt = new DataTable();
                if (ds.Tables.Count >= 3)
                {
                    pageCount = Int32.Parse(ds.Tables[2].Rows[0]["pageCount"].ToString());
                    recordCount = Int32.Parse(ds.Tables[2].Rows[0]["recordCount"].ToString());
                    dt = ds.Tables[1];
                }
                else if (ds.Tables.Count == 1)
                {
                    pageCount = 1;
                    recordCount = ds.Tables[0].Rows.Count;
                    dt = ds.Tables[0];
                }
                mode.PageCount = pageCount;
                mode.RecordCount = recordCount;
                if (dt.Rows.Count > 0)
                {
                    mode.Success = "000";
                    mode.Message = "成功";
                    mode.Data = SetMemberList(dt);
                }
                else
                {
                    mode.Success = "001";
                    mode.Message = "成功,暂无数据";
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "ValetOrder/GetMemberList", ex.Message.ToString());
                mode.Success = "002";
                mode.Message = "错误," + ex.Message.ToString();
            }
            return mode;
        }
        private List<ValetOrder_MemberList> SetMemberList(DataTable dt)
        {
            List<ValetOrder_MemberList> list = new List<ValetOrder_MemberList>();
            foreach (DataRow dr in dt.Rows)
            {
                ValetOrder_MemberList mode = new ValetOrder_MemberList();
                mode.UserId = dr["userid"].ToString().Trim();
                mode.UserName = dr["username"].ToString().Trim();
                mode.Name = dr["name"].ToString().Trim();
                mode.BusinessCode = dr["businesscode"].ToString().Trim();
                mode.BusinessName = dr["businessname"].ToString().Trim();
                mode.BusinessId = dr["businessid"].ToString().Trim();
                mode.CountNumber = dr["countNumber"].ToString().Trim();
                mode.TotalNumber = dr["totalNumber"].ToString().Trim();
                list.Add(mode);
            }
            return list;
        }

        /// <summary>
        /// 搜索页获取商品列表
        /// </summary>
        /// <param name="ywyId">业务员Id</param>
        /// <param name="userId">客户Id</param>
        /// <param name="searchValue">搜索条件</param>
        /// <param name="letter">厂家首字母</param>
        /// <param name="tags">排序类型</param>
        /// <param name="isKc">是否有货</param>
        /// <param name="CategoryId">商品分类Id</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">没页行数</param>
        /// <returns></returns>
        public ValetOrderModel<GoodsInfo> GetGoodsList(string ywyId, string userId, string searchValue, string letter, string tags, string isKc, string CategoryId, int pageIndex, int pageSize, string entid)
        {
            ValetOrderModel<GoodsInfo> mode = new ValetOrderModel<GoodsInfo>();
            mode.PageIndex = pageIndex;
            mode.PageSize = pageSize;
            ////加载用户信息
            string jgjb = "", clientlimit = "", KhType = "";
            try
            {
                //获取用户信息
                UserInfoDal dal = new UserInfoDal();
                List<UserInfo> user = new List<UserInfo>();
                user = dal.GetUserInfo(userId, entid);
                if (user.Count <= 0)
                {
                    mode.Success = "002";
                    mode.Message = "客户信息获取失败";
                    return mode;
                }
                entid = user[0].EntId;
                jgjb = user[0].Pricelevel;
                clientlimit = user[0].ClientLimit;
                KhType = user[0].KhType;
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "ValetOrder/GetGoodsList", ex.Message.ToString());
                mode.Success = "002";
                mode.Message = "客户信息获取失败," + ex.Message.ToString();
                return mode;
            }
            try
            {
                ///加载查询条件
                StringBuilder StrWhere = new StringBuilder();
                if (searchValue.IndexOf(" ") >= 0)
                {
                    string goodsValue, factoryValue;
                    string[] searchString = searchValue.Split(' ');
                    goodsValue = searchString[0];
                    factoryValue = searchString[1];
                    StrWhere.Append(" and b.mnemonic_code like '%" + goodsValue + "%' and b.Origin_mnemonic_code like '%" + factoryValue + "%' ");
                }
                else
                {
                    StrWhere.Append(" and(b.goodscode like '%" + searchValue + "%' or b.sub_title like '%" + searchValue + "%' or b.mnemonic_code like '%" + searchValue + "%'");
                    StrWhere.Append(" or b.drug_factory like '%" + searchValue + "%' or b.Origin_mnemonic_code like '%" + searchValue + "%') ");
                }
                //是否有货筛选
                if (isKc == "Y")
                {
                    StrWhere.Append(" and c.stock_quantity>0 ");
                }
                //商品价格大于0筛选
                StrWhere.Append(" and d.price>0.0");
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
                SqlParameter[] param = new SqlParameter[]{
                  new SqlParameter("@Type","GetList"),
                  new SqlParameter("@UserId",userId),
                  new SqlParameter("@Entid",entid),
                  new SqlParameter("@PageSize",pageSize),
                  new SqlParameter("@PageIndex",pageIndex),
                  new SqlParameter("@StrWhere",StrWhere.ToString()),
                  new SqlParameter("@Order",order.ToString()),
                  new SqlParameter("@CategoryId",CategoryId),
                  new SqlParameter("@Login_Id",ywyId),
                  new SqlParameter("@Jgjb",jgjb),
                  new SqlParameter("@khlb",KhType)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                DataSet ds = sql.RunProDataSet("Proc_GoodsList", param);
                mode.Success = "001";
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = new DataTable();
                    if (ds.Tables.Count >= 3)
                    {
                        dt = ds.Tables[1];
                    }
                    else
                    {
                        dt = ds.Tables[0];
                    }
                    if (dt.Rows.Count > 0)
                    {
                        mode.Data = SetGoodsList(dt);
                        mode.Success = "000";
                        mode.Message = "商品加载成功";
                    }
                }
                return mode;
            }
            catch (Exception ex)
            {
                mode.Success = "002";
                mode.Message = "商品信息加载失败," + ex.Message.ToString();
                return mode;

            }
        }
        /// <summary>
        /// 填充数据SearchGoods
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private List<GoodsInfo> SetGoodsList(DataTable dt, List<ImgInfo> ilist = null)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            ////商品信息
            List<GoodsInfo> gList = new List<GoodsInfo>();

            long InventoryShows = long.Parse(BaseConfiguration.InventoryShows);//页面商品库存最大显示数量
            foreach (DataRow dr in dt.Rows)
            {
                GoodsInfo gi = new GoodsInfo
                {
                    Article_Id = dr["article_id"].ToString(),
                    GoodsCode = dr["goodscode"].ToString(),
                    Sub_Title = dr["sub_title"].ToString(),
                    Drug_Spec = dr["drug_spec"].ToString(),
                    Package_Unit = dr["package_unit"].ToString(),
                    Drug_Factory = dr["drug_factory"].ToString(),
                    Big_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M),
                    Stock_Quantity = decimal.Parse(dr["stock_quantity"].ToString()),
                    Cxbs = dr["cxbs"].ToString(),
                    Fabh = dr["fabh"].ToString(),
                    Describe = dr["Describe"].ToString(),
                    ProposalPrice =Convert.ToDecimal(dr["proposalPrice"].ToString()),
                    CollectId = dr["collectId"].ToString(),
                    Valdate = dr["valdate"].ToString(),
                    OldValdate = dr["old_valdate"].ToString(),
                    Scattered=dr["scattered"].ToString(),
                    ApprovalNumber = dr["approval_number"].ToString(),
                    GoodsLimit = dr["goodslimit"].ToString().Trim(),
                    PassSum = int.Parse(dr["passSum"].ToString()),//评论数
                    RaveReviews = decimal.Parse(dr["raveReviews"].ToString()),//好评率
                    Price = dr["price"].ToString(),
                    Limit = dr["limit"].ToString().Trim(),
                    ImgList = ilist//商品图片列表
                };
                //是否允许拆零
                string scattered = dr["scattered"].ToString();
                if (scattered == "Y")
                {
                    gi.Min_Package = BasisConfig.ObjToDecimal(dr["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);//拆零包装
                }
                else
                {
                    gi.Min_Package = BasisConfig.ObjToDecimal(dr["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                }
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
                    gi.Inventory = dr["stock_quantity"].ToString();
                }
                if (dr["img_url"].ToString() != "")
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
            return gList;
        }
    }
}