using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.WxPayAPI
{
    public class WxPayException : Exception
    {
        public WxPayException(string msg): base(msg)
        {

        }
    }
}