using ImportStuff.ViewModels;
using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using StudentTracker.Core.Utilities;
using StudentTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class ImportController : BaseController
    {
        //
        // GET: /Import/

        public ActionResult Index()
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
                        destDirectory = Path.Combine(destDirectory, "Student", importId);
                        if (!Directory.Exists(destDirectory))
                        {
                            Directory.CreateDirectory(destDirectory);
                        }

                        string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles/";
                        string filename = Path.GetFileName(Request.Files[upload].FileName);


                        //fetch path of the local directory from iCATStaticItemClass.
                        string targetFolderPath = Path.GetTempPath();//Server.MapPath("~/Administrator/TempFiles/" + iCATGlobal.CurrentTenantInfo.TenantName);  // This is the part Im wondering about. Will this still function the way it should on the webserver after upload?
                        //create target filePath 
                        string targetFilePath = Path.Combine(destDirectory, "_" + filename);
                        //Check if directory exists.
                        if (Directory.Exists(targetFolderPath))
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
                            Directory.CreateDirectory(targetFolderPath);
                            //save file 
                            Request.Files[upload].SaveAs(targetFilePath);
                        }


                        Attachment att = new Attachment();
                        att.Filename = fileinfo.Name;
                        att.ParentType = "Student";
                        att.FilePath = Path.Combine(fileUrl, "Student", importId, fileinfo.Name);
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

                        List<Student> target = table.AsEnumerable().Skip(1)
                         .Select(row => new Student
                         {
                             // assuming column 0's type is Nullable<long>
                             RollNo = String.IsNullOrEmpty(row.Field<string>(0))
                                 ? "not found"
                                 : row.Field<string>(0),
                             StudentName = String.IsNullOrEmpty(row.Field<string>(1))
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

        [HttpGet]
        public JsonResult ShowExcelFileContent(jQueryDataTableViewModel param, string importId)
        {
            StudentContext context = new StudentContext();
            var studentData = context.Students.Where(s => s.ImportId == importId).ToList();
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = studentData.Count(),
                iTotalDisplayRecords = studentData.Count(),
                aaData = studentData
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ImportStaff()
        {
            return View();
        }

        public ActionResult ImportParents()
        {
            return View();
        }

    }
}
