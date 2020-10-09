using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.TianTaPay
{
    /// <summary>
    /// 配置账号信息
    /// </summary>
    public class TianTaConfig
    {
        private static volatile IConfig config;
        private static object syncRoot = new object();

        public static IConfig GetConfig()
        {
            if (config == null)
            {
                lock (syncRoot)
                {
                    if (config == null)
                        config = new ProjectConfig();
                }
            }
            return config;
        }
    }
}