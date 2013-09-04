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
        public DbSet<Region> Regions { get; set; }

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
    }
}
