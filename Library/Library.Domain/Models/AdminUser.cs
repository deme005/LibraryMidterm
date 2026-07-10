using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    internal class AdminUser : User
    {
        public AdminUser() : base() { }
        public AdminUser(int id, string username, string email, string password) : base(id, username, email, password)
        {
        }
    }
}
