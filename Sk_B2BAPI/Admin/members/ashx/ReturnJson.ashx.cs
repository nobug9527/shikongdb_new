using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Sk_B2BAPI.App_Code;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Mvc;
using Sk_B2BAPI.Models.Admin;

namespace Sk_B2BAPI.Admin.members.ashx
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
                    string type = context.Request.QueryString["type"].Trim();//请求类型
                    string json = context.Request.QueryString["json"].Trim();//请求参数(json类型)
                    string proc = context.Request.QueryString["proc"].Trim();//存储过程名称
                    string password = user.password;
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
                                case "ReturnDataSet":
                                    returnJson = ReturnDataSetJson(json, proc, user.userId, user.entId);
                                    break;
                                case "UpdateUserInfo":
                                    returnJson = UpdateUserInfo(json, password);
                                    break;
                                case "AuditUser":
                                    returnJson = AuditUser(json);
                                    break;
                                case "AuditCustomer":
                                    returnJson = AuditCustomer(json);
                                    break;
                                case "SaveCustomer":
                                    returnJson = SaveCustomer(json);
                                    break;
                                default:
                                    returnJson = JsonMethod.GetError(1, "参数错误！");
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
        /// 返回DataTable
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string GetReturnJson(string json, string proc, string userId, string entId)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entId)).ToArray();//动态解析json参数
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
        /// 获取多个表返回Json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string ReturnDataSetJson(string json, string proc, string userId, string entId)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet(proc, param);
            string r_json;
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
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        protected string UpdateUserInfo(string json,string password)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            string pwd = "";
            if (obj["pwd"].ToString().Trim() == password)
            {
                pwd = obj["pwd"].ToString().Trim();
            }
            else
            {
                pwd = Encryption.GetMD5_16(obj["pwd"].ToString().Trim());
            }
            string name = obj["name"].ToString();
            string sex = obj["sex"].ToString();
            string telphone = obj["telphone"].ToString();
            string birthday = obj["birthday"].ToString();
            string point=obj["point"].ToString();
            string status=obj["status"].ToString();
            string userIds= obj["userId"].ToString();

            StringBuilder strSql = new StringBuilder();
            strSql.Append($"update dt_users set name='{name}',sex='{sex}',telphone='{telphone}',birthday='{birthday}',point='{point}',status='{obj["status"].ToString()}',");
            strSql.Append($"password='{pwd}' where userId='{userIds}'"); //entId = @entId,
            SqlRun sql = new SqlRun(SqlRun.sqlstr);

            bool flag  = sql.ExecuteSql(strSql.ToString());
            if (flag)
            {
                return JsonMethod.GetError(0, "存盘成功");
            }
            else
            {
                return JsonMethod.GetError(1, "操作失败");
            }
        }
        /// <summary>
        /// 用户审核
        /// </summary>
        public string AuditUser(string json)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_users set status=@status where userId=@userId");
            SqlParameter[] pram = new SqlParameter[] { 
                new SqlParameter("@status",obj["status"].ToString()),
                new SqlParameter("@userId",obj["userId"].ToString()),
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int n= sql.ExecuteSql(strSql.ToString(), pram);
            if (n>0)
            {
                return JsonMethod.GetError(0, "操作成功");
            }
            else
            {
                return JsonMethod.GetError(1, "操作失败");
            }
        }
        /// <summary>
        /// 客户审核
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string AuditCustomer(string json)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_businessdoc set beactive=@status where businessid=@businessId and entid=@entId");
            SqlParameter[] pram = new SqlParameter[] { 
                new SqlParameter("@status",obj["status"].ToString()),
                new SqlParameter("@businessId",obj["businessId"].ToString()),
                new SqlParameter("@entId",obj["entId"].ToString()),
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int n = sql.ExecuteSql(strSql.ToString(), pram);
            if (n > 0)
            {
                return JsonMethod.GetError(0, "操作成功");
            }
            else
            {
                return JsonMethod.GetError(1, "操作失败");
            }
        }
        public string SaveCustomer(string json)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_businessdoc set businesscode=@businesscode,businessname=@businessname,entid=@entid,clienttype=@clienttype,");
            strSql.Append("beactive=@beactive,shortname=@shortname,businesscont=@businesscont,address=@address,wtr=@wtr,wtsyxq=@wtsyxq,");
            strSql.Append("xkzyxq=@xkzyxq,yyzzyxq=@yyzzyxq,gspzsyxq=@gspzsyxq where entid=@oldentid and businessid=@businessid ");
            SqlParameter[] pram = new SqlParameter[] { 
                new SqlParameter("@businesscode",obj["businesscode"].ToString()),
                new SqlParameter("@businessname",obj["businessname"].ToString()),
                new SqlParameter("@entid",obj["entid"].ToString()),
                new SqlParameter("@clienttype",obj["clienttype"].ToString()),
                new SqlParameter("@beactive",obj["beactive"].ToString()),
                new SqlParameter("@shortname",obj["shortname"].ToString()),
                new SqlParameter("@businesscont",obj["businesscont"].ToString()),
                new SqlParameter("@address",obj["address"].ToString()),
                new SqlParameter("@wtr",obj["wtr"].ToString()),
                new SqlParameter("@wtsyxq",obj["wtsyxq"].ToString()),
                new SqlParameter("@xkzyxq",obj["xkzyxq"].ToString()),
                new SqlParameter("@yyzzyxq",obj["yyzzyxq"].ToString()),
                new SqlParameter("@gspzsyxq",obj["gspzsyxq"].ToString()),
                new SqlParameter("@oldentid",obj["oldentid"].ToString()),
                new SqlParameter("@businessid",obj["businessid"].ToString())
            };
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int n = sql.ExecuteSql(strSql.ToString(), pram);
            if (n > 0)
            {
                return JsonMethod.GetError(0, "存盘成功");
            }
            else
            {
                return JsonMethod.GetError(1, "存盘失败");
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