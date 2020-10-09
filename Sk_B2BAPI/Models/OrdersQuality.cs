using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 质检报告订单汇总
    /// </summary>
    public class OrdersQuality
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; } = "";
        /// <summary>
        /// 订单日期
        /// </summary>
        public string Date { get; set; } = "";
        /// <summary>
        /// 单位编号
        /// </summary>
        public string Businesscode { get; set; } = "";
        /// <summary>
        /// 单位名称
        /// </summary>
        public string Businessname { get; set; } = "";
        /// <summary>
        /// 图片数量
        /// </summary>
        public int Pictures { get; set; } = 0;
    }
}