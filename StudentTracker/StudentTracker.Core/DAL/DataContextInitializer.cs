namespace StudentTracker.Core.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity;
    using System.Web.Security;
    using StudentTracker.Core.Entities;
    using StudentTracker.Core.App_Code;


    public class DataContextInitializer : CreateDatabaseIfNotExists<StudentContext>
    {
        protected override void Seed(StudentContext context)
        {
            //Seed obj = new Seed();
            //obj.AddCountriesToDB(context);
            //obj.AddStatesToDB(context);
           
        }

    }
}