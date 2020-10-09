using Aop.Api.Domain;
using NPOI.SS.Formula.Functions;
using Sk_B2BAPI.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sk_B2BAPI.Tool
{
    public static class BaseConfiguration
    {
        #region 基础配置
        /// <summary>
        /// 折扣拦截 订单金额满足条件可以参与优惠
        /// </summary>
        //public static decimal Threshold { get; set; } = 0;
        private static volatile string _Threshold;
        public static string Threshold
        {
            get { return _Threshold; }
            set { _Threshold = value; }
        }
        /// <summary>
        /// 账期付款折扣
        /// </summary>
        //public static decimal OffLine { get; set; } = 0;
        private static volatile string _OffLine;
        public static string OffLine
        {
            get { return _OffLine; }
            set { _OffLine = value; }
        }
        /// <summary>
        /// 线上付款折扣
        /// </summary>
        //public static decimal OnLine { get; set; } = 0;
        private static volatile string _OnLine;
        public static string OnLine
        {
            get { return _OnLine; }
            set { _OnLine = value; }
        }
        /// <summary>
        /// 证书到期预警天数
        /// </summary>
        private static volatile int _Day;
        public static int Day
        {
            get { return _Day; }
            set { _Day = value; }
        }
        /// <summary>
        /// 订单限定金额
        /// </summary>
        //public static decimal OrderAmount { get; set; } = 0;
        private static volatile string _OrderAmount;
        public static string OrderAmount
        {
            get { return _OrderAmount; }
            set { _OrderAmount = value; }
        }
        /// <summary>
        /// 默认机构
        /// </summary>
        //public static string EntId { get; set; } = "";
        private static volatile string _EntId;
        public static string EntId
        {
            get { return _EntId; }
            set { _EntId = value; }
        }
        /// <summary>
        /// url拼接
        /// </summary>
        //public static string SercerIp { get; set; } = "";
        private static volatile string _SercerIp;
        public static string SercerIp
        {
            get { return _SercerIp; }
            set { _SercerIp = value; }
        }
        /// <summary>
        /// 库存显示
        /// </summary>
        //public static long InventoryShows { get; set; } = 0;
        private static volatile string _InventoryShows;
        public static string InventoryShows
        {
            get { return _InventoryShows; }
            set { _InventoryShows = value; }
        }
        /// <summary>
        /// 库存保留小数位
        /// </summary>
        //public static string InventoryPlace { get; set; } = "0";
        private static volatile string _InventoryPlace;
        public static string InventoryPlace
        {
            get { return _InventoryPlace; }
            set { _InventoryPlace = value; }
        }
        /// <summary>
        /// 价格小数位
        /// </summary>
        //public static string PricePlace { get; set; } = "0";
        private static volatile string _PricePlace;
        public static string PricePlace
        {
            get { return _PricePlace; }
            set { _PricePlace = value; }
        }
        /// <summary>
        /// 中包装位数
        /// </summary>
        //public static string PackagePlace { get; set; } = "0";
        private static volatile string _PackagePlace;
        public static string PackagePlace
        {
            get { return _PackagePlace; }
            set { _PackagePlace = value; }
        }
        #endregion

        #region 阿里短信
        /// <summary>
        /// 阿里短信访问密钥，标识用户
        /// </summary>
        //public static string AccessKeyId { get; set; }
        private static volatile string _AccessKeyId;
        public static string AccessKeyId
        {
            get { return _AccessKeyId; }
            set { _AccessKeyId = value; }
        }
        /// <summary>
        /// 阿里短信访问密钥，验证用户
        /// </summary>
        //public static string AccessSecret { get; set; }
        private static volatile string _AccessSecret;
        public static string AccessSecret
        {
            get { return _AccessSecret; }
            set { _AccessSecret = value; }
        }
        /// <summary>
        /// 阿里短信签名名称
        /// </summary>
        //public static string SignName { get; set; }
        private static volatile string _SignName;
        public static string SignName
        {
            get { return _SignName; }
            set { _SignName = value; }
        }
        /// <summary>
        /// 阿里短信模板ID
        /// </summary>
        //public static string TemplateCode { get; set; }
        private static volatile string _TemplateCode;
        public static string TemplateCode
        {
            get { return _TemplateCode; }
            set { _TemplateCode = value; }
        }
        #endregion

        #region 乐信
        /// <summary>
        /// 用户名(乐信登录账号)
        /// </summary>
        //public static string AccName { get; set; }
        private static volatile string _AccName;
        public static string AccName
        {
            get { return _AccName; }
            set { _AccName = value; }
        }
        /// <summary>
        /// 密码(乐信登录密码
        /// </summary>
        //public static string AccPassword { get; set; }
        private static volatile string _AccPassword;
        public static string AccPassword
        {
            get { return _AccPassword; }
            set { _AccPassword = value; }
        }
        /// <summary>
        /// 签名(乐信签名)
        /// </summary>
        //public static string Sign { get; set; }
        private static volatile string _Sign;
        public static string Sign
        {
            get { return _Sign; }
            set { _Sign = value; }
        }
        #endregion

        #region 物流
        /// <summary>
        /// 物流信息单位编号
        /// </summary>
        //public static string ExpressCustomer { get; set; } = "";
        private static volatile string _ExpressCustomer;
        public static string ExpressCustomer
        {
            get { return _ExpressCustomer; }
            set { _ExpressCustomer = value; }
        }
        /// <summary>
        /// 物流信息单位密钥
        /// </summary>
        //public static string ExpressKey { get; set; } = "";
        private static volatile string _ExpressKey;
        public static string ExpressKey
        {
            get { return _ExpressKey; }
            set { _ExpressKey = value; }
        }
        #endregion

        #region 极光推送
        /// <summary>
        /// 极光推送用户名
        /// </summary>
        //public static string AppKey { get; set; } = "";
        private static volatile string _AppKey;
        public static string AppKey
        {
            get { return _AppKey; }
            set { _AppKey = value; }
        }
        /// <summary>
        /// 极光推送密码
        /// </summary>
        //public static string MasterSecret { get; set; } = "";
        private static volatile string _MasterSecret;
        public static string MasterSecret
        {
            get { return _MasterSecret; }
            set { _MasterSecret = value; }
        }
        /// <summary>
        /// Timer执行间隔时间(毫秒)
        /// </summary>
        //public static int Interval { get; set; } = 0;
        private static volatile int _Interval;
        public static int Interval
        {
            get { return _Interval; }
            set { _Interval = value; }
        }
        #endregion

        #region 甜塔支付
        /// <summary>
        /// 甜塔支付商户号
        /// </summary>
        //public static string TianTaMchID { get; set; } = "";
        private static volatile string _TianTaMchID;
        public static string TianTaMchID
        {
            get { return _TianTaMchID; }
            set { _TianTaMchID = value; }
        }
        /// <summary>
        /// 甜塔支付店铺编号(商户ID)-
        /// </summary>
        //public static string TianTaOperatorId { get; set; } = "";
        private static volatile string _TianTaOperatorId;
        public static string TianTaOperatorId
        {
            get { return _TianTaOperatorId; }
            set { _TianTaOperatorId = value; }
        }
        /// <summary>
        /// 甜塔支付密钥 
        /// </summary>
        //public static string TianTaKey { get; set; } = "";
        private static volatile string _TianTaKey;
        public static string TianTaKey
        {
            get { return _TianTaKey; }
            set { _TianTaKey = value; }
        }
        /// <summary>
        /// 甜塔支付AppId
        /// </summary>
        //public static string TianTaAppId { get; set; } = "";
        private static volatile string _TianTaAppId;
        public static string TianTaAppId
        {
            get { return _TianTaAppId; }
            set { _TianTaAppId = value; }
        }
        #endregion

        #region 微信支付
        /// <summary>
        /// 微信支付 APPID：绑定支付的APPID（必须配置）
        /// </summary>
        //public static string WeChatAppID { get; set; } = "";
        private static volatile string _WeChatAppID;
        public static string WeChatAppID
        {
            get { return _WeChatAppID; }
            set { _WeChatAppID = value; }
        }
        /// <summary>
        /// 微信支付 MCHID：商户号（必须配置）
        /// </summary>
        //public static string WeChatMchID { get; set; } = "";
        private static volatile string _WeChatMchID;
        public static string WeChatMchID
        {
            get { return _WeChatMchID; }
            set { _WeChatMchID = value; }
        }
        /// <summary>
        /// 微信支付  KEY：商户支付密钥，参考开户邮件设置（必须配置），请妥善保管，避免密钥泄露
        /// </summary>
        //public static string WeChatKey { get; set; } = "";
        private static volatile string _WeChatKey;
        public static string WeChatKey
        {
            get { return _WeChatKey; }
            set { _WeChatKey = value; }
        }
        /// <summary>
        /// 微信支付  APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置），请妥善保管，避免密钥泄露
        /// </summary>
        //public static string WeChatAppSecret { get; set; } = "";
        private static volatile string _WeChatAppSecret;
        public static string WeChatAppSecret
        {
            get { return _WeChatAppSecret; }
            set { _WeChatAppSecret = value; }
        }
        /// <summary>
        /// 微信支付 证书路径
        /// </summary>
        //public static string WeChatSSlCertPath { get; set; } = "";
        private static volatile string _WeChatSSlCertPath;
        public static string WeChatSSlCertPath
        {
            get { return _WeChatSSlCertPath; }
            set { _WeChatSSlCertPath = value; }
        }
        /// <summary>
        /// 微信支付 证书密码
        /// </summary>
        //public static string WeChatSSlCertPassword { get; set; } = "";
        private static volatile string _WeChatSSlCertPassword;
        public static string WeChatSSlCertPassword
        {
            get { return _WeChatSSlCertPassword; }
            set { _WeChatSSlCertPassword = value; }
        }
        #endregion

        #region 支付宝
        /// <summary>
        /// 支付宝网关地址
        /// </summary>
        //public static string AlipayServiceUrl { get; set; } = "";
        private static volatile string _AlipayServiceUrl;
        public static string AlipayServiceUrl
        {
            get { return _AlipayServiceUrl; }
            set { _AlipayServiceUrl = value; }
        }
        /// <summary>
        /// 应用ID
        /// </summary>
        //public static string AlipayAppId { get; set; } = "";
        private static volatile string _AlipayAppId;
        public static string AlipayAppId
        {
            get { return _AlipayAppId; }
            set { _AlipayAppId = value; }
        }
        /// <summary>
        /// 开发者私钥
        /// </summary>
        //public static string AlipayPrivateKey { get; set; } = "";
        private static volatile string _AlipayPrivateKey;
        public static string AlipayPrivateKey
        {
            get { return _AlipayPrivateKey; }
            set { _AlipayPrivateKey = value; }
        }
        /// <summary>
        /// 支付宝的公钥
        /// </summary>
        //public static string AlipayPublicKey { get; set; } = "";
        private static volatile string _AlipayPublicKey;
        public static string AlipayPublicKey
        {
            get { return _AlipayPublicKey; }
            set { _AlipayPublicKey = value; }
        }
        #endregion

        #region 建行
        /// <summary>
        /// 商户代码
        /// </summary>
        //public static string MERCHANTID { get; set; }
        private static volatile string _MERCHANTID;
        public static string MERCHANTID
        {
            get { return _MERCHANTID; }
            set { _MERCHANTID = value; }
        }
        /// <summary>
        /// 商户柜台代码
        /// </summary>
        //public static string POSID { get; set; }
        private static volatile string _POSID;
        public static string POSID
        {
            get { return _POSID; }
            set { _POSID = value; }
        }
        /// <summary>
        /// 分行代码
        /// </summary>
        //public static string BRANCHID { get; set; }
        private static volatile string _BRANCHID;
        public static string BRANCHID
        {
            get { return _BRANCHID; }
            set { _BRANCHID = value; }
        }
        /// <summary>
        /// 对应柜台公钥
        /// </summary>
        //public static string PublicKey { get; set; }
        private static volatile string _PublicKey;
        public static string PublicKey
        {
            get { return _PublicKey; }
            set { _PublicKey = value; }
        }
        #endregion

        #region 银联
        private static volatile string _NetPayPCMid;
        /// <summary>
        /// PC商户号
        /// </summary>
        public static string NetPayPCMid
        {
            set { _NetPayPCMid = value; }
            get { return _NetPayPCMid; }
        }
        private static volatile string _NetPayPCTid;
        /// <summary>
        /// PC终端号
        /// </summary>
        public static string NetPayPCTid
        {
            get { return _NetPayPCTid; }
            set { _NetPayPCTid = value; }
        }

        private static volatile string _NetPayAPPMid;
        /// <summary>
        /// APP商户号
        /// </summary>
        public static string NetPayAPPMid
        {
            set { _NetPayAPPMid = value; }
            get { return _NetPayAPPMid; }
        }
        private static volatile string _NetPayAPPTid;
        /// <summary>
        /// APP终端号
        /// </summary>
        public static string NetPayAPPTid
        {
            get { return _NetPayAPPTid; }
            set { _NetPayAPPTid = value; }
        }

        private static volatile string _NetPayAPPInstMid;
        /// <summary>
        /// APP机构商户号
        /// </summary>
        public static string NetPayAPPInstMid { get => _NetPayAPPInstMid; set => _NetPayAPPInstMid = value; }

        private static volatile string _NetPayPCInstMid;
        /// <summary>
        /// PC机构商户号
        /// </summary>
        public static string NetPayPCInstMid { get => _NetPayPCInstMid; set => _NetPayPCInstMid = value; }

        private static volatile string _NetPayMsgSrc;
        /// <summary>
        /// 消息来源
        /// </summary>
        public static string NetPayMsgSrc { get => _NetPayMsgSrc; set => _NetPayMsgSrc = value; }

        private static volatile string _NetPayMsgSrcId;
        /// <summary>
        /// 来源编号
        /// </summary>
        public static string NetPayMsgSrcId { get => _NetPayMsgSrcId; set => _NetPayMsgSrcId = value; }

        private static volatile string _NetPayKey;
        /// <summary>
        /// 密匙
        /// </summary>
        public static string NetPayKey { get => _NetPayKey; set => _NetPayKey = value; }

        private static volatile string _NetPaySubAppId;
        /// <summary>
        /// APP 使用微信时使用
        /// </summary>
        public static string NetPaySubAppId { get => _NetPaySubAppId; set => _NetPaySubAppId = value; }

        private static volatile string _NetPaySwApiUrl;
        /// <summary>
        /// 服务窗接口地址
        /// </summary>
        public static string NetPaySwApiUrl { get => _NetPaySwApiUrl; set => _NetPaySwApiUrl = value; }

        private static volatile string _NetPayApiUrl;
        /// <summary>
        /// APP和C扫B接口地址
        /// </summary>
        public static string NetPayApiUrl { get => _NetPayApiUrl; set => _NetPayApiUrl = value; }

        private static volatile string _NetPayNotifyUrl;
        /// <summary>
        /// 回传银联服务器通知地址
        /// </summary>
        public static string NetPayNotifyUrl { get => _NetPayNotifyUrl; set => _NetPayNotifyUrl = value; }

        private static volatile string _NetPayReturnUrl;
        /// <summary>
        /// 付款成功后，跳转页面，用于PC服务窗
        /// </summary>
        public static string NetPayReturnUrl { get => _NetPayReturnUrl; set => _NetPayReturnUrl = value; }
        #endregion
        public static void Initialize()
        {
            #region 支付宝初始化
            AlipayServiceUrl = XmlOperation.ReadXml("Alipay", "ServiceUrl");
            AlipayAppId = XmlOperation.ReadXml("Alipay", "AppId");
            AlipayPrivateKey = XmlOperation.ReadXml("Alipay", "PrivateKey");
            AlipayPublicKey = XmlOperation.ReadXml("Alipay", "PublicKey");
            #endregion

            #region 微信初始化
            WeChatAppID = XmlOperation.ReadXml("WeChat", "AppID");
            WeChatMchID = XmlOperation.ReadXml("WeChat", "MchID");
            WeChatKey = XmlOperation.ReadXml("WeChat", "Key");
            WeChatAppSecret = XmlOperation.ReadXml("WeChat", "AppSecret");
            WeChatSSlCertPath = XmlOperation.ReadXml("WeChat", "SSlCertPath");
            WeChatSSlCertPassword = XmlOperation.ReadXml("WeChat", "SSlCertPassword");
            #endregion

            #region 银联初始化
            NetPayPCMid = XmlOperation.ReadXml("NetPay", "PCMid");
            NetPayPCTid = XmlOperation.ReadXml("NetPay", "PCTid");
            NetPayAPPMid = XmlOperation.ReadXml("NetPay", "APPMid");
            NetPayAPPTid = XmlOperation.ReadXml("NetPay", "APPTid");
            NetPayAPPInstMid = XmlOperation.ReadXml("NetPay", "APPInstMid");
            NetPayPCInstMid = XmlOperation.ReadXml("NetPay", "PCInstMid");
            NetPayMsgSrc = XmlOperation.ReadXml("NetPay", "MsgSrc");
            NetPayMsgSrcId = XmlOperation.ReadXml("NetPay", "MsgSrcId");
            NetPayKey = XmlOperation.ReadXml("NetPay", "Key");
            NetPaySubAppId = XmlOperation.ReadXml("NetPay", "SubAppId");
            NetPaySwApiUrl = XmlOperation.ReadXml("NetPay", "SwApiUrl");
            NetPayApiUrl = XmlOperation.ReadXml("NetPay", "ApiUrl");
            NetPayNotifyUrl = XmlOperation.ReadXml("NetPay", "NotifyUrl");
            NetPayReturnUrl = XmlOperation.ReadXml("NetPay", "ReturnUrl");
            #endregion

            #region 甜塔初始化
            TianTaMchID = XmlOperation.ReadXml("TianTa", "MchID");
            TianTaOperatorId = XmlOperation.ReadXml("TianTa", "OperatorId");
            TianTaKey = XmlOperation.ReadXml("TianTa", "Key");
            TianTaAppId = XmlOperation.ReadXml("TianTa", "AppId");
            #endregion

            #region 建行初始化
            MERCHANTID = XmlOperation.ReadXml("Construction", "MERCHANTID");
            POSID = XmlOperation.ReadXml("Construction", "POSID");
            BRANCHID = XmlOperation.ReadXml("Construction", "BRANCHID");
            PublicKey = InterceptTheLastThirtyBits(Tool.XmlOperation.ReadXml("Construction", "PublicKey"), 30);
            #endregion

            #region 极光推送初始化
            AppKey = XmlOperation.ReadXml("Push", "AppKey");
            MasterSecret = XmlOperation.ReadXml("Push", "MasterSecret");
            Interval = int.Parse(XmlOperation.ReadXml("Push", "Interval"));
            #endregion

            #region 物流初始化
            ExpressCustomer = XmlOperation.ReadXml("Express", "ExpressCustomer");
            ExpressKey = XmlOperation.ReadXml("Express", "ExpressKey");
            #endregion

            #region 基础
            Threshold = XmlOperation.ReadXml("Base", "Threshold");// Convert.ToDecimal(XmlOperation.ReadXml("Base", "Threshold"));
            OrderAmount = BasisConfig.GetConfigString("OrderAmount");// decimal.Parse(BasisConfig.GetConfigString("OrderAmount").ToString());
            Day = int.Parse(XmlOperation.ReadXml("Base", "Day"));
            EntId = XmlOperation.ReadXml("Base", "EntId");
            SercerIp = BasisConfig.GetConfigString("SercerIp");
            OffLine = XmlOperation.ReadXml("Base", "OffLine");// Convert.ToDecimal(XmlOperation.ReadXml("Base", "OffLine"));
            OnLine = XmlOperation.ReadXml("Base", "OnLine");// Convert.ToDecimal(XmlOperation.ReadXml("Base", "OnLine"));
            InventoryShows = XmlOperation.ReadXml("Base", "InventoryShows");// long.Parse(XmlOperation.ReadXml("Base", "InventoryShows"));

            InventoryPlace = XmlOperation.ReadXml("Base", "InventoryPlace");
            PricePlace = XmlOperation.ReadXml("Base", "PricePlace");
            PackagePlace = XmlOperation.ReadXml("Base", "PackagePlace");
            #endregion

            #region 阿里短信初始化
            AccessKeyId = XmlOperation.ReadXml("AlNote", "AccessKeyId");
            AccessSecret = XmlOperation.ReadXml("AlNote", "AccessSecret");
            SignName = XmlOperation.ReadXml("AlNote", "SignName");
            TemplateCode = XmlOperation.ReadXml("AlNote", "TemplateCode");
            #endregion

            #region 乐信初始化
            AccName = XmlOperation.ReadXml("LxNote", "AccName");
            AccPassword = GenerateMD5(XmlOperation.ReadXml("LxNote", "AccPassword"));
            Sign = XmlOperation.ReadXml("LxNote", "Sign");
            #endregion
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString().ToUpper();
            }
        }

        /// <summary>
        /// 截取字符串后n位
        /// </summary>
        /// <param name="publicKey">数据源</param>
        /// <param name="count">n位</param>
        /// <returns>数据源后30位</returns>
        private static string InterceptTheLastThirtyBits(string publicKey, int count = 0)
        {
            if (publicKey.Length < count && publicKey.Length != 0)
            {
                LogQueue.Write(LogType.Error, "BaseConfiguration/InterceptTheLastThirtyBits", $"数据源长度小于截取长度:{publicKey.Length}");
            }
            return count == 0 ? "" : new string(publicKey.ToCharArray().Reverse<char>().ToArray<char>().Skip(0).Take(count).ToArray().Reverse<char>().ToArray<char>());
        }
    }
}