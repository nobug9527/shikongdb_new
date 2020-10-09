using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 充值选项
    /// </summary>
    public class RechargeGoods: RechargeRule
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// 类型序号
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>
        public string TypeName { get; set; } = "";

        /// <summary>
        /// 商品备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 是否可以参与
        /// </summary>
        public string IsAble { get; set; } = "";
    }
}