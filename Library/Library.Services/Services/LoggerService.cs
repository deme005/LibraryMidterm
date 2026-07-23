using Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Services.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly string FileName = "logs.txt";
        protected string filePath => @"C:\Users\deme\Desktop\‏\codes\doit_midterm\LibraryMidterm\Library\Library.Repository\Data\" + FileName;
        public void LogActivity(string username, string activity)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] User: {username} | Action: {activity}";

                File.AppendAllLines(filePath, new[] { logEntry });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOGGER ERROR] Could not write to log file: {ex.Message}");
            }
        }
    }
}
