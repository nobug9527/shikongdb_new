using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Models
{
    public class PayApi: Payment
    {
        /// <summary>
        /// 支付宝-APPID：应用标识
        /// 微信-APPID：绑定支付的APPID（必须配置）
        /// </summary>
        public string AppId {get;set;} = "";
        /// <summary>
        /// 支付宝-商户账号
        /// 微信-MCHID：商户号（必须配置）
        /// </summary>
        public string Merchants { get; set; } = "";
        /// <summary>
        ///支付宝-密钥
        ///微信-KEY：商户支付密钥，参考开户邮件设置（必须配置），请妥善保管，避免密钥泄露
        /// </summary>
        public string AppKey { get; set; } = "";
        /// <summary>
        /// 支付宝-
        /// 微信-APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置），请妥善保管，避免密钥泄露
        /// </summary>
        public string AppSecert { get; set; } = "";
        /// <summary>
        /// 支付宝-
        /// 微信-证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        /// </summary>
        public string SSlCertPath { get; set; } = "";
        /// <summary>
        /// 支付宝-
        /// 微信-访问证书的密码
        /// </summary>
        public string SSlCertPassword { get; set; } = "";
        /// <summary>
        /// 支付宝-
        /// 微信-支付结果通知回调url，用于商户接收支付结果
        /// </summary>
        public string NotifyUrl { get; set; } = "";
        /// <summary>
        /// 支付宝-
        /// 微信-退款结果通知回调url，用于商户接收退款结果
        /// </summary>
        public string RefundUrl { get; set; } = "";
        /// <summary>
        /// 支付宝-
        /// 微信-商户系统后台机器IP，此参数可手动配置也可在程序中自动获取
        /// </summary>
        public string Ip { get; set; } = "";
        /// <summary>
        /// 支付宝-
        /// 微信-代理服务器设置，默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        /// </summary>
        public string ProxyUrl { get; set; } = "";
        /// <summary>
        /// 支付宝-
        /// 微信-上报信息配置，测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        /// </summary>
        public int ReportLevel { get; set; }
        /// <summary>
        /// 日志级别，日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        /// </summary>
        public int LogLevel { get; set; }
        /// <summary>
        /// 网站地址
        /// </summary>
        public string Web_Url { get; set; } = "";
    }
}