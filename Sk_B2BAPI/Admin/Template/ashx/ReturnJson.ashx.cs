
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models.Admin;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Sk_B2BAPI.Admin.Template.ashx
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
                    string strWhere;
                    int pageIndex;
                    string status;
                    int txtType;
                    switch (type)
                    {
                        case "TemplateLIst":
                            strWhere = context.Request.Params["strWhere"];
                            pageIndex = int.Parse(context.Request.Params["pageIndex"]);
                            status = context.Request.Params["Status"];
                            returnJson = GetTemplateReturnJson(strWhere, status, pageIndex);
                            break;
                        case "TemplateRelationLIst":
                            strWhere = context.Request.Params["strWhere"];
                            pageIndex = int.Parse(context.Request.Params["pageIndex"]);
                            status = context.Request.Params["Status"];
                            txtType = int.Parse(context.Request.Params["txtType"]);
                            returnJson = GetTemplateRelationReturnJson(strWhere, status, txtType, pageIndex);
                            break;
                        case "SaveTemplate":
                            string title = context.Request.Params["title"];
                            string content = context.Request.Params["content"];
                            strWhere = context.Request.Params["strWhere"];
                            returnJson = SaveTemplate(title, content, strWhere);
                            break;
                        case "SaveTemplateRelation":
                            var sltTemplateRelation = context.Request.Params["sltTemplateRelation"];
                            txtType = int.Parse(context.Request.Params["txtType"]);
                            int txtSortId = int.Parse(context.Request.Params["txtSortId"]);
                            var sltcategory = context.Request.Params["sltcategory"];
                            var sltprom = context.Request.Params["sltprom"];
                            var templateimg = context.Request.Params["txtImgUrl"];
                            strWhere = context.Request.Params["strWhere"];
                            returnJson = SaveTemplateRelation(sltprom, sltTemplateRelation, sltcategory, txtType, templateimg, txtSortId, strWhere);
                            break;
                        case "AuditTemplate":
                            strWhere = context.Request.Params["id"];
                            status = context.Request.Params["status"];
                            returnJson = SetTemplateStatu(strWhere, status);
                            break;
                        case "AuditTemplateRelation":
                            strWhere = context.Request.Params["id"];
                            status = context.Request.Params["status"];
                            returnJson = SetTemplateRelationStatu(strWhere, status);
                            break;
                        case "TemDel":
                            strWhere = context.Request.Params["strWhere"];
                            returnJson = SetTemplateIsDel(strWhere);
                            break;
                        case "TemRelationDel":
                            strWhere = context.Request.Params["strWhere"];
                            returnJson = SetTemplateRelationIsDel(strWhere);
                            break;
                        case "TemplateDetail":
                            strWhere = context.Request.Params["strWhere"];
                            returnJson = GetTemplateDetail(strWhere);
                            break;
                        case "TemplateRelationDetail":
                            strWhere = context.Request.Params["strWhere"];
                            returnJson = GetTemplateRelationDetail(strWhere);
                            break;
                        case "selectTemplate":
                            returnJson = GetSelectTemplate();
                            break;
                        case "selectCategory":
                            returnJson = GetSelectCategory();
                            break;
                        case "selectTemPro":
                            returnJson = GetSelectTemPro();
                            break;
                        default:
                            returnJson = JsonMethod.GetError(4, "参数错误！");
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
                string msg = e.Message.ToString().Trim().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                returnJson = JsonMethod.GetError(3, msg);
            }
            context.Response.Write(returnJson);
        }
        /// <summary>
        /// 返回活动模板
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="isDel"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected string GetTemplateReturnJson(string templateName, string status, int page = 1, int size = 30)
        {
            string r_json = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Pc_Template", new SqlParameter[] {
                new SqlParameter("@type", "PC_GetTemplateList"),
                new SqlParameter("@TemplateName",templateName),
                new SqlParameter("@Status",status),
                new SqlParameter("@PageIndex", page),
                new SqlParameter("@PageSize",size)
            });
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
        /// 返回DataTable(活动模板)
        /// </summary>
        /// <returns></returns>
        protected string GetSelectTemplate()
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR("Pc_Template", new SqlParameter[] {
                new SqlParameter("@type", "PC_GetSelectTemplate")
            });

            if (ds.Rows.Count > 0)
            {
                ///加载商品分类
                string templatestr = "";
                DataTable dtc = ds;
                if (dtc.Rows.Count > 0)
                {
                    foreach (DataRow dr_c1 in dtc.Rows)
                    {
                        templatestr += "<option value='" + dr_c1["TemplateCode"] + "'>" + dr_c1["TemplateName"] + "</option>";
                    }
                }
                return "{\"flag\":\"0\",\"templatestr\":\"" + templatestr + "\"}";
            }
            else
            {
                string error = "没有活动模板，请添加->上架";
                return JsonMethod.GetError(1, error);
            }
        }

        /// <summary>
        /// 返回DataTable(商品分类)
        /// </summary>
        /// <returns></returns>
        protected string GetSelectCategory()
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR("Pc_Template", new SqlParameter[] {
                new SqlParameter("@type", "PC_GetSelectCategory")
            });

            if (ds.Rows.Count > 0)
            {
                ///加载商品分类
                string category = "";
                DataTable dtc = ds;
                if (dtc.Rows.Count > 0)
                {
                    foreach (DataRow dr_c1 in dtc.Rows)
                    {
                        category += "<option value='" + dr_c1["id"] + "'>" + dr_c1["title"] + "</option>";
                    }
                }
                return "{\"flag\":\"0\",\"category\":\"" + category + "\"}";
            }
            else
            {
                string error = "没有商品分类，请添加";
                return JsonMethod.GetError(1, error);
            }
        }
        /// <summary>
        /// 返回DataTable(商品组合)
        /// </summary>
        /// <returns></returns>
        protected string GetSelectTemPro()
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR("Pc_Template", new SqlParameter[] {
                new SqlParameter("@type", "PC_GetSelectTemPro")
            });

            if (ds.Rows.Count > 0)
            {
                ///加载商品组合
                string prom = "";
                DataTable dtc = ds;
                if (dtc.Rows.Count > 0)
                {
                    foreach (DataRow dr_c1 in dtc.Rows)
                    {
                        prom += "<option value='" + dr_c1["Id"] + "'>" + dr_c1["name"] + "</option>";
                    }
                }
                return "{\"flag\":\"0\",\"prom\":\"" + prom + "\"}";
            }
            else
            {
                string error = "没有主题商品组合，请添加";
                return JsonMethod.GetError(1, error);
            }
        }
        /// <summary>
        /// 返回模板绑定
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected string GetTemplateRelationReturnJson(string templateName, string status,int type, int page = 1, int size = 30)
        {
            string r_json = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataSet ds = sql.RunProDataSet("Pc_TemplateRelation", new SqlParameter[] {
                new SqlParameter("@type", "PC_GetTemplateRelationList"),
                new SqlParameter("@TemplateRelationName",templateName),
                new SqlParameter("@TypeCode",type),
                new SqlParameter("@Status",status),
                new SqlParameter("@PageIndex", page),
                new SqlParameter("@PageSize",size)
            });
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
        /// 返回活动模板
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="isDel"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected string GetTemplateDetail(string TemplateCode)
        {
            string r_json = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR("Pc_Template", new SqlParameter[] {
                new SqlParameter("@type", "PC_GetTemplateDetail"),
                new SqlParameter("@TemplateCode",TemplateCode)
            });
            if (ds.Rows.Count>0)
            {
                r_json = ds.Rows[0]["txt"].ToString(); 
            }
            else
            {
                r_json = "";
            }
            return r_json;
        }
        /// <summary>
        /// 返回模板绑定
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="isDel"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected string GetTemplateRelationDetail(string TemplateCode)
        {
            string r_json = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR("Pc_TemplateRelation", new SqlParameter[] {
                new SqlParameter("@type", "PC_GetTemplateRelationDetail"),
                new SqlParameter("@TemplateRelationCode",TemplateCode)
            });
            if (ds.Rows.Count > 0)
            {
                r_json = JsonMethod.GetDataTable(0, ds.Rows.Count, 1, ds);
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }
            return r_json;
        }

        protected string SetTemplateStatu(string templateCode, string status)
        {
            string r_json = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int num = sql.ExecuteNonQuery("Pc_Template", new SqlParameter[] {
                new SqlParameter("@type", "PC_UpTemplateStatus"),
                new SqlParameter("@TemplateCode",templateCode),
                new SqlParameter("@Status",status)
            });
            if (num > 0)
            {
                r_json = JsonMethod.GetError(0,"成功");
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }
            return r_json;
        }
        /// <summary>
        /// 活动绑定上架
        /// </summary>
        /// <param name="templateCode"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        protected string SetTemplateRelationStatu(string templateRelationCode, string status)
        {
            string r_json = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int num = sql.ExecuteNonQuery("Pc_TemplateRelation", new SqlParameter[] {
                new SqlParameter("@type", "PC_UpTemplateRelationStatus"),
                new SqlParameter("@TemplateRelationCode",templateRelationCode),
                new SqlParameter("@Status",status)
            });
            if (num > 0)
            {
                r_json = JsonMethod.GetError(0, "成功");
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }
            return r_json;
        }
        protected string SetTemplateIsDel(string templateCode)
        {
            string r_json = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int num = sql.ExecuteNonQuery("Pc_Template", new SqlParameter[] {
                new SqlParameter("@type", "PC_UpTemplateIsDel"),
                new SqlParameter("@StrWhere",templateCode)
            });
            if (num > 0)
            {
                r_json = JsonMethod.GetError(0, "成功");
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }
            return r_json;
        }
        protected string SetTemplateRelationIsDel(string templateRelationCode)
        {
            string r_json = "";
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int num = sql.ExecuteNonQuery("Pc_TemplateRelation", new SqlParameter[] {
                new SqlParameter("@type", "PC_UpTemplateRelationIsDel"),
                new SqlParameter("@StrWhere",templateRelationCode)
            });
            if (num > 0)
            {
                r_json = JsonMethod.GetError(0, "成功");
            }
            else
            {
                string error = "无数据";
                r_json = JsonMethod.GetError(1, error);
            }
            return r_json;
        }
        /// <summary>
        /// 保存活动模板数据
        /// </summary>
        /// <param name="title"></param>
        /// <param name="newsType"></param>
        /// <param name="sortId"></param>
        /// <param name="dates"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        protected string SaveTemplate(string title,string content,string strWhere)
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int num = sql.ExecuteNonQuery("Pc_Template", new SqlParameter[] {
                new SqlParameter("@type", "PC_AddTemplate"),
                new SqlParameter("@TemplateName",title),
                new SqlParameter("@TemplateText",content),
               new SqlParameter("@TemplateCode",strWhere)
            });
            if (num>0)
            {
                return JsonMethod.GetError(0, "操作成功");
            }
            else
            {
                return JsonMethod.GetError(1, "操作失败");
            }
        }

        /// <summary>
        /// 保存模板绑定
        /// </summary>
        /// <param name="title"></param>
        /// <param name="newsType"></param>
        /// <param name="sortId"></param>
        /// <param name="dates"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected string SaveTemplateRelation(string promotioncode, string templatecode, string categorycode,int txtType,string templateimg,int sort_id,string id)
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            int num = sql.ExecuteNonQuery("Pc_TemplateRelation", new SqlParameter[] {
                new SqlParameter("@type", "PC_AddTemplateRelation"),
                new SqlParameter("@PromotionCode",promotioncode),
                new SqlParameter("@TemplateCode",templatecode),
               new SqlParameter("@CategoryCode",categorycode),
               new SqlParameter("@TypeCode",txtType),
               new SqlParameter("@TemplateImg",templateimg),
               new SqlParameter("@Sort_id",sort_id),
               new SqlParameter("@strWhere",id),
            });
            if (num > 0)
            {
                return JsonMethod.GetError(0, "操作成功");
            }
            else
            {
                return JsonMethod.GetError(1, "操作失败");
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