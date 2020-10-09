using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 支付方式
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// 支付方式编号
        /// </summary>
        public string PayId { get; set; } = "";
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; } = "";
        /// <summary>
        /// logo图片
        /// </summary>
        public string LogoUrl { get; set; } = "";
    }
}