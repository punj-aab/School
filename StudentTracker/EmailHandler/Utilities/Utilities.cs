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
    }
}
