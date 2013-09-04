using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Organization
/// </summary>
public class Organization
{
    public int OrganizationId { get; set; }
    public string OrganizationName { get; set; }
    public int OrganizationTypeId { get; set; }
    public int Country { get; set; }
    public int State { get; set; }
    public string City { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Email { get; set; }
    public string Phone1 { get; set; }
    public string Phone2 { get; set; }
    public Int64 CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Int64 ModifiedBY { get; set; }
    public DateTime ModifiedDate { get; set; }
    public Int64 Deletedby { get; set; }
    public DateTime DeletedDate { get; set; }
    public int StatusId { get; set; }
    public string OrganizationDesc { get; set; }
}