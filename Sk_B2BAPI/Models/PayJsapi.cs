using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    /// <summary>
    /// 微信JSAPI返回结果集
    /// </summary>
    public class PayJsapi
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; } = "";
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; } = "";
        /// <summary>
        /// 随机字符
        /// </summary>
        public string NonceStr { get; set; } = "";
        /// <summary>
        /// 不同支付方式格式不同
        /// </summary>
        public string Package { get; set; } = "";
        /// <summary>
        /// 签名方式
        /// </summary>
        public string SignType { get; set; } = "";
        /// <summary>
        /// 签名
        /// </summary>
        public string PaySign { get; set; } = "";
    }
}