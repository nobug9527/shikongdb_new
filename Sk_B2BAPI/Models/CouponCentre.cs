using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class CouponCentre : Coupon
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; } = "";
    }
}