using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI.Construction
{
    public sealed class ConstructionConfig : IConstructionConfig
    {
        private static volatile IConstructionConfig config;
        private static readonly object syncRoot = new object();
        private ConstructionConfig()
        {

        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static IConstructionConfig Initialize()
        {
            if (config==null)
            {
                lock (syncRoot)
                {
                    if (config==null)
                    {
                        config = new ConstructionConfig();
                    }
                }
            }
            return config;
        }
        /// <summary>
        /// 商户柜台代码
        /// </summary>
        /// <returns></returns>
        public string GetPosId()
        {
            return BaseConfiguration.POSID;
        }
        /// <summary>
        /// 分行代码
        /// </summary>
        /// <returns></returns>
        public string GetBranchId()
        {
            try
            {
                return BaseConfiguration.BRANCHID;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// 商户代码
        /// </summary>
        /// <returns></returns>
        public string GetMerchantId()
        {
            return BaseConfiguration.MERCHANTID;
        }
        /// <summary>
        /// 对应柜台公钥后30位
        /// </summary>
        public string GetPub()
        {
            return BaseConfiguration.PublicKey;
        }
        
    }
}