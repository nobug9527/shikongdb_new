using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Construction
{
    /// <summary>
    /// 建设银行个人网银支付配置接口
    /// </summary>
    public interface IConstructionConfig
    {
        /// <summary>
        /// 商户代码
        /// </summary>
        /// <returns></returns>
        string GetMerchantId();
        /// <summary>
        /// 商户柜台代码
        /// </summary>
        /// <returns></returns>
        string GetPosId();
        /// <summary>
        /// 分行代码
        /// </summary>
        /// <returns></returns>
        string GetBranchId();
        /// <summary>
        /// 对应柜台公钥
        /// </summary>
        /// <returns></returns>
        string GetPub();
    }
}