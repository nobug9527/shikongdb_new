namespace Sk_B2BAPI.WxPayAPI
{
    /// <summary>
    /// 微信配置账号信息
    /// </summary>
    public class WxPayConfig
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
                        config = new DemoConfig();
                }
            }
            return config;
        }
    }
}