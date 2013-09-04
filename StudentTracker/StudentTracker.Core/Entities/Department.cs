using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Department
/// </summary>
public class Department
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public string DepartmentDesc { get; set; }
    public int OrganizationId { get; set; }
    public DateTime CreatedDate { get; set; }
    public long CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    public long UpdatedBy { get; set; }
    public DateTime DeletedDate { get; set; }
    public long DeletedBy { get; set; }

}