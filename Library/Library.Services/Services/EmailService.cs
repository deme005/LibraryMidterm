using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Library.Services.Services
{
    public class EmailService : IEmailService
    {
        public EmailService() { }
        public void SeedEmail(string to, string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("dimitrisheklashvili3@gmail.com", "xoiz sljf syir xwsd");
            smtpClient.EnableSsl = true;

            MailMessage mailMessage = new MailMessage("dimitrisheklashvili3@gmail.com", to, subject, body);
            smtpClient.Send(mailMessage);
        }
    }
}
