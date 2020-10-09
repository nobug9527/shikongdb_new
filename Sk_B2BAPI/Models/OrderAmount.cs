using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class OrderAmount:Orders
    {
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrdersAmount { get; set; }
        /// <summary>
        /// 订单真实金额
        /// </summary>
        public decimal RealAmount { get; set; }
        /// <summary>
        /// 订单优惠金额
        /// </summary>
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 订单是否免邮
        /// </summary>
        public string Free { get; set; } = "";
        /// <summary>
        /// 优惠券Code
        /// </summary>
        public int CouponCode { get; set; }
        /// <summary>
        /// 普通商品金额
        /// </summary>
        public decimal PtAmount { get; set; }
        /// <summary>
        /// 分摊金额DiscountApportion
        /// </summary>
        public decimal DAAmount { get; set; }
        /// <summary>
        /// 使用红包金额
        /// </summary>
        public decimal BonusAmount { get; set; }
        /// <summary>
        /// 线下付款折扣、线上付款折扣
        /// </summary>
        public decimal lineDiscount { get; set; }
    }
}