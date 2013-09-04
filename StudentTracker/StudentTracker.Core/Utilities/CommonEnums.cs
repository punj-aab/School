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
}
