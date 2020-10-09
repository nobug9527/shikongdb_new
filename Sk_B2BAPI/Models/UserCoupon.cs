using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 用户优惠券
    /// </summary>
    public class UserCoupon:Coupon
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserCouponId { get; set; }
        /// <summary>
        /// 用户主键
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        /// 优惠券主键
        /// </summary>
        public int CouponId { get; set; }
        /// <summary>
        /// 领取时间
        /// </summary>
        public DateTime ReceiveTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTIme { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime UseTime { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderId { get; set; } = "";
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; } = "";



    }
}