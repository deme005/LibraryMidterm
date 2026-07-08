using Library.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace Library.Domain
{


    public abstract class User
    {
        public int Id;

        private string username;

        private string email;

        private string password;

        public Role Role { get; set; } = Role.Client;


        public string Username

        {
            get { return username; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Username can not be empty!");
                }
                username = value.Trim();
            }
        }

        public string Email

        {
            get { return email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
                {
                    throw new ArgumentException("Username can not be empty!");
                }
                email = value.Trim();
            }
        }

        public string Password

        {
            get { return password; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Username can not be empty!");
                }
                password = value;
            }
        }



    }
}


