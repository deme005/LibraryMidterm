using Library.Domain;
using Library.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using Library.Domain.Models;
using System.Text.Json.Nodes;

namespace Library.Repository.Repositories
{
    public class Repositories : IFileMeneger
    {
        private readonly string filePath = "C:\\Users\\user\\OneDrive\\Desktop\\demes_doit_midterm\\LibraryMidterm\\Library\\Library.Repository\\Data\\data.txt";
        public void AddUser(User user)
        {
            string line = JsonSerializer.Serialize(user);
            File.AppendAllText(filePath, line);
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public User GetLastLoggedInUser()
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            List<User> users = GetAllUsers();
            User user = users.FirstOrDefault(s => s.Email == email);
            return user;
        }

        public User GetUserById(int id)
        {
            List<User> users = GetAllUsers();
            User user = users.FirstOrDefault(s => s.Id == id);
            return user;
        }

        public User GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges(List<User> users)
        {
            File.Delete(filePath);
            File.AppendAllLines(filePath, users.Select(s => JsonSerializer.Serialize(s)));
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

                    if (jsonNode != null && jsonNode[2] != null)
                    {
                        AdminUser admin = JsonSerializer.Deserialize<AdminUser>(item);
                        if (admin != null)
                        {
                            users.Add(admin);
                        }
                    }
                    else
                    {

                        User user = JsonSerializer.Deserialize<User>(item);
                        if (user != null)
                        {
                            users.Add(user);
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
