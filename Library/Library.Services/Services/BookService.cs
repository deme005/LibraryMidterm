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

        
        public void AddBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book), "Book cannot be null.");
            }
            _bookRepository.Add(book);
        }
        public void UpdateQuantity(string key, int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity cannot be negative.", nameof(newQuantity));

            var book = _bookRepository.GetByKey(key)
                ?? throw new KeyNotFoundException($"Book with ID {key} not found.");

            book.Quantity = newQuantity;
            _bookRepository.Update(book);
        }
        public void DeleteBook(string bookId)
        {
            Book book = _bookRepository.GetByKey(bookId);
            if (book == null)
            {
                throw new Exception("Book not found.");
            }
            _bookRepository.Delete(book);
        }
    }
}

