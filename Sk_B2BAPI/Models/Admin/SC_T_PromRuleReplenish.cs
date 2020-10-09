using System;
using System.Linq;
using System.Text;

namespace Sk_B2BAPI.Models.Admin
{
    ///<summary>
    ///促销的补充说明，如单品时 促销的品种信息
    ///</summary>
    public partial class SC_T_PromRuleReplenish
    {
        public SC_T_PromRuleReplenish()
        {


        }
        /// <summary>
        /// Desc:企业标识
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string entid { get; set; }

        /// <summary>
        /// Desc:规则编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ruleCode { get; set; }

        /// <summary>
        /// Desc:规则明细序号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? rule_sn { get; set; }

        /// <summary>
        /// Desc:商品内码或分组编码或品牌编码或分类编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ScnarioID { get; set; }

        /// <summary>
        /// Desc:商品促销说明
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string InfoContent { get; set; }

        /// <summary>
        /// Desc:促销类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string faType { get; set; }

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

    }


    public class PromRuleReplenish : SC_T_PromRuleReplenish
    {
        /// <summary>
        /// 内容名称。如商品名称，分类名称
        /// </summary>
        public string SecnarioName { set; get; }
        /// <summary>
        /// 内容编码。
        /// </summary>
        public string SecnarioCode { set; get; }
    }
}
