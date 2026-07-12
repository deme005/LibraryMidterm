using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Interfaces
{
    public interface IFileMeneger
    {
        public List<User> GetAllUsers();

        User GetUserById(int id);
        User GetUserByEmail(string email);
        User GetUserByUsername(string username);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);

        void SaveChanges(List<User> user);
        public User GetLastLoggedInUser();
    }
}
