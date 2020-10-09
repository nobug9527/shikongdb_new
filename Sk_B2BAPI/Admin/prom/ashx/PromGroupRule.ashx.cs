using Newtonsoft.Json;
using Sk_B2BAPI.App_Code;
using Sk_B2BAPI.Models.Admin;
using Sk_B2BAPI.Tool;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Sk_B2BAPI.Admin.prom.ashx
{
    /// <summary>
    /// PromRuleReplenish 的摘要说明
    /// </summary>
    public class PromGroupRule : IHttpHandler, IRequiresSessionState
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

                        if (type == "SaveProm")
                        {
                            var db = new SqlSugarDb();

                            var fabh = "";
                            if (string.IsNullOrWhiteSpace(obj.ruleCode))
                            {
                                db.Db.Ado.UseTran(() =>
                                {
                                    var maxbh = db.Db.Queryable<dt_Maxbh>().First(d => d.BiaoShi == "FAR")?.maxbh ?? "0";
                                    var nowBh = (int.Parse(maxbh) + 1).ToString();
                                    db.Db.Updateable<dt_Maxbh>().SetColumns(d => new dt_Maxbh { maxbh = nowBh }).Where(d => d.BiaoShi == "FAR").ExecuteCommand();
                                    fabh = "FAR" + nowBh.PadLeft(11, '0');
                                });
                            }
                            else
                            {
                                fabh = obj.ruleCode;
                                db.Db.Deleteable<SC_T_PromRuleCondition>().Where(d => d.entid == user.entId && d.ruleCode == fabh).ExecuteCommand();
                                db.Db.Deleteable<SC_T_PromRuleReplenish>().Where(d => d.entid == user.entId && d.ruleCode == fabh).ExecuteCommand();

                            }
                            var prom = new SC_T_PromRule();
                            prom.ruleCode = fabh;
                            prom.entid = user.entId;
                            prom.faType = obj.faType;
                            prom.PromScenario = "3";
                            prom.limitAmount = obj.xgAmount;
                            prom.Amount = obj.Quantity;
                            prom.contentType = "0";
                            prom.customerType = obj.customerType;
                            prom.describe = obj.describe;
                            prom.moreBuy = "0";
                            prom.ruleTitle = obj.faname;
                            var group = JsonConvert.DeserializeObject<List<PromGroupModel>>(obj.lsid);
                            var groupList = new List<SC_T_PromRuleCondition>();
                            var repList = new List<SC_T_PromRuleReplenish>();
                            var groupSn = 1;
                            var b = prom.faType == "GZH";//判断是否是组合
                            foreach (var item in group)
                            {
                                groupList.Add(new SC_T_PromRuleCondition()
                                {
                                    rule_sn = groupSn,
                                    PromScenario = prom.PromScenario,
                                    entid = user.entId,
                                    faType = prom.faType,
                                    giftPrice = decimal.Parse(item.Price),
                                    giftQuantity = b ? decimal.Parse(item.shl) : 0,
                                    meetCount = b ? 0 : decimal.Parse(item.MeetCount),
                                    ruleCode = fabh
                                });
                                repList.Add(new SC_T_PromRuleReplenish()
                                {
                                    rule_sn = groupSn,
                                    entid = user.entId,
                                    faType = prom.faType,
                                    ScnarioID = item.GoodsId,
                                    maxQuantity = b ? obj.Quantity * decimal.Parse(item.shl) : decimal.Parse(item.Total),
                                    limitQuantity = b ? obj.xgAmount * decimal.Parse(item.shl) : decimal.Parse(item.Limit),
                                    ruleCode = fabh
                                });
                                groupSn++;
                            }

                            db.Db.Insertable(groupList).ExecuteCommand();
                            db.Db.Insertable(repList).ExecuteCommand();
                            if (string.IsNullOrWhiteSpace(obj.ruleCode))
                                db.Db.Insertable(prom).ExecuteCommand();
                            else
                                db.Db.Updateable(prom).Where(d => d.entid == user.entId && d.ruleCode == obj.ruleCode).ExecuteCommand();
                            returnJson = JsonMethod.GetError(0, "操作成功");
                        }
                        if (type == "QuerySingleProm")
                        {
                            var db = new SqlSugarDb();
                            var model = db.Db.Queryable<SC_T_PromRule>().First(d => d.entid == obj.entid && d.ruleCode == obj.ruleCode);
                            var condition = db.Db.Queryable<SC_T_PromRuleCondition, SC_T_PromRuleReplenish>((d, e) => new object[] {
                                JoinType.Left,
                                d.entid == e.entid && d.ruleCode == e.ruleCode && d.rule_sn == e.rule_sn
                            })
                                .Where(d => d.entid == obj.entid && d.ruleCode == obj.ruleCode).Select((d, e) => new GroupReplenishes
                                {
                                    entid = d.entid,
                                    rule_sn = d.rule_sn,
                                    faType = d.faType,
                                    giftPrice = d.giftPrice,
                                    giftQuantity = d.giftQuantity,
                                    meetCount = d.meetCount,
                                    ruleCode = d.ruleCode,
                                    maxQuantity = e.maxQuantity,
                                    limitQuantity = e.limitQuantity,
                                    giftGoodsID = e.ScnarioID
                                }).ToList();

                            var scenarios = condition.Select(d => d.giftGoodsID).ToList();

                            var dic = db.Db.Queryable<dt_article_attribute, dt_article_inventory>((d, e) => new object[] { JoinType.Left, d.article_id == e.article_id }).Where((d, e) => d.entid == obj.entid && scenarios.Contains(SqlFunc.ToString(d.article_id))).Select((d, e) => new { d.sub_title, d.article_id, d.goodscode, d.drug_factory, d.drug_spec, d.min_package, d.price, e.stock_quantity }).ToList();

                            condition.ForEach(d =>
                            {
                                var goods = dic.First(m => d.giftGoodsID == m.article_id.ToString());
                                d.stock_quantity = goods.stock_quantity.ToString();
                                d.sub_title = goods.sub_title;
                                d.price = goods.price.ToString();
                                d.min_package = goods.min_package.ToString();
                                d.goodscode = goods.goodscode;
                                d.drug_spec = goods.drug_spec;
                                d.drug_factory = goods.drug_factory;
                            });


                            returnJson = JsonConvert.SerializeObject(new PromGroupUpdate() { flag = 0, promRule = model, conditions = condition });
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