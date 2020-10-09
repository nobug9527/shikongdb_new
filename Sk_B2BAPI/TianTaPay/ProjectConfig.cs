using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.TianTaPay
{
    public class ProjectConfig : IConfig
    {
        public ProjectConfig()
        {

        }
        /// <summary>
        /// 甜塔支付商户号
        /// </summary>
        /// <returns></returns>
        public string GetMchID()
        {
            return BaseConfiguration.TianTaMchID;
        }
        /// <summary>
        /// 甜塔支付店铺编号(商户ID)
        /// </summary>
        /// <returns></returns>
        public string GetOperatorId()
        {
            return BaseConfiguration.TianTaOperatorId;
        }
        /// <summary>
        /// 甜塔支付密钥 
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            return BaseConfiguration.TianTaKey;
        }
        /// <summary>
        /// 甜塔支付AppId
        /// </summary>
        /// <returns></returns>
        public string GetAppId()
        {
            return BaseConfiguration.TianTaAppId;
        }
        /// <summary>
        /// 甜塔支付版本号
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            return "V1.0.10";
        }
        /// <summary>
        /// 甜塔支付微信异步回调
        /// </summary>
        /// <returns></returns>
        public string GetNotifyUrlWeChat()
        {
            return "/Apliay/NotifyReturnTianTaWechat";
        }
        /// <summary>
        /// 甜塔支付支付宝异步回调
        /// </summary>
        /// <returns></returns>
        public string GetNotifyUrlAlipay()
        {
            return "/Apliay/NotifyReturnTianTaAlipay";
        }
    }
}