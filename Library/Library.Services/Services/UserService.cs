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
        private readonly IEmailService _emailService;
        private readonly ILoggerService _loggerService;
        private static readonly Random _random = new Random();


        public UserService(IFileMeneger fileManager, IEmailService emailService, ILoggerService loggerService)
        {
            _fileManager = fileManager;
            _emailService = emailService;
            _loggerService = loggerService;
            SeedDefaultAdmin();
        }


        public User LoginUser(string email, string password)
        {
            User user = _fileManager.GetUserByEmail(email);
            if (user != null && !string.IsNullOrWhiteSpace(user.Password))
            {
                if (BCrypt.Net.BCrypt.Verify(password.Trim(), user.Password.Trim()))
                {
                    return user;
                }
            }
            throw new Exception("Invalid email or password");

        }

        public void RegisterUser(string username, string email, string password)
        {
            var users = _fileManager.GetAllUsers();
            
            var existingUser = _fileManager.GetUserByEmail(email);
            if (existingUser != null)
            {
                throw new Exception("A user with this email already exists.");
            }

            int nextId = users.Any() ? users.Max(u => u.ID) + 1 : 1;

            ClientUser user = new ClientUser
            {
                ID = nextId,
                Username = username,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password.Trim()),
            };
            var newUser = user;

            _fileManager.AddUser(newUser);
        }

        public void SendVerificationCode(string email)
        {
            var user = _fileManager.GetUserByEmail(email);
            if (user == null)
            {
                throw new Exception("User with this email does not exist.");
            }

            string freshCode = _random.Next(100000, 999999).ToString();

            user.VerificationCode = freshCode;
            _fileManager.UpdateUser(user);

            _emailService.SendWelcomeVerificationEmail(user.Email, user.Username, freshCode);
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
        public void ChangePassword(string email, string currentPassword, string newPassword, string confirmPassword)
        {
            var user = _fileManager.GetUserByEmail(email);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(currentPassword?.Trim(), user.Password);

            if (!isPasswordCorrect)
            {
                throw new InvalidOperationException("Current password is incorrect.");
            }

            if (newPassword != confirmPassword)
            {
                throw new InvalidOperationException("New password and confirmation password do not match.");
            }

            if (currentPassword == newPassword)
            {
                throw new InvalidOperationException("New password cannot be the same as your current password.");
            }

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                throw new InvalidOperationException("New password must be at least 6 characters long.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword.Trim());
            _fileManager.UpdateUser(user);
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
