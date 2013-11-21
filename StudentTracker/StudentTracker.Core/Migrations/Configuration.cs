namespace StudentTracker.Core.Migrations
{
    using StudentTracker.Core.App_Code;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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
        }
    }
}
