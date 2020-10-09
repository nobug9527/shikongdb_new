using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 支付/退款 交易记录
    /// </summary>
    public class PayRecord
    {
        /// <summary>
        /// 支付订单编号
        /// </summary>
        public string OrderNo { get; set; } = "";
        /// <summary>
        /// 商户订单支付流水号
        /// </summary>
        public string Generate { get; set; } = "";
        /// <summary>
        /// 订单支付交易号
        /// </summary>
        public string ThirdParty { get; set; } = "";
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// 交易状态
        /// </summary>
        public int PayStatus { get; set; }
        /// <summary>
        /// 交易方式
        /// </summary>
        public string PayType { get; set; } = "";
        /// <summary>
        /// 交易类型
        /// </summary>
        public string Type { get; set; } = "";
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; } = "";
        /// <summary>
        /// 机构
        /// </summary>
        public string EntId { get; set; } = "";
        /// <summary>
        /// 二维码
        /// </summary>
        public string Url { get; set; } = "";
        /// <summary>
        /// 记录时间
        /// </summary>
        public string AddTime { get; set; } = "";
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string LastModifyTime { get; set; } = "";
    }
}