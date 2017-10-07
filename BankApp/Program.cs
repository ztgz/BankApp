using System;

namespace BankApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Menu menu = new Menu();
                menu.StartMenu();
            }
            catch (Exception e)
            {
                Console.WriteLine("Något gick snett under körning. Programmet avslutas.");
            }
        }
    }
}
