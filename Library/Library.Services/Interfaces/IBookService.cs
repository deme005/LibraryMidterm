using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Services.Interfaces
{
    public interface IBookService
    {
        List<Book> GetAvailableBooks();
        List<Book> SearchBooks(string name);
        void AdjustInventory(string bookId, int amount);
        void AddBook(Book book);
        void UpdateQuantity(string key, int newQuantity);
        void DeleteBook(string bookId);
    }
}
