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
using Sk_B2BAPI.Models.Admin;
using System.Web.Mvc;
using System.Xml;
using System.Configuration;

namespace Sk_B2BAPI.Admin.order.ashx
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
                                case "ReturnDataSet":
                                    returnJson = ReturnDataSetJson(json, proc, user.userId, user.entId);
                                    break;
                                case "ReturnNumber":
                                    returnJson = GetReturnJsonInt(json, proc, user.userId, user.entId);
                                    break;
                                case "ReturnListNumber":
                                    returnJson = GetReturnDs(json, proc, user.userId, user.entId);
                                    break;
                                case "ReturnOrderNumber":
                                    returnJson = SetConfig(json);
                                    break;
                                case "GetOrderAmount":
                                    returnJson = GetConfig();
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
            string msg;
            string r_json;
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
        /// 更新配置的数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <returns></returns>
        protected string SetConfig(string json)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            string r_json;
            if (obj["bonusAmount"] != null)
            {
                UpdateAppSetting("OrderAmount", obj["bonusAmount"].ToString());
                r_json = JsonMethod.GetError(0, "修改成功");
            }
            else
            {
                string msg = "修改失败";
                r_json = JsonMethod.GetError(1, msg);
            }
            return r_json;
        }
        /// <summary>
        /// 更新配置的数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <returns></returns>
        protected string GetConfig()
        {
            StringBuilder r_json = new StringBuilder();;
            string file = ConfigurationManager.AppSettings["OrderAmount"];
            r_json.Append("{");
            r_json.Append("\"flag\":" + 0 + ",");
            r_json.Append("\"obj\":\"" + file + "\"");
            r_json.Append("}");
            return r_json.ToString();
        }
        /// <summary>  
        /// 修改config文件(AppSetting节点)  
        /// </summary>  
        /// <param name="key">键</param>  
        /// <param name="value">要修改成的值</param>  
        public static void UpdateAppSetting(string key, string value)
        {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径   
            string strFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Web.config";
            doc.Load(strFileName);
            //找出名称为“add”的所有元素   
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性   
                XmlAttribute _key = nodes[i].Attributes["key"];
                //根据元素的第一个属性来判断当前的元素是不是目标元素   
                if (_key != null)
                {
                    if (_key.Value == key)
                    {
                        //对目标元素中的第二个属性赋值   
                        _key = nodes[i].Attributes["value"];

                        _key.Value = value;
                        break;
                    }
                }
            }
            //保存上面的修改   
            doc.Save(strFileName);
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