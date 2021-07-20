using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace wg_forge_backend
{
    public class EmailService
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public void SendEmail(string email, string subject, string message, bool isHtml = true)
        {
            var from = new MailAddress(Email, "CatTeam");
            var to = new MailAddress(email);
            var msg = new MailMessage(from, to)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = isHtml
            };
            using(SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(Email, Password);
                smtp.EnableSsl = true;
                smtp.Send(msg);
            }
        }

        public void SendConfirmationEmail(string email, string header, string href)
        {
            var current = Path.Combine(Directory.GetCurrentDirectory(),
                    "template", "Confirm_Account_Registration.html");
            var pathToFile = current;
            //var builder = new BodyBuilder();
            string htmlBody;
            using(StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                htmlBody = SourceReader.ReadToEnd();
            }
            string messageBody = string.Format(htmlBody,
                header,
                String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                email, href);
            SendEmail(email, "Confirm", messageBody);
        }

       
    }    
}
