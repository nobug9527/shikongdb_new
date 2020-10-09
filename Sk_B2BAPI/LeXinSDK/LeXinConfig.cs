using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sk_B2BAPI.LeXinSDK
{
    public class LeXinConfig
    {
        private static volatile ILeXinConfig leXinConfig;
        private static object syncRoot = new object();

        public static ILeXinConfig GetLeXinConfig()
        {
            if (leXinConfig == null)
            {
                lock (syncRoot)
                {
                    if (leXinConfig == null)
                        leXinConfig = new NoteConfig();
                }
            }
            return leXinConfig;
        }
    }
}