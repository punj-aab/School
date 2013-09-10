namespace StudentTracker.Core.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity;
    using System.Web.Security;


    public class DataContextInitializer : DropCreateDatabaseAlways<StudentContext>
    {
        protected override void Seed(StudentContext context)
        {
            WebSecurity.Register("Demo", "123456", "demo@demo.com", true, "Demo", "Demo");
            Roles.CreateRole("SiteAdmin");
            Roles.AddUserToRole("Demo", "SiteAdmin");
            Roles.CreateRole("OrganizationAdmin");
        }
    }
}