using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class OrderStatus
    {
        /// <summary>
        /// 订单状态
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 订单状态码
        /// </summary>
        public string Status { get; set; } = "";
        /// <summary>
        /// 订单条目数
        /// </summary>
        public int Quantity { get; set; } = 0;
    }
}