using Library.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    public class AdminUser : User
    {
        public AdminUser() : base()
        {
            Role = Role.Admin;
        }
        public AdminUser(int id, string username, string email, string password) : base(id, username, email, password)
        {
        }
    }
}
