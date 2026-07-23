using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Services.Interfaces
{
    public interface ILoggerService
    {
        void LogActivity(string username, string activity);
    }
}
