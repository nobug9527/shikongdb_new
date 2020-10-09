using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sk_B2BAPI.DAL
{
    public class CartDal
    {
        #region 
        /// <summary>
        /// 获取购物车商品列表
        /// </summary>
        /// <param name="userId">客户ID</param>
        /// <returns></returns>
        public DataTable GetCartEntList(string userId)
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append($"select distinct c.entid,c.entname from  dt_cart_keys a join Dt_UserEntDoc b on a.userId=b.userId and a.entid=b.entid join dt_entdoc c on b.entId=c.entid where b.status=2 and a.userId='{userId}' ");
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                dt = sql.RtDataTable(strSql.ToString());
            }
            catch (Exception ex)
            {
                dt = null;
                LogQueue.Write(LogType.Error, "Cart/GetCartEntList", ex.Message.ToString());
            }
            return dt;
        }


        /// <summary>
        /// 获取购物车商品列表
        /// </summary>
        /// <param name="entId">企业id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="jgjb">客户价格级别</param>
        /// <returns></returns>
        public CartList GetCartList(string entId, string userId, string goodsList, string jgjb,string clientType, string ywyId="")
        {
            ///获取购物车商品列表
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@Type","GetCartList"),
              new SqlParameter("@UserId",userId),
              new SqlParameter("@ywyId",ywyId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@Jgjb",jgjb),
              new SqlParameter("@List",goodsList),
              new SqlParameter("@clientType",clientType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Proc_CartList", param);
            CartList molde = new CartList();
            if (ds.Tables.Count > 0)
            {
                molde = SetCartList(ds,1,30); 
            }
            return molde;
        }
        /// <summary>
        /// 填充CartList,购物车列表【未分组】
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public CartList SetCartList(DataSet ds,int pageIndex = 1, int pageSize = 30)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            CartList model = new CartList();
            DataTable dt=new DataTable ();
            model.PageIndex = pageIndex;
            model.PageSize = pageSize;
            int cartNumber = 0;
            if (ds.Tables.Count >= 3)
            {
                model.PageCount = int.Parse(ds.Tables[2].Rows[0]["pageCount"].ToString());
                model.RountCount = int.Parse(ds.Tables[2].Rows[0]["recordCount"].ToString());
                dt = ds.Tables[1];
            }
            else
            {
                model.PageCount = 1;
                model.RountCount = ds.Tables[0].Rows.Count;
                dt = ds.Tables[0];
            }
            decimal real_Amount = 0M; //应付金额
            decimal order_Amount = 0M;//总金额
            decimal discount_Amount = 0M;//优惠金额

            long InventoryShows = long.Parse(BaseConfiguration.InventoryShows);//页面商品库存最大显示数量
            List<CartGoods> clist = new List<CartGoods>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["GoodsType"].ToString() != "ZP")
                {
                    cartNumber += 1;
                }
                CartGoods c = new CartGoods();
                c.Id = dr["cartId"].ToString();
                c.Article_Id = dr["article_id"].ToString();
                c.Sub_Title = dr["sub_title"].ToString();
                c.Drug_Factory = dr["drug_factory"].ToString();
                c.Drug_Spec = dr["drug_spec"].ToString();
                c.Big_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                c.Quantity = BasisConfig.ObjToDecimal(dr["quantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                c.Stock_Quantity = BasisConfig.ObjToDecimal(dr["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M);
                c.Fabs = dr["fabs"].ToString().Trim();
                c.Price = BasisConfig.ObjToDecimal(dr["price"].ToString(), BaseConfiguration.PricePlace,0.00M);
                c.RealPrice = BasisConfig.ObjToDecimal(dr["realprice"].ToString(), BaseConfiguration.PricePlace,0.00M);
                c.Scattered = dr["scattered"].ToString();
                c.Amount = c.Quantity * c.RealPrice;
                c.Fabh = dr["Fabh"].ToString();
                c.GoodsType = dr["GoodsType"].ToString();
                c.Multiple = decimal.Parse(dr["multiple"].ToString());
                //大包装控制 Y-取大包装 N-不取大包装
                string packControl = dr["packControl"].ToString().Trim();
                //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                string scattered = dr["scattered"].ToString().Trim();
                c.Zbz = BasisConfig.ObjToDecimal(dr["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                if (packControl == "Y")
                {
                    c.Min_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                }
                else
                {
                    if (scattered == "Y")
                    {
                        c.Min_Package = BasisConfig.ObjToDecimal(dr["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                    else
                    {
                        c.Min_Package = BasisConfig.ObjToDecimal(dr["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                }
                //图片
                if (dr["img_url"].ToString() != "")
                {
                    c.Img_Url = web_url + dr["img_url"].ToString().Trim();
                }
                else
                {
                    c.Img_Url = "";
                }
                if (InventoryShows > 0 && c.Stock_Quantity > InventoryShows)
                {
                    if (c.Stock_Quantity >= 1000)
                    {
                        c.Inventory = "充裕";
                    }
                    else
                    {
                        c.Inventory = "紧张";
                    }
                }
                else
                {
                    c.Inventory = BasisConfig.ObjToDecimal(dr["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace,0.00M).ToString();
                }
                //discount_Amount = discount_Amount + decimal.Parse(dr["discount_Amount"].ToString());//计算商品优惠金额
                //order_Amount = order_Amount+(c.Quantity * c.Price);//计算订单金额
                clist.Add(c);
            }
            model.Num = cartNumber;
            model.Order_Amount = order_Amount;
            model.Discount_Amount = discount_Amount;
            real_Amount=order_Amount - discount_Amount;
            if (real_Amount > 0)
            {
                model.Real_Amount = real_Amount;
            }
            else
            {
                model.Real_Amount = 0;
            }
            model.GoodsInfo = clist;

            return model;
        }
        /// <summary>
        /// 填充CartList,购物车列表【分组】
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public List<CartList> SetCartListGrouping(DataSet ds, int pageIndex = 1, int pageSize = 30)
        {
            ///获取网站ip
            string web_url = BaseConfiguration.SercerIp;
            List<CartList> list = new List<CartList>();
            CartList cl = new CartList();
            DataTable dt = new DataTable();
            cl.PageIndex = pageIndex;
            cl.PageSize = pageSize;
            int cartNumber = 0;
            if (ds.Tables.Count >= 3)
            {
                cl.PageCount = int.Parse(ds.Tables[2].Rows[0]["pageCount"].ToString());
                cl.RountCount = int.Parse(ds.Tables[2].Rows[0]["recordCount"].ToString());
                dt = ds.Tables[1];
            }
            else
            {
                cl.PageCount = 1;
                cl.RountCount = ds.Tables[0].Rows.Count;
                dt = ds.Tables[0];
            }
            decimal real_Amount = 0M; //应付金额
            decimal order_Amount = 0M;//总金额
            decimal discount_Amount = 0M;//优惠金额

            long InventoryShows = long.Parse(BaseConfiguration.InventoryShows);//页面商品库存最大显示数量

            #region 商品分组
            //购物车普通商品列表
            List<CartGoods> commonList = new List<CartGoods>();
            DataRow[] ptr = dt.Select("GoodsType='SP'");
            foreach (DataRow item in ptr)
            {
                Assignment(item, web_url, InventoryShows, ref cartNumber,ref commonList);
            }
            cl.CommonList = commonList;
            //购物车单品促销商品列表
            List<CartGoods> singleList = new List<CartGoods>();
            DataRow[] dpr = dt.Select("PromScenario='3'and Fabh<>'' and GoodsType<>'SP' ");
            foreach (DataRow item in dpr)
            {
                Assignment(item, web_url, InventoryShows, ref cartNumber, ref singleList);
            }
            cl.SingleList = singleList;
            //购物车组合促销商品列表
            List<CartGoods> groupList = new List<CartGoods>();
            DataRow[] zhr = dt.Select("PromScenario='0' and Fabh<>'' and GoodsType='GZH' ");
            foreach (DataRow item in zhr)
            {
                Assignment(item, web_url, InventoryShows, ref cartNumber, ref groupList);
            }
            cl.GroupList = groupList;
            //购物车品牌促销商品列表
            List<CartGoods> brandList = new List<CartGoods>();
            DataRow[] ppr = dt.Select("PromScenario='2'and Fabh<>'' and GoodsType<>'SP' ");
            foreach (DataRow item in ppr)
            {
                Assignment(item, web_url, InventoryShows, ref cartNumber, ref brandList);
            }
            cl.BrandList = brandList;
            //购物车分类促销商品列表
            List<CartGoods> classifyList = new List<CartGoods>();
            DataRow[] flr = dt.Select("PromScenario='1'and Fabh<>'' and GoodsType<>'SP' ");
            foreach (DataRow item in flr)
            {
                Assignment(item, web_url, InventoryShows, ref cartNumber, ref classifyList);
            }
            cl.ClassifyList = classifyList;
            //购物车全场促销商品列表
            List<CartGoods> allList = new List<CartGoods>();
            DataRow[] qcr = dt.Select("PromScenario='0' and Fabh<>'' and GoodsType not in('GZH','SP') ");
            foreach (DataRow item in qcr)
            {
                Assignment(item, web_url, InventoryShows, ref cartNumber, ref allList);
            }
            cl.AllList = allList;
            #endregion

            cl.Num = cartNumber;
            cl.Order_Amount = order_Amount;
            cl.Discount_Amount = discount_Amount;
            real_Amount = order_Amount - discount_Amount;
            if (real_Amount > 0)
            {
                cl.Real_Amount = real_Amount;
            }
            else
            {
                cl.Real_Amount = 0;
            }
            //cl.GoodsInfo = clist;
            list.Add(cl);

            return list;
        }
        private static void Assignment(DataRow dr,string web_url,long InventoryShows,ref int cartNumber,ref List<CartGoods> list)
        {
            if (dr["GoodsType"].ToString() != "ZP")
            {
                cartNumber += 1;
            }
            CartGoods c = new CartGoods
            {
                Id = dr["cartId"].ToString(),
                Article_Id = dr["article_id"].ToString(),
                Sub_Title = dr["sub_title"].ToString(),
                Drug_Factory = dr["drug_factory"].ToString(),
                Drug_Spec = dr["drug_spec"].ToString(),
                Big_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M),
                Quantity = BasisConfig.ObjToDecimal(dr["quantity"].ToString(), BaseConfiguration.InventoryPlace, 0.00M),
                Stock_Quantity = BasisConfig.ObjToDecimal(dr["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace, 0.00M),
                Fabs = dr["fabs"].ToString().Trim(),
                Price = BasisConfig.ObjToDecimal(dr["price"].ToString(), BaseConfiguration.PricePlace, 0.00M),
                RealPrice = BasisConfig.ObjToDecimal(dr["realprice"].ToString(), BaseConfiguration.PricePlace, 0.00M),
                Scattered = dr["scattered"].ToString()
            };
            c.Amount = c.Quantity * c.RealPrice;
            c.Fabh = dr["Fabh"].ToString();
            c.GoodsType = dr["GoodsType"].ToString();
            c.Multiple = decimal.Parse(dr["multiple"].ToString());
            //大包装控制 Y-取大包装 N-不取大包装
            string packControl = dr["packControl"].ToString().Trim();
            //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
            string scattered = dr["scattered"].ToString().Trim();
            if (packControl == "Y")
            {
                c.Min_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
            }
            else
            {
                if (scattered == "Y")
                {
                    c.Min_Package = BasisConfig.ObjToDecimal(dr["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                }
                else
                {
                    c.Min_Package = BasisConfig.ObjToDecimal(dr["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                }
            }
            //图片
            if (dr["img_url"].ToString() != "")
            {
                c.Img_Url = web_url + dr["img_url"].ToString().Trim();
            }
            else
            {
                c.Img_Url = "";
            }
            if (InventoryShows > 0 && c.Stock_Quantity > InventoryShows)
            {
                if (c.Stock_Quantity >= 1000)
                {
                    c.Inventory = "充裕";
                }
                else
                {
                    c.Inventory = "紧张";
                }
            }
            else
            {
                c.Inventory = BasisConfig.ObjToDecimal(dr["stock_quantity"].ToString(), BaseConfiguration.InventoryPlace, 0.00M).ToString();
            }
            list.Add(c);
        }
        #endregion

        #region 获取购物车选中购物车商品金额
        /// <summary>
        /// 获取购物车选中购物车商品金额
        /// </summary>
        /// <param name="entId">企业id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="jgjb">客户价格级别</param>
        /// <returns></returns>
        public List<CartList> GetCartAmount(string entId, string userId, string goodsList, string jgjb,string clientType,string ywyId)
        {
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@Type","GetCartList"),
              new SqlParameter("@UserId",userId),
              new SqlParameter("@ywyId",ywyId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@Jgjb",jgjb),
              new SqlParameter("@List",goodsList),
              new SqlParameter("@clientType",clientType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR("Proc_CartList", param);
            List<CartList> list = new List<CartList>();
            CartList c = new CartList();
            decimal real_Amount = 0M; //应付金额
            decimal order_Amount = 0M;//总金额
            decimal discount_Amount = 0M;//优惠金额

            if (dt.Rows.Count>0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int quantity=int.Parse(dr["quantity"].ToString());
                    decimal price=decimal.Parse(dr["price"].ToString());
                    order_Amount = order_Amount + quantity * price;
                    discount_Amount = discount_Amount + decimal.Parse(dr["discount_Amount"].ToString());
                    real_Amount = order_Amount - discount_Amount;
                }
            }
            c.RountCount = dt.Rows.Count;
            c.Real_Amount = order_Amount - discount_Amount;
            c.Order_Amount = order_Amount;
            c.Discount_Amount = discount_Amount;
            c.Real_Amount = real_Amount;
            
            list.Add(c);
            return list;
        }
        #endregion

        #region 选择商品加入购物车
        /// <summary>
        /// 选择商品加入购物车
        /// </summary>
        /// <param name="entId">企业id</param>
        /// <param name="userId">用户id</param>
        /// <param name="article_Id">商品id</param>
        /// <param name="quantity">数量</param>
        /// <param name="cartType">购物车类型</param>
        /// <param name="fabh">促销方案编号</param>
        /// <returns></returns>
        public bool CartAdd(string entId, string userId, string article_Id, decimal quantity, string cartType,string fabh,string bs,string ywyId="")
        {
            bool flag = false;
            ///获取购物车商品列表
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@Type","CartAdd_HY"),
              new SqlParameter("@UserId",userId),
              new SqlParameter("@ywyId",ywyId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@article_Id",article_Id),
              new SqlParameter("@fabh",fabh),
              new SqlParameter("@quantity",quantity),
              new SqlParameter("@carttype",cartType),
              new SqlParameter("@bs",bs)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int n = sql.ExecuteNonQuery("Proc_CartAdd", param);
            if (n > 0) { flag = true; }
            return flag;
        }
        #endregion

        #region 选择商品组加入购物车
        /// <summary>
        /// 选择商品组加入购物车
        /// </summary>
        /// <param name="entId">企业id</param>
        /// <param name="userId">用户id</param>
        /// <param name="quantity">数量</param>
        /// <param name="fabh">促销方案编号</param>
        /// <returns></returns>
        public string CartAddGroup(string entId,string userId,decimal quantity,string fabh, out int flag,string bs,string cartType)
        {
            flag = 0;
            string msg = "";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@type","CartAddGroup"),
                    new SqlParameter("@entId",entId),
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@quantity",quantity),
                    new SqlParameter("@fabh",fabh),
                    new SqlParameter("@bs",bs),
                    new SqlParameter("@carttype",cartType)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);

                DataTable table = sql.RunProcedureDR("Proc_CartAdd", parameters);
                if (table.Rows.Count > 0)
                {
                    flag = int.Parse(table.Rows[0]["flag"].ToString());
                    msg = table.Rows[0]["msg"].ToString();
                }
                return msg;
            }
            catch (Exception ex)
            {
                flag = 1;
                return ex.Message.ToString();
            }
        }
        #endregion

        #region 删除购物车商品
        /// <summary>
        /// 删除购物车商品
        /// </summary>
        /// <param name="dltType">删除类型</param>
        /// <param name="entId">企业Id</param>
        /// <param name="userId">用户id</param>
        /// <param name="cartId">购物车Id</param>
        /// <returns></returns>
        public bool DeleteCart(string entId, string userId, string dltType,string cartId,string cartType,string ywyId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","CartDelete"),
                new SqlParameter("@userId",userId),
                new SqlParameter("@ywyId",ywyId),
                new SqlParameter("@entId",entId),
                new SqlParameter("@dltType",dltType),
                new SqlParameter("@cartId",cartId),
                new SqlParameter("@cartType",cartType)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            bool flag = false;
            DataTable table = sql.RunProcedureDR("Proc_CartAdd", parameters);
            if (table.Rows.Count>0)
            {
                flag = int.Parse(table.Rows[0]["flag"].ToString()) == 0 ? true : false;
            }
            return flag;
        }
        #endregion

        #region 获取购物车条目数
        /// <summary>
        /// 获取购物车条目数
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetCartCount(string entId, string userId,string ywyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  a.entId,cartId from dt_cart_keys a(nolock)  join Fun_UserEntDoc('"+userId+"') b on a.entId=b.entid and ywyId='" + ywyId + "'");
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RtDataTable(strSql.ToString());
            return dt.Rows.Count;
        }
        #endregion

        #region 再次购买
        /// <summary>
        /// 再次购买
        /// </summary>
        /// <param name="userId">下单人Id</param>
        /// <param name="entId">下单人机构</param>
        /// <param name="billNo">再次购买商品的单号</param>
        /// <param name="loginId">登陆人Id</param>
        /// <param name="fabh"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public string OnceAgain(string userId, string entId, int billNo,string loginId,out bool flag,string jgjb,string khType, string cartType,string bs)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@type","OnceAgain"),
                    new SqlParameter("@userId",userId),
                    new SqlParameter("@entId",entId),
                    new SqlParameter("@billNo",billNo),
                    new SqlParameter("@loginId",loginId),
                    new SqlParameter("@cartType",cartType),
                    new SqlParameter("@jgjb",jgjb),
                    new SqlParameter("@khType",khType),
                    new SqlParameter("@bs",bs)
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                DataTable dt = sql.RunProcedureDR("Proc_OperationOrders", parameters);
                if (dt.Rows.Count > 0)
                {
                    flag = 0 == int.Parse(dt.Rows[0]["flag"].ToString()) ? true : false;
                    return dt.Rows[0]["msg"].ToString();
                }
                else
                {
                    flag = false;
                    return "订单存盘失败";
                }
            }
            catch (Exception ex)
            {
                flag = false;
                return ex.Message.ToString();
            }
        }
        #endregion

        #region 购物车选中商品
        /// <summary>
        /// 购物车选中商品
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="goodsList"></param>
        /// <param name="jgjb"></param>
        /// <returns></returns>
        public List<CartGoods> CartList(string entId, string userId, string goodsList, string jgjb)
        {
            ///获取购物车商品列表
            SqlParameter[] param = new SqlParameter[]{
              new SqlParameter("@Type","GetCartList"),
              new SqlParameter("@UserId",userId),
              new SqlParameter("@Entid",entId),
              new SqlParameter("@Jgjb",jgjb),
              new SqlParameter("@List",goodsList)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_CartList", param);
            List<CartGoods> list = new List<CartGoods>();
            if (table.Rows.Count > 0)
            {
                list = FillList(table);
            }
            return list;
        }
        /// <summary>
        /// 购物车选中商品数据填充
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<CartGoods> FillList(DataTable table)
        {
            List<CartGoods> lists = new List<CartGoods>();
            foreach (DataRow item in table.Rows)
            {
                CartGoods goods = new CartGoods()
                {
                    Article_Id=item["article_id"].ToString(),
                    Quantity=decimal.Parse(item["quantity"].ToString()),
                    Price= decimal.Parse(item["price"].ToString()),
                    GoodsType=item["GoodsType"].ToString(),
                    Fabh=item["Fabh"].ToString(),
                    EntId=item["entId"].ToString(),
                    Discount= decimal.Parse(item["discount"].ToString()),
                    Derate= decimal.Parse(item["derate"].ToString()),
                    PromScenario=item["PromScenario"].ToString()
                };
                lists.Add(goods);
            }
            return lists;
        }
        #endregion
    }
}