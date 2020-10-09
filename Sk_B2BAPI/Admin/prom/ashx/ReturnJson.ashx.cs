using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Sk_B2BAPI.App_Code;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Sk_B2BAPI.Models.Admin;

namespace Sk_B2BAPI.Admin.prom.ashx
{
    /// <summary>
    /// returnJson 的摘要说明
    /// </summary>
    public class ReturnJson : IHttpHandler,IRequiresSessionState
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
                    string type = context.Request.QueryString["type"].Trim();//请求类型
                    string json = context.Request.QueryString["json"].Trim();//请求参数(json类型)
                    string proc = context.Request.QueryString["proc"].Trim();//存储过程名称
                    JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                    if (obj["type"] != null)
                    {
                        List<string> rolestr = context.Session["role"] != null ? (List<string>)context.Session["role"] : null;
                        RoleFuns.SetAdminLog(user.username, obj["type"].ToString(), rolestr);
                        //执行查询返回列表
                        if (type == "ReturnList")
                        {
                            returnJson = GetReturnJson(json, proc, user.userId, user.entId);
                        }
                        //执行修改，插入返回影响行数
                        else if (type == "ReturnNumber")
                        {
                            returnJson = GetReturnJsonInt(json, proc, user.userId, user.entId);
                        }
                        //执行修改，插入返回执行结果
                        else if (type == "ReturnListNumber")
                        {
                            returnJson = GetReturnDs(json, proc, user.userId, user.entId);
                        }
                        else if (type == "ReturnDataColumnToJson")
                        {
                            returnJson = ReturnDataColumnToJson(json, proc, user.userId, user.entId);
                        }
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
                returnJson =  JsonMethod.GetError(4, msg);;
            }
            context.Response.Write(returnJson);
        }
        /// <summary>
        /// 获取数据返回数据列表
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string GetReturnJson(string json, string proc, string userId, string entid)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entid)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            string r_json;
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
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entid)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int n = sql.ExecuteNonQuery(proc, param);
            string r_json;
            string msg;
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
        /// <summary>
        /// 更新数据库的数据，返回一个表
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <returns></returns>
        protected string GetReturnDs(string json, string proc, string UserId, string entid)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, UserId, entid)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR(proc, param);
            string r_json;
            if (dt.Rows.Count > 0)
            {
                int flag = int.Parse(dt.Rows[0]["flag"].ToString() ?? "1");
                string msg = dt.Rows[0]["msg"].ToString();
                r_json = JsonMethod.GetError(flag, msg);
            }
            else
            {
                string msg = dt.Rows[0]["msg"].ToString();
                r_json = JsonMethod.GetError(1, msg);
            }
            return r_json;
        }


        protected string ReturnDataColumnToJson(string json, string proc, string userId, string entid)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entid)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            string r_json;
            if (ds.Tables.Count > 0)
            {
                r_json = JsonMethod.DataColumnToJson(ds);
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }
            return r_json;
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