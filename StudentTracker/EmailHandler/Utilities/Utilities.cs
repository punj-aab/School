// -----------------------------------------------------------------------
// <copyright file="Utilities.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EmailHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net.Mail;
    using System.Net;
    using EmailHandler.MQ;
    using System.Web.Security;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Utilities
    {
        public static void SendMailToQueue(Email email, string queuePath)
        {
            if (email != null)
            {
                var EmailQueue = new MessageQueueSender(queuePath);

                EmailQueue.SendMessage(email);
            }
        }

        public static void SendMailsToQueue(Email[] emails, string queuePath)
        {
            if (emails.Count() != 0)
            {
                var EmailQueue = new MessageQueueSender(queuePath);
                foreach (Email email in emails)
                {
                    EmailQueue.SendMessage(email);
                }
            }
        }

        public static bool SendMailThruGmail(Email email)
        {
            try
            {
                MailAddress fromAddress = new MailAddress(email.mailFrom);
                MailAddress toAddress = new MailAddress(email.mailTo);
                string subject = email.mailSubject;
                string body = email.mailBody;

                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(fromAddress.Address, toAddress.Address, subject, body);
                msg.IsBodyHtml = true;

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("lakhwant.enest@gmail.com", "L@KHA@123"),
                    EnableSsl = true
                };

                client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private const string EmailFrom = "lakhwant.enest@gmail.com";
        public static void SendConfirmationEmail(string userName)
        {
            var user = Membership.GetUser(userName.ToString());
            var confirmationGuid = user.ProviderUserKey.ToString();
            var verifyUrl = HttpContext.Current.Request.Url.GetLeftPart
               (UriPartial.Authority) + "/Account/Verify/" + confirmationGuid;

            MailAddress fromAddress = new MailAddress(EmailFrom);
            MailAddress toAddress = new MailAddress(user.Email);
            string subject = "Please Verify your Account";
            string body = "<html><head><meta content=\"text/html;charset=utf-8\" /></head><body><p>Dear " + user.UserName +
                       ", </p><p>To verify your account, please click the following link:</p>"
                       + "<p><a href=\"" + verifyUrl + "\" target=\"_blank\">" + verifyUrl
                       + "</a></p><div>Best regards,</div><div>Someone</div><p>Do not forward "
                       + "this email. The verify link is private.</p></body></html>";

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(fromAddress.Address, toAddress.Address, subject, body);
            msg.IsBodyHtml = true;

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("lakhwant.enest@gmail.com", "L@KHA@123"),
                EnableSsl = true
            };

            client.Send(msg);
        }
    }
}
