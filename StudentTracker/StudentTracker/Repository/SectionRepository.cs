using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentTracker.Repository
{
    public class SectionRepository : StudentTracker.Core.Repository.CommonRepository
    {
        PetaPoco.Database db = new PetaPoco.Database("DBConnectionString");
        //public List<Section> GetSections()
        //{
        //    string sql = "select SectionId,SectionName,SectionDescription,Sections.ClassId,Sections.InsertedOn,CreatedBy,Sections.ModifiedBy,Sections.ModifiedOn,Users.Username as InsertedByName,Users_1.Username as ModifiedByName, Classes.ClassName  ";
        //    sql += "from Sections join Classes on Sections.ClassId=Classes.ClassId join Users on Sections.CreatedBy=Users.UserId left join Users as Users_1 on  Sections.ModifiedBy=Users_1.UserId";
        //    List<Section> modelList = db.Query<Section>(sql).ToList();
        //    return modelList;
        //}


        //public Section GetSections(long id)
        //{
        //    string sql = "select SectionId,SectionName,SectionDescription,Sections.ClassId,Sections.InsertedOn,CreatedBy,Sections.ModifiedBy,Sections.ModifiedOn,Users.Username as InsertedByName,Users_1.Username as ModifiedByName, Classes.ClassName ";
        //    sql += "from Sections join Classes on Sections.ClassId=Classes.ClassId join Users on Sections.CreatedBy=Users.UserId left join Users as Users_1 on  Sections.ModifiedBy=Users_1.UserId ";
        //    sql += "where SectionId=@0";
        //    Section objModel = db.Query<Section>(sql, id).FirstOrDefault();
        //    return objModel;
        //}
        public bool Create(Section objSection)
        {
            int recAffected = 0;
            DBConnectionString.Section Section = new DBConnectionString.Section();
            Section.SectionName = objSection.SectionName;
            Section.SectionDescription = objSection.SectionDescription;
            Section.ClassId = objSection.ClassId;
            Section.CreatedBy = objSection.CreatedBy;
            Section.InsertedOn = DateTime.Now;
            recAffected = Convert.ToInt32(Section.Insert());
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool Update(Section objSection)
        {
            int recAffected = 0;
            DBConnectionString.Section Section = new DBConnectionString.Section();
            Section.SectionId = objSection.SectionId;
            Section.SectionName = objSection.SectionName;
            Section.SectionDescription = objSection.SectionDescription;
            Section.ClassId = objSection.ClassId;
            Section.CreatedBy = objSection.CreatedBy;
            Section.InsertedOn = objSection.InsertedOn;
            Section.ModifiedBy = objSection.ModifiedBy;
            Section.ModifiedOn = DateTime.Now;
            recAffected = Section.Update();
            if (recAffected > 0)
            {
                return true;
            }
            return false;
        }
        public bool Delete(long id)
        {
            int recAffectd = 0;
            recAffectd = DBConnectionString.Section.Delete(id);
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