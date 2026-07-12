using Library.Domain;
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
        
        public UserService(IFileMeneger fileManager)
        {
            _fileManager = fileManager;
        }

        
        public User LoginUser(string email, string password)
        {
            User user = _fileManager.GetUserByEmail(email);
            if(user != null)
            {
                if(BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return user;
                }
            }
            throw new Exception("Invalid email or password or not varified");

        }

        public void RegisterUser(string username, string email, string password)
        {
            var users = _fileManager.GetAllUsers();
            int nextId = users.Count + 1;

            var existingUser = _fileManager.GetUserByEmail(email);
            if(existingUser != null)
            {
                throw new Exception("A user with this email already exists.");
            }

            ClientUser user = new()
            {
                Id = nextId,
                Username = username,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
            };
            var newUser = user;

            _fileManager.AddUser(newUser);
        }

        public void SendVerificationCode(string email, string verificationCode)
        {
            throw new NotImplementedException();
        }
    }
}
