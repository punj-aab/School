﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;

namespace StudentTracker.Core.Utilities
{
    public class EmailManager
    {

        private const string EmailFrom = "library.lakhwant@gmail.com";
        public static void SendConfirmationEmail(string userName)
        {
            var user = Membership.GetUser(userName.ToString());
            var confirmationGuid = user.ProviderUserKey.ToString();
            var verifyUrl = HttpContext.Current.Request.Url.GetLeftPart
               (UriPartial.Authority) + "/Account/Verify/" + confirmationGuid;

            using (var client = new SmtpClient())
            {
                using (var message = new MailMessage(EmailFrom, user.Email))
                {
                    message.Subject = "Verification Email";
                    message.Body = "<html><head><meta content=\"text/html;charset=utf-8\" /></head><body><p>Dear " + user.UserName +
                       ", </p><p>To verify your account, please click the following link:</p>"
                       + "<p><a href=\"" + verifyUrl + "\" target=\"_blank\">Click here" 
                       + "</a></p><div>Best regards,</div><div>Student Admin</div><p>Do not forward "
                       + "this email. The verify link is private.</p></body></html>";

                    message.IsBodyHtml = true;

                    client.EnableSsl = true;
                    client.Send(message);
                };
            };
        }
    }

}