using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    public class Book
    {
		private string name;
		private string author;
		private string genre;

		public string Genre
		{
			get {  return genre; }
			set
			{
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Invalid input.");
                }
                genre = value;
            }
		}

		public string Author
		{
			get { return author; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Invalid input.");
                }
                author = value;
            }
        }


		public string Name
		{
			get { return name; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException("Invalid input.");
				}
				name = value;
			}
		}

	}
}
