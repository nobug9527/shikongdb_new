using System;
using System.Linq;
using System.Text;

namespace Sk_B2BAPI.Models.Admin
{
    ///<summary>
    ///促销的条件，如满xx减nn等 存储价格或赠品信息
    ///</summary>
    public partial class SC_T_PromRuleCondition
    {
        public SC_T_PromRuleCondition()
        {


        }
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
        /// Desc:商品享用的折扣，默认100%
        /// Default:100.00
        /// Nullable:True
        /// </summary>           
        public decimal? discount { get; set; }

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
        /// Desc:促销类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string faType { get; set; }

        /// <summary>
        /// Desc:参与规则的商品范围，如独立商品，商品分组，全部商品，品牌商品
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string PromScenario { get; set; }

    }

    public class PromCondition : SC_T_PromRuleCondition
    {
        /// <summary>
        /// 物品名称，如换购的商品名称，满赠的赠品名称
        /// </summary>
        public string giftName { set; get; }
    }
}
