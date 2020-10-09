using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using Sk_B2BAPI.Tool;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sk_B2BAPI.DAL
{
    public class EntdocDal
    {
        /// <summary>
        /// 机构列表
        /// </summary>
        /// <returns></returns>
        public List<Entdoc> EntdocList()
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","EntdocList")
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR("Proc_OperationEntdoc", parameters);
            List<Entdoc> list = new List<Entdoc>();
            if (table.Rows.Count>=0)
            {
                list = FillList(table);
            }
            return list;
        }
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Entdoc> FillList(DataTable table)
        {
            List<Entdoc> list = new List<Entdoc>();
            foreach (DataRow item in table.Rows)
            {
                Entdoc entdoc = new Entdoc()
                {
                    Id =int.Parse(item["id"].ToString().Trim()),
                    EntId=item["entid"].ToString().Trim(),
                    EntName=item["entname"].ToString().Trim(),
                    EntCode=item["entcode"].ToString().Trim(),
                    Status=int.Parse(item["status"].ToString().Trim())
                };
                list.Add(entdoc);
            }
            return list;
        }
        /// <summary>
        /// 读取所有的机构，带分页
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="soustr"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<dt_entdoc> GetEntdocListPager(int pageindex, int pagesize, string soustr, ref int total)
        {
            SqlSugarHelper sqlSugarHelper = new SqlSugarHelper();
            List<dt_entdoc> list = sqlSugarHelper.Db.Queryable<dt_entdoc>()
                .Where(m =>m.status==2 && m.entname.Contains(soustr))
                .ToPageList(pageindex, pagesize, ref total);
            return list;
        }
    }
}