using Library.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    public class Book : IKeyedEntity
    {
		private string title;
		private string author;
		private string isbn;
		private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("quantity cannot be negative."); 
                }
                quantity = value;
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
        public string Key => ISBN;

        

		public string Author
		{
			get { return author; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Invalid input.");
                }
                author = value.Trim();
            }
        }


		public string Title
		{
			get { return title; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException("Invalid input.");
				}
				title = value.Trim();
			}
		}

        public Book() { }

        public Book(string title, string author, string genre, string isbn, int quantity)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            Quantity = quantity;
        }



        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to increase must be positive.");
            }
                Quantity += amount;
        }
        public void DecreaseQuantity(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to decrease must be positive.");
            }
            if (Quantity - amount < 0)
            {
                throw new InvalidOperationException("Not enough books in stock."); 
            }
            Quantity -= amount;
        }

    }
}
