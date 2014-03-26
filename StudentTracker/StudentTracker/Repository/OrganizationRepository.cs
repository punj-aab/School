using StudentTracker.Core.Entities;
using StudentTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace StudentTracker.Repository
{
    public class OrganizationRepository : StudentTracker.Core.Repository.CommonRepository
    {
        PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
        public List<Organization> GetOrganizations()
        {
            string sql = "select OrganizationId,OrganizationName,OrganizationDesc,OrganizationTypeId,RegisterationNumber,CountryId,Organizations.StateId,Organizations.city, Address1,Address2,Organizations.Email,Phone1,Phone2,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Deletedby,DeletedDate,Users.Username as InsertedByName,Users_1.Username as ModifiedByName,Countries.CountryName as CountryName,States.StateName as StateName ";
            sql += "from Organizations join Countries on Organizations.CountryId=Countries.CountryCode join States on organizations.StateId=States.StateId join Users on Organizations.CreatedBy=Users.UserId  left join Users as Users_1 on  Organizations.ModifiedBy=Users_1.UserId ";
            List<Organization> modelList = db.Query<Organization>(sql).ToList();
            return modelList;
        }

        public Organization GetOrganizations(long organizationId)
        {
            string sql = "select OrganizationId,OrganizationName,OrganizationDesc,OrganizationTypeId,RegisterationNumber,CountryId,Organizations.StateId, Organizations.city, Address1,Address2,Organizations.Email,Phone1,Phone2,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Deletedby,DeletedDate,Users.Username as InsertedByName,Users_1.Username as ModifiedByName,Countries.CountryName as CountryName,States.StateName as StateName ";
            sql += "from Organizations join Countries on Organizations.CountryId=Countries.CountryCode join States on organizations.StateId=States.StateId join Users on Organizations.CreatedBy=Users.UserId  left join Users as Users_1 on  Organizations.ModifiedBy=Users_1.UserId ";
            sql += "where Organizations.OrganizationId=@organizationId";
            Organization objModel = db.Query<Organization>(sql, new { organizationId = organizationId }).SingleOrDefault();
            return objModel;
        }

        public bool Create(Organization objOrganization)
        {
            int recAffected = 0;
            DBConnectionString.Organization organization = new DBConnectionString.Organization();
            organization.Address1 = objOrganization.Address1;
            organization.Address2 = objOrganization.Address2;
            organization.City = objOrganization.City;
            organization.CountryId = objOrganization.CountryId;
            organization.CreatedBy = objOrganization.CreatedBy;
            organization.CreatedDate = DateTime.Now;
            organization.Email = objOrganization.Email;
            organization.OrganizationDesc = objOrganization.OrganizationDesc;
            organization.OrganizationName = objOrganization.OrganizationName;
            organization.OrganizationTypeId = objOrganization.OrganizationTypeId;
            organization.Phone1 = objOrganization.Phone1;
            organization.Phone2 = objOrganization.Phone2;
            organization.RegisterationNumber = objOrganization.RegisterationNumber;
            organization.StateId = objOrganization.StateId;
            recAffected = Convert.ToInt32(organization.Insert());
            if (recAffected > 0)
            {
                return true;
            }
            return false;

        }

        public bool Update(Organization objOrganization)
        {
            int recAffected = 0;
            DBConnectionString.Organization organization = new DBConnectionString.Organization();
            organization.OrganizationId = objOrganization.OrganizationId;
            organization.Address1 = objOrganization.Address1;
            organization.Address2 = objOrganization.Address2;
            organization.City = objOrganization.City;
            organization.CountryId = objOrganization.CountryId;
            organization.CreatedBy = objOrganization.CreatedBy;
            organization.CreatedDate = objOrganization.CreatedDate;
            organization.Email = objOrganization.Email;
            organization.OrganizationDesc = objOrganization.OrganizationDesc;
            organization.OrganizationName = objOrganization.OrganizationName;
            organization.OrganizationTypeId = objOrganization.OrganizationTypeId;
            organization.Phone1 = objOrganization.Phone1;
            organization.Phone2 = objOrganization.Phone2;
            organization.RegisterationNumber = objOrganization.RegisterationNumber;
            organization.StateId = objOrganization.StateId;
            organization.ModifiedBy = objOrganization.ModifiedBy;
            organization.ModifiedDate = DateTime.Now;
            recAffected = Convert.ToInt32(organization.Insert());
            if (recAffected > 0)
            {
                return true;
            }
            return false;

        }

        public Organization GetOrganization()
        {
            Organization objModel = db.Query<Organization>("exec usp_getOrganizations output").SingleOrDefault();
            return objModel;
        }

        public IEnumerable<ServiceViewModel> GetOrganizationServices(long organizationId)
        {
            List<ServiceViewModel> obj = new List<ServiceViewModel>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd = new SqlCommand("usp_GetOrganizationServices", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("organizationId", SqlDbType.BigInt, int.MaxValue).Value = organizationId;
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ServiceViewModel sv = new ServiceViewModel();
                        sv.IsAdded = Convert.ToBoolean(dr["IsAdded"]);
                        sv.Modified = Convert.ToBoolean(dr["Modified"]);
                        sv.ServiceDescription = dr["ServiceDescription"].ToString();
                        sv.ServiceId = Convert.ToInt32(dr["ServiceId"]);
                        sv.ServiceName = dr["ServiceName"].ToString();
                        sv.CategoryName = dr["CategoryName"].ToString();
                        sv.ServiceCategoryId = Convert.ToInt32(dr["ServiceCategoryId"]);
                        sv.Id = Convert.ToInt32(dr["Id"]);
                        obj.Add(sv);
                    }
                }
                return obj;
            }
            //Dictionary<string, long> parameters = new Dictionary<string, long>();
            //parameters["organizationId"] = organizationId;

            //string storedProcedure = "exec usp_GetOrganizationServices @organizationId";
            //return db.Query<ServiceViewModel>(storedProcedure, new { organizationId = organizationId }).ToList();
        }
    }
}