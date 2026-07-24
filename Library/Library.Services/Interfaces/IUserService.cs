using Library.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Services.Interfaces
{
    public interface IUserService
    {
        void RegisterUser(string username, string email, string password);

        User LoginUser(string email, string password);

        void SendVerificationCode(string email);

        bool VerifyUser(string email, string verificationCode);
        void ChangePassword(string email, string currentPassword, string newPassword, string confirmPassword);
    }
}
