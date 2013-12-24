namespace StudentTracker.Core.Migrations
{
    using StudentTracker.Core.App_Code;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;

    internal sealed class Configuration : DbMigrationsConfiguration<StudentTracker.Core.DAL.StudentContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(StudentTracker.Core.DAL.StudentContext context)
        {
            //Seed obj = new Seed();
            //obj.AddCountriesToDB(context);
            //obj.AddStatesToDB(context);

            //WebSecurity.Register("Demo", "123456", "demo@demo.com", true, "Demo", "Demo");
            //Roles.CreateRole("OrganizationAdmin");
            //Roles.CreateRole("SiteAdmin");
            //Roles.AddUserToRole("Demo", "SiteAdmin");
            //Roles.CreateRole("Student");
            //Roles.CreateRole("Parent");
            //Roles.CreateRole("Teacher");
            //Roles.CreateRole("OtherStaff");
        }
    }
}
