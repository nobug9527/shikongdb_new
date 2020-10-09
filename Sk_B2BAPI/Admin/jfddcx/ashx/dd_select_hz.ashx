<%@ WebHandler Language="C#" Class="dd_select_hz" %>

using System;
using System.Web;
using System.Data;
using System.Web.SessionState;
using System.Data.SqlClient;

public class dd_select_hz : IHttpHandler {
   
    public void ProcessRequest (HttpContext context) {
        string Json = "";
        int TYPE, count;
        try
        {
            string dwmch = context.Request.QueryString["username"].ToString().Trim();
            string yema = context.Request.QueryString["page"].ToString().Trim();
            SqlParameter[] sqlpmt = new SqlParameter[]{
                new SqlParameter("@TYPE","getJFDD"),              
                new SqlParameter("@var",dwmch),
                 new SqlParameter("@page",yema)
                };
            DataSet ds = DTcms.DBUtility.DbHelperSQL.RunProcedure("Proc_Premiun_details", sqlpmt, "Table");
            if (ds.Tables[0].Rows.Count > 0)
            {
                TYPE = 0;
                count = 0;
                 int  pageCount = Convert.ToInt32(ds.Tables[1].Rows[0]["result"].ToString().Trim());
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
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}