using DTcms.DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models.Admin;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.SessionState;

namespace DTcms.Web.admin.SignLog.ashx
{
    /// <summary>
    /// ReturnData 的摘要说明
    /// </summary>
    public class ReturnData : IHttpHandler,IRequiresSessionState
    {
        public int return_code;
        public void ProcessRequest(HttpContext context)
        {
            string Json = "";
            
            try
            {
                Dt_User user = RoleFuns.IsLoginAdmin(context.Session["user"]);
                if (user != null)//登录检测，权限检测 context.Session["user"]
                {
                    string type = context.Request.QueryString["Type"].Trim();
                    if (type == "Search")
                    {
                        Json = Search(context);
                    }
                    else if (type == "SaveSign")
                    {
                        Json = SaveSign(context);
                    }
                    else if (type == "delSign")
                    {
                        Json = delSign(context);
                    }
                    else if (type == "CreateSign")
                    {
                        Json = CreateSign(context,user.userId,user.entId);
                    }
                    else if (type == "GetSign")
                    {
                        Json = GetSign(context,user.userId, user.entId);
                    }
                    else if (type == "UpdateData")
                    {
                        Json = UpdateData(context);
                    }
                }
                else
                {
                    Json = JsonMethod.GetError(2, "登陆超时,请重新登陆！");
                }
            }
            catch (Exception e)
            {
                return_code = 2;
                string error = e.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                Json = DTcms.Common.GetJson.GetErrorJson(return_code, 0, error);
            }
            JObject jo = (JObject)JsonConvert.DeserializeObject(Json);
            context.Response.Write(jo);
        }

        private string UpdateData(HttpContext context)
        {
            string Json = "";
            int rowsAffected = 0;
            string sqltype = context.Request.QueryString["sqltype"].Trim();
            string Procedure = context.Request.QueryString["Procedure"].Trim();
            string FaBh = context.Request.QueryString["FaBh"].Trim();
            string T = context.Request.QueryString["T"].Trim();

            SqlParameter[] sqlpmt = new SqlParameter[] {
                new SqlParameter("@TYPE",sqltype),
                new SqlParameter("@RewardFa",FaBh),
                new SqlParameter("@Bz",T)

            };
            int n = DbHelperSQL.RunProcedure(Procedure, sqlpmt, out rowsAffected);
            if (rowsAffected > 0)
            {
                Json = DTcms.Common.GetJson.GetErrorJson(0, 0, "成功");
            }
            else
            {
                Json = DTcms.Common.GetJson.GetErrorJson(1, 0, "失败");
            }
            return Json;
        }

        private string GetSign(HttpContext context,string userid, string entid)
        {
            string Json = "";
            string sqltype = context.Request.QueryString["sqltype"].Trim();
            string Procedure = context.Request.QueryString["Procedure"].Trim();
            string fbStatus = context.Request.QueryString["fbStatus"].Trim();
            string SignModel = context.Request.QueryString["SignModel"].Trim();
            string keyWord = context.Request.QueryString["keyWord"].Trim();
            int pageSize = Convert.ToInt32(context.Request.QueryString["PageSize"].ToString().Trim());
            int PageIndex = Convert.ToInt32(context.Request.QueryString["PageIndex"].ToString().Trim());
            SqlParameter[] sqlpmt = new SqlParameter[] {
                new SqlParameter("@TYPE",sqltype),
                new SqlParameter("@Status",fbStatus),
                new SqlParameter("@Bz",SignModel),
                new SqlParameter("@RewardFa",keyWord),
                new SqlParameter("@Entid",entid),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@PageIndex",PageIndex)

            };
            DataSet ds = DbHelperSQL.RunProcedure(Procedure, sqlpmt, "Table");
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

        private string CreateSign(HttpContext context,string userid,string entid)
        {
            string Json = "";
            int rowsAffected = 0;
            string sqltype = context.Request.QueryString["sqltype"].Trim();
            string Procedure = context.Request.QueryString["Procedure"].Trim();
            string FATypeID = context.Request.QueryString["FATypeID"].Trim();
            string TypeId = context.Request.QueryString["TypeId"].Trim();
            string Status = context.Request.QueryString["Status"].Trim();
            string GzXs = context.Request.QueryString["GzXs"].Trim();
            SqlParameter[] sqlpmt = new SqlParameter[] {
                new SqlParameter("@TYPE",sqltype),
                new SqlParameter("@KuHu",FATypeID),
                new SqlParameter("@Bz",TypeId),
                new SqlParameter("@Status",Status),
                new SqlParameter("@GzXs",GzXs),
                new SqlParameter("@Entid",entid)
            };
            int n = DbHelperSQL.RunProcedure(Procedure, sqlpmt, out rowsAffected);
            if (rowsAffected > 0)
            {
                Json = DTcms.Common.GetJson.GetErrorJson(0, 0, "成功");
            }
            else
            {
                Json = DTcms.Common.GetJson.GetErrorJson(1, 0, "失败");
            }
            return Json;
        }

        private string delSign(HttpContext context)
        {
            string Json = "";
            int rowsAffected = 0;
            string sqltype = context.Request.QueryString["sqltype"].Trim();
            string Procedure = context.Request.QueryString["Procedure"].Trim();
            string Id = context.Request.QueryString["Id"].Trim();

            SqlParameter[] sqlpmt = new SqlParameter[] {
                new SqlParameter("@TYPE",sqltype),
                new SqlParameter("@Id",Id)

            };
            int n = DbHelperSQL.RunProcedure(Procedure, sqlpmt, out rowsAffected);
            if (rowsAffected > 0)
            {
                Json = DTcms.Common.GetJson.GetErrorJson(0, 0, "成功");
            }
            else
            {
                Json = DTcms.Common.GetJson.GetErrorJson(1, 0, "失败");
            }
            return Json;
        }

        private string SaveSign(HttpContext context)
        {
            string Json = "";
            int rowsAffected = 0;
            string sqltype = context.Request.QueryString["sqltype"].Trim();
            string Procedure = context.Request.QueryString["Procedure"].Trim();
            string jlXs = context.Request.QueryString["jlXs"].Trim();
            string qdGz = context.Request.QueryString["qdGz"].Trim();
            string jlGz = context.Request.QueryString["jlGz"].Trim();


            SqlParameter[] sqlpmt = new SqlParameter[] {
                new SqlParameter("@TYPE",sqltype),
                new SqlParameter("@RewardForm",jlXs),
                new SqlParameter("@SignRule",qdGz),
                new SqlParameter("@SignReward",jlGz)

            };
            int n = DbHelperSQL.RunProcedure(Procedure, sqlpmt, out rowsAffected);
            if (rowsAffected > 0)
            {
                Json = DTcms.Common.GetJson.GetErrorJson(0, 0, "成功");
            }
            else
            {
                Json = DTcms.Common.GetJson.GetErrorJson(1, 0, "失败");
            }
            return Json;
        }

        protected string Search(HttpContext context)
        {
            string Json = "";
            string sqltype = context.Request.QueryString["sqltype"].Trim();
            string Procedure = context.Request.QueryString["Procedure"].Trim();
            SqlParameter[] sqlpmt = new SqlParameter[] {
                new SqlParameter("@TYPE",sqltype)
            };
            DataSet ds = DbHelperSQL.RunProcedure(Procedure, sqlpmt, "Table");
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}