using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Repository.Repositories.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Repository.Repositories
{
    public class BookRepositories : GenericRepository<Book>, IBookRepository
    {
        protected override string FileName => "books.txt";

        public Book GetByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return null;
            }
            return Get(b => b.Title.Equals(title));
        }
    }
}
