using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Sk_B2BAPI.WxPayAPI
{
    public class SafeXmlDocument : XmlDocument
    {
        public SafeXmlDocument()
        {
            this.XmlResolver = null;
        }
    }
}