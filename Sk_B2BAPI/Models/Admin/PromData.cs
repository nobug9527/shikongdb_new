using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models.Admin
{
    /// <summary>
    /// 促销传递数据的model
    /// </summary>
    public class PromData
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string faname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fabs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string faType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startDates { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endDates { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string describe { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string contentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string JieTi { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string goodsid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal xgAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lsid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ScenarioId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PromScenario { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string entid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ruleCode { get; set; }
        public string salesCode { set; get; }
    }

    public class PromLimit
    {
        /// <summary>
        /// 
        /// </summary>
        public string mztj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string zk { get; set; }
        /// <summary>
        /// 锡类散
        /// </summary>
        public string zpmc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string zpid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string shl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string jg { get; set; }
    }

    public class PromUpdate
    {
        public int flag { set; get; }
        public string message { set; get; }
        public SC_T_PromRule promRule { set; get; }
        public List<PromCondition> conditions { set; get; }
        public List<PromRuleReplenish> replenishes { set; get; }
    }

    public class PromSalesUpdate
    {
        public int flag { set; get; }
        public string message { set; get; }
        public SC_T_PromSales promSales { set; get; }
    }
    public class PromGroupUpdate
    {
        public int flag { set; get; }
        public string message { set; get; }
        public SC_T_PromRule promRule { set; get; }
        public List<GroupReplenishes> conditions { set; get; }
    }
    /// <summary>
    /// 用于修改商品组合与抢购的model
    /// </summary>
    public class GroupReplenishes
    {
        /// <summary>
        /// Desc:规则编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ruleCode { get; set; }

        /// <summary>
        /// Desc:企业编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string entid { get; set; }

        /// <summary>
        /// Desc:规则序号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? rule_sn { get; set; }

        /// <summary>
        /// Desc:需要满足的条件
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? meetCount { get; set; }

        /// <summary>
        /// Desc:商品参与活动的价格
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? giftPrice { get; set; }

        /// <summary>
        /// Desc:赠品内码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string giftGoodsID { get; set; }

        /// <summary>
        /// Desc:参与单次活动的商品数量
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? giftQuantity { get; set; }

        /// <summary>
        /// Desc:参与活动的该商品的总数量
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? maxQuantity { get; set; }

        /// <summary>
        /// Desc:单人次可购买商品的最大数量
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? limitQuantity { get; set; }

        /// <summary>
        /// Desc:促销类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string faType { get; set; }
        public string sub_title { get; set; }
        public string goodscode { get; set; }
        public string drug_factory { get; set; }
        public string drug_spec { get; set; }
        public string min_package { get; set; }
        public string price { get; set; }
        public string stock_quantity { get; set; }
    }

}