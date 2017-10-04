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

        private List<Account> accounts = new List<Account>();

        public Bank()
        {
            /*Test data*/
            Customer cust = new Customer(1005, "559268-7528", "Berglunds snabbköp", "Berguvsvägen  8",
                "Luleå", "", "S-958 22", "Sweden", "0921-12 34 65");
            customers.Add(cust);

            cust = new Customer(1024, "556392-8406", "Folk och fä HB", "Åkergatan 24",
                "Bräcke", "", "S-844 67", "Sweden", "0695-34 67 21");
            customers.Add(cust);

            cust = new Customer(1032, "551553-1910", "Great Lakes Food Market", "2732 Baker Blvd.",
                "Eugene", "OR", "97403", "USA", "(503) 555-7555");
            customers.Add(cust);

            Account account = new Account(13019, 1005, 1488.80m);
            accounts.Add(account);
            account = new Account(13020, 1005, 613.20m);
            accounts.Add(account);
            account = new Account(13093, 1024, 695.62m);
            accounts.Add(account);
            account = new Account(13128, 1032, 392.20m);
            accounts.Add(account);
            account = new Account(13130, 1032, 4807.00m);
            accounts.Add(account);
        }

        //The main loop of the bank app
        public void Run()
        {
            //Becomes true when application is exiting
            bool exitApp = false;

            //While bank is running
            do
            {
                Console.Clear();

                PrintMainMenu();

                //What is choosen in the main menu
                int choise = ReadIntFromKeyboard();

                switch (choise)
                {
                    case 1:
                        //Search for customer based on name or city
                        CustomerSearchMenu();
                        break;
                    case 2:
                        //Show info for a customer
                        CustomerInfoMenu();
                        break;
                    case 0:
                        exitApp = true;
                        break;
                    default:

                        break;
                }

            } while (!exitApp);
        }

        private void PrintMainMenu()
        {
            Console.WriteLine("HUVUDMENY");
            Console.WriteLine("0) Avsluta och spara");
            Console.WriteLine("1) Sök kund");
            Console.WriteLine("2) Visa kundbild");
            Console.WriteLine();
        }

        private int ReadIntFromKeyboard()
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

        private void CustomerSearchMenu()
        {
            Console.Clear();

            //Get search from the user
            Console.WriteLine("* Sök kund *");
            Console.Write("Namn eller postort? ");
            string customerInfo = Console.ReadLine();

            //Filter the customers based on name or city
            var filtredCustomers = CustomersByNameOrCity(customerInfo);

            //Print the filtred list
            Console.WriteLine("\nKundnr| Kund");
            foreach (var filtredCustomer in filtredCustomers)
            {
                Console.WriteLine(" {0} | {1}", filtredCustomer.CustomerNumber, filtredCustomer.Name);
            }

            WaitForKey();
        }

        private void CustomerInfoMenu()
        {
            Console.Clear();

            //Get input from user
            Console.WriteLine("* Visa kundbild *");
            Console.WriteLine("Kundnummer eller kontonummer: ");

            string customerSearch = Console.ReadLine();

            //if owner didn't search for an number, return to main menu
            if(!int.TryParse(customerSearch, out int searchNumber))
            {
                Console.WriteLine("\nDu måste ange kundnummer eller kontonummer som en siffra.");
                WaitForKey();
                return;
            }

            //A customer number has 4 digits and a account number have 5
            //Otherwise it's a Invalid number 
            if (searchNumber >= 100_000 || searchNumber < 1000)
            {
                Console.WriteLine("\nDu har inte angivit en korrekt siffra.");
                Console.WriteLine("Ett kundnummer är mellan 1000-9999");
                Console.WriteLine("och ett kontonummer är mellan 10000-99999.");

                //Wait for keypress an return to main menu
                WaitForKey();
                return;
            }

            //If user searched for an account number, try to get owner of account
            if (searchNumber >= 10000 && searchNumber < 100_000)
            {
                int? customerNumber = GetOwnerOfAccount(searchNumber);

                //If owner was found, set searchNumber to customerNumber
                if (customerNumber != null)
                {
                    searchNumber = customerNumber.Value;
                }
                else
                {
                    Console.WriteLine("\nAngivet konto kunde inte hittas.");
                    WaitForKey();
                    return;
                }
            }

            //Get info based on owners customer number
            Customer customer = GetCustomerByNumber(searchNumber);
            List<Account> filtredAccounts = AccountsByOwnerNumber(searchNumber);

            //if a customer was found, print info
            if (customer != null)
            {
                //Info about customer
                Console.WriteLine("\nOrganistionsnummer: {0}", customer.OrganisationNumber);
                Console.WriteLine("Namn: {0}", customer.Name);
                Console.WriteLine("Adress: {0}, {1}, {2}, {3}{4}", customer.Address, customer.PostNumber, customer.City,
                    (customer.Region != "" ? $"{customer.Region}, " : ""), customer.Country);
                Console.WriteLine("Telefonnummer: {0}", customer.PhoneNumber);

                //Info about customers accounts
                Console.WriteLine("\nKonton");
                foreach (var account in filtredAccounts)
                {
                    Console.WriteLine("{0}: {1} kr", account.AccountNumber, account.Balance);
                }
            }
            else
            {
                Console.WriteLine("\nKunde inte hitta kund eller konto.");
            }

            WaitForKey();
        }

        public void WaitForKey()
        {
            Console.WriteLine("\nTryck valfri tanget för att gå tillbaka till meny...");
            Console.ReadKey();
        }

        //Filter the customer list based on if name or city contains the string
        private List<Customer> CustomersByNameOrCity(string customerInfo)
        {
            //Make customerinfo to lower
            customerInfo = customerInfo.ToLower();

            //Filter on name and city (The search is lowercase)
            var filtredCustomers = customers.Where(c => c.Name.ToLower().Contains(customerInfo) ||
                                                        c.City.ToLower().Contains(customerInfo));

            //Return the filtred customers as a list
            return filtredCustomers.ToList();
        }

        //Filter the accounts list by owner
        private List<Account> AccountsByOwnerNumber(int number)
        {
            var filtredAccounts = accounts.Where(a => a.OwnersCustomerNumber == number);
            return filtredAccounts.ToList();
        }

        //Get customer from customers based on customer number
        private Customer GetCustomerByNumber(int number)
        {
            Customer customer = customers.SingleOrDefault(c => c.CustomerNumber == number);
            return customer;
        }

        //Get the owner of an account
        private int? GetOwnerOfAccount(int accountNumber)
        {
            Account account = accounts.SingleOrDefault(a => a.AccountNumber == accountNumber);
            return account?.OwnersCustomerNumber;
        }
    }
}
