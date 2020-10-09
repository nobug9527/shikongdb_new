using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.TianTaPay
{
    public class TianTaException:Exception
    {
        public TianTaException(string msg) : base(msg)
        {

        }
    }
}