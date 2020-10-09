using DTcms.DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DTcms.Web.admin.SignLog.ashx
{
    /// <summary>
    /// returnJson 的摘要说明
    /// </summary>
    public class returnJson : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string returnJson = "";
            try
            {
                Dt_User user = RoleFuns.IsLoginAdmin(context.Session["user"]);
                string type = context.Request.QueryString["type"].Trim();//请求类型
                string json = context.Request.QueryString["json"].Trim();//请求参数(json类型)
                string proc = context.Request.QueryString["proc"].Trim();//存储过程名称
                if (user != null)
                {
                    switch (type)
                    {
                        case "ReturnList":
                            returnJson = GetReturnJson(json, proc, user.userId, user.entId);
                            break;
                        case "ReturnDataSet":
                            returnJson = ReturnDataSetJson(json, proc, user.userId, user.entId);
                            break;
                        case "ReturnNumber":
                            returnJson = GetReturnJsonInt(json, proc, user.userId, user.entId);
                            break;
                        case "ReturnListNumber":
                            returnJson = GetReturnDs(json, proc, user.userId, user.entId);
                            break;
                        default:
                            returnJson = JsonMethod.GetError(1, "参数错误！");
                            break;
                    }
                }
                else
                {
                    returnJson = JsonMethod.GetError(2, "登陆超时,请重新登陆！");
                }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                returnJson = JsonMethod.GetError(1, error);
            }
            JObject jo = (JObject)JsonConvert.DeserializeObject(returnJson);
            context.Response.Write(jo);
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
        /// <summary>
        /// 更新数据库的数据，返回一个表
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <returns></returns>
        protected string GetReturnDs(string json, string proc, string UserId, string entid)
        {
            string r_json = "";
            SqlParameter[] param = (JsonMethod.ListParameter(json, UserId, entid)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RunProcedureDR(proc, param);
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
        /// <summary>
        /// 获取多个表返回Json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string ReturnDataSetJson(string json, string proc, string userId, string entId)
        {
            string r_json = "";
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            if (ds.Tables.Count > 0)
            {
                r_json = JsonMethod.DataSetToJson("0", ds);
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