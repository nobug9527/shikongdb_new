using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.App_Code
{
    public class GetColumnName
    {
        /// <summary>
        /// 获取字段中文名字
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable GetChinaName(DataTable dt)
        {
            string cmdText = "select fdname,chnname from dt_fldlist where fdname in(";
            if (dt == null || dt.Columns.Count == 0)
            {
                return null;
            }
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName.Trim() != "ROWSTAT")
                {
                    cmdText += "'" + dc.ColumnName.Trim() + "',";
                }
            }
            cmdText = cmdText.Substring(0, cmdText.Length - 1);
            cmdText += ")";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dtcm = sql.RtDataTable(cmdText);

            return dtcm;
        }
    }
}