using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using StudentTracker.Core.Repositories;
using StudentTracker.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
        protected bool DeleteAttachedFile(string fileName, string subDirectory1, Int32 itemId)
        {
            string directory = string.Empty;
            directory = Server.MapPath("~/Attachments/AttachedFiles");
            directory = directory + "/" + itemId;
            bool ifExist = Directory.Exists(directory);
            StudentContext db = new StudentContext();
            var attachment = db.Attachments.Where(a => a.ItemId == itemId && a.Filename == fileName).FirstOrDefault();
            if (attachment != null)
            {
                db.Attachments.Remove(attachment);
            }
            db.SaveChanges();
            //if not then create a directory
            if (ifExist)
            {
                string filePath = Path.Combine(directory, subDirectory1, fileName);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            return true;
        }

        protected void SaveFiles(string token, string subdirectory, long itemId)
        {
            string fileUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/Attachments/AttachedFiles";
            string tempDirectory = Server.MapPath("~/Attachments/TempFiles");
            StudentContext db = new StudentContext();
            tempDirectory = tempDirectory + "/" + token;
            if (Directory.Exists(tempDirectory))
            {
                string destDirectory = Server.MapPath("~/Attachments/AttachedFiles");
                destDirectory = Path.Combine(destDirectory, subdirectory, itemId.ToString());
                if (!Directory.Exists(destDirectory))
                {
                    Directory.CreateDirectory(destDirectory);
                }

                foreach (var file in Directory.GetFiles(tempDirectory))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    Attachment att = new Attachment();
                    att.Filename = fileInfo.Name;
                    att.ParentType = subdirectory;
                    string filePath = Request.Url.GetLeftPart(UriPartial.Authority) + "/Attachments/AttachedFiles";
                    att.FilePath = Path.Combine(filePath, subdirectory, itemId.ToString(), fileInfo.Name);
                    att.ItemId = itemId;
                    db.Attachments.Add(att);
                    System.IO.File.Move(file, Path.Combine(destDirectory, fileInfo.Name));
                }
                Directory.Delete(tempDirectory);
            }

            db.SaveChanges();
        }

        protected void DeleteFiles(string subdirectory, long itemId)
        {
            StudentContext db = new StudentContext();
            var attachments = db.Attachments.Where(a => a.ParentType == subdirectory && a.ItemId == itemId).ToList();
            foreach (var file in attachments)
            {
                db.Attachments.Remove(file);
            }
            db.SaveChanges();
            string destDirectory = Server.MapPath("~/Attachments/AttachedFiles");
            destDirectory = Path.Combine(destDirectory, subdirectory, itemId.ToString());
            if (Directory.Exists(destDirectory))
            {
                Directory.Delete(destDirectory, true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
            }
            catch (Exception)
            {
                // Log.WriteException(ex);
            }
        }

    }
}
