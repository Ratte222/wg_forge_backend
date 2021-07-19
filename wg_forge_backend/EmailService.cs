using System;
using System.Collections.Generic;
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
        //string _mail;
        //string _password;
        //public async Task SendEmailAsync(string email, string subject, string message)
        //{
        //    var emailMessage = new MimeMessage();

        //    emailMessage.From.Add(new MailboxAddress("Администрация сайта", "admin@metanit.com"));
        //    emailMessage.To.Add(new MailboxAddress("", email));
        //    emailMessage.Subject = subject;
        //    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        //    {
        //        Text = message
        //    };

        //    using (var client = new SmtpClient())
        //    {
        //        await client.ConnectAsync("smtp.metanit.com", 465, true);
        //        await client.AuthenticateAsync("admin@metanit.com", "password");
        //        await client.SendAsync(emailMessage);

        //        await client.DisconnectAsync(true);
        //    }
        //}

        //public EmailService(string email, string password)
        //{
        //    _mail = email;
        //    _password = password;
        //}

        public bool SendEmail(string _EMailAddresTo, string _Subject, string _Body)
        {
            // sender - set the address and the name displayed in the letter 
            MailAddress from = new MailAddress(Email, "CatTeam");
            // who are we sending 
            MailAddress to = new MailAddress(_EMailAddresTo);
            // create a message object 
            MailMessage m = new MailMessage(from, to);
            // letter subject
            m.Subject = _Subject;
            // text of the letter
            //m.Body = "<h2>Письмо-тест работы smtp-клиента</h2>";
            m.Body = _Body;
            // letter represents html code 
            m.IsBodyHtml = true;
            // the address of the smtp server and the port from which we will send the letter 
            // "smtp.gmail.com", 587
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // loggin and password
            smtp.Credentials = new NetworkCredential(Email, Password);

            smtp.EnableSsl = true;
            smtp.Send(m);
            //smtp.SendCompleted += Smtp_SendCompleted;
            m.Dispose();
            smtp.Dispose();

            //Console.Read();
            return true;
        }
    }

    //public class MailAddressModel
    //{
    //    public string Email { get; set; }
    //    public string Password { get; set; }
    //}
}
