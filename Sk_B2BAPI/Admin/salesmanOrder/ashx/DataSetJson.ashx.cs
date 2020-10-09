using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sk_B2BAPI.Admin.salesmanOrder.ashx
{
    /// <summary>
    /// DataSetJson 的摘要说明
    /// </summary>
    public class DataSetJson : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}