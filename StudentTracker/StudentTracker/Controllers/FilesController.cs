using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class FilesController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public string UploadFiles(int? chunk, string name, string token)
        {
            string LogoUrl = string.Empty;
            string directory;
            string savingFileName;
            string guid;
            try
            {
                directory = Server.MapPath("~/Attachments/TempFiles/" + token);
                guid = Guid.NewGuid().ToString();
                var fileData = Request.Files[0];
                //Check if directory already exists
                bool ifExist = Directory.Exists(directory);
                //if not then create a directory
                if (!ifExist)
                {
                    Directory.CreateDirectory(directory);
                }

                savingFileName = fileData.FileName;

                string tempFilePath = Path.Combine(directory, savingFileName);
                fileData.SaveAs(tempFilePath);
                LogoUrl = tempFilePath;
                return LogoUrl + "," + guid;
            }
            catch (Exception ex)
            {
                //Recording and Notifying the Exception Details
                //GlobalFunctions.HandleLogError(ex.Message, ex.InnerException, objRouteData: ControllerContext.RouteData);

                throw;
            }
            finally
            {

            }
        }

        public bool DeleteTempFile(string token, string fileName)
        {
            string directory = string.Empty;
            directory = Server.MapPath("~/Attachments/TempFiles");
            directory = Path.Combine(directory, token);
            bool ifExist = Directory.Exists(directory);

            //if not then create a directory
            if (ifExist)
            {
                string filePath = Path.Combine(directory, fileName);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            return true;
        }

        public bool DeleteAttachedFile(string fileName, string subDirectory1, Int32 itemId)
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

        public void SaveFiles(string token, string subdirectory, int itemId)
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
                    att.FilePath = Path.Combine(fileUrl, itemId.ToString(), fileInfo.Name);
                    att.ItemId = itemId;
                    db.Attachments.Add(att);
                    System.IO.File.Move(file, Path.Combine(destDirectory, fileInfo.Name));
                }
                Directory.Delete(tempDirectory);
            }

            db.SaveChanges();
        }

        public void DeleteFiles(string subdirectory, int itemId)
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
            if (!Directory.Exists(destDirectory))
            {
                Directory.Delete(destDirectory);
            }
        }
    }
}
