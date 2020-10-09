using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 充值规则
    /// </summary>
    public class RechargeRule
    {
        /// <summary>
        /// 规则Id
        /// </summary>
        public string RuleId { get; set; } = "";
        /// <summary>
        /// 商品Id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 是否有活动
        /// </summary>
        public string IsActive { get; set; } = "";
        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal BonusAmount { get; set; }

        /// <summary>
        /// 红包个数
        /// </summary>
        public int BonusNum { get; set; }
    }
}