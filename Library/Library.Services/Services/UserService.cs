using Library.Domain;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;


namespace Library.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IFileMeneger _fileManager;
        private readonly EmailService _emailService;

        public UserService(IFileMeneger fileManager, EmailService emailService)
        {
            _fileManager = fileManager;
            _emailService = emailService;
            SeedDefaultAdmin();
        }


        public User LoginUser(string email, string password)
        {
            User user = _fileManager.GetUserByEmail(email);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return user;
                }
            }
            throw new Exception("Invalid email or password");

        }

        public void RegisterUser(string username, string email, string password)
        {
            var users = _fileManager.GetAllUsers();
            int nextId = users.Count + 1;

            var existingUser = _fileManager.GetUserByEmail(email);
            if (existingUser != null)
            {
                throw new Exception("A user with this email already exists.");
            }

            ClientUser user = new ClientUser
            {
                ID = nextId,
                Username = username,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
            };
            var newUser = user;

            _fileManager.AddUser(newUser);
        }

        public void SendVerificationCode(string email, string verificationCode)
        {
            _emailService.SeedEmail(email, "Verification Code", verificationCode);
        }

        public bool VerifyUser(string email, string verificationCode)
        {
            var user = _fileManager.GetUserByEmail(email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if(user.VerificationCode == verificationCode)
            {
                user.IsVerified = true;
                _fileManager.UpdateUser(user);
                Console.WriteLine("Verification successful");
                return true;
            }
            Console.WriteLine("Invalid verification code. try again.");
            return false;
        }
        private void SeedDefaultAdmin()
        {
            var users = _fileManager.GetAllUsers();

            if (users == null || !users.Any(u => u.Email == "admin@library.com"))
            {
                var admin = new AdminUser
                {
                    ID = 1000,
                    Username = "admin",
                    Email = "admin@library.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                    Role = Role.Admin,
                    IsVerified = true
                };

                _fileManager.AddUser(admin);
            }
        }
    }
}
