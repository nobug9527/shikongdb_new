using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models.Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Xml;

namespace Sk_B2BAPI.Admin.rules.ashx
{
    /// <summary>
    /// ReturnJson 的摘要说明
    /// </summary>
    /// <summary>
    /// ReturnJson 的摘要说明
    /// </summary>
    public class ReturnJson : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string returnJson = "";
            try
            {
                Dt_User user = RoleFuns.IsLoginAdmin(context.Session["user"]);
                if (user != null)//登录检测，权限检测 context.Session["user"]
                {
                    string type = context.Request.Params["type"].Trim();//请求类型
                    string json = context.Request.Params["json"].Trim();//请求参数(json类型)
                    string proc = context.Request.Params["proc"].Trim();//存储过程名称
                    JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                    if (obj["type"] != null)
                    {
                        List<string> rolestr = context.Session["role"] != null ? (List<string>)context.Session["role"] : null;
                        RoleFuns.SetAdminLog(user.username, obj["type"].ToString(), rolestr);
                        //if (/*rolestr == null || RoleFuns.SetAdminLog(user.username, obj["type"].ToString(), rolestr) == 0*/false)
                        //{
                        //    returnJson = JsonMethod.GetError(4, "抱歉您没有权限进入！");
                        //}
                        //else {
                        switch (type)
                        {
                            case "ReturnList":
                                returnJson = GetReturnJson(json, proc, user.userId, user.entId);
                                break;
                            case "ReturnNumber":
                                returnJson = GetReturnJsonInt(json, proc, user.userId, user.entId);
                                break;
                            default:
                                returnJson = JsonMethod.GetError(4, "参数错误！");
                                break;
                        }
                        //}
                    }


                }
                else
                {
                    returnJson = JsonMethod.GetError(2, "登陆超时,请重新登陆！");
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                returnJson = JsonMethod.GetError(2, msg);
            }
            context.Response.Write(returnJson);
        }
        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string GetReturnJson(string json, string proc, string userId, string entId)
        {
            string r_json = "";
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            if (ds.Tables.Count >= 3)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    int recordCount = Convert.ToInt32(ds.Tables[2].Rows[0]["recordCount"].ToString().Trim());
                    int pageCount = Convert.ToInt32(ds.Tables[2].Rows[0]["pageCount"].ToString().Trim());
                    r_json = JsonMethod.GetDataTable(0, recordCount, pageCount, ds.Tables[1]);
                }
                else
                {
                    string error = "无数据";
                    r_json = JsonMethod.GetError(1, error);
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int recordCount = 0;
                    int pageCount = 0;
                    r_json = JsonMethod.GetDataTable(0, recordCount, pageCount, ds.Tables[0]);
                }
                else
                {
                    string error = "无数据";
                    r_json = JsonMethod.GetError(1, error);
                }
            }
            return r_json;
        }

        /// <summary>
        /// 更新数据目录
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <returns></returns>
        protected string GetReturnJsonInt(string json, string proc, string userId, string entid)
        {
            try
            {
                string r_json = "";
                var msg = "";
                SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entid)).ToArray();//动态解析json参数
                SqlRun sql = new SqlRun(SqlRun.sqlstr);
                int n = sql.ExecuteNonQuery(proc, param);
                if (n > 0)
                {
                    msg = "更新成功";
                    r_json = JsonMethod.GetError(0, msg);
                }
                else
                {
                    msg = "更新失败！";
                    r_json = JsonMethod.GetError(1, msg);
                }
                return r_json;
            }
            catch (Exception ex)
            {
                string r_json = JsonMethod.GetError(1, ex.Message);
                return r_json;
            }
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