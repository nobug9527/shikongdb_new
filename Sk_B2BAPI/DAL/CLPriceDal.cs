using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sk_B2BAPI.DAL
{
    /// <summary>
    /// 大宗商品阶梯价格 数据处理DAL层 CommodityLadderPriceDal
    /// </summary>
    public class CLPriceDal
    {
        /// <summary>
        /// 保存大宗商品阶梯价格
        /// </summary>
        /// <param name="entid">机构ID</param>
        /// <param name="article_id">文章ID</param>
        /// <param name="pricelevel">价格级别</param>
        /// <param name="price">价格</param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public string SaveCLPrice(string entid, int article_id,string pricelevel,decimal price, out bool flag)
        {
            string lastmodifytime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","SaveCLPrice"),
                new SqlParameter("@entid",entid),
                new SqlParameter("@article_id",article_id),
                new SqlParameter("@pricelevel",pricelevel),
                new SqlParameter("@price",price),
                new SqlParameter("@lastmodifytime",lastmodifytime)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int number = sql.ExecuteNonQuery("Proc_OperationCLPrice", parameters);
            if (number > 0)
            {
                flag = true;
                return "保存大宗商品阶梯价格成功";
            }
            else
            {
                flag = false;
                return "保存大宗商品阶梯价格失败";
            }
        }
        /// <summary>
        /// 获取大宗商品阶梯价格
        /// </summary>
        /// <param name="entid">机构ID</param>
        /// <param name="article_id">文章ID</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">单页条目</param>
        /// <param name="recordCount">总条目</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>集合List<CLPrice></returns>
        public List<CLPrice> QueryCLPrice(string entid, int article_id)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@type","QueryCLPrice"),
                new SqlParameter("@entid",entid),
                new SqlParameter("@article_id",article_id)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("Proc_OperationCLPrice", parameters);
            List<CLPrice> cLPrices = new List<CLPrice>();
            if (set.Tables.Count >= 1)
            {
                cLPrices = FillList(set.Tables[0]);
            }
            return cLPrices;
        }
        /// <summary>
        /// 填充Payment
        /// </summary>
        /// <param name="table">数据源</param>
        /// <returns>集合List<CLPrice></returns>
        public List<CLPrice> FillList(DataTable table)
        {
            List<CLPrice> cLPrices = new List<CLPrice>();
            foreach (DataRow item in table.Rows)
            {
                CLPrice price = new CLPrice();
                price.Entid = item["entid"].ToString().Trim();
                price.Article_Id = int.Parse(item["article_id"].ToString());
                price.PriceLevel = item["pricelevel"].ToString().Trim();
                price.Price = decimal.Parse(item["price"].ToString());
                price.LastModifyTime = item["lastmodifytime"].ToString().Trim();
                cLPrices.Add(price);
            }
            return cLPrices;
        }
        /// <summary>
        /// 维护大宗商品阶梯价格
        /// </summary>
        /// <param name="entid">机构ID</param>
        /// <param name="article_id">文章ID</param>
        /// <param name="pricelevel">价格级别</param>
        /// <param name="price">价格</param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public string UpdateCLPrice(string entid, int article_id, string pricelevel, decimal price, out bool flag)
        {
            string lastmodifytime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","UpdateCLPrice"),
                new SqlParameter("@entid",entid),
                new SqlParameter("@article_id",article_id),
                new SqlParameter("@pricelevel",pricelevel),
                new SqlParameter("@price",price),
                new SqlParameter("@lastmodifytime",lastmodifytime)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int number = sql.ExecuteNonQuery("Proc_OperationCLPrice", parameters);
            if (number > 0)
            {
                flag = true;
                return "维护大宗商品阶梯价格成功";
            }
            else
            {
                flag = false;
                return "维护大宗商品阶梯价格失败";
            }
        }
    }
}