using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace StudentTracker.Core.Utilities
{
    public class UserStatistics
    {
        private const int TokenSizeInBytes = 16;
        private readonly HttpContextBase _context;
        private long userId;

        private long organizationId;

        private string course;

        private string department;

        private string userClass;


        public UserStatistics(HttpContextBase context)
        {
            _context = context;
            StudentContext db = new StudentContext();
            User user = new User();
            if (_context.Session["User"] != null)
            {
                user = _context.Session["User"] as User;
            }
            else
            {
                user = db.Users.Where(u => u.Username == context.User.Identity.Name).FirstOrDefault();
                _context.Session["User"] = user;
            }
            userId = user.UserId;
            organizationId = user.OrgainzationId;
        }

        public long UserId
        {
            get
            {
                return this.userId;
            }
        }

        public long OrganizationId
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

        public static string GenerateToken()
        {
            using (var prng = new RNGCryptoServiceProvider())
            {
                return GenerateToken(prng);
            }
        }

        internal static string GenerateToken(RandomNumberGenerator generator)
        {
            byte[] tokenBytes = new byte[TokenSizeInBytes];
            generator.GetBytes(tokenBytes);
            return HttpServerUtility.UrlTokenEncode(tokenBytes);
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
