using StudentTracker.Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace StudentTracker.Core.Utilities
{
    public class UserStatistics
    {
        private readonly HttpContextBase _context;
        private long userId;

        private int organizationId;

        private string course;

        private string department;

        private string userClass;


        public UserStatistics(HttpContextBase context)
        {
            _context = context;
            if (_context.Session["UserId"] != null)
            {
                userId = Convert.ToInt64(_context.Session["UserId"]);
            }
            else
            {
                var user = WebSecurity.GetUser(context.User.Identity.Name);
                if (user != null)
                {
                    userId = (long)user.ProviderUserKey;
                }
            }
        }



        public  long UserId
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

        public string UserCourse
        {
            get
            {
                return this.course;
            }
        }

        public string UserDepartment
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

        public static string GetToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = new Guid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return token;
        }

        public static bool CheckTokenStatus(string token)
        {
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddHours(-24))
            {
                return false;
            }
            return true;
        }
    }
}
