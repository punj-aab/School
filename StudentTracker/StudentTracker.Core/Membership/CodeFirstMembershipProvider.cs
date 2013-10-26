using StudentTracker.Core.DAL;
using StudentTracker.Core.Entities;
using StudentTracker.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

public class CodeFirstMembershipProvider : MembershipProvider
{

    #region Properties

    private const int TokenSizeInBytes = 16;

    public override string ApplicationName
    {
        get
        {
            return this.GetType().Assembly.GetName().Name.ToString();
        }
        set
        {
            this.ApplicationName = this.GetType().Assembly.GetName().Name.ToString();
        }
    }

    public override int MaxInvalidPasswordAttempts
    {
        get { return 5; }
    }

    public override int MinRequiredNonAlphanumericCharacters
    {
        get { return 0; }
    }

    public override int MinRequiredPasswordLength
    {
        get { return 6; }
    }

    public override int PasswordAttemptWindow
    {
        get { return 0; }
    }

    public override MembershipPasswordFormat PasswordFormat
    {
        get { return MembershipPasswordFormat.Hashed; }
    }

    public override string PasswordStrengthRegularExpression
    {
        get { return String.Empty; }
    }

    public override bool RequiresUniqueEmail
    {
        get { return true; }
    }

    #endregion

    #region Functions

    public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
    {
        if (string.IsNullOrEmpty(username))
        {
            status = MembershipCreateStatus.InvalidUserName;
            return null;
        }
        if (string.IsNullOrEmpty(password))
        {
            status = MembershipCreateStatus.InvalidPassword;
            return null;
        }
        if (string.IsNullOrEmpty(email))
        {
            status = MembershipCreateStatus.InvalidEmail;
            return null;
        }

        string HashedPassword = Crypto.HashPassword(password);
        if (HashedPassword.Length > 128)
        {
            status = MembershipCreateStatus.InvalidPassword;
            return null;
        }

        using (StudentContext Context = new StudentContext())
        {
            if (Context.Users.Where(Usr => Usr.Username == username).Any())
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            if (Context.Users.Where(Usr => Usr.Email == email).Any())
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            User NewUser = new User
            {
                Username = username,
                Password = HashedPassword,
                StatusId = Convert.ToInt32(UserStatus.Active),
                Email = email,
                InsertedOn = DateTime.UtcNow,
                LastPasswordChangedDate = DateTime.UtcNow,
                PasswordFailuresSinceLastSuccess = 0,
                LastLoginDate = DateTime.UtcNow,
                LastActivityDate = DateTime.UtcNow,
                LastLockoutDate = DateTime.UtcNow,
                IsLockedOut = false,
                LastPasswordFailureDate = DateTime.UtcNow
            };
            NewUser.ConfirmPassword = HashedPassword;
            try
            {
                Context.Users.Add(NewUser);
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            status = MembershipCreateStatus.Success;
            return new MembershipUser(Membership.Provider.Name, NewUser.Username, NewUser.UserId, NewUser.Email, null, null, NewUser.StatusId == 1, NewUser.IsLockedOut, NewUser.InsertedOn, NewUser.LastLoginDate.Value, NewUser.LastActivityDate.Value, NewUser.LastPasswordChangedDate.Value, NewUser.LastLockoutDate.Value);
        }
    }


    public string CreateUserAndAccount(string userName, string password, bool requireConfirmation, IDictionary<string, object> values)
    {
        return CreateAccount(userName, password, requireConfirmation);
    }

    public override bool ValidateUser(string username, string password)
    {
        if (string.IsNullOrEmpty(username))
        {
            return false;
        }
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }
        using (StudentContext Context = new StudentContext())
        {
            User User = null;
            User = Context.Users.FirstOrDefault(Usr => Usr.Username == username);
            if (User == null)
            {
                return false;
            }
            if (User.StatusId != (int)UserStatus.Active)
            {
                return false;
            }
            if (User.IsLockedOut)
            {
                return false;
            }
            String HashedPassword = User.Password;
            Boolean VerificationSucceeded = (HashedPassword != null && Crypto.VerifyHashedPassword(HashedPassword, password));
            if (VerificationSucceeded)
            {
                User.PasswordFailuresSinceLastSuccess = 0;
                User.LastLoginDate = DateTime.UtcNow;
                User.LastActivityDate = DateTime.UtcNow;
            }
            else
            {
                int Failures = User.PasswordFailuresSinceLastSuccess;
                if (Failures < MaxInvalidPasswordAttempts)
                {
                    User.PasswordFailuresSinceLastSuccess += 1;
                    User.LastPasswordFailureDate = DateTime.UtcNow;
                }
                else if (Failures >= MaxInvalidPasswordAttempts)
                {
                    User.LastPasswordFailureDate = DateTime.UtcNow;
                    User.LastLockoutDate = DateTime.UtcNow;
                    User.IsLockedOut = true;
                }
            }
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges
                User.ConfirmPassword = User.Password;
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            if (VerificationSucceeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public override MembershipUser GetUser(string username, bool userIsOnline)
    {
        if (string.IsNullOrEmpty(username))
        {
            return null;
        }
        using (StudentContext Context = new StudentContext())
        {
            User User = null;
            User = Context.Users.FirstOrDefault(Usr => Usr.Username == username);
            if (User != null)
            {
                if (userIsOnline)
                {
                    User.LastActivityDate = DateTime.UtcNow;
                    User.ConfirmPassword = User.Password;
                    Context.SaveChanges();
                }
                return new MembershipUser(Membership.Provider.Name, User.Username, User.UserId, User.Email, null, null, User.StatusId == (int)UserStatus.Active, User.IsLockedOut, User.InsertedOn, User.LastLoginDate.Value, User.LastActivityDate.Value, User.LastPasswordChangedDate.Value, User.LastLockoutDate.Value);
            }
            else
            {
                return null;
            }
        }
    }

    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    {
        if (providerUserKey is long) { }
        else
        {
            return null;
        }

        using (StudentContext Context = new StudentContext())
        {
            User User = null;
            User = Context.Users.Find(providerUserKey);
            if (User != null)
            {
                if (userIsOnline)
                {
                    User.LastActivityDate = DateTime.UtcNow;
                    Context.SaveChanges();
                }
                return new MembershipUser(Membership.Provider.Name, User.Username, User.UserId, User.Email, null, null, User.StatusId == (int)UserStatus.Active, User.IsLockedOut, User.InsertedOn, User.LastLoginDate.Value, User.LastActivityDate.Value, User.LastPasswordChangedDate.Value, User.LastLockoutDate.Value);
            }
            else
            {
                return null;
            }
        }
    }

    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
        if (string.IsNullOrEmpty(username))
        {
            return false;
        }
        if (string.IsNullOrEmpty(oldPassword))
        {
            return false;
        }
        if (string.IsNullOrEmpty(newPassword))
        {
            return false;
        }
        using (StudentContext Context = new StudentContext())
        {
            User User = null;
            User = Context.Users.FirstOrDefault(Usr => Usr.Username == username);
            if (User == null)
            {
                return false;
            }
            String HashedPassword = User.Password;
            Boolean VerificationSucceeded = (HashedPassword != null && Crypto.VerifyHashedPassword(HashedPassword, oldPassword));
            if (VerificationSucceeded)
            {
                User.PasswordFailuresSinceLastSuccess = 0;
            }
            else
            {
                int Failures = User.PasswordFailuresSinceLastSuccess;
                if (Failures < MaxInvalidPasswordAttempts)
                {
                    User.PasswordFailuresSinceLastSuccess += 1;
                    User.LastPasswordFailureDate = DateTime.UtcNow;
                }
                else if (Failures >= MaxInvalidPasswordAttempts)
                {
                    User.LastPasswordFailureDate = DateTime.UtcNow;
                    User.LastLockoutDate = DateTime.UtcNow;
                    User.IsLockedOut = true;
                }
                Context.SaveChanges();
                return false;
            }
            String NewHashedPassword = Crypto.HashPassword(newPassword);
            if (NewHashedPassword.Length > 128)
            {
                return false;
            }
            User.Password = NewHashedPassword;
            User.ConfirmPassword = NewHashedPassword;
            User.LastPasswordChangedDate = DateTime.UtcNow;
            Context.SaveChanges();
            return true;
        }
    }

    public override bool UnlockUser(string userName)
    {
        using (StudentContext Context = new StudentContext())
        {
            User User = null;
            User = Context.Users.FirstOrDefault(Usr => Usr.Username == userName);
            if (User != null)
            {
                User.IsLockedOut = false;
                User.PasswordFailuresSinceLastSuccess = 0;
                Context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public override int GetNumberOfUsersOnline()
    {
        DateTime DateActive = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(Convert.ToDouble(Membership.UserIsOnlineTimeWindow)));
        using (StudentContext Context = new StudentContext())
        {
            return Context.Users.Where(Usr => Usr.LastActivityDate > DateActive).Count();
        }
    }

    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    {
        if (string.IsNullOrEmpty(username))
        {
            return false;
        }
        using (StudentContext Context = new StudentContext())
        {
            User User = null;
            User = Context.Users.FirstOrDefault(Usr => Usr.Username == username);
            if (User != null)
            {
                Context.Users.Remove(User);
                Context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public override string GetUserNameByEmail(string email)
    {
        using (StudentContext Context = new StudentContext())
        {
            User User = null;
            User = Context.Users.FirstOrDefault(Usr => Usr.Email == email);
            if (User != null)
            {
                return User.Username;
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        MembershipUserCollection MembershipUsers = new MembershipUserCollection();
        using (StudentContext Context = new StudentContext())
        {
            totalRecords = Context.Users.Where(Usr => Usr.Email == emailToMatch).Count();
            IQueryable<User> Users = Context.Users.Where(Usr => Usr.Email == emailToMatch).OrderBy(Usrn => Usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);
            foreach (User user in Users)
            {
                MembershipUsers.Add(new MembershipUser(Membership.Provider.Name, user.Username, user.UserId, user.Email, null, null, user.StatusId == (int)UserStatus.Active, user.IsLockedOut, user.InsertedOn, user.LastLoginDate.Value, user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
            }
        }
        return MembershipUsers;
    }

    public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        MembershipUserCollection MembershipUsers = new MembershipUserCollection();
        using (StudentContext Context = new StudentContext())
        {
            totalRecords = Context.Users.Where(Usr => Usr.Username == usernameToMatch).Count();
            IQueryable<User> Users = Context.Users.Where(Usr => Usr.Username == usernameToMatch).OrderBy(Usrn => Usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);
            foreach (User user in Users)
            {
                MembershipUsers.Add(new MembershipUser(Membership.Provider.Name, user.Username, user.UserId, user.Email, null, null, user.StatusId == (int)UserStatus.Active, user.IsLockedOut, user.InsertedOn, user.LastLoginDate.Value, user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
            }
        }
        return MembershipUsers;
    }

    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
    {
        MembershipUserCollection MembershipUsers = new MembershipUserCollection();
        using (StudentContext Context = new StudentContext())
        {
            totalRecords = Context.Users.Count();
            IQueryable<User> Users = Context.Users.OrderBy(Usrn => Usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);
            foreach (User user in Users)
            {
                MembershipUsers.Add(new MembershipUser(Membership.Provider.Name, user.Username, user.UserId, user.Email, null, null, user.StatusId == (int)UserStatus.Active, user.IsLockedOut, user.InsertedOn, user.LastLoginDate.Value, user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
            }
        }
        return MembershipUsers;
    }

    public string CreateAccount(string userName, string password, bool requireConfirmationToken)
    {

        if (string.IsNullOrEmpty(userName))
        {
            throw new MembershipCreateUserException(MembershipCreateStatus.InvalidUserName);
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);
        }

        string hashedPassword = Crypto.HashPassword(password);
        if (hashedPassword.Length > 128)
        {
            throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);
        }

        using (StudentContext Context = new StudentContext())
        {
            if (Context.Users.Where(Usr => Usr.Username == userName).Any())
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateUserName);
            }

            string token = string.Empty;
            if (requireConfirmationToken)
            {
                token = GenerateToken();
            }

            User NewUser = new User
            {
                Username = userName,
                Password = hashedPassword,
                StatusId = (int)UserStatus.Pending,
                Email = string.Empty,
                InsertedOn = DateTime.UtcNow,
                LastPasswordChangedDate = DateTime.UtcNow,
                PasswordFailuresSinceLastSuccess = 0,
                LastLoginDate = DateTime.UtcNow,
                LastActivityDate = DateTime.UtcNow,
                LastLockoutDate = DateTime.UtcNow,
                IsLockedOut = false,
                LastPasswordFailureDate = DateTime.UtcNow,
                ConfirmationToken = token
            };

            Context.Users.Add(NewUser);
            Context.SaveChanges();
            return token;
        }

    }
    public string CreateAccount(User user)
    {

        if (string.IsNullOrEmpty(user.Username))
        {
            throw new MembershipCreateUserException(MembershipCreateStatus.InvalidUserName);
        }

        if (string.IsNullOrEmpty(user.Password))
        {
            throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);
        }

        if (string.IsNullOrEmpty(user.Email))
        {
            throw new MembershipCreateUserException(MembershipCreateStatus.InvalidEmail);
        }

        string hashedPassword = Crypto.HashPassword(user.Password);
        if (hashedPassword.Length > 128)
        {
            throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);
        }

        using (StudentContext Context = new StudentContext())
        {
            if (Context.Users.Where(Usr => Usr.Username == user.Username).Any())
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateUserName);
            }

            if (Context.Users.Where(Usr => Usr.Email == user.Email).Any())
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateEmail);

            }

            string token = string.Empty;

            token = GenerateToken();


            User NewUser = new User
            {
                Username = user.Username,
                Password = hashedPassword,
                StatusId = (int)UserStatus.Pending,
                Email = user.Email,
                InsertedOn = DateTime.UtcNow,
                LastPasswordChangedDate = DateTime.UtcNow,
                PasswordFailuresSinceLastSuccess = 0,
                LastLoginDate = DateTime.UtcNow,
                LastActivityDate = DateTime.UtcNow,
                LastLockoutDate = DateTime.UtcNow,
                IsLockedOut = false,
                LastPasswordFailureDate = DateTime.UtcNow,
                ConfirmationToken = token,
                RegistrationToken = token,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OrgainzationId = user.OrgainzationId
            };
            NewUser.ConfirmPassword = hashedPassword;
            Context.Users.Add(NewUser);
            Context.SaveChanges();
            return token;
        }

    }


    private static string GenerateToken()
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

    #endregion

    #region Not Supported

    //CodeFirstMembershipProvider does not support password retrieval scenarios.
    public override bool EnablePasswordRetrieval
    {
        get { return false; }
    }
    public override string GetPassword(string username, string answer)
    {
        throw new NotSupportedException("Consider using methods from WebSecurity module.");
    }

    //CodeFirstMembershipProvider does not support password reset scenarios.
    public override bool EnablePasswordReset
    {
        get { return true; }
    }
    public override string ResetPassword(string username, string answer)
    {
        string pass = string.Empty;
        using (StudentContext Context = new StudentContext())
        {

            var currentUser = Context.Users.Where(u => u.Username == username).SingleOrDefault();

            Random rdm = new Random(9978787);
            pass = rdm.Next().ToString();
            string hashedPass = Crypto.HashPassword(pass);
            currentUser.Password = hashedPass;
            currentUser.ConfirmPassword = hashedPass;
            Context.SaveChanges();
        }
        return pass;
    }

    //CodeFirstMembershipProvider does not support question and answer scenarios.
    public override bool RequiresQuestionAndAnswer
    {
        get { return false; }
    }
    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
    {
        throw new NotSupportedException("Consider using methods from WebSecurity module.");
    }

    //CodeFirstMembershipProvider does not support UpdateUser because this method is useless.
    public override void UpdateUser(MembershipUser user)
    {
        using (StudentContext Context = new StudentContext())
        {
            Int64 userId = Convert.ToInt64(user.ProviderUserKey);
            var currentUser = Context.Users.Where(u => u.UserId == userId).SingleOrDefault();
            currentUser.StatusId = (int)UserStatus.Active;
            currentUser.ConfirmPassword = currentUser.Password;
            Context.SaveChanges();
        }
    }

    #endregion
}