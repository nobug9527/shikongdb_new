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

namespace Sk_B2BAPI.Admin.main.ashx
{
    /// <summary>
    /// ReturnJson 的摘要说明
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
                                case "ReturnNumber":
                                    returnJson = GetReturnJsonInt(json, proc, user.userId, user.entId);
                                    break;
                                case "ReturnListNumber":
                                    returnJson = GetReturnDs(json, proc, user.userId, user.entId);
                                    break;
                                case "SaveBase":
                                    returnJson = SaveBase(json, proc, user.userId, user.entId);
                                    break;
                                case "GetWebSiteBase":
                                    returnJson = GetWebSiteBase(json, proc, user.userId, user.entId);
                                    break;
                                case "GetConfig":
                                    //returnJson = GetWebConfig(json, proc, user.userId, user.entId);
                                    break;
                                case "jiancerole":
                                    returnJson = GetRoleJson(json, proc, user.userId, user.entId);
                                    break;
                                case "Jy":
                                    returnJson = UpdateUpJson(context.Request.QueryString["status"].Trim(), context.Request.QueryString["strWhere"].Trim());
                                    break;
                                case "Qy":
                                    returnJson = UpdateUpJson(context.Request.QueryString["status"].Trim(), context.Request.QueryString["strWhere"].Trim());
                                    break;
                                case "delUser":
                                    returnJson = DelAdminUserJson(context.Request.QueryString["strWhere"].Trim());
                                    break;
                                case "delMethod":
                                    returnJson = UpdateDelMethodJson(context.Request.QueryString["status"].Trim(), context.Request.QueryString["strWhere"].Trim());
                                    break;
                                case "delRole":
                                    returnJson = UpdateDelRoleJson(context.Request.QueryString["status"].Trim(), context.Request.QueryString["strWhere"].Trim());
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

        protected string UpdateUpJson(string status, string strWhere)
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR("Proc_Admin_MembersQuery", new SqlParameter[] {
                new SqlParameter("@type", "PC_UpAdminUser"),
                 new SqlParameter("@strWhere", strWhere),
                new SqlParameter("@status", status),
            });

            string r_json;
            if (ds.Rows.Count > 0 && ds.Rows[0]["flag"].ToString() == "1")
            {
                r_json = JsonMethod.GetError(0, "禁用成功");
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }

            return r_json;
        }

        protected string UpdateDelRoleJson(string status, string strWhere)
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR("Proc_Admin_MembersQuery", new SqlParameter[] {
                new SqlParameter("@type", "PC_DelRole"),
                 new SqlParameter("@strWhere", strWhere)
            });

            string r_json;
            if (ds.Rows.Count > 0 && ds.Rows[0]["flag"].ToString() == "1")
            {
                r_json = JsonMethod.GetError(0, "删除成功");
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }

            return r_json;
        }

        protected string DelAdminUserJson(string strWhere)
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR("Proc_Admin_MembersQuery", new SqlParameter[] {
                new SqlParameter("@type", "PC_DelUser"),
                 new SqlParameter("@strWhere", strWhere)
            });

            string r_json;
            if (ds.Rows.Count > 0 && ds.Rows[0]["flag"].ToString() == "1")
            {
                r_json = JsonMethod.GetError(0, "删除成功");
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }

            return r_json;
        }

        protected string UpdateDelMethodJson(string status, string strWhere)
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR("Proc_Admin_MembersQuery", new SqlParameter[] {
                new SqlParameter("@type", "PC_DelMethodUrl"),
                 new SqlParameter("@strWhere", strWhere)
            });

            string r_json;
            if (ds.Rows.Count > 0 && ds.Rows[0]["flag"].ToString() == "1")
            {
                r_json = JsonMethod.GetError(0, "删除成功");
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }

            return r_json;
        }


        /// <summary>
        /// 检测用户的角色权限
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entId"></param>
        /// <returns></returns>
        protected string GetRoleJson(string json, string proc,string userId, string entId)
        {
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entId)).ToArray();//动态解析json参数
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR(proc, param);
            string r_json;
            if (ds.Rows.Count > 0)
            {
                int recordCount = 0;
                int pageCount = 0;
                r_json = JsonMethod.GetDataTable(0, recordCount, pageCount, ds);
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }
            return r_json;
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
        /// <summary>
        /// 网站设置修改
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="UserId"></param>
        /// <param name="entId"></param>
        /// <returns></returns>
        protected string SaveBase(string json, string proc, string UserId, string entId)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            ///修改webconfig节点
            BasisConfig.UpdateAppSetting("SercerIp", obj["ip"].ToString());
            StringBuilder strSql = new StringBuilder();
            //修改商品主表
            strSql.Append("select entid from dt_system_base(nolock) where entid='"+entId+"'");
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RtDataTable(strSql.ToString());
            //清空StringBuilder
            strSql.Length = 0;
            if (dt.Rows.Count > 0)
            {
                strSql.Append("update dt_system_base set web_name=@web_name,web_ip=@web_ip,company=@company,complaints=@complaints,xxjyz=@xxjyz,xxfwz=@xxfwz,icp=@icp,beizhu=@beizhu,img_app=@img_app,img_logo=@img_logo,img_left=@img_left,img_right=@img_right,img_service=@img_service,link_service=@link_service where entid=@entid");
            }
            else
            {
                strSql.Append("insert into dt_system_base(entId,web_name,web_ip,company,complaints,xxjyz,xxfwz,icp,beizhu,img_app,img_logo,img_left,img_right,img_service,link_service) ");
                strSql.Append(" values(@entId,@web_name,@web_ip,@company,@complaints,@xxjyz,@xxfwz,@icp,@beizhu,@img_app,@img_logo,@img_left,@img_right,@img_service,@link_service);");
            }
            SqlParameter[] prmt = new SqlParameter[] { 
                new SqlParameter("@entid",entId),
                new SqlParameter("@web_name",obj["title"].ToString()),
                new SqlParameter("@web_ip",obj["ip"].ToString()),
                new SqlParameter("@company",obj["company"].ToString()),
                new SqlParameter("@complaints",obj["complaints"].ToString()),
                new SqlParameter("@xxjyz",obj["xxjyz"].ToString()),
                new SqlParameter("@xxfwz",obj["xxfwz"].ToString()),
                new SqlParameter("@icp",obj["ICP"].ToString()),
                new SqlParameter("@beizhu",obj["beizhu"].ToString()),
                new SqlParameter("@img_app",obj["app_url"].ToString()),
                new SqlParameter("@img_logo",obj["logo_url"].ToString()),
                new SqlParameter("@img_left",obj["left_url"].ToString()),
                new SqlParameter("@img_right",obj["right_url"].ToString()),
                new SqlParameter("@img_service",obj["kf_url"].ToString()),
                new SqlParameter("@link_service",obj["kf_link"].ToString())
            };

            int n = sql.ExecuteSql(strSql.ToString(), prmt);
            if (n > 0)
            {
                return JsonMethod.GetError(0, "提交成功");
            }
            else
            {
                return JsonMethod.GetError(1, "提交失败");
            }
        }
        /// <summary>
        /// 查询网站配置信息
        /// </summary>
        [ValidateInput(false)]
        protected string GetWebSiteBase(string json, string proc, string UserId, string entId)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            StringBuilder strSql = new StringBuilder();
            //修改商品主表
            strSql.Append("select entId,web_name,web_ip,company,complaints,xxjyz,xxfwz,icp,beizhu,img_app,img_logo,img_left,img_right,img_service,link_service from dt_system_base(nolock) where entid='" + entId + "'");
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable dt = sql.RtDataTable(strSql.ToString());
            if (dt.Rows.Count > 0)
            {
                return JsonMethod.GetDataTable(0, 1, 1, dt);
            }
            else
            {
                return JsonMethod.GetError(1, "无数据");
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