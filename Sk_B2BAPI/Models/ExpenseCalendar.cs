using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 消费记录
    /// </summary>
    public class ExpenseCalendar
    {
        /// <summary>
        /// 消费流水Id
        /// </summary>
        public string RecordNo { get; set; } = "";
        /// <summary>
        /// 记录时间
        /// </summary>
        public string Addtime { get; set; } = "";
        /// <summary>
        /// 消费来源
        /// </summary>
        public string Platform { get; set; } = "";
        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 消费方式
        /// </summary>
        public string PayType { get; set; } = "";
        /// <summary>
        /// 类型 -1【支出】1【收入】
        /// </summary>
        public string Type { get; set; } = "";
        /// <summary>
        /// 备注
        /// </summary>
        public string Remake { get; set; } = "";

    }
}