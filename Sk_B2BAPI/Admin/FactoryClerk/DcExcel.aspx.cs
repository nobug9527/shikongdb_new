using DTcms.DBUtility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace DTcms.Web.admin.FactoryClerk
{
    public partial class DcExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string type = Request.QueryString["type"].Trim();//请求类型
                string json = Request.QueryString["json"].Trim();//请求参数(json类型)
                string proc = Request.QueryString["proc"].Trim();//存储过程名称
                DataTable dt = GetReturnJson(json, proc);
                if (dt.Rows.Count > 0)
                {
                    string[] top = Columns(dt);
                    StreamExport(dt, top, "Excel文件.xls");
                }
            }
        }
        /// <summary>
        /// 返回一张表
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <returns></returns>
        protected DataTable GetReturnJson(string json, string proc)
        {
            DataTable dt = null;
            SqlParameter[] param = (ListParameter(json)).ToArray();//动态解析json参数
            DataSet ds = DbHelperSQL.RunProcedure(proc, param, "Table");
            if (ds.Tables.Count >= 3)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    dt = ds.Tables[1];

                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        /// <summary>
        /// 动态解析json生成List<SqlParameter>
        /// </summary>
        /// <returns></returns>
        public static List<SqlParameter> ListParameter(string json)
        {
            List<SqlParameter> listPrmt = new List<SqlParameter>();
            JObject o = JObject.Parse(json);
            IEnumerable<JProperty> properties = o.Properties();
            foreach (JProperty item in properties)
            {
                if (item.Name.Trim() != "")
                {

                    listPrmt.Add(new SqlParameter("@" + item.Name, item.Value.ToString().Trim() ?? ""));
                }
            }
            return listPrmt;
        }
        /// <summary>
        /// DataTable通过流导出Excel
        /// </summary>
        /// <param name="ds">数据源DataSet</param>
        /// <param name="columns">DataTable中列对应的列名(可以是中文),若为null则取DataTable中的字段名</param>
        /// <param name="fileName">保存文件名(例如：a.xls)</param>
        /// <returns></returns>
        public bool StreamExport(System.Data.DataTable dt, string[] columns, string fileName)
        {
            if (dt.Rows.Count > 65535) //总行数大于Excel的行数 
            {
                throw new Exception("预导出的数据总行数大于excel的行数");
            }
            if (string.IsNullOrEmpty(fileName)) return false;

            StringBuilder content = new StringBuilder();
            StringBuilder strtitle = new StringBuilder();
            content.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'>");
            content.Append("<head><title></title><meta http-equiv='Content-Type' content=\"text/html; charset=gb2312\">");
            //注意：[if gte mso 9]到[endif]之间的代码，用于显示Excel的网格线，若不想显示Excel的网格线，可以去掉此代码
            content.Append("<!--[if gte mso 9]>");
            content.Append("<xml>");
            content.Append(" <x:ExcelWorkbook>");
            content.Append("  <x:ExcelWorksheets>");
            content.Append("   <x:ExcelWorksheet>");
            content.Append("    <x:Name>Sheet1</x:Name>");
            content.Append("    <x:WorksheetOptions>");
            content.Append("      <x:Print>");
            content.Append("       <x:ValidPrinterInfo />");
            content.Append("      </x:Print>");
            content.Append("    </x:WorksheetOptions>");
            content.Append("   </x:ExcelWorksheet>");
            content.Append("  </x:ExcelWorksheets>");
            content.Append("</x:ExcelWorkbook>");
            content.Append("</xml>");
            content.Append("<![endif]-->");
            content.Append("</head><body><table style='border-collapse:collapse;table-layout:fixed;'><tr>");

            if (columns != null)
            {
                for (int i = 0; i < columns.Length - 1; i++)
                {
                    if (columns[i] != null && columns[i] != "")
                    {
                        content.Append("<td><b>" + columns[i] + "</b></td>");
                    }
                    else
                    {
                        content.Append("<td><b>" + dt.Columns[i].ColumnName + "</b></td>");
                    }
                }
            }
            else
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    content.Append("<td><b>" + dt.Columns[j].ColumnName + "</b></td>");
                }
            }
            content.Append("</tr>\n");

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                content.Append("<tr>");
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    object obj = dt.Rows[j][k];
                    Type type = obj.GetType();
                    if (type.Name == "Int32" || type.Name == "Single" || type.Name == "Double" || type.Name == "Decimal")
                    {
                        double d = obj == DBNull.Value ? 0.0d : Convert.ToDouble(obj);
                        if (type.Name == "Int32" || (d - Math.Truncate(d) == 0))
                            content.AppendFormat("<td style='vnd.ms-excel.numberformat:#,##0'>{0}</td>", obj);
                        else
                            content.AppendFormat("<td style='vnd.ms-excel.numberformat:#,##0.00'>{0}</td>", obj);
                    }
                    else
                        content.AppendFormat("<td style='vnd.ms-excel.numberformat:@'>{0}</td>", obj);
                }
                content.Append("</tr>\n");
            }
            content.Append("</table></body></html>");
            content.Replace("&nbsp;", "");
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";  //"application/ms-excel";
            Response.Charset = "UTF-8";
            // Response.ContentEncoding = System.Text.Encoding.UTF7;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            fileName = System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.Write(content.ToString());
            //pages.Response.End();  //注意，若使用此代码结束响应可能会出现“由于代码已经过优化或者本机框架位于调用堆栈之上,无法计算表达式的值。”的异常。
            HttpContext.Current.ApplicationInstance.CompleteRequest(); //用此行代码代替上一行代码，则不会出现上面所说的异常。
            return true;
        }
        private string[] Columns(DataTable dt)
        {
            string ColumnName = "";
            DataTable dt_top = getChinaName(dt);

            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName.Contains("ROWSTAT"))
                    continue;
                DataRow[] dr = dt_top.Select("fdname = '" + dc.ColumnName + "'");
                string chnname = dr.Length == 1 ? dr[0]["chnname"].ToString() : dc.ColumnName;
                ColumnName += chnname + ",";
            }
            string[] Excel_Top = ColumnName.Split(',');
            return Excel_Top;
        }
        public static DataTable getChinaName(DataTable dt)
        {
            string cmdText = "select fdname,chnname from fldlist where fdname in(";
            if (dt == null || dt.Columns.Count == 0)
            {
                return null;
            }
            foreach (DataColumn dc in dt.Columns)
            {
                cmdText += "'" + dc.ColumnName.Trim() + "',";
            }
            cmdText = cmdText.Substring(0, cmdText.Length - 1);
            cmdText += ")";
            return DbHelperSQL.Query(cmdText).Tables[0];
        }
    }
}