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
    public class BorrowService : IBorrowService
    {
        private readonly IFileMeneger _userRepository;
        private readonly IBorrowRepository _borrowRepository;
        private readonly IBookRepository _bookRepository;

        public BorrowService(IBorrowRepository borrowRepository, IBookRepository bookRepository, IFileMeneger userRepositoey)
        {
            _borrowRepository = borrowRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepositoey;
        }

        public void ApproveRequest(string borrowRecordKey)
        {
            var record = _borrowRepository.GetByKey(borrowRecordKey);
            if (record == null)
            {
                throw new Exception("Borrow record not found.");
            }
            var book = _bookRepository.GetByKey(record.ISBN);
            if (book == null || book.Quantity <= 0)
            {
                throw new InvalidOperationException("Book is out of stock or does not exist.");
            }
            book.Quantity -= 1;
            _bookRepository.Update(book);

            record.BorrowStatus = Status.Approved;
            _borrowRepository.Update(record);
        }

        public List<BorrowRecord> GetActiveBorrows()
        {

            return _borrowRepository.GetAll().Where(b => b.BorrowStatus == Status.Pending || b.BorrowStatus == Status.Approved).ToList();
        }

        public List<BorrowRecord> GetUserHistory(string userId)
        {

            int targetId = int.Parse(userId);
            return _borrowRepository.GetAll().Where(b => b.UserId == targetId).ToList();
        }

        public void RejectRequest(string borrowRecordKey)
        {
            var record = _borrowRepository.GetByKey(borrowRecordKey);
            if (record == null)
            {
                throw new Exception("Borrow record not found.");
            }

            record.BorrowStatus = Status.Rejected;
            _borrowRepository.Update(record);
        }

        public void RequestBorrow(ClientUser client, string bookKey)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            if (!client.IsVerified)
            {
                throw new UnauthorizedAccessException("Account must be verified to borrow books.");
            }
            var book = _bookRepository.GetByKey(bookKey);
            if (book == null)
            {
                throw new Exception("Book not found.");
            }
            if (book.Quantity <= 0)
            {
                throw new InvalidOperationException("Book is out of stock.");
            }
            var allBorrows = _borrowRepository.GetAll();
            string nextKey = (allBorrows.Count + 1).ToString();

            BorrowRecord record = new BorrowRecord
            {
                BorrowID = nextKey,
                UserId = client.ID,
                ISBN = book.Key,
                BorrowStatus = Status.Pending,
            };

            _borrowRepository.Add(record);
        }

        public void ReturnBook(string borrowRecordKey)
        {
            var record = _borrowRepository.GetByKey(borrowRecordKey);
            if (record == null)
            {
                throw new Exception("Borrow record not found.");
            }
            decimal totalFine = CalculateFine(record);
            record.BorrowStatus = Status.Returned;
            _borrowRepository.Update(record);

            var book = _bookRepository.GetByKey(record.ISBN);
            if (book != null)
            {
                book.Quantity++;
                _bookRepository.Update(book);
            }
        }
        public void ApproveBorrow(string borrowId)
        {
            var record = _borrowRepository.GetByKey(borrowId);

            var book = _bookRepository.GetByKey(record.ISBN);

            if (book.Quantity <= 0)
            {
                throw new InvalidOperationException("Cannot approve: Book is out of stock.");
            }
            record.BorrowStatus = Status.Approved;
            record.DateOfBorrow = DateTime.Now;
            record.ReturnDate = DateTime.Now.AddDays(14); 

            book.Quantity -= 1;

            _borrowRepository.Update(record);
            _bookRepository.Update(book);
        }

        public void RejectBorrow(string borrowId)
        {
            var record = _borrowRepository.GetByKey(borrowId);

            record.BorrowStatus = Status.Rejected;
            _borrowRepository.Update(record);
        }
        public Decimal CalculateFine(BorrowRecord record)
        {
            if (record == null || record.BorrowStatus != Status.Approved)
            {
                return 0m;
            }
            if (DateTime.Now > record.ReturnDate)
            {
                int overdueDays = (DateTime.Now - record.ReturnDate).Days;
                if (overdueDays < 1)
                {
                    overdueDays = 1;
                }
                decimal dailyRate = 1.0m;
                return overdueDays * dailyRate;
            }

            return 0m;
        }
    }
}
