using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 红包
    /// </summary>
    public class Bonus
    {
        /// <summary>
        /// 红包序号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal BonusAmount { get; set; }

        /// <summary>
        /// 红包个数
        /// </summary>
        public int BonusNum { get; set; }
        /// <summary>
        /// 领取时间
        /// </summary>
        public string ReceiveTime { get; set; } = "";
        /// <summary>
        /// 红包来源
        /// </summary>
        public string Source { get; set; } = "";
    }
}