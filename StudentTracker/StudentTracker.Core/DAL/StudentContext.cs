using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
namespace StudentTracker.Core.DAL
{
    public class StudentContext : DbContext
    {

        #region Constructors
        //Constructor with connection string.
        public StudentContext(string connectionString)
            : base(connectionString)
        {
        }

        //Empty Constructor
        public StudentContext()
        {
        }

        #endregion

        #region Properties
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<RegistrationToken> RegistrationTokens { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<SecurityQuestion> SecurityQuestions { get; set; }
        public DbSet<ELetter> ELetters { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<FormattingField> FormattingFields { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Student> Students { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// This is overridden method used for apply conventions on database
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        #endregion

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<UserSubjects> UserSubjects { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<TeacherSubjects> TeacherSubjects { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<OrganizationServices> OrganizationServices { get; set; }
        public DbSet<ServiceCategory> ServiceCategory { get; set; }

        public DbSet<Tag> Tags{ get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
