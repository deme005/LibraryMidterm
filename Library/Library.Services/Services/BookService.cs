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
    public class BookService
    {

        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAll();
        }

        public Book GetBookById(int id)
        {
            return _bookRepository.GetByKey(id.ToString());
        }

        public Book GetBookByTitle(string title)
        {
            return _bookRepository.GetByTitle(title);
        }

        public void AddBook(Book book, User currentUser)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }

            if (currentUser.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException("Only admins can delete books.");
            }
            _bookRepository.Add(book);
        }

        public void UpdateBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            _bookRepository.Update(book);
        }

        public void DeleteBook(Book book, User currentUser)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }

            if (currentUser.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException("Only admins can delete books.");
            }
            _bookRepository.Delete(book);
        }

        public  List<Book> SearchBooks(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<Book>();
            }

            return _bookRepository.GetAll().Where
                (b => b.Title.Contains(keyword.ToLower()) ||
                b.Author.Contains(keyword.ToLower())).ToList();
        }

        public List<Book> GetAvailableBooks()
        {
            return _bookRepository.GetAll()
                .Where(b => b.Quantity > 0)
                .ToList();
        }
    }
}

