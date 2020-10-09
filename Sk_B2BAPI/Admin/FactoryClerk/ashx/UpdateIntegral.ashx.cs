using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DTcms.Web.admin.FactoryClerk.ashx
{
    /// <summary>
    /// UpdateIntegral 的摘要说明
    /// </summary>
    public class UpdateIntegral : IHttpHandler
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
            string dwId = context.Request.QueryString["dwId"].ToString().Trim();
            string JF = context.Request.QueryString["JF"].ToString().Trim();
            string GdPoint = context.Request.QueryString["GdPoint"].ToString().Trim();
            string xyJf = context.Request.QueryString["xyJf"].ToString().Trim();
            string Procedure = context.Request.QueryString["SqlProcedure"].ToString().Trim();
            string SqlType = context.Request.QueryString["SqlType"].ToString().Trim();
            SqlParameter[] sqlpmt = new SqlParameter[]
                {
                   new SqlParameter("@type",SqlType),
                   new SqlParameter("@DwId",dwId),
                   new SqlParameter("@JF ",JF),
                   new SqlParameter("@YPoint",xyJf),
                   new SqlParameter("@GdPoint",GdPoint)
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