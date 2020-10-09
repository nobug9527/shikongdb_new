using DTcms.DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DTcms.Web.admin.FactoryClerk.ashx
{
    /// <summary>
    /// returnJson 的摘要说明
    /// </summary>
    public class returnJson : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string returnJson = "";
            try
            {
                string type = context.Request.QueryString["type"].Trim();//请求类型
                string json = context.Request.QueryString["json"].Trim();//请求参数(json类型)
                string proc = context.Request.QueryString["proc"].Trim();//存储过程名称
                //执行查询返回列表
                if (type == "ReturnList")
                {
                    returnJson = GetReturnJson(json, proc);
                }
                //执行修改，插入返回影响行数
                else if (type == "ReturnNumber")
                {
                    returnJson = GetReturnJsonInt(json, proc);
                }
                else if (type == "ReturnListNumber")
                {
                    returnJson = GetReturnListNumber(json, proc);
                }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                returnJson = DTcms.Common.GetJson.GetErrorJson(2, 0, error);
            }
            JObject jo = (JObject)JsonConvert.DeserializeObject(returnJson);
            context.Response.Write(jo);
        }
        protected string GetReturnJson(string json, string proc)
        {
            string r_json = "";
            SqlParameter[] param = (ListParameter(json)).ToArray();//动态解析json参数
            DataSet ds = DbHelperSQL.RunProcedure(proc, param, "Table");
            if (ds.Tables.Count >= 3)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    int recordCount = Convert.ToInt32(ds.Tables[2].Rows[0]["recordCount"].ToString().Trim());
                    int pageCount = Convert.ToInt32(ds.Tables[2].Rows[0]["pageCount"].ToString().Trim());
                    r_json = DTcms.Common.GetJson.GetDataJson(0, recordCount, pageCount, ds.Tables[1]);
                }
                else
                {
                    string error = "无数据";
                    r_json = DTcms.Common.GetJson.GetErrorJson(1, 0, error);
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int recordCount = 0;
                    int pageCount = 0;
                    r_json = DTcms.Common.GetJson.GetDataJson(0, recordCount, pageCount, ds.Tables[0]);
                }
                else
                {
                    string error = "无数据";
                    r_json = DTcms.Common.GetJson.GetErrorJson(1, 0, error);
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
        protected string GetReturnJsonInt(string json, string proc)
        {
            string r_json = "";
            var msg = "";
            int rowsAffected;
            SqlParameter[] param = (ListParameter(json)).ToArray();//动态解析json参数
            int n = DbHelperSQL.RunProcedure(proc, param, out rowsAffected);
            if (rowsAffected > 0)
            {
                msg = "操作成功";
                r_json = DTcms.Common.GetJson.GetErrorJson(0, 0, msg);
            }
            else
            {
                msg = "操作失败！";
                r_json = DTcms.Common.GetJson.GetErrorJson(1, 0, msg);
            }
            return r_json;
        }
        protected string GetReturnListNumber(string json, string proc)
        {
            string r_json = "";
            SqlParameter[] param = (ListParameter(json)).ToArray();//动态解析json参数
            DataSet ds = DbHelperSQL.RunProcedure(proc, param, "Table");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int flag = Convert.ToInt32(ds.Tables[0].Rows[0]["flag"].ToString().Trim());
                string msg = ds.Tables[0].Rows[0]["msg"].ToString().Trim();
                r_json = DTcms.Common.GetJson.GetErrorJson(flag, 0, msg);
            }
            else
            {
                string error = "无数据";
                r_json = DTcms.Common.GetJson.GetErrorJson(1, 0, error);
            }
            return r_json;
        }
        /// <summary>
        /// 动态解析json生成List<SqlParameter>
        /// </summary>
        /// <returns></returns>
        public static List<SqlParameter> ListParameter(string json)
        {
            List<SqlParameter> listPrmt = new List<SqlParameter>();
            JObject o = JObject.Parse(json);
            IEnumerable<JProperty> properties = o.Properties();
            foreach (JProperty item in properties)
            {
                if (item.Name.Trim() != "")
                {

                    listPrmt.Add(new SqlParameter("@" + item.Name, item.Value.ToString().Trim() ?? ""));
                }
            }
            return listPrmt;
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