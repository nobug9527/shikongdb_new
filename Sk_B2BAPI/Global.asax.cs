using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Sk_B2BAPI.Tool;

namespace Sk_B2BAPI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            LogQueue.Start();
            AdminCaoZuoLog.Start();
            //ScheduledTask.Instance().Start();
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BaseConfiguration.Initialize();
        }
        protected void Application_End()
        {
            LogQueue.Stop();
            AdminCaoZuoLog.Stop();
            //ScheduledTask.Instance().Stop();
        }
    }
}