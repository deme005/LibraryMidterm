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
    public class BookService : IBookService
    {

        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public List<Book> GetAvailableBooks()
        {
            return _bookRepository.GetAll().Where(b => b.Quantity > 0).ToList();
        }

        public List<Book> SearchBooks(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<Book>();
            }
            return _bookRepository.GetAll().Where(b => b.Title.Contains(name.ToLower()) || b.Author.Contains(name.ToLower())).ToList();
        }

        public void AdjustInventory(string bookId, int amount)
        {
            Book book = _bookRepository.GetByKey(bookId);
            if (book == null)
            {
                throw new Exception("Book not found.");
            }
            if (book.Quantity + amount < 0)
            {
                throw new InvalidOperationException("Not enough copies available to borrow.");
            }

            book.Quantity += amount;
            _bookRepository.Update(book);
        }
    }
}

