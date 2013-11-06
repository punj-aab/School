using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTracker.Core.Utilities
{
    class CommonEnums
    {
    }

    public enum UserStatus
    {
        Active = 1,
        Disabled = 2,
        Pending = 3,
        NotApproved = 4,
        NotVerified = 5
    }

    public enum OrganizationTypes
    {
        College = 1,
        School = 2,
        University = 3,
        Institute = 4,
        Unknown = 5
    }

    public enum Days
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7
    }

    public enum UserRoles
    {
        OrganizationAdmin = 1,
        SiteAdmin = 2,
        Student = 3,
        Teacher = 4,
        OrganizationDirector = 5,
        OtherStaff = 6

    }

    public enum NotificationTypes
    {
        SMS = 1,
        Email = 2
    }
}
