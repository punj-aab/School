namespace StudentTracker.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Long(nullable: false, identity: true),
                        InsertedOn = c.DateTime(nullable: false),
                        StatusId = c.Int(nullable: false),
                        MasterId = c.Long(nullable: false),
                        Username = c.String(nullable: false),
                        Email = c.String(),
                        Password = c.String(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        LastPasswordFailureDate = c.DateTime(),
                        LastActivityDate = c.DateTime(),
                        LastLockoutDate = c.DateTime(),
                        LastLoginDate = c.DateTime(),
                        ConfirmationToken = c.String(),
                        IsLockedOut = c.Boolean(nullable: false),
                        LastPasswordChangedDate = c.DateTime(),
                        PasswordVerificationToken = c.String(),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        RoleName = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        idSpecified = c.Int(nullable: false),
                        name = c.String(),
                        code = c.String(),
                        continent_id = c.Long(nullable: false),
                        continent_idSpecified = c.Int(nullable: false),
                        country_group_id = c.Long(nullable: false),
                        country_group_idSpecified = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        RegionId = c.Long(nullable: false, identity: true),
                        idSpecified = c.Int(nullable: false),
                        name = c.String(),
                        code = c.String(),
                        country_id = c.Int(nullable: false),
                        country_idSpecified = c.Int(nullable: false),
                        has_tz = c.Long(nullable: false),
                        has_tzSpecified = c.Int(nullable: false),
                        std_offset = c.Long(nullable: false),
                        std_offsetSpecified = c.Int(nullable: false),
                        timezone = c.String(),
                    })
                .PrimaryKey(t => t.RegionId);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        OrganizationId = c.Long(nullable: false, identity: true),
                        OrganizationName = c.String(nullable: false),
                        OrganizationDesc = c.String(nullable: false),
                        OrganizationTypeId = c.Int(nullable: false),
                        RegisterationNumber = c.String(nullable: false),
                        CountryId = c.Int(nullable: false),
                        RegionId = c.Long(nullable: false),
                        City = c.String(nullable: false),
                        Address1 = c.String(nullable: false),
                        Address2 = c.String(),
                        Email = c.String(nullable: false),
                        Phone1 = c.String(nullable: false),
                        Phone2 = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBY = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Deletedby = c.Long(nullable: false),
                        DeletedDate = c.DateTime(nullable: false),
                        StatusId = c.Int(nullable: false),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 100),
                        ConfirmPassword = c.String(),
                    })
                .PrimaryKey(t => t.OrganizationId)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .ForeignKey("dbo.Region", t => t.RegionId)
                .Index(t => t.CountryId)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Long(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false),
                        DepartmentDesc = c.String(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        DeletedDate = c.DateTime(nullable: false),
                        DeletedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Long(nullable: false),
                        CourseName = c.String(nullable: false),
                        CourseDescription = c.String(nullable: false),
                        OrganisationId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        InsertedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                        Organization_OrganizationId = c.Long(),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.Subjects", t => t.CourseId)
                .ForeignKey("dbo.Organizations", t => t.Organization_OrganizationId)
                .Index(t => t.CourseId)
                .Index(t => t.Organization_OrganizationId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectId = c.Long(nullable: false, identity: true),
                        SubjectName = c.String(nullable: false),
                        SubjectDescription = c.String(nullable: false),
                        CourseId = c.Long(nullable: false),
                        InsertedOn = c.String(),
                        CreatedBy = c.String(),
                        ModifiedOn = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.SubjectId);
            
            CreateTable(
                "dbo.Profile",
                c => new
                    {
                        ProfileId = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        Address1 = c.String(nullable: false),
                        Address2 = c.String(nullable: false),
                        City = c.String(),
                        StateId = c.Int(nullable: false),
                        ZipCode = c.String(),
                        Phone1 = c.String(),
                        Phone2 = c.String(),
                        EmailAddress1 = c.String(),
                        EmailAddress2 = c.String(),
                        InsertedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProfileId);
            
            CreateTable(
                "dbo.RegistrationToken",
                c => new
                    {
                        TokenId = c.Long(nullable: false, identity: true),
                        Token = c.Guid(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        SectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TokenId);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        SectionId = c.Int(nullable: false, identity: true),
                        SectionName = c.String(nullable: false),
                        SectionDescription = c.String(nullable: false),
                        ClassId = c.String(nullable: false),
                        InsertedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SectionId);
            
            CreateTable(
                "dbo.RoleUser",
                c => new
                    {
                        Role_RoleId = c.Guid(nullable: false),
                        User_UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_RoleId, t.User_UserId })
                .ForeignKey("dbo.Roles", t => t.Role_RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .Index(t => t.Role_RoleId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.DepartmentOrganization",
                c => new
                    {
                        Department_DepartmentId = c.Long(nullable: false),
                        Organization_OrganizationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_DepartmentId, t.Organization_OrganizationId })
                .ForeignKey("dbo.Departments", t => t.Department_DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.Organization_OrganizationId, cascadeDelete: true)
                .Index(t => t.Department_DepartmentId)
                .Index(t => t.Organization_OrganizationId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.DepartmentOrganization", new[] { "Organization_OrganizationId" });
            DropIndex("dbo.DepartmentOrganization", new[] { "Department_DepartmentId" });
            DropIndex("dbo.RoleUser", new[] { "User_UserId" });
            DropIndex("dbo.RoleUser", new[] { "Role_RoleId" });
            DropIndex("dbo.Courses", new[] { "Organization_OrganizationId" });
            DropIndex("dbo.Courses", new[] { "CourseId" });
            DropIndex("dbo.Organizations", new[] { "RegionId" });
            DropIndex("dbo.Organizations", new[] { "CountryId" });
            DropForeignKey("dbo.DepartmentOrganization", "Organization_OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.DepartmentOrganization", "Department_DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.RoleUser", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.RoleUser", "Role_RoleId", "dbo.Roles");
            DropForeignKey("dbo.Courses", "Organization_OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Courses", "CourseId", "dbo.Subjects");
            DropForeignKey("dbo.Organizations", "RegionId", "dbo.Region");
            DropForeignKey("dbo.Organizations", "CountryId", "dbo.Country");
            DropTable("dbo.DepartmentOrganization");
            DropTable("dbo.RoleUser");
            DropTable("dbo.Sections");
            DropTable("dbo.RegistrationToken");
            DropTable("dbo.Profile");
            DropTable("dbo.Subjects");
            DropTable("dbo.Courses");
            DropTable("dbo.Departments");
            DropTable("dbo.Organizations");
            DropTable("dbo.Region");
            DropTable("dbo.Country");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
        }
    }
}
