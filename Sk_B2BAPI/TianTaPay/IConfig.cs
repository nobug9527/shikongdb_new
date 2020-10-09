using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.TianTaPay
{
    public interface IConfig
    {
        /// <summary>
        /// 甜塔支付商户号
        /// </summary>
        /// <returns></returns>
        string GetMchID();
        /// <summary>
        /// 甜塔支付店铺编号(商户ID)
        /// </summary>
        /// <returns></returns>
        string GetOperatorId();
        /// <summary>
        /// 甜塔支付密钥
        /// </summary>
        /// <returns></returns>
        string GetKey();
        /// <summary>
        /// 甜塔支付AppId
        /// </summary>
        /// <returns></returns>
        string GetAppId();
        /// <summary>
        /// 甜塔支付版本号
        /// </summary>
        /// <returns></returns>
        string GetVersion();
        /// <summary>
        /// 甜塔支付微信异步回调
        /// </summary>
        /// <returns></returns>
        string GetNotifyUrlWeChat();
        /// <summary>
        /// 甜塔支付支付宝异步回调
        /// </summary>
        /// <returns></returns>
        string GetNotifyUrlAlipay();
    }
}