using System;
using System.Web;
using System.Data;
using System.Web.SessionState;
using System.Data.SqlClient;

namespace DTcms.Web.admin.jfddcx.ashx
{
    /// <summary>
    /// ddmx 的摘要说明
    /// </summary>
    public class ddmx : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Json = "";
            int TYPE, count;
            try
            {
                string djbh = context.Request.QueryString["djbh"].ToString().Trim();

                SqlParameter[] sqlpmt = new SqlParameter[]{
                new SqlParameter("@TYPE","ddmx"),              
                new SqlParameter("@djbh",djbh),
                };
                DataSet ds = DTcms.DBUtility.DbHelperSQL.RunProcedure("Proc_Premiun_user", sqlpmt, "Table");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TYPE = 0;
                    count = 0;
                    int pageCount = 0;
                    Json = DTcms.Common.GetJson.GetDataJson(TYPE, count, pageCount, ds.Tables[0]);
                }

                else
                {
                    TYPE = 1;
                    string error = "查询无结果";
                    count = 0;
                    Json = DTcms.Common.GetJson.GetErrorJson(TYPE, count, error);

                }
            }
            catch (Exception ex)
            {
                TYPE = 2;
                string error = ex.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                Json = DTcms.Common.GetJson.GetErrorJson(TYPE, 0, error);
            }
            context.Response.Write(Json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}