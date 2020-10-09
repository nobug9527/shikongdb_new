using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Sk_B2BAPI.LeXinSDK
{
    public class NoteSafeXmlDocument: XmlDocument
    {
        public NoteSafeXmlDocument()
        {
            this.XmlResolver = null;
        }
    }
}