using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    public class ClientUser : User
    {
        private decimal fines;
        public decimal Fines
        {
            get { return fines; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Fines cannot be negative."); 
                }
                fines = value;
            }
        }
        public ClientUser() : base() 
        {
            Role = Enums.Role.Client;
        }

        public ClientUser(int id, string username, string email,  string password): base(id, username, email, password)
        {
        }
        
    }
}
