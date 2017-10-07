using System;

namespace BankApp.Extensions
{
    static class ReadFromKeyboard
    {
        //Forces the user to input a number
        public static int GetInt()
        {
            int number;
            string input;

            //Run until a valid number has been typed
            do
            {
                Console.Write("> ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out number));

            return number;
        }

        //Forces the user to input non-empty string
        public static string GetString()
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (input != "")
                    return input;

                Console.WriteLine("Fältet kan inte vara tomt försök igen");
            }
        }

        //Force the user to input a decimalnumber
        public static decimal GetDecimal()
        {
            do
            {
                Console.Write("> ");
                string ammountStr = Console.ReadLine();

                if (decimal.TryParse(ammountStr, out decimal amount))
                    return amount;
            } while (true);
        }
    }
}
