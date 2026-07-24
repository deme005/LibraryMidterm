using Library.Domain;
using Library.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using Library.Domain.Models;
using System.Text.Json.Nodes;
using Library.Repository.Repositories.Helper;

namespace Library.Repository.Repositories
{
    public class UserRepositories : IFileMeneger
    {
        protected string FileName = "data.txt";
        // protected string filePath => @"C:\Users\deme\Desktop\‏\codes\doit_midterm\LibraryMidterm\Library\Library.Repository\Data\" + FileName;
        protected string filePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Library.Repository\Data", FileName);
        //private readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.txt");

        public UserRepositories()
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "");
            }
        }

        public void AddUser(User user)
        {
            if (user == null) return;
            
            string line = JsonSerializer.Serialize(user, user.GetType());
            File.AppendAllText(filePath, line + Environment.NewLine);
        }

        public void DeleteUser(int id)
        {
            List<User> users = GetAllUsers();
            List<User> remainingUsers = users.Where(s => s.ID != id).ToList();
            SaveChanges(remainingUsers);
        }

        public User GetUserByEmail(string email)
        {
            List<User> users = GetAllUsers();
            return users.FirstOrDefault(s => s.Email != null && s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public User GetUserByUsername(string username)
        {
            List<User> users = GetAllUsers();
            return users.FirstOrDefault(s => s.Username != null && s.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public User GetUserById(int id)
        {
            List<User> users = GetAllUsers();
            return users.FirstOrDefault(s => s.ID == id);
        }

        public void SaveChanges(List<User> users)
        {
            File.Delete(filePath);
            File.AppendAllLines(filePath, users.Select(s => JsonSerializer.Serialize(s, s.GetType())));
        }

        public void UpdateUser(User user)
        {
            List<User> users = GetAllUsers();
            int index = users.FindIndex(s => s.ID == user.ID);
            if (index != -1)
            {
                users[index] = user;
                SaveChanges(users);
            }
        }

        public List<User> GetAllUsers()
        {
            if (!File.Exists(filePath))
            {
                return new List<User>();
            }

            string[] lines = File.ReadAllLines(filePath);
            List<User> users = new List<User>();

            foreach (var item in lines)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;

                try
                {
                    var jsonNode = JsonNode.Parse(item);
                    if (jsonNode == null) continue;

                    int roleVal = jsonNode["Role"]?.GetValue<int>() ?? -1;
                    if (roleVal != null)
                    {
                        string roleStr = roleVal.ToString();

                        if (roleStr == "Admin" || roleStr == "1")
                        {
                            AdminUser admin = JsonSerializer.Deserialize<AdminUser>(item);
                            if (admin != null) users.Add(admin);
                        }
                        else
                        {
                            ClientUser client = JsonSerializer.Deserialize<ClientUser>(item);
                            if (client != null) users.Add(client);
                        }
                    }
                }
                catch (JsonException)
                {
                    continue;
                }
            }
            return users;
        }
    }
}
