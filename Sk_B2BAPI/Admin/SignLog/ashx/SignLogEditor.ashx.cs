using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Sk_B2BAPI.Admin.SignLog.ashx
{
    /// <summary>
    /// SignLogEditor 的摘要说明
    /// </summary>
    public class SignLogEditor : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string Json = "";

            try
            {
                Dt_User user = RoleFuns.IsLoginAdmin(context.Session["user"]);
                if (user != null)//登录检测，权限检测 context.Session["user"]
                {
                    string type = context.Request.QueryString["Type"].Trim();
                    if (type == "GetClientType")
                    {
                        Json = GetClientType(context, user.userId, user.entId);
                    }
                }
                else
                {
                    Json = JsonMethod.GetError(2, "登陆超时,请重新登陆！");
                }
            }
            catch (Exception e)
            {
                Json = JsonMethod.GetError(1, e.Message);
            }
            //.Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "")
            context.Response.Write(Json);
        }

        private string GetClientType(HttpContext context, string userid, string entid)
        {
            string Json = "";
            string sqlStr = "select TypeID,ClientType from dt_CustomerType where status=1   order by TypeID asc";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RtDataTable(sqlStr);
            if (dt.Rows.Count > 0)
            {
                Json = JsonMethod.GetDataTable(0, dt.Rows.Count, 1, dt);
            }
            else
            {
                Json = JsonMethod.GetError(1, "客户分类加载失败");
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