using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.Text.RegularExpressions;

namespace EmailHandler
{
    public class Mail
    {
        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Content"></param>
        /// <param name="EmailAddress"></param>
        public void SendMail(string from, string Subject, string Content, string EmailAddress)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);// new MailAddress(ConfigurationManager.AppSettings["SupportEmail"]);
            message.To.Add(new MailAddress(EmailAddress));

            message.Subject = Subject;
            message.Body = Content;

            SmtpClient client = new SmtpClient();
            client.Send(message);
        }

        /// <summary>
        /// Send multiple mails, Contacts will be filled in the BCC section 
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Content"></param>
        /// <param name="EmailAddresses">EmailAdress should be filled with contacts seperated by ';'</param>
        /// <param name="isHtmlMail"></param>
        public void SendMailMultipleContacts(string Subject, string Content, string EmailAddresses, bool isHtmlMail)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["SupportEmail"]);

            string[] contacts = EmailAddresses.Split(';');
            foreach (string contact in contacts)
            {
                if (IsValidEmail(contact))
                    message.Bcc.Add(new MailAddress(contact));
            }

            message.Subject = Subject;
            message.Body = Content;
            message.IsBodyHtml = isHtmlMail;

            SmtpClient client = new SmtpClient();
            client.Send(message);
        }

        /// <summary>
        /// e-mail adress is valid
        /// </summary>
        /// <param name="EmailAdrress"></param>
        /// <returns></returns>

        public static bool IsValidEmail(string EmailAdrress)
        {
            return Regex.IsMatch(EmailAdrress, @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
        }
    }
}
