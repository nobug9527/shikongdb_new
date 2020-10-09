using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Sk_B2BAPI.Models.Admin
{
    ///<summary>
    ///促销的规则汇总信息
    ///</summary>
    public partial class SC_T_PromRule
    {
        public SC_T_PromRule()
        {


        }
        /// <summary>
        /// Desc:企业标识
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string entid { get; set; }

        /// <summary>
        /// Desc:促销类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string faType { get; set; }

        /// <summary>
        /// Desc:规则编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ruleCode { get; set; }

        /// <summary>
        /// Desc:规则标题
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ruleTitle { get; set; }

        /// <summary>
        /// Desc:客户类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string customerType { get; set; }

        /// <summary>
        /// Desc:满足条件的类型，是满足金额 1 还是满足数量减总金额 0 满足数量减单价2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string contentType { get; set; }

        /// <summary>
        /// Desc:规则内 活动的可购买次数，例，组合商品的套数
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? Amount { get; set; }

        /// <summary>
        /// Desc:规则详细的描述
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string describe { get; set; }

        /// <summary>
        /// Desc:活动单人可购买次数
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? limitAmount { get; set; }

        /// <summary>
        /// Desc:参与规则的商品范围  ，如独立商品 3，商品分组 4，全部商品 0，品牌商品 2,分类商品 1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string PromScenario { get; set; }

        /// <summary>
        /// Desc:暂未使用
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string SecnarioId { get; set; }

        /// <summary>
        /// Desc:是否多买多赠。
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string moreBuy { get; set; }
        /// <summary>
        /// Desc:控制规则是否可以被引用，已上架2 未上架1或空 已删除0
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string isShow { get; set; } = "1";

    }
}
