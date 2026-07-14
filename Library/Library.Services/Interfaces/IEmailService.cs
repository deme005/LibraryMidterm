using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Services.Interfaces
{
    public interface IEmailService
    {
        void SeedEmail(string to, string subject, string body);
    }
}
