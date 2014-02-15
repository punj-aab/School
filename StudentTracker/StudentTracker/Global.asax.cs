

using EmailHandler;
using EmailHandler.Threads;
using StudentTracker.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace StudentTracker
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //StartEmailHandler();
        }

        private void StartEmailHandler()
        {
            string queuePath = ".\\private$\\EMAILQUEUE";
            Email email = new Email() { mailTo = "ersinghlakhwant@gmail.com", mailFrom = "lakhwant.enest@gmail.com", mailSubject = "QueueMail", mailBody = "Test MAil" };
            //Utilities.SendMailThruGmail(email);
            //Utilities.SendMailToQueue(email, queuePath);
            //var reciver = new QueueListener(queuePath);
            //reciver.Start();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var ex = Server.GetLastError();
            var status = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;

            // Is Ajax request? return json
            if (httpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                httpContext.ClearError();
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = status;
                httpContext.Response.TrySkipIisCustomErrors = true;
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.Write("{ success: false, message: \"Error occured in server.\" }");
                httpContext.Response.End();
            }
            else
            {
                var currentController = " ";
                var currentAction = " ";
                var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

                if (currentRouteData != null)
                {
                    if (currentRouteData.Values["controller"] != null &&
                        !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                    {
                        currentController = currentRouteData.Values["controller"].ToString();
                    }

                    if (currentRouteData.Values["action"] != null &&
                        !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                    {
                        currentAction = currentRouteData.Values["action"].ToString();
                    }
                }

                var controller = new ErrorController();
                var routeData = new RouteData();

                httpContext.ClearError();
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = status;
                httpContext.Response.TrySkipIisCustomErrors = true;

                routeData.Values["controller"] = "Error";
                routeData.Values["action"] = status == 404 ? "NotFound" : "Index";

                controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            }
        }
    }
}