using StudentTracker.Core.Repositories;
using StudentTracker.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StudentTracker.Controllers
{
    public class BaseController : Controller
    {
           public BaseController()
        {
        }
           public BaseController(string conn)
        {
            //if (this.repository == null)
            //    this.repository = new Repository(conn);
        }
        /// <summary>
        /// Log to write messages to
        /// </summary>
        //private ILog log = new Log();

        ///// <summary>
        ///// Gets the log to write messages to
        ///// </summary>
        //public ILog Log
        //{
        //    get { return this.log; }
        //}

        protected IRepository repository = null;
        protected UserStatistics _userStatistics = null;

        protected override void Initialize(RequestContext controllerContext)
        {
            try
            {
                //repository = new Repository();
                
                //this.Log.Initialize(ConfigurationManager.AppSettings["LogPath"], "APILogger");
                //this.Log.WriteLine("Starting {0}", "Gateway API");

                base.Initialize(controllerContext);
                _userStatistics = new UserStatistics(HttpContext);
            }
            catch (Exception)
            {
                //Log.WriteException(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (repository != null)
                    repository.Dispose();

                base.Dispose(disposing);
            }
            catch (Exception)
            {
               // Log.WriteException(ex);
            }
        }

    }
}
