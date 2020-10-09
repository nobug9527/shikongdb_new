using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// App支付返回结果集
    /// </summary>
    public class PayApp
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string Appid { get; set; } = "";
        /// <summary>
        /// 商户号
        /// </summary>
        public string Partnerid { get; set; } = "";
        /// <summary>
        /// 预下单标识
        /// </summary>
        public string Prepayid { get; set; } = "";
        /// <summary>
        /// 随机字符
        /// </summary>
        public string Noncestr { get; set; } = "";
        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; set; } = "";
        /// <summary>
        /// 不同支付方式有不同默认值
        /// </summary>
        public string Package { get; set; } = "";
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; } = "";
    }
}