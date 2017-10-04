using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    class Bank
    {
        private List<Customer> customers = new List<Customer>();

        public Bank()
        {
            Customer cust = new Customer(1005, "559268-7528", "Berglunds snabbköp", "Berguvsvägen  8",
                "Luleå", "", "S-958 22", "Sweden", "0921-12 34 65");
            customers.Add(cust);

            cust = new Customer(1024, "556392-8406", "Folk och fä HB", "Åkergatan 24",
                "Bräcke", "", "S-844 67", "Sweden", "0695-34 67 21");
            customers.Add(cust);

            cust = new Customer(1032, "551553-1910", "Great Lakes Food Market", "2732 Baker Blvd.",
                "Eugene", "OR", "97403", "USA", "(503) 555-7555");
            customers.Add(cust);
        }

        public void Run()
        {
            bool exitApp = false;

            //While bank is running
            do
            {   
                Console.Clear();
                PrintMainMenu();
                ReadIntFromKeyboard();

            } while (!exitApp);
        }

        private void PrintMainMenu()
        {
            Console.WriteLine("HUVUDMENY");
            Console.WriteLine("0) Avsluta och spara");
            
        }

        private int ReadIntFromKeyboard()
        {
            int number;
            string input;

            //Run until a valid number has been typed
            do
            {
                Console.Write(">");
                input = Console.ReadLine();
            } while ( !int.TryParse(input, out number) );

            return number;
        }

    }
}
