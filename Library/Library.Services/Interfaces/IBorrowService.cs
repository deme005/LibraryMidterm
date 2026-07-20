using Library.Domain.Interfaces;
using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Services.Interfaces
{
    public interface IBorrowService
    {
        void RequestBorrow(ClientUser client, string bookKey);
        void ReturnBook(string borrowRecordKey);
        List<BorrowRecord> GetUserHistory(string userId);
        List<BorrowRecord> GetActiveBorrows();
        void ApproveRequest(string borrowRecordKey);
        void RejectRequest(string borrowRecordKey);
    }
}
