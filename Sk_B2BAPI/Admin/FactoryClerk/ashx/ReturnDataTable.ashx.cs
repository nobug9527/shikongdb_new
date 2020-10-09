using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.SessionState;
using System.IO;

namespace DTcms.Web.admin.FactoryClerk.ashx
{
    /// <summary>
    /// ReturnDataTable 的摘要说明
    /// </summary>
    public class ReturnDataTable : IHttpHandler
    {
        public int return_code;
        public void ProcessRequest(HttpContext context)
        {
            string Json = "";
            try
            {
                string type = context.Request.QueryString["Type"].Trim();
                string Login_id = "";
                string xjson = context.Request.QueryString["Json"];
                if (type == "DataTable")
                {
                    Json = ReturnJson(xjson, Login_id);
                }
                else if (type == "ExecuteNonQuery")
                {
                    Json = ExecuteNonQuery(xjson, Login_id);
                }
                else
                {
                    Json = DTcms.Common.GetJson.GetErrorJson(2, 0, "参数错误");
                }
            }
            catch (Exception e)
            {
                return_code = 2;
                string error = e.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
            }
            JObject jo = (JObject)JsonConvert.DeserializeObject(Json);
            context.Response.ContentType = "text/plain";
            context.Response.Write(jo);
        }
        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="Json"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        protected string ReturnJson(string Json, string Login_id)
        {
            string Procedure = "";
            JsonReader reader = new JsonTextReader(new StringReader(Json));
            string t = "";
            string v = "";
            while (reader.Read())
            {
                if (reader.TokenType.ToString() == "PropertyName")
                {
                    t = t + "," + reader.Value.ToString();


                }
                else if (reader.TokenType.ToString() == "String")
                {
                    v = v + "," + reader.Value.ToString();
                }
            }
            string[] type = t.Split(',');
            string[] value = v.Split(',');
            List<SqlParameter> ilistStr = new List<SqlParameter>();
            for (int i = 1; i < type.Length; i++)
            {
                if (type[i] != "" && type[i] != "Procedure")
                {
                    ilistStr.Add(new SqlParameter("@" + type[i], value[i]));
                }
                else if (type[i] == "Procedure")
                {
                    Procedure = value[i].ToString();
                }
            }
            ilistStr.Add(new SqlParameter("@Login_id", Login_id));
            SqlParameter[] param = ilistStr.ToArray();
            DataSet ds = DbHelperSQL.RunProcedure(Procedure, param, "Table");    //db.RunProDataSet(sql, param);
            if (ds.Tables.Count >= 3)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    return_code = 0;
                    int recordCount = Convert.ToInt32(ds.Tables[2].Rows[0]["recordCount"].ToString().Trim());
                    int pageCount = Convert.ToInt32(ds.Tables[2].Rows[0]["pageCount"].ToString().Trim());
                    Json = DTcms.Common.GetJson.GetDataJson(return_code, recordCount, pageCount, ds.Tables[1]);
                }
                else
                {
                    return_code = 1;
                    string error = "无数据";
                    Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return_code = 0;
                    int recordCount = 0;
                    int pageCount = 0;
                    Json = DTcms.Common.GetJson.GetDataJson(return_code, recordCount, pageCount, ds.Tables[0]);
                }
                else
                {
                    return_code = 1;
                    string error = "无数据";
                    Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
                }
            }
            return Json;
        }
        /// <summary>
        /// 返回影响行数
        /// </summary>
        protected string ExecuteNonQuery(string Json, string Login_id)
        {
            string Procedure = "";
            JsonReader reader = new JsonTextReader(new StringReader(Json));
            string t = "";
            string v = "";
            while (reader.Read())
            {
                if (reader.TokenType.ToString() == "PropertyName")
                {
                    t = t + "," + reader.Value.ToString();


                }
                else if (reader.TokenType.ToString() == "String")
                {
                    v = v + "," + reader.Value.ToString();
                }
            }
            string[] type = t.Split(',');
            string[] value = v.Split(',');
            List<SqlParameter> ilistStr = new List<SqlParameter>();
            for (int i = 1; i < type.Length; i++)
            {
                if (type[i] != "" && type[i] != "Procedure")
                {
                    ilistStr.Add(new SqlParameter("@" + type[i], value[i]));
                }
                else if (type[i] == "Procedure")
                {
                    Procedure = value[i].ToString();
                }
            }
            ilistStr.Add(new SqlParameter("@Login_id", Login_id));
            SqlParameter[] param = ilistStr.ToArray();
            int rowsAffected=0;
            int n = DbHelperSQL.RunProcedure(Procedure, param, out rowsAffected);    //db.RunProDataSet(sql, param);
            if (rowsAffected > 0)
            {
                Json = DTcms.Common.GetJson.GetErrorJson(0, 0, "操作成功！");
            }
            else
            {
                Json = DTcms.Common.GetJson.GetErrorJson(1, 0, "操作失败！");
            }

            return Json;
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