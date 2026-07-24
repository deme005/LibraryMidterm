using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Repository.Repositories.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Repository.Repositories
{
    public class BorrowRepositories : GenericRepository<BorrowRecord>, IBorrowRepository
    {
        protected override string FileName => "borrows.txt";

        protected string filePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Library.Repository\Data", FileName);

        public List<BorrowRecord> GetActiveBorrows()
        {
            return GetAll().Where(b => b.BorrowStatus == Status.Approved).ToList();
        }
        public List<BorrowRecord> getHistoryByUserId(string userId)
        {
            return GetAll().Where(b => b.UserId.Equals(userId)).ToList();
        }
    }
}
