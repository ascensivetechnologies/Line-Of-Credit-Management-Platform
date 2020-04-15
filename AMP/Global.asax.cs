using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AMP.App_Start;
using FluentScheduler;
using AMP.Services;

namespace AMP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MappingConfig.AutoMapperStartUp();
            JobManager.Initialize(new SchedulerRegistery());
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            //log error
            LogException(exception);

            //process 404 HTTP errors
            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {

            }
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;


            //ignore 404 HTTP errors
            var httpException = exc as HttpException;
            //if (httpException != null && httpException.GetHttpCode() == 404 &&
            //    !EngineContext.Current.Resolve<CommonSettings>().Log404Errors)
            //    return;

            try
            {
                //log
                new Dashboard2ServiceLayer().InsertLog(new ViewModels.Dashboad2.LogsModel()
                {
                    FullMessage = httpException.ToString(),
                    Message = httpException.Message,
                    ServiceName = exc.TargetSite.DeclaringType.Name
                });
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
        }

    }
    
    
}
