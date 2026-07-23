using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Services.Interfaces
{
    public interface IEmailService
    {
        void SeedEmail(string to, string subject, string body);
        string GetWelcomeEmailTemplate(string username, string verificationCode);
        void SendWelcomeVerificationEmail(string toEmail, string username, string verificationCode);
    }
}
