using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 个人中心角标
    /// </summary>
    public class CornerMark
    {
        /// <summary>
        /// 优惠券数量
        /// </summary>
        public int Coupon { get; set; } = 0;
        /// <summary>
        /// 充值活动
        /// </summary>
        public int RechargeRule { get; set; } = 0;
        /// <summary>
        /// 红包数量
        /// </summary>
        public int Bonus { get; set; } = 0;
        /// <summary>
        /// 消息数量
        /// </summary>
        public int Message { get; set; } = 0;
        /// <summary>
        /// APP最新版本
        /// </summary>
        public string VersionNo { get; set; } = "";
        /// <summary>
        /// 待付款
        /// </summary>
        public int NoPay { get; set; } = 0;
        /// <summary>
        /// 待收货
        /// </summary>
        public int NotReceiving { get; set; } = 0;
        /// <summary>
        /// 待评价
        /// </summary>
        public int NoEvaluation { get; set; } = 0;
        /// <summary>
        /// 售后
        /// </summary>
        public int AfterSale { get; set; } = 0;
    }
}