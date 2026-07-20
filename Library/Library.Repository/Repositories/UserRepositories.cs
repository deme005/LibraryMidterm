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
        protected string FileName = "users.txt";

        protected string filePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", FileName);


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
            string line = JsonSerializer.Serialize(user);
            File.AppendAllText(filePath, line + Environment.NewLine);
        }

        public void DeleteUser(int id)
        {
            List<User> users = GetAllUsers();
            List<User> remainingUsers = users.Where(s => s.Id != id).ToList();
            SaveChanges(remainingUsers);
        }

        

        public User GetUserByEmail(string email)
        {
            List<User> users = GetAllUsers();
            User user = users.FirstOrDefault(s => s.Email == email);
            return user;
        }
        public User GetUserByUsername(string username)
        {
            List<User> users = GetAllUsers();
            User user = users.FirstOrDefault(s => s.Username == username);
            return user;
        }

        public User GetUserById(int id)
        {
            List<User> users = GetAllUsers();
            User user = users.FirstOrDefault(s => s.Id == id);
            return user;
        }


        public void SaveChanges(List<User> users)
        {
            File.Delete(filePath);
            File.AppendAllLines(filePath, users.Select(s => JsonSerializer.Serialize(s, s.GetType())));
        }

        public void UpdateUser(User user)
        {
            List<User> users = GetAllUsers();
            int index = users.FindIndex(s => s.Id == user.Id);
            if(index != -1)
            {
                users[index] = user;
            }
            SaveChanges(users);
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
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                try
                {
                    var jsonNode = JsonNode.Parse(item);
                    if (jsonNode == null) continue;

                    var roleNode = jsonNode["Role"];
                    if (roleNode != null)
                    {
                        string roleStr = roleNode.ToString();

                        if (roleStr == "Admin" || roleStr == "1")
                        {
                            AdminUser admin = JsonSerializer.Deserialize<AdminUser>(item);
                            if (admin != null)
                            {
                                users.Add(admin);
                            }
                        }
                        else
                        {

                            ClientUser user = JsonSerializer.Deserialize<ClientUser>(item);
                            if (user != null)
                            {
                                users.Add(user);
                            }
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
