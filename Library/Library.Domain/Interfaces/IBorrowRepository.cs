using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Interfaces
{
    public interface IBorrowRepository : IGenericRepository<BorrowRecord>
    {
        List<BorrowRecord> GetActiveBorrows();
        List<BorrowRecord> getHistoryByUserId(string userId);
    }
}
