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
        private static readonly IFileMeneger _repo = new UserRepositories();
        private static readonly EmailService _emailService = new EmailService();
        private static readonly UserService _userService = new UserService(_repo, _emailService);
        private static readonly BookService _bookService = new BookService(new BookRepositories());

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


            _userService.RegisterUser(username, email, password);
        }

        public static void loginMenu()
        {
            Console.WriteLine("Enter Email: ");
            string email = Console.ReadLine();
            Console.WriteLine("Enter password: ");
            string password = Console.ReadLine();

            _userService.LoginUser(email, password);
        }
        public static void FirstMenu()
        {
            bool loop = false;
            while (!loop)
            {
                Menu();

                int.TryParse(Console.ReadLine(), out int loop1);
                switch (loop1)
                {
                    case 1:
                        RegisterMenu();
                        break;
                    case 2:
                        loginMenu();
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("invalid input");
                        break;

                }
            }


        }
        public static void ClientMenu(User client)
        {
            ClientUser clientUser = client as ClientUser;
            
            bool loggedIn = true;
            while (loggedIn)
            {
                Console.WriteLine("1. Browse Book Catalog");
                Console.WriteLine("2. Search for a Book");
                Console.WriteLine("3. Request to Borrow a Book");
                Console.WriteLine("4. Return a Book");
                Console.WriteLine("5. View My Borrow Requests & History");
                Console.WriteLine("6. Pay Fines");
                Console.WriteLine("7. Verify account");
                Console.WriteLine("8. Log Out");

                int.TryParse(Console.ReadLine(), out int loop);

                switch (loop)
                {
                    case 1:
                        ViewAllBooks();
                        break;
                    case 2:
                        SearchBook(_bookService);
                        break;
                    case 3:
                        RequestBorrow(clientUser);
                        break;
                    case 4:
                        ReturnBook(clientUser);
                        break;
                    case 5:
                        ViewMyBorrows(clientUser);
                        break;
                    case 6:
                        PayFines(clientUser);
                        break;
                    case 7:
                        break;
                    case 8:
                        Console.WriteLine("Logging out...");
                        loggedIn = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
        public static void ViewAllBooks()
        {
            Console.Clear();
            Console.WriteLine("--- Book Catalog ---");
            var books = _bookService.GetAvailableBooks();
            if (books == null || books.Count == 0)
            {
                Console.WriteLine("No books found in the library catalog.");
            }
            else
            {
                foreach (var book in books)
                {
                    Console.WriteLine($"ID: {book.Key} | Title: {book.Title} | Author: {book.Author} | Quantity: {book.Quantity}");
                }
            }
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }
        public static void SearchBook(BookService bookService)
        {
            Console.WriteLine("Enter the name of the book: ");
            string bookName = Console.ReadLine();
            var foundBooks = bookService.SearchBooks(bookName);
            foreach (var book in foundBooks)
            {
                Console.WriteLine($"- {book.Title} by {book.Author} (Qty: {book.Quantity})");
            }
        }
        public static void RequestBorrow(ClientUser clientUser)
        {
            

        }
        public static void ReturnBook(ClientUser clientUser)
        {
            // Implementation
        }
        public static void ViewMyBorrows(ClientUser clientUser)
        {
            // Implementation
        }
        public static void PayFines(ClientUser clientUser)
        {
            // Implementation
        }

        public static void ShowAdminMenu(User admin)
        {
            AdminUser adminUser = admin as AdminUser;
            bool loggedIn = true;

            while (loggedIn)
            {
                Console.WriteLine($"\n=== ADMIN DASHBOARD (Welcome, {adminUser.Username}) ===");
                Console.WriteLine("1. View Book Catalog (კატალოგის დათვალიერება)");
                Console.WriteLine("2. Add New Book (ახალი წიგნის დამატება)");
                Console.WriteLine("3. Update Book Quantity (წიგნის რაოდენობის მართვა)");
                Console.WriteLine("4. Remove a Book (წიგნის წაშლა)");
                Console.WriteLine("5. View & Manage Borrow Requests (მოთხოვნების მართვა - Approve/Reject)");
                Console.WriteLine("6. Send Due Date Warnings & Alerts (შეტყობინებების გაგზავნა)");
                Console.WriteLine("7. View All Users (მომხმარებლების სია)");
                Console.WriteLine("8. Log Out (გამოსვლა)");
                Console.Write("Choose an option: ");

                int.TryParse(Console.ReadLine(), out int choice);
                switch (choice)
                {
                    case 1:
                        ViewAllBooks();
                        break;
                    case 2:
                        AddNewBook();
                        break;
                    case 3:
                        UpdateBookQuantity();
                        break;
                    case 4:
                        RemoveBook();
                        break;
                    case 5:
                        ManageBorrowRequests();
                        break;
                    case 6:
                        SendDueWarnings();
                        break;
                    case 7:
                        ViewAllUsers();
                        break;
                    case 8:
                        Console.WriteLine("Logging out of admin panel...");
                        loggedIn = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
        public static void AddNewBook()
        {
            // Implementation
        }
        public static void UpdateBookQuantity()
        {
            // Implementation 
        }
        public static void RemoveBook()
        {
            // Implementation 
        }
        public static void ManageBorrowRequests()
        {
            // Implementation 
        }
        public static void SendDueWarnings()
        {
            // Implementation 
        }
        public static void ViewAllUsers()
        {
            // Implementation 
        }
    }
}