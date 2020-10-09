using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models.Admin;
using Sk_B2BAPI.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using SqlSugar;
namespace Sk_B2BAPI.Admin.prom.ashx
{
    /// <summary>
    /// PromSales 的摘要说明
    /// </summary>
    public class PromSales : IHttpHandler, IRequiresSessionState
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
                    //string type = context.Request.QueryString["type"].Trim();//请求类型
                    string json = context.Request.QueryString["json"].Trim();//请求参数(json类型)
                    //string proc = context.Request.QueryString["proc"].Trim();//存储过程名称
                    var obj = JsonConvert.DeserializeObject<PromData>(json);
                    var type = obj.type;
                    if (type != null)
                    {
                        List<string> rolestr = context.Session["role"] != null ? (List<string>)context.Session["role"] : null;
                        RoleFuns.SetAdminLog(user.username, "促销管理", rolestr);
                        //if (/*rolestr == null || RoleFuns.SetAdminLog(user.username, obj["type"].ToString(), rolestr) == 0*/false)
                        //{
                        //    returnJson = JsonMethod.GetError(4, "抱歉您没有权限进入！");
                        //}
                        //else {
                        //执行查询返回列表
                        if (type == "SaveProm")
                        {
                            var db = new SqlSugarDb();
                            var fabh = "";
                            if (string.IsNullOrWhiteSpace(obj.salesCode))
                            {
                                db.Db.Ado.UseTran(() =>
                                {
                                    var maxbh = db.Db.Queryable<dt_Maxbh>().First(d => d.BiaoShi == "FAH")?.maxbh ?? "0";
                                    var nowBh = (int.Parse(maxbh) + 1).ToString();
                                    db.Db.Updateable<dt_Maxbh>().SetColumns(d => new dt_Maxbh { maxbh = nowBh }).Where(d => d.BiaoShi == "FAH").ExecuteCommand();
                                    fabh = "FAH" + nowBh.PadLeft(11, '0');
                                });
                            }
                            else
                            {
                                fabh = obj.salesCode;
                            }
                            var prom = new SC_T_PromSales();
                            prom.salesCode = fabh;
                            prom.entid = user.entId;
                            prom.faType = obj.faType;
                            prom.salesName = obj.faname;
                            prom.ruleCode = obj.ruleCode;
                            prom.startDate = obj.startDates;
                            prom.endDate = obj.endDates;
                            if (string.IsNullOrWhiteSpace(obj.salesCode))
                                db.Db.Insertable(prom).ExecuteCommand();
                            else
                                db.Db.Updateable(prom).Where(d => d.entid == user.entId && d.salesCode == obj.salesCode).ExecuteCommand();
                            returnJson = JsonMethod.GetError(0, "操作成功");
                        }

                        if (type == "QuerySingleProm")
                        {
                            var db = new SqlSugarDb();
                            var model = db.Db.Queryable<SC_T_PromSales>().First(d => d.entid == obj.entid && d.salesCode == obj.salesCode);
                            returnJson = JsonConvert.SerializeObject(new PromSalesUpdate() { promSales = model, flag = 0, message = "成功" });
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