using Library.Domain;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Repository.Repositories;
using Library.Services.Interfaces;
using Library.Services.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.UI
{
    internal class ConsoleUI
    {
        private static readonly IFileMeneger _repo = new UserRepositories();
        private static readonly EmailService _emailService = new EmailService();
        private static readonly UserService _userService = new UserService(_repo, _emailService);
        private static readonly BookService _bookService = new BookService(new BookRepositories());
        private static readonly BorrowService _borrowService = new BorrowService(new BorrowRepositories(), new BookRepositories(), new UserRepositories());

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

        public static User loginMenu()
        {
            Console.WriteLine("Enter Email: ");
            string email = Console.ReadLine();
            Console.WriteLine("Enter password: ");
            string password = Console.ReadLine();

            try
            {
                User loggedInUser = _userService.LoginUser(email, password);

                Console.WriteLine($"Welcome back, {loggedInUser.Username}!");
                return loggedInUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
                return null; 
            }
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
                        User loggedInUser = loginMenu();
                        if (loggedInUser != null)
                        {
                            RouteUserToMenu(loggedInUser);
                        }
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
                        VerifyAccount();
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
        private static void ViewAllBooks()
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
        private static void SearchBook(BookService bookService)
        {
            Console.WriteLine("Enter the name of the book: ");
            string bookName = Console.ReadLine();
            var foundBooks = bookService.SearchBooks(bookName);
            foreach (var book in foundBooks)
            {
                Console.WriteLine($"- {book.Title} by {book.Author} (Qty: {book.Quantity})");
            }
        }
        private static void RequestBorrow(ClientUser clientUser)
        {
            Console.Clear();
            Console.WriteLine("--- Request to Borrow a Book ---");

            var availableBooks = _bookService.GetAvailableBooks();
            if (availableBooks == null || availableBooks.Count == 0)
            {
                Console.WriteLine("No books are currently available to borrow.");
                return;
            }

            foreach (var b in availableBooks)
            {
                Console.WriteLine($"• [ISBN: {b.Key}] {b.Title} (Qty: {b.Quantity})");
            }

            Console.Write("\nEnter the ISBN of the book you want to borrow: ");
            string isbn = Console.ReadLine();

            try
            {
                _borrowService.RequestBorrow(clientUser, isbn);

                Console.WriteLine("\n[Success] Borrow request submitted! Waiting for Admin approval.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Error] {ex.Message}");
            }
        }
        private static void ReturnBook(ClientUser clientUser)
        {
            Console.Clear();
            Console.WriteLine("--- Return a Book ---");

            // Get active borrows for this user
            var activeBorrows = _borrowService.GetUserHistory(clientUser.ID.ToString())
                .Where(b => b.BorrowStatus == Status.Approved)
                .ToList();

            if (activeBorrows.Count == 0)
            {
                Console.WriteLine("You currently have no active borrowed books to return.");
                return;
            }

            Console.WriteLine("Your active borrowed books:");
            foreach (var item in activeBorrows)
            {
                Console.WriteLine($"[Borrow ID: {item.BorrowID}] ISBN: {item.ISBN} | Status: {item.BorrowStatus}");
            }

            Console.Write("\nEnter the Borrow ID of the record you want to return: ");
            string recordKey = Console.ReadLine();

            try
            {
                _borrowService.ReturnBook(recordKey);

                Console.WriteLine("\n[Success] Book returned successfully! Stock updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Error] {ex.Message}");
            }

            
        }
        private static void ViewMyBorrows(ClientUser clientUser)
        {
            Console.Clear();
            Console.WriteLine("--- My Borrow History ---");

            var history = _borrowService.GetUserHistory(clientUser.ID.ToString());

            if (history == null || history.Count == 0)
            {
                Console.WriteLine("You have no borrowing history on record.");
            }
            else
            {
                foreach (var record in history)
                {
                    Console.WriteLine($"[ID: {record.BorrowID}] ISBN: {record.ISBN} | Status: {record.BorrowStatus}");
                }
            }
        }
        private static void PayFines(ClientUser clientUser)
        {
            Console.Clear();
            Console.WriteLine("--- Pay Outstanding Fines ---");

            if (clientUser.Fines <= 0)
            {
                Console.WriteLine("You currently have no outstanding fines! Fine balance: $0.00");
            }
            else
            {
                Console.WriteLine($"Your current balance due: ${clientUser.Fines:F2}");
                Console.Write("Would you like to pay your fines now? (y/n): ");
                string answer = Console.ReadLine();

                if (answer?.ToLower() == "y")
                {
                    clientUser.Fines = 0;
                    Console.WriteLine("\n[Success] All outstanding fines have been cleared!");
                }
                else
                {
                    Console.WriteLine("\nFine payment canceled.");
                }
            }
        }
        private static void VerifyAccount()
        {
            Console.WriteLine("\n--- ACCOUNT VERIFICATION ---");

            Console.Write("Enter Email: ");
            string email = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Email cannot be empty.");
                return;
            }

            Console.Write("Enter Verification Code: ");
            string code = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(code))
            {
                Console.WriteLine("Verification code cannot be empty.");
                return;
            }

            try
            {
                // Calling your existing UserService method
                bool isSuccess = _userService.VerifyUser(email, code);

                if (isSuccess)
                {
                    Console.WriteLine("Account verified successfully! You can now log in.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Verification Error: {ex.Message}");
            }
        }

        public static void AdminMenu(User admin)
        {
            AdminUser adminUser = admin as AdminUser;
            bool loggedIn = true;

            while (loggedIn)
            {
                Console.WriteLine($"\n=== ADMIN DASHBOARD (Welcome, {adminUser.Username}) ===");
                Console.WriteLine("1. View Book Catalog ");
                Console.WriteLine("2. Add New Book ");
                Console.WriteLine("3. Update Book Quantity ");
                Console.WriteLine("4. Remove a Book ");
                Console.WriteLine("5. View & Manage Borrow Requests (Approve/Reject)");
                Console.WriteLine("6. View All Users ");
                Console.WriteLine("7. Log Out ");
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
                        ViewAllUsers();
                        break;
                    case 7:
                        Console.WriteLine("Logging out of admin panel...");
                        loggedIn = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
        private static void AddNewBook()
        {
            Console.Write("Enter Book ISBN: ");
            string isbn = Console.ReadLine()?.Trim();
            Console.WriteLine("\n--- ADD NEW BOOK ---");
            Console.Write("Enter Title: ");
            string title = Console.ReadLine();
            Console.Write("Enter Author: ");
            string author = Console.ReadLine();
            Console.Write("Enter Quantity: ");
            int.TryParse(Console.ReadLine(), out int quantity);

            Book newBook = new Book
            {
                ISBN = isbn,
                Title = title,
                Author = author,
                Quantity = quantity
            };

            _bookService.AddBook(newBook);
            Console.WriteLine("Book added successfully!");
        }
        private static void UpdateBookQuantity()
        {
            Console.WriteLine("\n--- UPDATE BOOK QUANTITY ---");
            Console.Write("Enter Book ID: ");
            string key = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(key))
            {
                Console.WriteLine("Book ID can not be empty");
                return;
            }

            Console.Write("Enter New Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int newQty))
            {
                Console.WriteLine("Invalid quantity format.");
                return;
            }

            try
            {
                _bookService.UpdateQuantity(key, newQty);
                Console.WriteLine("Book quantity updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private static void RemoveBook()
        {
            Console.WriteLine("\n--- REMOVE BOOK ---");

            Console.Write("Enter Book ID to remove: ");
            string bookId = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(bookId))
            {
                Console.WriteLine("Book ID cannot be empty.");
                return;
            }

            Console.Write($"Are you sure you want to remove book ID {bookId}? (y/n): ");
            string confirm = Console.ReadLine().Trim().ToLower();

            if (confirm == "y")
            {
                try
                {
                    _bookService.DeleteBook(bookId);
                    Console.WriteLine("Book removed successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Action canceled.");
            }
        }
        private static void ManageBorrowRequests()
        {
            var activeBorrows = _borrowService.GetActiveBorrows();
            var pendingRequests = activeBorrows.Where(b => b.BorrowStatus == Status.Pending).ToList();

            if (!pendingRequests.Any())
            {
                Console.WriteLine("No pending borrow requests.");
                return;
            }

            Console.WriteLine("=== PENDING BORROW REQUESTS ===");
            foreach (var req in pendingRequests)
            {
                Console.WriteLine($"ID: {req.BorrowID} | User ID: {req.UserId} | Book ISBN: {req.ISBN}");
            }

            Console.Write("\nEnter Borrow ID to manage (or press Enter to cancel): ");
            string borrowId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(borrowId)) return;

            Console.Write("Type 'A' to Approve or 'R' to Reject: ");
            string choice = Console.ReadLine()?.Trim().ToUpper();

            try
            {
                if (choice == "A")
                {
                    _borrowService.ApproveBorrow(borrowId);
                    Console.WriteLine("Request approved successfully!");
                }
                else if (choice == "R")
                {
                    _borrowService.RejectBorrow(borrowId);
                    Console.WriteLine("Request rejected.");
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private static void ViewAllUsers()
        {
            var users = _repo.GetAllUsers(); 

            if (users == null || !users.Any())
            {
                Console.WriteLine("No users found.");
                return;
            }

            Console.WriteLine("=== ALL REGISTERED USERS ===");
            foreach (var user in users)
            {
                string role = user is ClientUser ? "Client" : "Admin";
                decimal fines = (user is ClientUser client) ? client.Fines : 0m;

                Console.WriteLine($"ID: {user.ID} | Name: {user.Username} | Role: {role} | Fines Owed: ${fines:F2}");
            }
        }

        private static void RouteUserToMenu(User loggedInUser)
        {
            if (loggedInUser == null)
            {
                Console.WriteLine("No user is currently logged in.");
                return;
            }

            if (loggedInUser is AdminUser)
            {
                AdminMenu(loggedInUser);
            }
            else if (loggedInUser is ClientUser client)
            {
                ClientMenu(client);
            }
            else
            {
                Console.WriteLine("Unknown user role.");
            }
        }

    }
}