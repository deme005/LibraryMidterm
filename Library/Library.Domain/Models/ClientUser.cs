using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    public class ClientUser : User
    {
        public ClientUser() : base() { }

        public ClientUser(int id, string username, string email,  string password): base(id, username, email, password)
        {
        }
        
    }
}
