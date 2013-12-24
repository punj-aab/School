using EmailHandler;
using ImportStuff.ViewModels;
using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using StudentTracker.Core.Utilities;
using StudentTracker.Repository;
using StudentTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class ImportController : BaseController
    {
        //
        // GET: /Import/
        StudentRepository repository = new StudentRepository();
        public ActionResult Index(string type)
        {
            return View();
        }

        public ActionResult ImportStudents()
        {
            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string FileImportExcel(IEnumerable<HttpPostedFileBase> file, string importId, string type)
        {
            string message = string.Empty;
            foreach (string upload in Request.Files)
            {
                try
                {
                    FileInfo fileinfo = new FileInfo(Request.Files[upload].FileName);
                    if (fileinfo.Extension == ".xlsx")
                    {

                        string fileUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/Attachments/ImportedFiles";
                        StudentContext db = new StudentContext();

                        string destDirectory = Server.MapPath("~/Attachments/ImportedFiles");
                        destDirectory = Path.Combine(destDirectory, type, importId);
                        if (!Directory.Exists(destDirectory))
                        {
                            Directory.CreateDirectory(destDirectory);
                        }

                        string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles/";
                        string filename = Path.GetFileName(Request.Files[upload].FileName);
                        filename = Regex.Replace(filename, @"\s+", "");

                        //fetch path of the local directory from iCATStaticItemClass.
                        //string targetFolderPath = Path.GetTempPath();//Server.MapPath("~/Administrator/TempFiles/" + iCATGlobal.CurrentTenantInfo.TenantName);  // This is the part Im wondering about. Will this still function the way it should on the webserver after upload?
                        //create target filePath 
                        string targetFilePath = Path.Combine(destDirectory, filename);
                        //Check if directory exists.
                        if (Directory.Exists(destDirectory))
                        {
                            //if file with the same name exist in the directory.
                            if (System.IO.File.Exists(targetFilePath))
                            {
                                while (System.IO.File.Exists(targetFilePath))//while file exist in the directory.
                                {
                                    try
                                    {
                                        //delete file from with target filepath.
                                        System.IO.File.Delete(targetFilePath);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                // save file 
                                Request.Files[upload].SaveAs(targetFilePath);
                            }
                            else
                            {
                                //save file
                                Request.Files[upload].SaveAs(targetFilePath);
                            }
                        }
                        else
                        {
                            //if directory doesn't exist create a new directory.
                            Directory.CreateDirectory(destDirectory);
                            //save file 
                            Request.Files[upload].SaveAs(targetFilePath);
                        }


                        Attachment att = new Attachment();
                        att.Filename = fileinfo.Name;
                        att.ParentType = type;
                        att.FilePath = Path.Combine(fileUrl, type, importId, filename);
                        att.ItemId = Convert.ToInt64(importId);
                        db.Attachments.Add(att);
                        db.SaveChanges();
                        string worksheetName = "Sheet1";
                        //Initilze the Excel handler constructor with filepath and sheet name to fetch.
                        ExcelHandler objExcel = new ExcelHandler(targetFilePath, worksheetName);
                        DataTable table = new DataTable();
                        //create a list of a dynamic type object.
                        //List<ExpandoObject> result = new List<ExpandoObject>();
                        //call function from excelhandler class to get all the rows in the excel file.
                        bool test = objExcel.ReadDocument(0, ref table);

                        var userId = _userStatistics.UserId;
                        var organizationId = _userStatistics.OrganizationId;

                        if (type == "student")
                        {

                            List<Student> target = table.AsEnumerable().Skip(1)
                             .Select(row => new Student
                             {
                                 // assuming column 0's type is Nullable<long>
                                 RollNo = String.IsNullOrEmpty(row.Field<string>(0))
                                     ? "not found"
                                     : row.Field<string>(0),
                                 FullName = String.IsNullOrEmpty(row.Field<string>(1))
                                     ? "not found"
                                     : row.Field<string>(1),
                                 CourseName = String.IsNullOrEmpty(row.Field<string>(2))
                                     ? "not found"
                                     : row.Field<string>(2),
                                 ClassName = String.IsNullOrEmpty(row.Field<string>(3))
                                     ? "not found"
                                     : row.Field<string>(3),
                                 SectionName = String.IsNullOrEmpty(row.Field<string>(4))
                                     ? "not found"
                                     : row.Field<string>(4),
                                 DepartmentName = String.IsNullOrEmpty(row.Field<string>(5))
                                     ? "not found"
                                     : row.Field<string>(5),
                                 Email = String.IsNullOrEmpty(row.Field<string>(6))
                                     ? "not found"
                                     : row.Field<string>(6),
                                 Remarks = String.IsNullOrEmpty(row.Field<string>(7))
                                 ? "not found"
                                 : row.Field<string>(7),
                                 InsertedOn = DateTime.Now,
                                 ImportId = importId,

                             }).ToList();

                            SaveStudentsInDB(target, importId);
                        }
                        else if(type=="staff")
                        {
                            List<Staff> target = table.AsEnumerable().Skip(1)
                            .Select(row => new Staff
                            {
                                // assuming column 0's type is Nullable<long>
                                FullName = String.IsNullOrEmpty(row.Field<string>(0))
                                    ? "not found"
                                    : row.Field<string>(0),
                               
                                Email = String.IsNullOrEmpty(row.Field<string>(1))
                                    ? "not found"
                                    : row.Field<string>(1),
                                Remarks = String.IsNullOrEmpty(row.Field<string>(3))
                                ? "not found"
                                : row.Field<string>(3),
                                InsertedOn = DateTime.Now,
                                ImportId = importId,
                                StaffTypeName = String.IsNullOrEmpty(row.Field<string>(2))
                                    ? "not found"
                                    : row.Field<string>(2),

                            }).ToList();

                            SaveStaffInDB(target, importId);
                        }
                    }
                    else
                    {
                        message += "File {" + fileinfo.FullName + "} is not supported.";
                    }
                }
                catch (Exception)
                {
                    message += "File(s) does not support.";

                }
            }
            return importId;
        }

        public bool SaveStudentsInDB(List<Student> students, string importId)
        {
            try
            {
                StudentContext context = new StudentContext();
                int organizationId = Convert.ToInt32(_userStatistics.OrganizationId);
                foreach (Student student in students)
                {
                    student.CourseId = GetCourseId(context, student.CourseName, organizationId);
                    student.ImportId = importId;
                    student.OrganizationId = organizationId;
                    student.ClassId = GetClassId(context, student.ClassName, student.CourseId, organizationId);
                    student.SectionId = GetSectionId(context, student.SectionName, student.ClassId, student.CourseId, organizationId);
                    context.Students.Add(student);
                }
                context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public bool SaveStaffInDB(List<Staff> staff, string importId)
        {
            try
            {
                StudentContext context = new StudentContext();
                int organizationId = Convert.ToInt32(_userStatistics.OrganizationId);
                foreach (Staff ind in staff)
                {
                    ind.ImportId = importId;
                    ind.OrganizationId = organizationId;
                    ind.StaffTypeId = GetStaffTypeId(ind.StaffTypeName);
                    context.Staff.Add(ind);
                }
                context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public long GetClassId(StudentContext context, string className, long courseId, int organizationId)
        {
            var classes = context.Classes.Where(c => c.ClassName == className && c.OrganizationId == organizationId);
            if (classes == null || classes.ToList().Count() == 0)
            {
                Class newClass = new Class();
                newClass.ClassName = className;
                newClass.CourseId = courseId;
                newClass.OrganizationId = _userStatistics.OrganizationId == 0 ? 1 : Convert.ToInt32(_userStatistics.OrganizationId);
                newClass.Description = "Created by Code";
                newClass.InsertedBy = _userStatistics.UserId;
                newClass.InsertedOn = DateTime.Now;
                context.Classes.Add(newClass);
                context.SaveChanges();
                return newClass.ClassId;
            }
            else
            {
                return classes.FirstOrDefault().ClassId;
            }

        }

        public int GetSectionId(StudentContext context, string sectionName, long classId, long courseId, int organizationId)
        {
            var section = context.Sections.Where(c => c.SectionName == sectionName && c.ClassId == classId && c.OrganizationId == organizationId);
            if (section == null || section.ToList().Count() == 0)
            {
                Section newSection = new Section();
                newSection.SectionName = sectionName;
                newSection.OrganizationId = _userStatistics.OrganizationId == 0 ? 1 : Convert.ToInt32(_userStatistics.OrganizationId);
                newSection.SectionDescription = "Created by Code";
                newSection.CreatedBy = _userStatistics.UserId;
                newSection.InsertedOn = DateTime.Now;
                newSection.ClassId = classId;
                newSection.CourseId = courseId;
                context.Sections.Add(newSection);
                context.SaveChanges();
                return newSection.SectionId;
            }
            else
            {
                return section.FirstOrDefault().SectionId;
            }

        }

        public long GetCourseId(StudentContext context, string courseName, int organizationId)
        {
            var course = context.Courses.Where(c => c.CourseName == courseName && c.OrganisationId == organizationId);
            if (course == null || course.ToList().Count() == 0)
            {
                Course newCourse = new Course();
                newCourse.CourseName = courseName;
                newCourse.OrganisationId = _userStatistics.OrganizationId == 0 ? 1 : Convert.ToInt32(_userStatistics.OrganizationId);
                newCourse.CourseDescription = "Created by Code";
                newCourse.CreatedBy = _userStatistics.UserId;
                newCourse.InsertedOn = DateTime.Now;
                context.Courses.Add(newCourse);
                context.SaveChanges();
                return newCourse.CourseId;
            }
            else
            {
                return course.FirstOrDefault().CourseId;
            }

        }

        public int GetStaffTypeId(string staffTypeName)
        {
            if (staffTypeName == "teacher")
            {
                return StaffTypes.Teacher;
            }
            else if (staffTypeName == "coach")
            {
                return StaffTypes.Coach;
            }
            else
            {
                return StaffTypes.Clerk;
            }
        }

        [HttpGet]
        public JsonResult ShowExcelFileContentForStudent(jQueryDataTableViewModel param, string importId)
        {
            StudentContext context = new StudentContext();
            // var studentData = context.Students.Where(s => s.ImportId == importId).ToList();
            List<Student> objModelList = repository.GetImportedSudents(importId);

            return Json(new
                        {
                            sEcho = param.sEcho,
                            iTotalRecords = objModelList.Count(),
                            iTotalDisplayRecords = objModelList.Count(),
                            aaData = objModelList
                        }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ShowExcelFileContentForStaff(jQueryDataTableViewModel param, string importId)
        {
            StudentContext context = new StudentContext();
            // var studentData = context.Students.Where(s => s.ImportId == importId).ToList();
            List<Staff> objModelList = repository.GetImportedStaff(importId);

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = objModelList.Count(),
                iTotalDisplayRecords = objModelList.Count(),
                aaData = objModelList
            }, JsonRequestBehavior.AllowGet);
        }

        public bool SendRegistrationEmailToStudents(string importId)
        {
            try
            {
                List<Student> objModelList = repository.GetImportedSudents(importId);
                StudentContext context = new StudentContext();
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                //  RegistrationToken tokenObj = new RegistrationToken();
                foreach (var student in objModelList)
                {
                    string token = UserStatistics.GenerateToken();

                    RegistrationToken tokenObj = new RegistrationToken();
                    tokenObj.ClassId = student.ClassId;
                    tokenObj.CourseId = student.CourseId;
                    tokenObj.DepartmentId = student.DepartmentId;
                    tokenObj.OrganizationId = student.OrganizationId;
                    tokenObj.RoleId = Convert.ToInt32(UserRoles.Student);
                    tokenObj.SectionId = student.SectionId;
                    tokenObj.Token = token;
                    tokenObj.StudentId = student.StudentId;
                    context.RegistrationTokens.Add(tokenObj);
                    parameters.Add(token, student.Email);
                }

                context.SaveChanges();

                Task.Factory.StartNew(() => SendEmails(parameters));

                //SendEmails(parameters);
                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public bool SendRegistrationEmailToStaff(string importId)
        {
            try
            {
                List<Staff> objModelList = repository.GetImportedStaff(importId);
                StudentContext context = new StudentContext();
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                //  RegistrationToken tokenObj = new RegistrationToken();
                foreach (var student in objModelList)
                {
                    string token = UserStatistics.GenerateToken();

                    RegistrationToken tokenObj = new RegistrationToken();
                    tokenObj.ClassId = student.ClassId;
                    tokenObj.CourseId = student.CourseId;
                    tokenObj.DepartmentId = student.DepartmentId;
                    tokenObj.OrganizationId = student.OrganizationId;
                    tokenObj.RoleId = Convert.ToInt32(UserRoles.Student);
                    tokenObj.SectionId = -1;
                    tokenObj.Token = token;
                    tokenObj.StudentId = student.StaffId;
                    context.RegistrationTokens.Add(tokenObj);
                    parameters.Add(token, student.Email);
                }

                context.SaveChanges();

                Task.Factory.StartNew(() => SendEmails(parameters));

                //SendEmails(parameters);
                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void SendEmails(Dictionary<string, string> parameters)
        {

            foreach (var user in parameters)
            {
                var verifyUrl = "http://" + Request.UrlReferrer.Authority + "/sas/sashome/RegisterUser/" + user.Key;

                Utilities.SendRegistationEmail(user.Key, user.Value, verifyUrl);
            }
        }

        public ActionResult ImportStaff()
        {
            return View();
        }

        public ActionResult ImportParents()
        {
            return View();
        }

        public FileResult DownloadSample(string type)
        {
            string filePath = string.Empty;
            if (type == "staff")
            {
                filePath = Server.MapPath("~/App_Data/Sample/SampleStaff.xlsx");
            }
            else if (type == "student")
            {
                filePath = Server.MapPath("~/App_Data/Sample/SampleStudent.xlsx");
            }
            return File(filePath, "text/octet-stream", "sample.xlsx");
        }

    }
}
