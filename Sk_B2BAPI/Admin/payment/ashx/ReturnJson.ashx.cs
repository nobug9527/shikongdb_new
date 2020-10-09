using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Sk_B2BAPI.Models.Admin;
using System.Collections.Generic;

namespace Sk_B2BAPI.Admin.payment.ashx
{
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
                    string proc = context.Request.Params["proc"].Trim();//存储过程名称
                    string json = context.Request.Params["json"].Trim();//请求参数(json类型)
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
                            //执行查询返回列表
                            switch (type)
                            {
                                case "ReturnList":
                                    returnJson = GetReturnJson(json, proc, user.userId, user.entId);
                                    break;
                                case "SavePayment":
                                    returnJson = SavePayment(json, proc, user.userId, user.entId);
                                    break;
                                case "ReturnNumber":
                                    returnJson = EnablePayment(json, proc, user.userId, user.entId);
                                    break;
                                case "ReturnDataTable":
                                    returnJson = ReturnDataTableJson(json, proc, user.userId, user.entId);
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
        /// <param name="entId"></param>
        /// <returns></returns>
        protected string GetReturnJson(string json, string proc, string userId, string entId)
        {
            string r_json;
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            if (ds.Tables.Count >= 3)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    int recordCount = Convert.ToInt32(ds.Tables[2].Rows[0]["recordCount"].ToString().Trim());
                    int pageCount = Convert.ToInt32(ds.Tables[2].Rows[0]["pageCount"].ToString().Trim());
                    DataTable table = sql.GetChinaName(ds.Tables[1]);
                    r_json = JsonMethod.GetDataTable(0, recordCount, pageCount, table, ds.Tables[1]);
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
        /// 数据存盘返回成功与否
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userid"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string SavePayment(string json,string proc,string userid,string entid)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userid, entid)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int number = sql.ExecuteNonQuery(proc, param);
            if (number>0)
            {
                return JsonMethod.GetError(0, "操作成功");
            }
            else
            {
                return JsonMethod.GetError(1, "操作失败");
            }
        }
        /// <summary>
        /// 修改是否启用
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userid"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string EnablePayment(string json,string proc,string userid,string entid)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userid, entid)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int number = sql.ExecuteNonQuery(proc, param);
            if (number > 0)
            {
                return JsonMethod.GetError(0, "操作成功");
            }
            else
            {
                return JsonMethod.GetError(1, "操作失败");
            }
        }

        /// <summary>
        /// 获取多个表返回Json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string ReturnDataTableJson(string json, string proc, string userId, string entId)
        {
            string r_json;
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable table = sql.RunProcedureDR(proc, param);
            if (table.Rows.Count > 0)
            {
                r_json = JsonMethod.DataTableToJson("0",table);
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