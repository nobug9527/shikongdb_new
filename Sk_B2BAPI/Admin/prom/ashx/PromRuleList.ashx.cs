using Newtonsoft.Json;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models.Admin;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using SqlSugar;

namespace Sk_B2BAPI.Admin.prom.ashx
{
    /// <summary>
    /// PromRuleList 的摘要说明
    /// </summary>
    public class PromRuleList : IHttpHandler, IRequiresSessionState
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
                    string json = context.Request.QueryString["json"].Trim();//请求参数(json类型)
                    var obj = JsonConvert.DeserializeObject<PromRuleListModel>(json);
                    var type = obj.type;
                    if (type != null)
                    {
                        List<string> rolestr = context.Session["role"] != null ? (List<string>)context.Session["role"] : null;
                        RoleFuns.SetAdminLog(user.username, "促销管理", rolestr);
                        var updateStatus = 0;
                        switch (type)
                        {
                            case "QueryList":
                                {
                                    var db = new SqlSugarDb();
                                    var total = 0;

                                    var strWhere = obj.strWhere;
                                    var status = obj.status;
                                    var faType = obj.faType;
                                    System.Linq.Expressions.Expression<Func<SC_T_PromRule, dt_PromType, bool>> lab;
                                    if (string.IsNullOrEmpty(status))
                                        lab = (d, e) => (d.ruleTitle.Contains(strWhere) || d.describe.Contains(strWhere)) && d.faType.Contains(faType);
                                    else
                                        lab = (d, e) => (d.ruleTitle.Contains(strWhere) || d.describe.Contains(strWhere)) && d.faType.Contains(faType) && SqlFunc.IsNull(d.isShow, "1") == status;
                                    var data = db.Db.Queryable<SC_T_PromRule, dt_PromType>((d, e) => new object[] { JoinType.Left, d.faType == e.faType })
                                        .Where(lab)
                                        .Select((d, e) => new { d.entid, d.ruleCode, d.ruleTitle, d.isShow, e.faName, d.describe, d.faType })
                                        .ToDataTablePage(obj.PageIndex, obj.PageSize, ref total);
                                    var totalPage = total % obj.PageSize == 0 ? total / obj.PageSize : total / obj.PageSize + 1;
                                    returnJson = JsonMethod.GetDataTable(0, total, totalPage, data);
                                }
                                break;
                            case "QueryPromList":
                                {
                                    var db = new SqlSugarDb();
                                    var total = 0;
                                    var data = db.Db.Queryable<SC_T_PromRule, dt_PromType>((d, e) => new object[] { JoinType.Left, d.faType == e.faType })
                                                .Where((d, e) => (d.ruleTitle.Contains(obj.strWhere) || d.describe.Contains(obj.strWhere)) && d.faType == obj.faType && d.isShow == "2")
                                                .Select((d, e) => new { d.entid, d.ruleCode, d.ruleTitle, e.faName, d.describe, d.faType })
                                                .ToDataTablePage(obj.PageIndex, obj.PageSize, ref total);
                                    var totalPage = total % obj.PageSize == 0 ? total / obj.PageSize : total / obj.PageSize + 1;
                                    returnJson = JsonMethod.GetDataTable(0, total, totalPage, data);
                                    break;
                                }
                            case "UpdateRlueDown": updateStatus = 1; break;
                            case "UpdateRlueShow": updateStatus = 2; break;
                            case "DeleteRlue": updateStatus = 0; break;
                        }
                        if (type != "QueryList" && type != "QueryPromList")
                        {
                            var db = new SqlSugarDb();
                            db.Db.Updateable<SC_T_PromRule>().SetColumns(d => new SC_T_PromRule() { isShow = updateStatus.ToString() }).Where(d => d.ruleCode == obj.ruleCode && d.entid == obj.entid).ExecuteCommand();
                            returnJson = JsonMethod.GetError(0, "操作成功");
                        }


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
                returnJson = JsonMethod.GetError(4, msg); ;
            }
            context.Response.Write(returnJson);
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