



















// This file was automatically generated by the PetaPoco T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String Name: `DBConnectionString`
//     Provider:               `System.Data.SqlClient`
//     Connection String:      `Data Source=anil\sqlexpress;Initial Catalog=Student;User ID=sa;password=**zapped**;`
//     Schema:                 ``
//     Include Views:          `False`



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;

namespace DBConnectionString
{

	public partial class DBConnectionStringDB : Database
	{
		public DBConnectionStringDB() 
			: base("DBConnectionString")
		{
			CommonConstruct();
		}

		public DBConnectionStringDB(string connectionStringName) 
			: base(connectionStringName)
		{
			CommonConstruct();
		}
		
		partial void CommonConstruct();
		
		public interface IFactory
		{
			DBConnectionStringDB GetInstance();
		}
		
		public static IFactory Factory { get; set; }
        public static DBConnectionStringDB GetInstance()
        {
			if (_instance!=null)
				return _instance;
				
			if (Factory!=null)
				return Factory.GetInstance();
			else
				return new DBConnectionStringDB();
        }

		[ThreadStatic] static DBConnectionStringDB _instance;
		
		public override void OnBeginTransaction()
		{
			if (_instance==null)
				_instance=this;
		}
		
		public override void OnEndTransaction()
		{
			if (_instance==this)
				_instance=null;
		}
        

		public class Record<T> where T:new()
		{
			public static DBConnectionStringDB repo { get { return DBConnectionStringDB.GetInstance(); } }
			public bool IsNew() { return repo.IsNew(this); }
			public object Insert() { return repo.Insert(this); }

			public void Save() { repo.Save(this); }
			public int Update() { return repo.Update(this); }

			public int Update(IEnumerable<string> columns) { return repo.Update(this, columns); }
			public static int Update(string sql, params object[] args) { return repo.Update<T>(sql, args); }
			public static int Update(Sql sql) { return repo.Update<T>(sql); }
			public int Delete() { return repo.Delete(this); }
			public static int Delete(string sql, params object[] args) { return repo.Delete<T>(sql, args); }
			public static int Delete(Sql sql) { return repo.Delete<T>(sql); }
			public static int Delete(object primaryKey) { return repo.Delete<T>(primaryKey); }
			public static bool Exists(object primaryKey) { return repo.Exists<T>(primaryKey); }
			public static bool Exists(string sql, params object[] args) { return repo.Exists<T>(sql, args); }
			public static T SingleOrDefault(object primaryKey) { return repo.SingleOrDefault<T>(primaryKey); }
			public static T SingleOrDefault(string sql, params object[] args) { return repo.SingleOrDefault<T>(sql, args); }
			public static T SingleOrDefault(Sql sql) { return repo.SingleOrDefault<T>(sql); }
			public static T FirstOrDefault(string sql, params object[] args) { return repo.FirstOrDefault<T>(sql, args); }
			public static T FirstOrDefault(Sql sql) { return repo.FirstOrDefault<T>(sql); }
			public static T Single(object primaryKey) { return repo.Single<T>(primaryKey); }
			public static T Single(string sql, params object[] args) { return repo.Single<T>(sql, args); }
			public static T Single(Sql sql) { return repo.Single<T>(sql); }
			public static T First(string sql, params object[] args) { return repo.First<T>(sql, args); }
			public static T First(Sql sql) { return repo.First<T>(sql); }
			public static List<T> Fetch(string sql, params object[] args) { return repo.Fetch<T>(sql, args); }
			public static List<T> Fetch(Sql sql) { return repo.Fetch<T>(sql); }
			public static List<T> Fetch(long page, long itemsPerPage, string sql, params object[] args) { return repo.Fetch<T>(page, itemsPerPage, sql, args); }
			public static List<T> Fetch(long page, long itemsPerPage, Sql sql) { return repo.Fetch<T>(page, itemsPerPage, sql); }
			public static List<T> SkipTake(long skip, long take, string sql, params object[] args) { return repo.SkipTake<T>(skip, take, sql, args); }
			public static List<T> SkipTake(long skip, long take, Sql sql) { return repo.SkipTake<T>(skip, take, sql); }
			public static Page<T> Page(long page, long itemsPerPage, string sql, params object[] args) { return repo.Page<T>(page, itemsPerPage, sql, args); }
			public static Page<T> Page(long page, long itemsPerPage, Sql sql) { return repo.Page<T>(page, itemsPerPage, sql); }
			public static IEnumerable<T> Query(string sql, params object[] args) { return repo.Query<T>(sql, args); }
			public static IEnumerable<T> Query(Sql sql) { return repo.Query<T>(sql); }

		}

	}
	



    
	[TableName("Countries")]


	[PrimaryKey("Id")]



	[ExplicitColumns]
    public partial class Country : DBConnectionStringDB.Record<Country>  
    {



		[Column] public int Id { get; set; }





		[Column] public string CountryCode { get; set; }





		[Column] public string CountryName { get; set; }



	}

    
	[TableName("States")]


	[PrimaryKey("Id")]



	[ExplicitColumns]
    public partial class State : DBConnectionStringDB.Record<State>  
    {



		[Column] public int Id { get; set; }





		[Column] public string StateId { get; set; }





		[Column] public string CountryCode { get; set; }





		[Column] public string StateName { get; set; }





		[Column] public string Description { get; set; }



	}

    
	[TableName("Organizations")]


	[PrimaryKey("OrganizationId")]



	[ExplicitColumns]
    public partial class Organization : DBConnectionStringDB.Record<Organization>  
    {



		[Column] public long OrganizationId { get; set; }





		[Column] public string OrganizationName { get; set; }





		[Column] public string OrganizationDesc { get; set; }





		[Column] public int OrganizationTypeId { get; set; }





		[Column] public string RegisterationNumber { get; set; }





		[Column] public int CountryId { get; set; }





		[Column] public int StateId { get; set; }





		[Column] public string City { get; set; }





		[Column] public string Address1 { get; set; }





		[Column] public string Address2 { get; set; }





		[Column] public string Email { get; set; }





		[Column] public string Phone1 { get; set; }





		[Column] public string Phone2 { get; set; }





		[Column] public long? CreatedBy { get; set; }





		[Column] public DateTime? CreatedDate { get; set; }





		[Column] public long? ModifiedBy { get; set; }





		[Column] public DateTime? ModifiedDate { get; set; }





		[Column] public long? Deletedby { get; set; }





		[Column] public DateTime? DeletedDate { get; set; }





		[Column] public string UserName { get; set; }





		[Column] public string Password { get; set; }





		[Column] public string ConfirmPassword { get; set; }



	}

    
	[TableName("Departments")]


	[PrimaryKey("DepartmentId")]



	[ExplicitColumns]
    public partial class Department : DBConnectionStringDB.Record<Department>  
    {



		[Column] public long DepartmentId { get; set; }





		[Column] public string DepartmentName { get; set; }





		[Column] public string DepartmentDesc { get; set; }





		[Column] public long OrganizationId { get; set; }





		[Column] public DateTime? CreatedDate { get; set; }





		[Column] public long? CreatedBy { get; set; }





		[Column] public DateTime? UpdatedDate { get; set; }





		[Column] public long? UpdatedBy { get; set; }





		[Column] public DateTime? DeletedDate { get; set; }





		[Column] public long? DeletedBy { get; set; }



	}

    
	[TableName("Courses")]


	[PrimaryKey("CourseId")]



	[ExplicitColumns]
    public partial class Course : DBConnectionStringDB.Record<Course>  
    {



		[Column] public long CourseId { get; set; }





		[Column] public string CourseName { get; set; }





		[Column] public string CourseDescription { get; set; }





		[Column] public int OrganisationId { get; set; }





		[Column] public long? CreatedBy { get; set; }





		[Column] public DateTime? InsertedOn { get; set; }





		[Column] public long? ModifiedBy { get; set; }





		[Column] public DateTime? ModifiedOn { get; set; }





		[Column] public long? Organization_OrganizationId { get; set; }



	}

    
	[TableName("Profile")]


	[PrimaryKey("ProfileId")]



	[ExplicitColumns]
    public partial class Profile : DBConnectionStringDB.Record<Profile>  
    {



		[Column] public long ProfileId { get; set; }





		[Column] public long UserId { get; set; }





		[Column] public string Address1 { get; set; }





		[Column] public string Address2 { get; set; }





		[Column] public string City { get; set; }





		[Column] public int StateId { get; set; }





		[Column] public string ZipCode { get; set; }





		[Column] public string Phone1 { get; set; }





		[Column] public string Phone2 { get; set; }





		[Column] public string EmailAddress1 { get; set; }





		[Column] public string EmailAddress2 { get; set; }





		[Column] public DateTime InsertedOn { get; set; }





		[Column] public DateTime ModifiedOn { get; set; }



	}

    
	[TableName("RegistrationToken")]


	[PrimaryKey("TokenId")]



	[ExplicitColumns]
    public partial class RegistrationToken : DBConnectionStringDB.Record<RegistrationToken>  
    {



		[Column] public long TokenId { get; set; }





		[Column] public string Token { get; set; }





		[Column] public int OrganizationId { get; set; }





		[Column] public int DepartmentId { get; set; }





		[Column] public int CourseId { get; set; }





		[Column] public int SectionId { get; set; }





		[Column] public long CreatedBy { get; set; }



	}

    
	[TableName("Classes")]


	[PrimaryKey("ClassId")]



	[ExplicitColumns]
    public partial class Class : DBConnectionStringDB.Record<Class>  
    {



		[Column] public long ClassId { get; set; }





		[Column] public string ClassName { get; set; }





		[Column] public string Description { get; set; }





		[Column] public DateTime? InsertedOn { get; set; }





		[Column] public long InsertedBy { get; set; }





		[Column] public DateTime? ModifiedOn { get; set; }





		[Column] public long? ModifiedBy { get; set; }





		[Column] public long OrganizationId { get; set; }





		[Column] public long CourseId { get; set; }



	}

    
	[TableName("Sections")]


	[PrimaryKey("SectionId")]



	[ExplicitColumns]
    public partial class Section : DBConnectionStringDB.Record<Section>  
    {



		[Column] public int SectionId { get; set; }





		[Column] public string SectionName { get; set; }





		[Column] public string SectionDescription { get; set; }





		[Column] public long ClassId { get; set; }





		[Column] public DateTime InsertedOn { get; set; }





		[Column] public long CreatedBy { get; set; }





		[Column] public long? ModifiedBy { get; set; }





		[Column] public DateTime? ModifiedOn { get; set; }



	}

    
	[TableName("Subjects")]


	[PrimaryKey("SubjectId")]



	[ExplicitColumns]
    public partial class Subject : DBConnectionStringDB.Record<Subject>  
    {



		[Column] public long SubjectId { get; set; }





		[Column] public string SubjectName { get; set; }





		[Column] public string SubjectDescription { get; set; }





		[Column] public long CourseId { get; set; }





		[Column] public DateTime InsertedOn { get; set; }





		[Column] public long CreatedBy { get; set; }





		[Column] public DateTime? ModifiedOn { get; set; }





		[Column] public long? ModifiedBy { get; set; }





		[Column] public long ClassId { get; set; }



	}

    
	[TableName("ClassRoom")]


	[PrimaryKey("ClassRoomId")]



	[ExplicitColumns]
    public partial class ClassRoom : DBConnectionStringDB.Record<ClassRoom>  
    {



		[Column] public long ClassRoomId { get; set; }





		[Column] public long DepartmentId { get; set; }





		[Column] public string Name { get; set; }





		[Column] public string Description { get; set; }





		[Column] public string Location { get; set; }





		[Column] public DateTime InsertedOn { get; set; }





		[Column] public long InsertedBy { get; set; }





		[Column] public DateTime? ModifiedOn { get; set; }





		[Column] public long? ModifiedBy { get; set; }



	}

    
	[TableName("Schedule")]


	[PrimaryKey("ScheduleId")]



	[ExplicitColumns]
    public partial class Schedule : DBConnectionStringDB.Record<Schedule>  
    {



		[Column] public long ScheduleId { get; set; }





		[Column] public string ScheduleName { get; set; }





		[Column] public string Description { get; set; }





		[Column] public long OrganizationId { get; set; }





		[Column] public long CourseId { get; set; }





		[Column] public long ClassId { get; set; }





		[Column] public long SubjectId { get; set; }





		[Column] public long DepartmentId { get; set; }





		[Column] public long ClassRoomId { get; set; }





		[Column] public DateTime InsertedOn { get; set; }





		[Column] public long InsertedBy { get; set; }





		[Column] public DateTime? ModifiedOn { get; set; }





		[Column] public long? ModifiedBy { get; set; }





		[Column] public string StartTime { get; set; }





		[Column] public string EndTime { get; set; }





		[Column] public int DayId { get; set; }





		[Column] public string DayIds { get; set; }



	}

    
	[TableName("ScheduleDay")]


	[PrimaryKey("Id")]



	[ExplicitColumns]
    public partial class ScheduleDay : DBConnectionStringDB.Record<ScheduleDay>  
    {



		[Column] public long Id { get; set; }





		[Column] public long ScheduleId { get; set; }





		[Column] public long? DayId { get; set; }



	}

    
	[TableName("RoleUser")]


	[PrimaryKey("Role_RoleId", autoIncrement=false)]

	[ExplicitColumns]
    public partial class RoleUser : DBConnectionStringDB.Record<RoleUser>  
    {



		[Column] public Guid Role_RoleId { get; set; }





		[Column] public long User_UserId { get; set; }



	}

    
	[TableName("__MigrationHistory")]


	[PrimaryKey("MigrationId", autoIncrement=false)]

	[ExplicitColumns]
    public partial class __MigrationHistory : DBConnectionStringDB.Record<__MigrationHistory>  
    {



		[Column] public string MigrationId { get; set; }





		[Column] public byte[] Model { get; set; }





		[Column] public string ProductVersion { get; set; }



	}

    
	[TableName("Users")]


	[PrimaryKey("UserId")]



	[ExplicitColumns]
    public partial class User : DBConnectionStringDB.Record<User>  
    {



		[Column] public long UserId { get; set; }





		[Column] public DateTime InsertedOn { get; set; }





		[Column] public int StatusId { get; set; }





		[Column] public long MasterId { get; set; }





		[Column] public string Username { get; set; }





		[Column] public string Email { get; set; }





		[Column] public string Password { get; set; }





		[Column] public string FirstName { get; set; }





		[Column] public string LastName { get; set; }





		[Column] public int OrgainzationId { get; set; }





		[Column] public int PasswordFailuresSinceLastSuccess { get; set; }





		[Column] public DateTime? LastPasswordFailureDate { get; set; }





		[Column] public DateTime? LastActivityDate { get; set; }





		[Column] public DateTime? LastLockoutDate { get; set; }





		[Column] public DateTime? LastLoginDate { get; set; }





		[Column] public string ConfirmationToken { get; set; }





		[Column] public bool IsLockedOut { get; set; }





		[Column] public DateTime? LastPasswordChangedDate { get; set; }





		[Column] public string PasswordVerificationToken { get; set; }





		[Column] public DateTime? PasswordVerificationTokenExpirationDate { get; set; }



	}

    
	[TableName("Roles")]


	[PrimaryKey("RoleId", autoIncrement=false)]

	[ExplicitColumns]
    public partial class Role : DBConnectionStringDB.Record<Role>  
    {



		[Column] public Guid RoleId { get; set; }





		[Column] public string RoleName { get; set; }





		[Column] public string Description { get; set; }



	}


}



