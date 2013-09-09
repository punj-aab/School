using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentTracker.Core.Utilities
{
    public class UserStatistics
    {
        private long userId;

        private int organizationId;

        private string course;

        private string department;

        private string userClass;

        public long UserId
        {
            get
            {
                return this.userId;
            }
        }

        public int OrganizationId
        {
            get
            {
                return this.organizationId;
            }
        }

        public string Course
        {
            get
            {
                return this.course;
            }
        }

        public string Department
        {
            get
            {
                return this.department;
            }
        }

        public string UserClass
        {
            get
            {
                return this.userClass;
            }
        }

    }
}
