using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sk_B2BAPI.DAL
{
    public class DispatchInfoDal
    {
        
        /// <summary>
        /// 获取配送方式信息
        /// </summary>
        /// <param name="enable">是否启用</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">单页条目</param>
        /// <param name="recordCount">总条目</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>集合List<Payment></returns>
        public List<Dispatch> QueryPriceLevel(string enable)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@type","QueryDispatch"),
                new SqlParameter("@enable",enable)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet set = sql.RunProDataSet("Proc_OperationDispatch", parameters);
            List<Dispatch> dispatch = new List<Dispatch>();
            if (set.Tables[0].Rows.Count >= 0)
            {
                dispatch = FillList(set.Tables[0]);
            }
            return dispatch;
        }
        /// <summary>
        /// 填充Dispatch
        /// </summary>
        /// <param name="table">数据源</param>
        /// <returns>集合List<Dispatch></returns>
        public List<Dispatch> FillList(DataTable table)
        {
            List<Dispatch> payments = new List<Dispatch>();
            foreach (DataRow item in table.Rows)
            {
                Dispatch dispatch = new Dispatch
                {
                    DispatchId = item["DispatchId"].ToString().Trim(),
                    DispatchName = item["DispatchName"].ToString().Trim(),
                    Enable = item["Enable"].ToString().Trim(),
                    UserId = item["UserId"].ToString().Trim(),
                    Entid=item["Entid"].ToString().Trim()
                };
                payments.Add(dispatch);
            }
            return payments;
        }

        #region---已弃用---
        /*
        /// <summary>
        /// 保存配送方式
        /// </summary>
        /// <param name="dispatchName">配送方式</param>
        /// <param name="enable">是否启用</param>
        /// <param name="flag"></param>
        /// <returns>是否成功</returns>
        public string SavePriceLevel(string dispatchName, string enable, out bool flag)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","SaveDispatch"),
                new SqlParameter("@dispatchName",dispatchName),
                new SqlParameter("@enable",enable)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int number = sql.ExecuteNonQuery("Proc_OperationDispatch", parameters);
            if (number > 0)
            {
                flag = true;
                return "保存配送方式成功";
            }
            else
            {
                flag = false;
                return "保存配送方式失败";
            }
        }
        /// <summary>
        /// 维护配送方式
        /// </summary>
        /// <param name="dispatchId">配送方式编号</param>
        /// <param name="dispatchName">配送方式</param>
        /// <param name="enable">是否启用</param>
        /// <param name="flag"></param>
        /// <returns>是否成功</returns>
        public string UpdatePriceLevel(string dispatchId, string dispatchName, string enable, out bool flag)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@type","UpdateDispatch"),
                new SqlParameter("@dispatchId",dispatchId),
                new SqlParameter("@dispatchName",dispatchName),
                new SqlParameter("@enable",enable)
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int number = sql.ExecuteNonQuery("Proc_OperationDispatch", parameters);
            if (number > 0)
            {
                flag = true;
                return "维护配送方式成功";
            }
            else
            {
                flag = false;
                return "维护配送方式失败";
            }
        }
        */
        #endregion
    }
}