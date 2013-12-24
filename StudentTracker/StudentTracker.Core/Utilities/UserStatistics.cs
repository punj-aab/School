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

        private List<Service> servciesForThisOrganization = new List<Service>();

        public UserStatistics(HttpContextBase context)
        {
            _context = context;
            StudentContext db = new StudentContext();
            User user =null;
            if (_context.Session["User"] != null)
            {
                user = _context.Session["User"] as User;
            }
            else
            {
                if (!string.IsNullOrEmpty(context.User.Identity.Name))
                {
                    user = db.Users.Where(u => u.Username == context.User.Identity.Name).FirstOrDefault();
                    _context.Session["User"] = user;
                }
            }
            if (user != null)
            {
                userId = user.UserId;
                organizationId = user.OrganizationId;

                var services = db.OrganizationServices.Where(os => os.OrganizationId == organizationId && os.StatusId == 1).Join(db.Services, os => os.ServiceId, s => s.ServiceId, (os, s) => new { s.ServiceName, s.ServiceId, os.OrganizationId }).ToList();
                servciesForThisOrganization = (from s in services
                                               select new Service
                                               {
                                                   ServiceId = s.ServiceId,
                                                   ServiceName = s.ServiceName
                                               }).ToList();
            }
        }

        public List<Service> Services
        {
            get
            {
               return this.servciesForThisOrganization;
            }
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
