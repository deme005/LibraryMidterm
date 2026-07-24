using Library.Domain.Enums;
using Library.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace Library.Domain
{


    public abstract class User 
    {
        private int id;

        private string username;

        private string email;

        private string password;
        
        public bool IsVerified { get; set; } = false;
        

        public Role Role { get; set; } = Role.Client;
        public string VerificationCode { get; set; }


        public int ID
        {
            get { return id; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("ID can not be zero or negative!");
                }
                id = value;
            }
        }
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

        protected User() { }
        protected User (int id, string username, string email, string password)
        {
            ID = id;
            Username = username;
            Email = email;
            Password = password;
        }

    }
}


