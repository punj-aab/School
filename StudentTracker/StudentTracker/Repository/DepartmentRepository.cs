using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentTracker.Repository
{
    public class DepartmentRepository : StudentTracker.Core.Repository.CommonRepository
    {
        PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
        //public List<Department> GetDepartments()
        //{
        //    string sql = "select Departments.DepartmentId,Departments.DepartmentName,Departments.DepartmentDesc,Departments.CreatedBy,Departments.CreatedDate,Departments.UpdatedBy,Departments.UpdatedDate, Organizations.OrganizationName,Users.Username as InsertedByName,Users_1.Username as ModifiedByName,Departments.OrganizationId ";
        //    sql += "from Departments join Organizations on Departments.OrganizationId=Organizations.OrganizationId join Users on Departments.CreatedBy=Users.UserId left join Users as Users_1 on  Departments.UpdatedBy=Users_1.UserId";
        //    List<Department> departmentList = db.Query<Department>(sql).ToList();
        //    return departmentList;
        //}
        //public Department GetDepartments(long id)
        //{
        //    string sql = "select Departments.DepartmentId,Departments.DepartmentName,Departments.DepartmentDesc,Departments.CreatedBy,Departments.CreatedDate,Departments.UpdatedBy,Departments.UpdatedDate, Organizations.OrganizationName,Users.Username as InsertedByName,Users_1.Username as ModifiedByName, Departments.OrganizationId ";
        //    sql += "from Departments join Organizations on Departments.OrganizationId=Organizations.OrganizationId join Users on Departments.CreatedBy=Users.UserId left join Users as Users_1 on  Departments.UpdatedBy=Users_1.UserId ";
        //    sql += "where DepartmentId=@0";
        //    Department objDepartment = db.Query<Department>(sql, id).FirstOrDefault();
        //    return objDepartment;
        //}
        //public bool CreateDepartment(Department objDepartment)
        //{
        //    int recAffected = 0;
        //    DBConnectionString.Department Department = new DBConnectionString.Department();
        //    Department.DepartmentName = objDepartment.DepartmentName;
        //    Department.DepartmentDesc = objDepartment.DepartmentDesc;
        //    Department.OrganizationId = objDepartment.OrganizationId;
        //    Department.CreatedDate = DateTime.Now;
        //    Department.CreatedBy = objDepartment.CreatedBy;
        //    recAffected = Convert.ToInt32(Department.Insert());
        //    if (recAffected > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        public bool UpdateDepartment(Department objDepartment)
        {
            int recAffected = 0;
            DBConnectionString.Department Department = new DBConnectionString.Department();
            Department.DepartmentId = objDepartment.DepartmentId;
            Department.DepartmentName = objDepartment.DepartmentName;
            Department.DepartmentDesc = objDepartment.DepartmentDesc;
            Department.OrganizationId = objDepartment.OrganizationId;
            Department.CreatedDate = objDepartment.CreatedDate;
            Department.CreatedBy = objDepartment.CreatedBy;
            Department.UpdatedBy = objDepartment.UpdatedBy;
            Department.UpdatedDate = DateTime.Now;
            recAffected = Department.Update();
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool DeleteDepartment(long id)
        {
            int recAffectd = 0;
            recAffectd = DBConnectionString.Department.Delete(id);
            if (recAffectd > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}