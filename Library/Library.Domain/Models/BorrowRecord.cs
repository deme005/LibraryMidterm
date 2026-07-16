using Library.Domain.Enums;
using Library.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    public class BorrowRecord : IKeyedEntity
    {
        private string borrowID;
        private int userId; 
        private string isbn; 
        private DateTime returnDate; 
        public Status BorrowStatus { get; set; }

        public string BorrowID
        {
            get { return borrowID; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Invalid input.");
                }
                borrowID = value.Trim();
            }
        }
        public string Key => BorrowID;
        public int UserId
        {
            get { return userId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Invalid input.");
                }
                userId = value;
            }
        }
        public string ISBN
        {
            get { return isbn; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Invalid input.");
                }
                isbn = value.Trim();
            }
        }
        public DateTime ReturnDate
        {
            get { return returnDate; }
            set
            {
                if (value < DateTime.Now)
                {
                    throw new ArgumentException("Return date cannot be in the past.");
                }
                returnDate = value;
            }
        }

        public BorrowRecord() { }

        public BorrowRecord(string borrowID, int userID, string isbn, DateTime returnDate, Status borrowStatus)
        {
            BorrowID = borrowID;
            UserId = userID;
            ISBN = isbn;
            ReturnDate = returnDate;
            BorrowStatus = borrowStatus;
        }
    }
}
