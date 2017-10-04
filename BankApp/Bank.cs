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
                    case 3:
                        //Create new customer
                        CustomerCreateMenu();
                        break;
                    case 5:
                        //Create a new account menu
                        NewAccountMenu();
                        break;
                    case 0:
                        exitApp = true;
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
            Console.WriteLine("3) Skapa kund");
            Console.WriteLine("5) Lägg till konto");
            Console.WriteLine();
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

            //Search for customer information
            CustomerInfoSearch(searchNumber);

            WaitForKey();
        }

        private void NewAccountMenu()
        {
            Console.Clear();

            //Get customernumber from console
            Console.WriteLine("* Skapa konto för kund *");
            Console.Write("Kundnummer? ");
            string strNumber = Console.ReadLine();

            //If input could be parsed to int and the number is four digits
            if (int.TryParse(strNumber, out int custumerNumber) && custumerNumber >= 1000 && custumerNumber < 10000)
            {
                //Get the customer
                Customer customer = GetCustomerByNumber(custumerNumber);
                
                //If customer was found create account
                if (customer != null)
                {
                    CreateAccount(customer);
                }
                else
                {
                    Console.WriteLine("\nCould not find customer.");    
                }

            }
            else
            {
                Console.WriteLine("\nKontonummer måste anges med ett 4-siffrigt nummber (t.ex. 1201)");
            }

            WaitForKey();
        }

        private void CustomerCreateMenu()
        {
            Console.Clear();
            Console.WriteLine("* Skapa ny kund *");

            //Get customer number info
            Console.Write("Organisationsnummer? ");
            string orgNumber = Console.ReadLine();

            Console.Write("Namn? ");
            string name = Console.ReadLine();

            Console.Write("Adress? ");
            string adress = Console.ReadLine();

            Console.Write("Stad? ");
            string city = Console.ReadLine();

            Console.Write("Region? ");
            string region = Console.ReadLine();

            Console.Write("Postnummer? ");
            string postNumber = Console.ReadLine();

            Console.Write("Land? ");
            string country = Console.ReadLine();

            Console.Write("Telefon? ");
            string phone = Console.ReadLine();

            //Create the customer
            CreateCustomer(orgNumber, name, adress, city, region, postNumber, country, phone);

            WaitForKey();
            
        }

        //Print information about customer based on the customer number
        private void CustomerInfoSearch(int customerNumber)
        {
            //Get info based on owners customer number
            Customer customer = GetCustomerByNumber(customerNumber);
            List<Account> filtredAccounts = AccountsByOwnerNumber(customerNumber);

            //if a customer was found, print info
            if (customer != null)
            {
                //Info about customer
                Console.WriteLine("\nOrganistionsnummer: {0}", customer.OrganisationNumber);
                Console.WriteLine("Namn: {0}", customer.Name);
                Console.WriteLine("Adress: {0}, {1}, {2}, {3}{4}", customer.Address, customer.PostNumber, customer.City,
                    (customer.Region != "" ? $"{customer.Region}, " : ""), customer.Country);
                Console.WriteLine("Telefonnummer: {0}", customer.PhoneNumber);

                decimal sum = 0; //To calulate total sum of customers accounts

                //Info about customers accounts
                Console.WriteLine("\nKonton");
                foreach (var account in filtredAccounts)
                {
                    Console.WriteLine("{0}: {1} kr", account.AccountNumber, account.Balance);
                    sum += account.Balance;
                }
                Console.WriteLine("\nTotalt på alla konton: {0} kr.", sum);

            }
            else
            {
                Console.WriteLine("\nKunde inte hitta kund.");
            }
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

        private void WaitForKey()
        {
            Console.WriteLine("\nTryck valfri tanget för att gå tillbaka till meny...");
            Console.ReadKey();
        }

        //Create new account
        private void CreateAccount(Customer customer)
        {
            //Order the accounts based on account number
            OrderAccounts();

            //Get the number of the last account if there are any accounts, otherwise put last to 9999
            int lastAccountNumber = accounts.Count > 0 ? accounts.LastOrDefault().AccountNumber : 9999;

            //If the last account number has been reached, look for free spaces in list
            if (lastAccountNumber >= 99999)
            {
                int addToNumber = 10000; //What you need to add to i to get the equivalent account number in the loop
                
                //loop through all account numbers to see if spot is free
                for (int i = 0; i+addToNumber < 100_000; i++)
                {
                    //there is a free spot, store the number before the free spot
                    if (accounts[i].AccountNumber != i+addToNumber)
                    {
                        lastAccountNumber = i+addToNumber-1;
                        break;
                    }
                }

                //If no new spot where found
                if (lastAccountNumber >= 99999)
                {
                    Console.WriteLine("\nKontakta admin. Alla kontoplatser är upptagna");
                    return;
                }
            }

            int newAccountNumber = lastAccountNumber + 1;
            
            //Add the account to the list
            AddAccount(newAccountNumber, customer.CustomerNumber, 0);

            //Info that account was created
            Console.WriteLine("\nKonto {0} skapades för kund {1} ({2})", newAccountNumber, customer.CustomerNumber, customer.Name);
        }

        //Create new customer
        private void CreateCustomer(string organisationNumber, string name, string adress,
            string city, string region, string postNumber, string country, string phoneNumber)
        {
            //Order customers by customer number
            OrderCustomers();

            //Find a free customer number, if no customers set last account to 999, to make new account to 1000
            int lastCustomerNumber = customers.Count > 0 ? customers.LastOrDefault().CustomerNumber : 999;

            //If the last customer number has been reached, look for free spaces in list
            if (lastCustomerNumber >= 9999)
            {
                int addToNumber = 1000; //What you need to add to i to get the equivalent customer number in the loop

                //loop through all customer numbers to see if spot is free
                for (int i = 0; i + addToNumber < 100_00; i++)
                {
                    //there is a free spot, store the number before the free spot
                    if (customers[i].CustomerNumber != i + addToNumber)
                    {
                        lastCustomerNumber = i + addToNumber - 1;
                        break;
                    }
                }

                //If no new spot where found
                if (lastCustomerNumber >= 9999)
                {
                    Console.WriteLine("\nKontakta admin. Kundlista är ful.");
                    return;
                }
            }

            int newCustomerNumber = lastCustomerNumber + 1;

            //Add the account to the list
            AddCustomer(newCustomerNumber, organisationNumber, name, adress,
                city, region, postNumber, country, phoneNumber);

            //Info that customer was created
            Console.WriteLine("\nKund {0} skapades", newCustomerNumber);

            //Skapa konto åt kund
            Customer customer = GetCustomerByNumber(newCustomerNumber);
            CreateAccount(customer);

        }

        //Add account to list of accounts
        private void AddAccount(int accountNumber, int ownersCustomerNumber, decimal balance)
        {
            accounts.Add(new Account(accountNumber, ownersCustomerNumber, balance));
        }
        
        //Add customer to the list of customers
        private void AddCustomer(int customerNumber, string organisationNumber, string name, string adress,
            string city, string region, string postNumber, string country, string phoneNumber)
        {
            customers.Add(new Customer(customerNumber, organisationNumber, name, adress, city,
                region, postNumber, country, phoneNumber));
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
            //search for the accounts based on criteria
            var filtredAccounts = accounts.Where(a => a.OwnersCustomerNumber == number).OrderBy(a => a.AccountNumber);
            return filtredAccounts.ToList();
        }

        //Get customer from customers based on customer number
        private Customer GetCustomerByNumber(int number)
        {
            Customer customer = customers.SingleOrDefault(c => c.CustomerNumber == number);
            return customer;
        }

        //Order customers by customer number
        private void OrderCustomers()
        {
            customers = customers.OrderBy(c => c.CustomerNumber).ToList();
        }

        //Order accounts by account number
        private void OrderAccounts()
        {
            accounts = accounts.OrderBy(a => a.AccountNumber).ToList();
        }

        //Get the owner of an account
        private int? GetOwnerOfAccount(int accountNumber)
        {
            Account account = accounts.SingleOrDefault(a => a.AccountNumber == accountNumber);
            return account?.OwnersCustomerNumber;
        }
    }
}
