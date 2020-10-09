using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.LeXinSDK
{
    public class LeXinException:Exception
    {
        public LeXinException(string msg) : base(msg)
        {

        }
    }
}