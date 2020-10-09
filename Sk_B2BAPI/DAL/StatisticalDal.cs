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
    public class StatisticalDal
    {
        /// <summary>
        /// 商品点击/搜索统计
        /// </summary>
        /// <returns></returns>
        public static bool GoodsStatistical(string category, string searchValue, string entId)
        {
            bool flag = false;
            try
            {
                SqlParameter[] param = new SqlParameter[]{
                  new SqlParameter("@Type","GoodsStatistical"),
                  new SqlParameter("@SearchValue",searchValue),
                  new SqlParameter("@Category",category),
                  new SqlParameter("@Entid",entId),
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                if (sql.ExecuteNonQuery("Proc_Goods_Statistical", param) > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "StatisticalDal/GoodsStatistical", ex.Message.ToString());
                throw ex;
            }
            return flag;
        }
        /// <summary>
        /// 添加用户收藏商品
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="article_Id"></param>
        /// <returns></returns>
        public static bool AddGoodsCollection(string entId, string userId, string article_Id)
        {
            bool flag = false;
            try
            {
                SqlParameter[] param = new SqlParameter[]{
                  new SqlParameter("@Type","AddGoodsCollection"),
                  new SqlParameter("@entId",entId),
                  new SqlParameter("@userId",userId),
                  new SqlParameter("@article_Id",article_Id),  
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                if (sql.ExecuteNonQuery("Proc_Goods_Statistical", param) > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "StatisticalDal/AddGoodsCollection", ex.Message.ToString());
                throw ex;
            }
            return flag;
        }
        /// <summary>
        /// 删除商品收藏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DltGoodsCollection(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dt_article_collection where id=" + id);
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            bool flag = sql.ExecuteSql(strSql.ToString());
            return flag;
        }

        /// <summary>
        /// 查询用户收藏品种
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<GoodsStatistical> GoodsCollectionQuery(string entId, string userId, int pageIndex, int pageSize, out int recordCount, out int pageCount)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]{
                  new SqlParameter("@Type","GetGoodsCollection"),
                  new SqlParameter("@entId",entId),
                  new SqlParameter("@userId",userId),
                  new SqlParameter("@pageIndex",pageIndex),  
                  new SqlParameter("@pageSize",pageSize), 
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                DataSet ds = sql.RunProDataSet("Proc_Goods_Statistical", param);
                DataTable dt=null;
                if (ds.Tables.Count >= 3)
                {
                    dt = ds.Tables[1];
                    recordCount = int.Parse(ds.Tables[2].Rows[0]["recordCount"].ToString());
                    pageCount = int.Parse(ds.Tables[2].Rows[0]["pageCount"].ToString());
                }
                else
                {
                    dt = ds.Tables[0];
                    recordCount = dt.Rows.Count;
                    pageCount = 1;
                }
                List<GoodsStatistical> list = new List<GoodsStatistical>();
                if (dt.Rows.Count > 0)
                {
                    list = SetGoodsInfo(dt);
                }
                return list;
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "StatisticalDal/GoodsCollectionQuery", ex.Message.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 填充商品信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<GoodsStatistical> SetGoodsInfo(DataTable dt)
        {
            List<GoodsStatistical> list = new List<GoodsStatistical>();
            foreach (DataRow dr in dt.Rows)
            {
                GoodsStatistical g = new GoodsStatistical();
                g.Id =int.Parse(dr["id"].ToString().Trim());
                g.Article_Id = dr["article_id"].ToString().Trim();
                g.Sub_Title = dr["sub_title"].ToString().Trim();
                g.Drug_Spec = dr["drug_spec"].ToString().Trim();
                g.Drug_Factory = dr["drug_factory"].ToString ().Trim();
                g.Package_Unit = dr["package_unit"].ToString().Trim();
                g.Price = BasisConfig.ObjToDecimal(dr["price"].ToString().Trim(), BaseConfiguration.PricePlace,0.00M);
                g.Stock_Quantity = BasisConfig.ObjToDecimal(dr["stock_quantity"].ToString().Trim(), BaseConfiguration.InventoryPlace,0.00M);
                //大包装控制 Y-取大包装 N-不取大包装
                string packControl = dr["packControl"].ToString().Trim();
                //中包装控制 Y-取中包装，不拆零 N-不取中包装，拆零
                string scattered = dr["scattered"].ToString().Trim();
                if (packControl == "Y")
                {
                    g.Min_Package = BasisConfig.ObjToDecimal(dr["big_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                }
                else
                {
                    if (scattered == "Y")
                    {
                        g.Min_Package = BasisConfig.ObjToDecimal(dr["min_package"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                    else
                    {
                        g.Min_Package = BasisConfig.ObjToDecimal(dr["scatteredPackage"].ToString(), BaseConfiguration.PackagePlace, 0.00M);
                    }
                }
                list.Add(g);
            }
            return list;
        }
        /// <summary>
        /// 填充到货提醒信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<GoodsArrival> SetGoodsArrivalInfo(DataTable dt)
        {
            List<GoodsArrival> list = new List<GoodsArrival>();
            GoodsArrival g;
            foreach (DataRow dr in dt.Rows)
            {   g= new GoodsArrival();
                g.Id = int.Parse(dr["Id"].ToString().Trim());
                g.AddTime = dr["AddTime"].ToString().Trim();
                g.ArrivalStatus = int.Parse(dr["ArrivalStatus"].ToString().Trim());
                g.EntId = dr["EntId"].ToString().Trim();
                g.NewTime = dr["NewTime"].ToString().Trim();
                g.ProductId = dr["ProductId"].ToString().Trim();
                g.ReadStatus = int.Parse(dr["ReadStatus"].ToString().Trim());
                g.Status = int.Parse(dr["Status"].ToString().Trim());
                g.UserId = dr["UserId"].ToString().Trim();
                g.Remak = dr["Remak"].ToString().Trim();
                list.Add(g);
            }
            return list;
        }
        /// <summary>
        /// 添加用户到货提醒
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="article_Id"></param>
        /// <returns></returns>
        public static bool AddGoodsArrival(string entId, string userId, string article_Id)
        {
            bool flag = false;
            try
            {
                SqlParameter[] param = new SqlParameter[]{
                  new SqlParameter("@Type","AddGoodsArrival"),
                  new SqlParameter("@entId",entId),
                  new SqlParameter("@userId",userId),
                  new SqlParameter("@article_Id",article_Id),
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                if (sql.ExecuteNonQuery("Proc_Goods_Statistical", param) > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "StatisticalDal/AddGoodsArrival", ex.Message.ToString());
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 检测是否有到货提醒 有为true 没有为false
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CheckGoodsArrival(string entId, string userId, string article_Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id from  Zzsk_ArrivalReminder  where ProductId="+ article_Id + " and UserId='"+ userId + "' and EntId='"+ entId + "'");
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            bool flag = sql.RunSqlDataTable(strSql.ToString()).Rows.Count>0;
            return flag;
        }

        /// <summary>
        /// 修改到货提醒阅读状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ReadGoodsArrival(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Update Zzsk_ArrivalReminder Set ReadStatus=1  where Id=" + id);
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            bool flag = sql.ExecuteSql(strSql.ToString());
            return flag;
        }
        /// <summary>
        /// 删除商品到货提醒
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DltGoodsArrival(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Zzsk_ArrivalReminder where Id=" + id);
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            bool flag = sql.ExecuteSql(strSql.ToString());
            return flag;
        }
        /// <summary>
        /// 查询用户到货提醒
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<GoodsArrival> GoodsArrivalQuery(string entId, string userId, int pageIndex, int pageSize, out int recordCount, out int pageCount)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]{
                  new SqlParameter("@Type","GetGoodsArrival"),
                  new SqlParameter("@entId",entId),
                  new SqlParameter("@userId",userId),
                  new SqlParameter("@pageIndex",pageIndex),
                  new SqlParameter("@pageSize",pageSize),
                };
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                DataSet ds = sql.RunProDataSet("Proc_Goods_Statistical", param);
                DataTable dt = null;
                if (ds.Tables.Count >= 3)
                {
                    dt = ds.Tables[1];
                    recordCount = int.Parse(ds.Tables[2].Rows[0]["recordCount"].ToString());
                    pageCount = int.Parse(ds.Tables[2].Rows[0]["pageCount"].ToString());
                }
                else
                {
                    dt = ds.Tables[0];
                    recordCount = dt.Rows.Count;
                    pageCount = 1;
                }
                List<GoodsArrival> list = new List<GoodsArrival>();
                if (dt.Rows.Count > 0)
                {
                    list = SetGoodsArrivalInfo(dt);
                }
                return list;
            }
            catch (Exception ex)
            {
                LogQueue.Write(LogType.Error, "StatisticalDal/GoodsArrivalQuery", ex.Message.ToString());
                throw ex;
            }
        }


    }
}