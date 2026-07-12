using System.ComponentModel.Design;
using Library.Services.Services;
using Library.Repository.Repositories;
using Library.Domain.Models;

namespace Library.UI
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool loop = false;
            while (!loop)
            {
                ConsoleUI.Menu();

                int.TryParse(Console.ReadLine(), out int loop1);
                switch (loop1)
                {
                    case 1:
                        ConsoleUI.RegisterMenu();
                        break;
                    case 2:
                        ConsoleUI.loginMenu();
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("invalid input");
                        break;

                }
            }
        }
    }
}
