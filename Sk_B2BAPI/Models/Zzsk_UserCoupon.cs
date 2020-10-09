using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;

namespace Sk_B2BAPI.Models
{
    public partial class Zzsk_UserCoupon
    {
        public Zzsk_UserCoupon()
        {

        }
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public int UserCouponId { get; set; }
        public string UserId { get; set; } = "";
        public string entid { get; set; } = "";
        public int CouponId { get; set; }
        public string ReceiveTime { get; set; } = "";
        public string EndTIme { get; set; } = "";
        public string UseTime { get; set; } = "";
        public string OrderId { get; set; } = "";
        public string Remarks { get; set; } = "";
        public int Status { get; set; }
    }
}