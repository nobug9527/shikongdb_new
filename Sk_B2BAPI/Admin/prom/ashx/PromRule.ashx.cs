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
    /// PromRule 的摘要说明
    /// </summary>
    public class PromRule : IHttpHandler, IRequiresSessionState
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
                            prom.PromScenario = obj.PromScenario;
                            prom.limitAmount = obj.xgAmount;
                            prom.Amount = obj.Quantity;
                            prom.contentType = obj.contentType;
                            prom.customerType = obj.customerType;
                            prom.describe = obj.describe;
                            prom.moreBuy = obj.JieTi;
                            prom.ruleTitle = obj.faname;
                            var limit = JsonConvert.DeserializeObject<List<PromLimit>>(obj.lsid);
                            var limitList = new List<SC_T_PromRuleCondition>();
                            var limitSn = 1;
                            foreach (var item in limit)
                            {
                                limitList.Add(new SC_T_PromRuleCondition()
                                {
                                    rule_sn = limitSn,
                                    PromScenario = prom.PromScenario,
                                    discount = decimal.Parse(item.zk),
                                    entid = user.entId,
                                    faType = prom.faType,
                                    giftGoodsID = item.zpid,
                                    giftPrice = decimal.Parse(item.jg),
                                    giftQuantity = decimal.Parse(item.shl),
                                    meetCount = decimal.Parse(item.mztj),
                                    ruleCode = fabh
                                });
                                limitSn++;
                            }

                            db.Db.Insertable(limitList).ExecuteCommand();
                            var scenarioId = obj.ScenarioId;
                            var scenario = (EPromScenario)Enum.Parse(typeof(EPromScenario), prom.PromScenario);
                            var repList = new List<SC_T_PromRuleReplenish>();
                            var replenishSn = 1;
                            switch (scenario)
                            {
                                case EPromScenario.全部商品:
                                    break;
                                case EPromScenario.独立商品:
                                case EPromScenario.分类商品:
                                    foreach (var item in scenarioId.Split('|'))
                                    {
                                        if (string.IsNullOrEmpty(item)) { continue; }
                                        var data = item.Split('_');
                                        repList.Add(new SC_T_PromRuleReplenish()
                                        {
                                            entid = user.entId,
                                            faType = prom.faType,
                                            rule_sn = replenishSn,
                                            ruleCode = fabh,
                                            limitQuantity = decimal.Parse(data[2]),
                                            maxQuantity = decimal.Parse(data[1]),
                                            ScnarioID = data[0]
                                        });
                                        replenishSn++;
                                    }
                                    break;
                                case EPromScenario.品牌商品:
                                case EPromScenario.商品分组:

                                    foreach (var item in scenarioId.Split('|'))
                                    {
                                        if (string.IsNullOrEmpty(item)) { continue; }
                                        repList.Add(new SC_T_PromRuleReplenish()
                                        {
                                            entid = user.entId,
                                            faType = prom.faType,
                                            rule_sn = replenishSn,
                                            ruleCode = fabh,
                                            ScnarioID = item
                                        });
                                        replenishSn++;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            if (repList.Count > 0)
                            {
                                db.Db.Insertable(repList).ExecuteCommand();
                            }
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
                            var condition = db.Db.Queryable<SC_T_PromRuleCondition>().Where(d => d.entid == obj.entid && d.ruleCode == obj.ruleCode).ToList();

                            var giftIds = condition.Select(d => d.giftGoodsID).ToList();
                            var conditionData = condition.Select(d => new PromCondition()
                            {
                                PromScenario = d.PromScenario,
                                discount = d.discount,
                                entid = d.entid,
                                rule_sn = d.rule_sn,
                                faType = d.faType,
                                giftGoodsID = d.giftGoodsID,
                                giftPrice = d.giftPrice,
                                giftQuantity = d.giftQuantity,
                                meetCount = d.meetCount,
                                ruleCode = d.ruleCode
                            }).ToList();
                            if (model.faType == "DHG")
                            {
                                var dic = db.Db.Queryable<dt_article_attribute>().Where(d => d.entid == obj.entid && giftIds.Contains(SqlFunc.ToString(d.article_id))).Select(d => new { d.sub_title, d.article_id }).ToList().ToDictionary(d => d.article_id.ToString(), d => d.sub_title);
                                conditionData.ForEach(d => d.giftName = dic[d.giftGoodsID]);
                            }
                            if (model.faType == "DMZ")
                            {
                                var dic = db.Db.Queryable<dt_giftdoc>().Where(d => d.entid == obj.entid && giftIds.Contains(d.goodsid)).Select(d => new { d.goodsname, d.goodsid }).ToList().ToDictionary(d => d.goodsid, d => d.goodsname);
                                conditionData.ForEach(d => d.giftName = dic[d.giftGoodsID]);
                            }

                            var replenish = db.Db.Queryable<SC_T_PromRuleReplenish>().Where(d => d.entid == obj.entid && d.ruleCode == obj.ruleCode).ToList();
                            var scenario = (EPromScenario)Enum.Parse(typeof(EPromScenario), model.PromScenario);
                            var scenarios = replenish.Select(d => d.ScnarioID).ToList();
                            var replenishData = replenish.Select(d => new PromRuleReplenish()
                            {
                                ScnarioID = d.ScnarioID,
                                entid = d.entid,
                                faType = d.faType,
                                InfoContent = d.InfoContent,
                                rule_sn = d.rule_sn,
                                limitQuantity = d.limitQuantity,
                                maxQuantity = d.maxQuantity,
                                ruleCode = d.ruleCode
                            }).ToList();
                            if (scenario == EPromScenario.分类商品)
                            {
                                var dic = db.Db.Queryable<dt_article_category>().Where(d => d.channel_id == 7 && scenarios.Contains(SqlFunc.ToString(d.id))).Select(d => new { d.title, d.id }).ToList();
                                replenishData.ForEach(d =>
                                {
                                    var mm = dic.First(m => d.ScnarioID == m.id.ToString());
                                    d.SecnarioName = mm.title;
                                    d.SecnarioCode = mm.id.ToString();

                                });
                            }
                            if (scenario == EPromScenario.独立商品)
                            {
                                var dic = db.Db.Queryable<dt_article_attribute, dt_article_inventory>((d, e) => new object[] { JoinType.Left, d.article_id == e.article_id }).Where((d, e) => d.entid == obj.entid && scenarios.Contains(SqlFunc.ToString(d.article_id))).Select((d, e) => new { d.sub_title, d.article_id, d.goodscode, d.drug_factory, d.drug_spec, d.min_package, d.price, e.stock_quantity }).ToList();


                                replenishData.ForEach(d =>
                                {
                                    var mm = dic.First(m => d.ScnarioID == m.article_id.ToString());
                                    d.SecnarioName = $"商品：{mm.sub_title}&nbsp;&nbsp;&nbsp;生产公司:{mm.drug_factory}&nbsp;&nbsp;&nbsp;规格:{mm.drug_spec}&nbsp;&nbsp;&nbsp;中包装：{mm.min_package}&nbsp;&nbsp;&nbsp;价格:{mm.price}&nbsp;&nbsp;&nbsp;库存:{mm.stock_quantity}";
                                    d.SecnarioCode = mm.goodscode;

                                });
                            }

                            returnJson = JsonConvert.SerializeObject(new PromUpdate() { flag = 0, promRule = model, conditions = conditionData, replenishes = replenishData });
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