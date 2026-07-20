using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Book GetByTitle(string title);
    }
}
