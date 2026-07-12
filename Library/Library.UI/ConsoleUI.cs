using Library.Domain;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Repository.Repositories;
using Library.Services.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.UI
{
    public class ConsoleUI
    {
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
        public string GetInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public static void Menu()
        {
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
        }
        public static void RegisterMenu()
        {
            Console.WriteLine("Enter your username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter your email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();
            
            IFileMeneger repo = new Repositories();
            UserService userService = new UserService(repo);

            userService.RegisterUser(username, email, password);
        }

        public static void loginMenu()
        {
            Console.WriteLine("Enter Email: ");
            string email = Console.ReadLine();
            Console.WriteLine("Enter password: ");
            string password = Console.ReadLine();

            IFileMeneger repo = new Repositories();
            UserService userService = new UserService(repo);

            userService.LoginUser(email, password);
        }



    }
}
