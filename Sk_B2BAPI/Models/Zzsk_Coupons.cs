using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;

namespace Sk_B2BAPI.Models
{
    public partial class Zzsk_Coupons
    {
        public Zzsk_Coupons()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public string couponName { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public int couponCode { get; set; }
        public int couponsNumber { get; set; }
        public string startingTime { get; set; } = "";
        public string endTime { get; set; } = "";
        public int typeCoding { get; set; }
        public int status { get; set; }
        public int receivingType { get; set; }
        public int SceneCoding { get; set; }
        public int IsDel { get; set; }
        public string ProductCode { get; set; } = "";
        public decimal? AllAmount { get; set; }
        public string Num { get; set; } = "";
        public int? types { get; set; }
        public string SceneId { get; set; }
        public decimal? fullAmount { get; set; }
        public decimal? deduction { get; set; }
        public decimal? maximumAmount { get; set; }
        public decimal? discount { get; set; }
        public string number { get; set; } = "";
        public string entid { get; set; } = "";
    }
}