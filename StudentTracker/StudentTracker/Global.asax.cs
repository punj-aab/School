

using EmailHandler;
using EmailHandler.Threads;
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
            StartEmailHandler();
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
    }
}