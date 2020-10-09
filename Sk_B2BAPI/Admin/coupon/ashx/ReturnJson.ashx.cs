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
using System.Web.Mvc;
using System.Web.SessionState;

namespace Sk_B2BAPI.Admin.coupon.ashx
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
                        //else {
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
            SqlParameter[] param = (JsonMethod.ListParameter(json, userId, entid)).ToArray();//动态解析json参数
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
        /// 返回DataTable(渠道分类)
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
            if (ds.Rows.Count > 0)
            {
                ///加载节点
                string objList = "";
                DataTable dtc = ds;
                if (dtc.Rows.Count > 0)
                {
                    switch (typename)
                    {
                        case "Pc_GetClassType"://设备类型//应用范围
                            foreach (DataRow dr_c1 in dtc.Rows)
                            {
                                objList += "<option value='" + dr_c1["Code"] + "'>" + dr_c1["Name"] + "</option>";
                            }
                            break;
                        case "Pc_GetCouponType"://领取类型
                            foreach (DataRow dr_c1 in dtc.Rows)
                            {
                                objList += "<option value='" + dr_c1["typeCoding"] + "'>" + dr_c1["typeName"] + "</option>";
                            }
                            break;
                        case "Pc_GetBrandType"://品牌类型
                            var num = 0;
                            foreach (DataRow dr_c1 in dtc.Rows)
                            {
                                //;
                                objList += "<input type='checkbox' id='chkPpBox" + num + "' value='" + dr_c1["billno"] + "' title='" + dr_c1["name"] + "' />&nbsp;<label for='chkPpBox" + num + "''>" + dr_c1["name"] + "</label>&nbsp;&nbsp;&nbsp;&nbsp";
                                num++;
                            }
                            break;
                        case "Pc_GoodsGroup"://商品分组
                            var n = 0;
                            foreach (DataRow dr_c1 in dtc.Rows)
                            {
                                //;
                                objList += "<input type='checkbox' id='chkPpBox" + n + "' value='" + dr_c1["billno"] + "' title='" + dr_c1["name"] + "' />&nbsp;<label for='chkPpBox" + n + "''>" + dr_c1["name"] + "</label>&nbsp;&nbsp;&nbsp;&nbsp";
                                n++;
                            }
                            break;
                        case "Pc_GetHelperType":
                        case "Pc_GetCateGoryType"://分类类型
                            foreach (DataRow dr_c1 in dtc.Rows)
                            {
                                objList += "<option value='" + dr_c1["id"] + "'>" + dr_c1["title"] + "</option>";
                            }
                            break;
                        case "Pc_GetShengType"://省
                            foreach (DataRow dr_c1 in dtc.Rows)
                            {
                                objList += "<option value='" + dr_c1["id"] + "'>" + dr_c1["name"] + "</option>";
                            }
                            break;
                        case "Pc_GetShiType"://市
                            foreach (DataRow dr_c1 in dtc.Rows)
                            {
                                objList += "<option value='" + dr_c1["id"] + "'>" + dr_c1["name"] + "</option>";
                            }
                            break;
                        case "Pc_GetQuType"://区
                            foreach (DataRow dr_c1 in dtc.Rows)
                            {
                                objList += "<option value='" + dr_c1["id"] + "'>" + dr_c1["name"] + "</option>";
                            }
                            break;
                        default:
                            break;
                    }

                }
                return "{\"flag\":\"0\",\"objList\":\"" + objList + "\"}";
            }
            else
            {
                string error = "无数据";
                return JsonMethod.GetError(1, error);
            }
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
        /// 返回DataTable(优惠券详情)
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string GetDetailReturnJson(string entId, string id)
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR("Pc_Coupon", new SqlParameter[] {
                new SqlParameter("@type", "PC_GetCouponDetail"),
                new SqlParameter("@couponCode",id),
                new SqlParameter("@entId",entId)
            });
            string r_json;
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
        /// <summary>
        /// 返回DataTable(规则详情)
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string GeTypeDetailReturnJson(string userId, string entId, string id)
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR("Pc_Coupon", new SqlParameter[] {
                new SqlParameter("@type", "PC_GetTypeDetail"),
                new SqlParameter("@strWhere",id),
                new SqlParameter("@entId",entId)
            });
            string r_json;
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
        /// <summary>
        /// 删除设备类型
        /// </summary>
        /// <param name="json"></param>
        /// <param name="proc"></param>
        /// <param name="userId"></param>
        /// <param name="entid"></param>
        /// <returns></returns>
        protected string GetDelTypeReturnJson(string userId, string entId, string id)
        {
            SqlRun sql = new SqlRun(SqlRun.sqlstr);
            DataTable ds = sql.RunProcedureDR("Pc_Coupon", new SqlParameter[] {
                new SqlParameter("@type", "PC_DelType"),
                new SqlParameter("@strWhere",id),
                new SqlParameter("@entId",entId)
            });
            string r_json;
            if (ds.Rows.Count > 0 && ds.Rows[0]["flag"].ToString() == "1")
            {
                r_json = JsonMethod.GetError(0, "删除成功");
            }
            else if (ds.Rows.Count > 0 && ds.Rows[0]["flag"].ToString() == "2")
            {
                string error = "无法删除，请清除所属优惠券";
                r_json = JsonMethod.GetError(1, error);
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