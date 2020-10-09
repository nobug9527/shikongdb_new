using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DTcms.DBUtility;

namespace DTcms.Web.admin.FactoryClerk.ashx
{
    /// <summary>
    /// InfoAdmin 的摘要说明
    /// </summary>
    public class InfoAdmin : IHttpHandler
    {
        public int return_code;
        public void ProcessRequest(HttpContext context)
        {
            string Json = "";
            try
            {
                string type = context.Request.QueryString["Type"].Trim();
                if (type == "ExecuteNonQuery")
                {
                    Json = DataTable(context);
                }
            }
            catch (Exception e)
            {
                return_code = 2;
                string error = e.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
            }
            context.Response.Write(Json);
        }
        protected string DataTable(HttpContext context)
        {
            string Json = "";
            int rowsAffected = 0;
            string list = context.Request.QueryString["list"].ToString().Trim();
            string ywyId = context.Request.QueryString["ywyId"].ToString().Trim();
            string Procedure = context.Request.QueryString["SqlProcedure"].ToString().Trim();
            string SqlType = context.Request.QueryString["SqlType"].ToString().Trim();
            SqlParameter[] sqlpmt = new SqlParameter[] 
                { 
                   new SqlParameter("@type",SqlType),
                       new SqlParameter("@List",list),
                        new SqlParameter("@YwyId ",ywyId)
                };
            int n = DbHelperSQL.RunProcedure(Procedure, sqlpmt, out rowsAffected);
            if (rowsAffected > 0)
            {
                Json = DTcms.Common.GetJson.GetErrorJson(0, 0, "操作成功！");
            }
            else
            {
                Json = DTcms.Common.GetJson.GetErrorJson(1, 0, "操作失败！");
            }
            return Json;
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