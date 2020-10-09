using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 用户欠款
    /// </summary>
    public class UserDebt
    {
        /// <summary>
        /// 信贷期
        /// </summary>
        public int Xdq { get; set; }
        /// <summary>
        /// 信贷额
        /// </summary>
        public decimal Xde { get; set; }
        /// <summary>
        /// 最早欠款日期
        /// </summary>
        public string DebtDate { get; set; } = "";
        /// <summary>
        /// 欠款天数
        /// </summary>
        public int DebtDays { get; set; }
        /// <summary>
        /// 欠款金额
        /// </summary>
        public decimal DebtMoney { get; set; }
    }
}