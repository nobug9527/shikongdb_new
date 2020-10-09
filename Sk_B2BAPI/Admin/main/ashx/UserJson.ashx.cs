using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models.Admin;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Sk_B2BAPI.Admin.main.ashx
{
    /// <summary>
    /// UserJson 的摘要说明
    /// </summary>
    public class UserJson : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = context.Request.QueryString["type"].Trim();//请求类型
            string json = context.Request.QueryString["json"].Trim();//请求参数(json类型)
            string proc = context.Request.QueryString["proc"].Trim();//存储过程名称
            string returnJson = "";
            switch (type)
            {
                case "login":
                    returnJson = UserLogin(json, proc, context);
                    break;
                case "loginOut":
                    returnJson = loginOut(context);
                    break;
                case "UrlAddress":
                    Dt_User user = RoleFuns.IsLoginAdmin(context.Session["user"]);
                    if (user == null)
                    {
                        returnJson = JsonMethod.GetError(2, "登陆超时,请重新登陆！");
                    }
                    else {
                        returnJson = GettIndexUrlJson(user.entId,user.username);
                    }
                    break;
                default:
                    returnJson = JsonMethod.GetError(4, "参数错误！");
                    break;
            }
            context.Response.Write(returnJson);
        }
        
        /// <summary>
        /// 管理员登陆
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <returns></returns>
        protected string UserLogin(string json, string proc, HttpContext context)
        {
            string r_json = "";
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            var admincode = obj["code"].ToString();
            //Log.Debug("登录回传验证码：", admincode);
            var code = context.Session["AdminLognCode"];
            //Log.Debug("后台保存二维码：", code.ToString());
            if (string.IsNullOrEmpty(admincode) || code == null || code.ToString().ToLower() != admincode.ToLower())
            {
                return JsonMethod.GetError(1, "验证码不正确！");
            }
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@type",obj["type"].ToString()),
                new SqlParameter("@username",obj["username"].ToString()),
            };

            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                var pass = Encryption.GetMD5_16(obj["password"].ToString().ToString());
                if (dt.Rows[0]["password"].ToString() == Encryption.GetMD5_16(obj["password"].ToString().ToString()))
                {
                    context.Session.Timeout = 500;
                    //DataTable dq = ds.Tables[1];

                    context.Session["user"] = dt;
                    //context.Session["userId"] = dt.Rows[0]["userId"].ToString();
                    //context.Session["entId"] = dt.Rows[0]["entId"].ToString();
                    //context.Session["name"] = dt.Rows[0]["name"].ToString();
                    //context.Session["password"] = dt.Rows[0]["password"].ToString();
                    //context.Session["roleId"] = dt.Rows[0]["role_id"].ToString();
                    //context.Session["roleType"] = dt.Rows[0]["role_type"].ToString();
                    //context.Session["username"] = dt.Rows[0]["username"].ToString();

                    context.Session["role"] = null;// DateTableTool.DataTableToList<Rolestr>(dq).Select(a=>a.power).ToList();
                    RoleFuns.AddAdminLoginLog(dt.Rows[0]["username"].ToString(), context.Request.ServerVariables.Get("REMOTE_ADDR").ToString(), context.Request.ServerVariables.Get("REMOTE_PORT").ToString());
                    r_json = JsonMethod.GetError(0, "登陆成功");
                }
                else
                {
                    r_json = JsonMethod.GetError(1, "密码错误");
                }
            }
            else
            {
                r_json = JsonMethod.GetError(1, "该账号不存在");
            }
            return r_json;
        }
        /// <summary>
        ///  返回DataTable(权限访问)
        /// </summary>
        /// <param name="userId">用户主键</param>
        /// <param name="entId"></param>
        /// <param name="ConfigType">权限等级</param>
        /// <returns></returns>
        protected string GettIndexUrlJson(string entid,string username)
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            List<Dt_Method> list = new List<Dt_Method>();
            if (entid == "superintendent")
            {
                list = DateTableTool.DataTableToList<Dt_Method>(sql.RtDataTable($"select * from Dt_Method m join [Dt_RoleMethod] rm on rm.MethodId=m.Id join dt_users us on rm.RoleId=us.role_id and us.entid='superintendent'"));
            }
            else {
                list = DateTableTool.DataTableToList<Dt_Method>(sql.RtDataTable($"select m.* from Dt_Method m join [Dt_RoleMethod] rm on rm.MethodId=m.Id join dt_users us on rm.RoleId=us.role_id where m.status=1  and us.entid='" + entid + "' and us.username='"+ username + "' and m.Source in('B2B','ALL')"));
            }
            
            if (list.Count() > 0)
            {
                ///加载Html
                string urlList = "";
                foreach (Dt_Method meth in list.Where(a => a.FatherId == null))
                {

                    urlList += "<dl id='menu-" + meth.Id + "'>";
                    urlList += "     <dt ><i class='Hui-iconfont ztbj' style='font-size: 20px'>" + meth.IcoAddress + "</i> " + meth.Title + "<i class='Hui-iconfont menu_dropdown-arrow'>&#xe6d5;</i></dt>";
                    urlList += "    <dd>";
                    urlList += "        <ul>";
                    foreach (Dt_Method son in list.Where(a => a.FatherId == meth.Id).ToList())
                    {
                        urlList += "<li><a data-href='" + son.Power + "' data-title='" + son.Title + "' href='javascript:void(0)'><i class='Hui-iconfont'>" + son.IcoAddress + "</i> " + son.Title + "</a></li>";
                    }
                    urlList += "        </ul>";
                    urlList += "    </dd>";
                    urlList += "</dl>";


                }
                return "{\"flag\":\"0\",\"urlList\":\"" + urlList + "\"}";
            }
            else
            {
                string error = "无数据";
                return JsonMethod.GetError(1, error);
            }
        }
        private class Rolestr
        {
            public string power { get; set; }
        }
        /// <summary>
        /// 用户退出登陆
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected string loginOut(HttpContext context)
        {
            context.Session["user"] = null;
            context.Session["role"] = null;
            return JsonMethod.GetError(0, "退出成功");
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