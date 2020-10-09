using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Sk_B2BAPI.Admin.manage.ashx
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
                    JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                    if (obj["type"] != null)
                    {
                        List<string> rolestr = context.Session["role"] != null ? (List<string>)context.Session["role"] : null;
                        RoleFuns.SetAdminLog(user.username, obj["type"].ToString(), rolestr);
                        //if (/*rolestr == null || RoleFuns.SetAdminLog(user.username, obj["type"].ToString(), rolestr) == 0*/false)
                        //{
                        //    returnJson = JsonMethod.GetError(4, "抱歉您没有权限进入！");
                        //}
                        //else
                        //{
                            switch (type)
                            {
                                case "ReturnNumber":
                                    returnJson = GetReturnJsonInt(json, proc, user.userId, user.entId);
                                    break;
                                case "ReturnList":
                                    returnJson = GetReturnJson(json, proc, user.userId, user.entId);
                                    break;
                                case "ReturnBox":
                                    returnJson = GetReturnJsonBox(json, proc, obj["type"].ToString(), user.userId, user.entId);
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
                returnJson = JsonMethod.GetError(3, msg);
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
            List<SqlParameter> listPrmt = new List<SqlParameter>();
            JObject o = JObject.Parse(json);
            IEnumerable<JProperty> properties = o.Properties();
            bool flag = true;
            foreach (JProperty item in properties)
            {
                if (item.Name.Trim() != "")
                {
                    if (item.Name.ToString() == "Entid")
                    {
                        flag = false;
                    }

                    if (item.Name.ToString() == "PassWord")//密码md5加密
                    {
                        listPrmt.Add(new SqlParameter("@" + item.Name, Encryption.GetMD5_16(item.Value.ToString())));
                    }
                    else if (item.Name.ToString() == "XPassWord")//密码md5加密
                    {
                        listPrmt.Add(new SqlParameter("@" + item.Name, Encryption.GetMD5_16(item.Value.ToString())));
                    }
                    else
                    {
                        listPrmt.Add(new SqlParameter("@" + item.Name, item.Value.ToString().Trim() ?? ""));
                    }
                }
            }
            if (userId != "")
            {
                listPrmt.Add(new SqlParameter("@UserId", userId));
            }
            if (entid != "" && flag == true)
            {
                listPrmt.Add(new SqlParameter("@entId", entid));
            }
            SqlParameter[] param = listPrmt.ToArray();//动态解析json参数
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            string familyid = "";
            if (obj["type"].ToString()== "GetAdminList") {
                familyid = Familylist(userId, entid);
                Log.Info("家族主键", familyid);
            }
            if (familyid != "" && flag == true)
            {
                listPrmt.Add(new SqlParameter("@familyid", familyid));
            }
            //获取顶级以及最底层所有会员的状态
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
        /// 获取整个机构的管理员，只获取下级
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string Familylist(string userId,string entid) {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunSqlDataTable($"select UserId,FatherUserId from dt_users where  role_type=2 and entid='{entid}'");
            if (ds.Rows.Count>0) {
                StringBuilder builder = new StringBuilder();
                return DgId(userId,ds.Rows, builder).ToString(); 
            }
            else {
                return "";
            }
            
        }
        
        /// <summary>
        /// 递归循环
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        StringBuilder DgId(string userId, DataRowCollection ds,StringBuilder builder) {
            foreach (DataRow model in ds) {
                if (model["FatherUserId"].ToString()== userId) {
                    builder.Append("'"+model["FatherUserId"].ToString()+"',");
                    DgId(model["FatherUserId"].ToString(), ds, builder);
                }
            }
            return builder;
        }


        /// <summary>
        /// 返回DataTable()
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string GetReturnJsonBox(string json, string proc, string typename, string userId, string entid)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entid)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR(proc, param);
            ///加载节点
            string objList = "";
            DataTable dtc = ds;
            if (dtc.Rows.Count > 0)
            {
                switch (typename)
                {
                    case "Pc_GetFatherMethodUrl"://权限路径
                        foreach (DataRow dr_c1 in dtc.Rows)
                        {
                            objList += "<option value='" + dr_c1["id"] + "'>" + dr_c1["Title"] + "</option>";
                        }
                        break;
                    default:
                        break;
                }

            }
            return "{\"flag\":\"0\",\"objList\":\"" + objList + "\"}";

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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}