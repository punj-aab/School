using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentTracker.Core.Entities;
using System.Data;
using Dapper;
using StudentTracker.Core.Repositories;
using System.IO;

namespace StudentTracker.Core.Repository
{
    public class CommonRepository : BaseRepository
    {
        public List<Organization> SelectOrganizations()
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "dbo.usp_getOrganizations";
                return connection.Query<Organization>(storedProcedure, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Organization SelectOrganizations(long organizationId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "dbo.usp_getOrganizations";
                return connection.Query<Organization>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public Course GetCourses(long courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_getCourses";
                return connection.Query<Course>(storedProcedure, new { courseId = courseId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public List<Course> GetCourses(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_getCourses";
                return connection.Query<Course>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public List<Class> GetClasses(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_getClasses";
                return connection.Query<Class>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Class GetClasses(long classId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_getClasses";
                return connection.Query<Class>(storedProcedure, new { classId = classId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public List<Subject> GetSubjects(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_getSubjects";
                return connection.Query<Subject>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Subject GetSubjects(long subjectId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_getSubjects";
                return connection.Query<Subject>(storedProcedure, new { subjectId = subjectId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public List<ClassRoom> GetClassRooms(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_getClassRooms";
                return connection.Query<ClassRoom>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public ClassRoom GetClassRooms(long classRoomId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_getClassRooms";
                return connection.Query<ClassRoom>(storedProcedure, new { classRoomId = classRoomId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public List<Department> GetDepartments(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_getDepartments";
                return connection.Query<Department>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Department GetDepartments(long departmentId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_getDepartments";
                return connection.Query<Department>(storedProcedure, new { departmentId = departmentId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public Group GetGroups(long groupId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetGroups";
                return connection.Query<Group>(storedProcedure, new { groupId = groupId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public List<Group> GetGroups(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetGroups";
                return connection.Query<Group>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public List<Organization> GetOrganizations()
        {
            return this.Get<Organization>("Select OrganizationId, OrganizationName from organizations");
        }
        public Organization GetOrganizations(long id)
        {
            return this.SingleOrDefault<Organization>("Select OrganizationId, OrganizationName from organizations where OrganizationId = @id", id);
        }
        public List<Template> GetTemplates(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetTemplates";
                return connection.Query<Template>(storedProcedure, new { OrganizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Template GetTemplates(long templateId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetTemplates";
                return connection.Query<Template>(storedProcedure, new { TemplateId = templateId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public List<Section> GetSections(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetSections";
                return connection.Query<Section>(storedProcedure, new { organizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public List<Event> GetEvents(long? organizationId = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetEvents";
                return connection.Query<Event>(storedProcedure, new { OrganizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Event GetEvents(long eventId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetEvents";
                return connection.Query<Event>(storedProcedure, new { EventId = eventId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public Section GetSections(long sectionId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetSections";
                return connection.Query<Section>(storedProcedure, new { sectionId = sectionId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public List<ELetter> GetUsersForEvent(long eventId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetUsersForEvent";
                return connection.Query<ELetter>(storedProcedure, new { EventId = eventId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public List<ELetter> GetEletters(long userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetEletters";
                return connection.Query<ELetter>(storedProcedure, new { UserId = userId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public ELetter GetEletters(long userId, long eLetterId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetEletters";
                return connection.Query<ELetter>(storedProcedure, new { UserId = userId, EletterId = eLetterId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public List<UserGroup> GetGroupUsers(long groupId) //This used for get total user of a particular group
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetGroupUsers";
                return connection.Query<UserGroup>(storedProcedure, new { GroupId = groupId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public List<Student> GetImportedSudents(string importId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetImportedStudents";
                return connection.Query<Student>(storedProcedure, new { ImportId = importId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<Staff> GetImportedStaff(string importId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetImportedStaff";
                return connection.Query<Staff>(storedProcedure, new { ImportId = importId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        //public List<Group> GetOrganiztionGroup(long organizationId)
        //{
        //    using (IDbConnection connection = OpenConnection())
        //    {
        //        const string query = "select * from UserGroup where UserId = @UserId";
        //        return connection.Query<Group>(query, new { UserId = organizationId }).ToList();
        //    }
        //}

        public List<UserGroup> GetUserGroups(long userID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "select * from UserGroup where UserId = @UserId";
                return connection.Query<UserGroup>(query, new { UserId = userID }).ToList();
            }
        }


        public List<Student> GetStudents(long organizationId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetStudents";
                return connection.Query<Student>(storedProcedure, new { OrganizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        //public Student GetStudents(long organizationId,long studentId)
        //{
        //    using (IDbConnection connection = OpenConnection())
        //    {
        //        const string storedProcedure = "usp_GetStudents";
        //        return connection.Query<Student>(storedProcedure, new { OrganizationId = organizationId, StudentId=studentId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
        //    }
        //}
        public List<StaffTypes> GetStaffTypes()
        {
            return this.Get<StaffTypes>("select * from StaffTypes");
        }
        public Staff GetStaff(long staffId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetStaff";
                return connection.Query<Staff>(storedProcedure, new { StaffId = staffId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public List<Staff> GetStaff(long? organizationId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetStaff";
                return connection.Query<Staff>(storedProcedure, new { OrganizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public List<TeacherSubjects> GetTeacherSubjects(long userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_GetTeacherSubjects";
                return connection.Query<TeacherSubjects>(storedProcedure, new { UserId = userId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Profile GetUserProfile(long userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_getUserProfile";
                return connection.Query<Profile>(storedProcedure, new { UserId = userId }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }

        public bool CreateOrganization(Organization objOrganization, RegistrationToken objToken)
        {
            var parameters = new
            {
                Address1 = objOrganization.Address1,
                Address2 = objOrganization.Address2,
                City = objOrganization.City,
                CountryId = objOrganization.CountryId,
                CreatedBy = objOrganization.CreatedBy,
                CreatedDate = DateTime.Now,
                Email = objOrganization.Email,
                OrganizationDesc = objOrganization.OrganizationDesc,
                OrganizationName = objOrganization.OrganizationName,
                OrganizationTypeId = objOrganization.OrganizationTypeId,
                Phone1 = objOrganization.Phone1,
                Phone2 = objOrganization.Phone2,
                RegisterationNumber = objOrganization.RegisterationNumber,
                StateId = objOrganization.StateId
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_addOrganization";
                int rowsAffected = 0;
                rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objOrganization.OrganizationId = id);
                objToken.OrganizationId = objOrganization.OrganizationId;
                if (rowsAffected > 0)
                {
                    rowsAffected = 0;
                    rowsAffected = CreateRegistrationToken(objToken, connection);
                }

                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CreateCourse(Course objCourse)
        {
            var parameters = new
            {
                CourseName = objCourse.CourseName,
                CourseDescription = objCourse.CourseDescription,
                OrganisationId = objCourse.OrganisationId,
                CreatedBy = objCourse.CreatedBy,
                InsertedOn = DateTime.Now,
            };

            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_addCourse";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objCourse.CourseId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CreateClass(Class objClass)
        {
            var parameters = new
            {
                ClassName = objClass.ClassName,
                Description = objClass.Description,
                OrganizationId = objClass.OrganizationId,
                CourseId = objClass.CourseId,
                InsertedOn = DateTime.Now,
                InsertedBy = objClass.InsertedBy
            };

            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_addClass";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objClass.ClassId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CreateDepartment(Department objDep)
        {
            var parameters = new
            {
                DepartmentName = objDep.DepartmentName,
                DepartmentDesc = objDep.DepartmentDesc,
                OrganizationId = objDep.OrganizationId,
                CreatedDate = objDep.CreatedDate,
                CreatedBy = objDep.CreatedBy
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_AddDepartment";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objDep.DepartmentId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CreateSubject(Subject objSubject)
        {
            var parameters = new
            {
                SubjectName = objSubject.SubjectName,
                SubjectDescription = objSubject.SubjectDescription,
                OrganizationId = objSubject.OrganizationId,
                CourseId = objSubject.CourseId,
                ClassId = objSubject.ClassId,
                CreatedBy = objSubject.CreatedBy,
                InsertedOn = DateTime.Now
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_addSubject";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objSubject.SubjectId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CreateClassRoom(ClassRoom objClass)
        {
            var parameters = new
            {
                Name = objClass.Name,
                Description = objClass.Description,
                Location = objClass.Location,
                InsertedOn = DateTime.Now,
                InsertedBy = objClass.InsertedBy,
                DepartmentId = objClass.DepartmentId,
                OrganizationId = objClass.OrganizationId
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_addClassRoom";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objClass.ClassRoomId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CreateSection(Section objSection)
        {
            var parameters = new
            {
                SectionName = objSection.SectionName,
                SectionDescription = objSection.SectionDescription,
                ClassId = objSection.ClassId,
                CreatedBy = objSection.CreatedBy,
                InsertedOn = DateTime.Now
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_addSection";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objSection.SectionId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CreateGroup(Group objGroup)
        {
            var parameters = new
            {
                GroupName = objGroup.GroupName,
                Description = objGroup.Description,
                InsertedOn = objGroup.InsertedOn,
                InsertedBy = objGroup.InsertedBy,
                OrganizationId = objGroup.OrganizationId
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_AddGroups";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objGroup.GroupId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public int CreateToken(RegistrationToken objToken)
        {
            var parameters = new
            {
                Token = objToken.Token,
                OrganizationId = objToken.OrganizationId,
                DepartmentId = objToken.DepartmentId,
                CourseId = objToken.CourseId,
                ClassId = objToken.ClassId,
                SectionId = objToken.SectionId,
                RoleId = objToken.RoleId,
                CreatedBy = objToken.CreatedBy,
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_AddRegistrationToken";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                //SetIdentity<int>(connection, id => objToken.TokenId = id);
                return rowsAffected;
            }
        }
        public bool CreateTemplate(Template objTemplate)
        {
            var parameters = new
            {
                Name = objTemplate.Name,
                Description = objTemplate.Description,
                TemplateText = objTemplate.TemplateText,
                IsActive = objTemplate.IsActive,
                InsertedOn = objTemplate.InsertedOn,
                InsertedBy = objTemplate.InsertedBy,
                OrganizationId = objTemplate.OrganizationId,
                TemplateTypeId = objTemplate.TemplateTypeId
            };
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_AddTemplates";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objTemplate.TemplateId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CreateEvent(Event objEvent)
        {
            var parameters = new
            {
                EventTypeId = objEvent.EventTypeId,
                EventName = objEvent.EventName,
                Description = objEvent.Description,
                StartDate = objEvent.StartDate,
                EndDate = objEvent.EndDate,
                StartTime = objEvent.StartTime,
                EndTime = objEvent.EndTime,
                IsActive = objEvent.IsActive,
                NotificationTypeId = objEvent.NotificationTypeId,
                OrganizationId = objEvent.OrganizationId,
                CourseId = objEvent.CourseId,
                ClassId = objEvent.ClassId,
                SectionId = objEvent.SectionId,
                InsertedOn = objEvent.InsertedOn,
                InsertedBy = objEvent.InsertedBy
            };

            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_AddEvent";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objEvent.EventId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CreateEletter(ELetter objEletter)
        {
            var parameters = new
            {
                UserId = objEletter.UserId,
                EventId = objEletter.EventId,
                TemplateId = objEletter.TemplateId,
                OrganizationId = objEletter.OrganizationId,
                IsRead = false,
                InsertedOn = DateTime.Now,
                InsertedBy = objEletter.InsertedBy
            };

            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_AddEletters";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objEletter.EletterId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool AssignGroupToUser(UserGroup objUserGroup)
        {
            var parameters = new
            {
                UserId = objUserGroup.UserId,
                GroupId = objUserGroup.GroupId,
                InsertedOn = DateTime.Now,
                InsertedBy = objUserGroup.InsertedBy,
                StudentId = objUserGroup.StudentId,
                StaffId = objUserGroup.Default
            };

            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_AssignGroupToUser";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objUserGroup.GroupId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool AssignSubjectToUser(UserSubjects objUserSubject)
        {
            var parameters = new
            {
                UserId = objUserSubject.UserId,
                SubjectId = objUserSubject.SubjectId,
                InsertedOn = DateTime.Now,
                InsertedBy = objUserSubject.InsertedBy,
                StudentId = objUserSubject.StudentId
            };

            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_AssignSubjectToUser";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                //  SetIdentity<int>(connection, id => objUserSubject.GroupId = id);
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public long CreateStudent(Student objStudent)
        {
            var parameters = new
            {
                UserId = objStudent.UserId,
                CourseId = objStudent.CourseId,
                DepartmentId = objStudent.DepartmentId,
                ClassId = objStudent.ClassId,
                SectionId = objStudent.SectionId,
                RollNo = objStudent.RollNo,
                InsertedOn = objStudent.InsertedOn,
                InsertedBy = objStudent.InsertedBy,
                Email = objStudent.Email,
                OrganizationId = objStudent.OrganizationId,
                FullName = objStudent.FullName,
            };

            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_AddStudent";
                int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                SetIdentity<int>(connection, id => objStudent.StudentId = id);
                if (rowsAffected > 0)
                {
                    return objStudent.StudentId;
                }
                return -1;
            }
        }

        public bool DeleteGroup(long groupId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string storedProcedure = "usp_DeleteGroup";
                int recAffected = connection.Execute(storedProcedure, new { GroupId = groupId }, commandType: CommandType.StoredProcedure);
                if (recAffected > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool DeleteOrganization(long organizationId)
        {
            using (IDbConnection connection = OpenConnection())
            {

                const string storedProcedureCourses = "usp_GetOrganization_Courses";
                const string storedProcedureDepartments = "usp_GetOrganization_Departments";

                List<OrganizationCoursesRoot> objCourseModelList = connection.Query<OrganizationCoursesRoot>(storedProcedureCourses, new { OrganizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
                List<OrganizationCoursesRoot> objDepartmentModelList = connection.Query<OrganizationCoursesRoot>(storedProcedureDepartments, new { OrganizationId = organizationId }, commandType: CommandType.StoredProcedure).ToList();
                IEnumerable<long> orgIds = objCourseModelList.Select(x => x.OrganizationId).Distinct();
                IEnumerable<long?> courseIds = objCourseModelList.Where(x => x.CourseId != null).Select(x => x.CourseId).Distinct();
                IEnumerable<long?> classIds = objCourseModelList.Where(x => x.ClassId != null).Select(x => x.ClassId).Distinct();
                IEnumerable<long?> subjectIds = objCourseModelList.Where(x => x.SubjectId != null).Select(x => x.SubjectId).Distinct();
                IEnumerable<long?> departmentIds = objDepartmentModelList.Where(x => x.DepartmentId != null).Select(x => x.DepartmentId).Distinct();

                IDbTransaction transaction = connection.BeginTransaction();

                try
                {
                    // DELETE ALL SUBJECTS
                    foreach (long subjectId in subjectIds)
                    {
                        connection.Execute("Delete from Schedule where SubjectId = @SubjectId", new { SubjectId = subjectId }, transaction);
                        DeleteFiles("SubjectController", subjectId, connection, transaction);
                    }
                    // DELETE ALL SUBJECTS AND SECTION FOR A CLASS
                    foreach (long classId in classIds)
                    {
                        connection.Execute("Delete from Subjects where ClassId = @ClassId", new { ClassId = classId }, transaction);
                        connection.Execute("Delete from Sections where ClassId = @ClassId", new { ClassId = classId }, transaction);
                        DeleteFiles("ClassController", classId, connection, transaction);
                    }
                    // DELETE ALL CLASSES
                    foreach (long courseId in courseIds)
                    {
                        connection.Execute("Delete from Classes where CourseId = @CourseId", new { CourseId = courseId }, transaction);
                        DeleteFiles("CourseController", courseId, connection, transaction);
                    }
                    // DELETE ALL CLASSROOMS
                    foreach (long departmentId in departmentIds)
                    {
                        connection.Execute("Delete from ClassRoom where DepartmentId = @DepartmentId", new { DepartmentId = departmentId }, transaction);
                        DeleteFiles("DepartmentController", departmentId, connection, transaction);
                    }
                    // DELETE ALL COURSES OF A SELECTED ORGANIZATION
                    connection.Execute("Delete from Courses where OrganisationId = @OrganisationId", new { OrganisationId = organizationId }, transaction);
                    // DELETE ALL DEPARTMENTS OF A SELECTED ORGANIZATION
                    connection.Execute("Delete from Departments where OrganizationId = @OrganisationId", new { OrganisationId = organizationId }, transaction);
                    // DELETE ORGANIZATION
                    connection.Execute("Delete from Organizations where OrganizationId = @OrganizationId", new { OrganizationId = organizationId }, transaction);
                    // FINALLY DELETE ALL FILES OF ORGANIZATION
                    DeleteFiles("OrganizationController", organizationId, connection, transaction);
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
                transaction.Commit();
                return true;
            }
        }
        public bool DeleteSchedule(long id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int recordAffected = this.DeleteSchedule(id, connection, transaction);
                    if (recordAffected > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public bool DeleteSubject(long id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                if (this.DeleteSubject(id, connection) > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool DeleteClass(long id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int recordAffected = DeleteClass(id, connection, transaction);
                    if (recordAffected > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
                return false;
            }
        }
        public bool DeleteCourse(long id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int recordAffected = DeleteCourse(id, connection, transaction);
                    if (recordAffected > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
                return false;
            }
        }
        public bool DeleteClassRoom(long id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    int recordAffected = DeleteClassRoom(id, connection, transaction);
                    if (recordAffected > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
                return false;
            }
        }
        public void DeleteUserGroup(long userId, long groupId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                connection.Execute("Delete from UserGroup where UserId = @UserId and GroupId=@GroupId", new { UserId = userId, GroupId = groupId });
            }
        }


        private int DeleteSchedule(long id, IDbConnection connection, IDbTransaction transaction)
        {
            int recordAffected = 0;
            return recordAffected = connection.Execute("Delete from Schedule where ScheduleId = @ScheduleId", new { ScheduleId = id }, transaction);
        }
        private int DeleteSubject(long id, IDbConnection connection)
        {
            int recordAffected = 0;
            List<long?> scheduleIds = connection.Query<long?>("select ScheduleId from Schedule where SubjectId = @id", new { id = id }).ToList();
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                foreach (long scheduleId in scheduleIds)
                {
                    this.DeleteSchedule(scheduleId, connection, transaction);
                }
                recordAffected = connection.Execute("Delete from Subjects where SubjectId = @SubjectId", new { SubjectId = id }, transaction);
                if (recordAffected > 0)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
            catch
            {
                transaction.Rollback();
            }
            return recordAffected;
        }
        private int DeleteClass(long id, IDbConnection connection, IDbTransaction transaction)
        {
            int recordAffected = 0;
            recordAffected = connection.Execute("delete from Subjects where ClassId = @id", new { id = id }, transaction);
            return recordAffected += connection.Execute("Delete from Classes where ClassId = @id", new { id = id }, transaction);
        }
        private int DeleteCourse(long id, IDbConnection connection, IDbTransaction transaction)
        {
            int recordAffected = 0;
            recordAffected = connection.Execute("delete from Schedule where CourseId= @id", new { id = id }, transaction);
            recordAffected += connection.Execute("delete from Subjects where CourseId= @id", new { id = id }, transaction);
            recordAffected += connection.Execute("Delete from Classes where CourseId = @id", new { id = id }, transaction);
            return recordAffected += connection.Execute("Delete from Courses where CourseId = @id", new { id = id }, transaction);
        }
        private int DeleteDepartment(long id, IDbConnection connection, IDbTransaction transaction)
        {
            int recordAffected = 0;
            return recordAffected = connection.Execute("Delete from Departments where DepartmentId = @id", new { id = id }, transaction);
        }
        private int DeleteClassRoom(long id, IDbConnection connection, IDbTransaction transaction)
        {
            int recordAffected = 0;
            return recordAffected = connection.Execute("Delete from ClassRoom where ClassRoomId = @id", new { id = id }, transaction);
        }
        private int CreateRegistrationToken(RegistrationToken objToken, IDbConnection connection)
        {
            var parameters = new
            {
                Token = objToken.Token,
                OrganizationId = objToken.OrganizationId,
                DepartmentId = 0,
                CourseId = objToken.CourseId,
                ClassId = objToken.ClassId,
                SectionId = objToken.SectionId,
                RoleId = objToken.RoleId,
                CreatedBy = objToken.CreatedBy,
            };
            const string storedProcedure = "usp_AddRegistrationToken";
            int rowsAffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            //SetIdentity<int>(connection, id => objToken.TokenId = id);
            return rowsAffected;
        }
        private void DeleteFiles(string subdirectory, long itemId, IDbConnection connection, IDbTransaction transaction)
        {
            connection.Execute("Delete from Attachments where ParentType = @ParentType and ItemId=@ItemId", new { ParentType = subdirectory, ItemId = itemId }, transaction);
            string destDirectory = System.Web.HttpContext.Current.Server.MapPath("~/Attachments/AttachedFiles");
            destDirectory = Path.Combine(destDirectory, subdirectory, itemId.ToString());
            if (Directory.Exists(destDirectory))
            {
                Directory.Delete(destDirectory, true);
            }
        }


        public RegistrationToken GetRegistrationCode(string registrationCode)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "select * from RegistrationToken where Token = @RegistrationToken";
                return connection.Query<RegistrationToken>(query, new { RegistrationToken = registrationCode }).SingleOrDefault();
            }
        }



        public List<Subject> GetUserSubjects(long userID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "select S.SubjectId, SubjectName from Subjects as S " +
                            "join UserSubjects as US on S.SubjectId = US.SubjectId" +
                            " where US.UserId = @UserId";
                return connection.Query<Subject>(query, new { UserId = userID }).ToList();
            }
        }

        public int GroupUserCount(long groupId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "select COUNT(*) as Count from UserGroup where GroupId = @GroupId";
                return connection.Query<int>(query, new { GroupId = groupId }).SingleOrDefault();
            }
        }

        public long SubjectUserCount(long userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "select COUNT(*) from UserSubjects where UserId = @UserId";
                return connection.Query<long>(query, new { UserId = userId }).SingleOrDefault();
            }
        }
    }
}



