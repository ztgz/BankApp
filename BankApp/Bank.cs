using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.Transactions;

namespace BankApp
{
    class Bank
    {
        private List<Customer> _customers = new List<Customer>();

        private List<Account> _accounts = new List<Account>();

        private Journal journal; //Keeps track of all transactions that happend

        public Bank()
        {
            Customer cust = new Customer(1005, "559268-7528", "Berglunds snabbköp", "Berguvsvägen  8",
                "Luleå", "", "S-958 22", "Sweden", "0921-12 34 65");
            _customers.Add(cust);

            cust = new Customer(1024, "556392-8406", "Folk och fä HB", "Åkergatan 24",
                "Bräcke", "", "S-844 67", "Sweden", "0695-34 67 21");
            _customers.Add(cust);

            cust = new Customer(1032, "551553-1910", "Great Lakes Food Market", "2732 Baker Blvd.",
                "Eugene", "OR", "97403", "USA", "(503) 555-7555");
            _customers.Add(cust);
            
            //Account account = new Account(13019, 1005, 1488.80m);
            Account account = new Account(13019, 1005, 10000.00m);
            account.SetSavingInterest(2.5m);
            _accounts.Add(account);
            account = new Account(13020, 1005, 613.20m);
            _accounts.Add(account);
            account = new Account(13093, 1024, 695.62m);
            _accounts.Add(account);
            account = new Account(13128, 1032, 392.20m);
            _accounts.Add(account);
            account = new Account(13130, 1032, 4807.00m);
            _accounts.Add(account);
            /*
            account = new Account(13128, 1032, 0m);
            _accounts.Add(account);
            account = new Account(13130, 1032, 0.00m);
            _accounts.Add(account);*/

            journal = new Journal();
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
                    case 3:
                        //Create new customer
                        CustomerCreateMenu();
                        break;
                    case 4:
                        //Remove customer
                        CustomerRemoveMenu();
                        break;
                    case 5:
                        //Create a new account menu
                        AccountCreateMenu();
                        break;
                    case 6:
                        //Create a new account menu
                        AccountRemoveMenu();
                        break;
                    case 7:
                        //Deposit money to an account
                        DepositMenu();
                        break;
                    case 8:
                        //Withdraw money from an account
                        WithdrawalMenu();
                        break;
                    case 9:
                        //Withdraw money from an account
                        TransferMoneyMenu();
                        break;
                    case 10:
                        //The daliy transactions for all accounts
                        DailyTransactionMenu();
                        break;
                    case 11:
                        //All transactions for an account
                        AccountTransactionMenu();
                        break;
                    case 12:
                        //Set interest-rate for an account
                        AccountSetInterestMenu();
                        break;
                    case 13:
                        //Add daily rent to accounts
                        AddDaliyInterestMenu();
                        break;
                    case 14:
                        //Set creditlimit and interest for an account
                        AccountSetCreditMenu();
                        break;
                    case 0:
                        exitApp = true;
                        break;
                }

                //Holds the menu so it dosen't restart
                WaitForKey();

            } while (!exitApp);
        }

        private void PrintMainMenu()
        {
            Console.WriteLine("HUVUDMENY");
            Console.WriteLine("0) Avsluta och spara");
            Console.WriteLine("1) Sök kund");
            Console.WriteLine("2) Visa kundbild");
            Console.WriteLine("3) Skapa kund");
            Console.WriteLine("4) Ta bort kund");
            Console.WriteLine("5) Lägg till konto");
            Console.WriteLine("6) Ta bort konto");
            Console.WriteLine("7) Insättning på konto");
            Console.WriteLine("8) Uttag från konto");
            Console.WriteLine("9) Överföring mellan konton");
            Console.WriteLine("10) Daglig transaktions historik");
            Console.WriteLine("11) Visa transaktioner för konto");
            Console.WriteLine("12) Ange sparränta på konto");
            Console.WriteLine("13) Daglig sparränta");
            Console.WriteLine("14) Ange kreditgräns och skuldränta på konto");
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
        }

        private void CustomerInfoMenu()
        {
            Console.Clear();

            //Get input from user
            Console.WriteLine("* Visa kundbild *");
            Console.WriteLine("Ange kundnummer eller kontonummer: ");

            int searchNumber = ReadIntFromKeyboard();

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
                if (AccountExist(searchNumber))
                {
                    searchNumber = GetOwnerOfAccount(searchNumber).Value;
                }
                else
                {
                    //Account do not exsist. Hold and return to menu
                    WaitForKey();
                    return;
                }
            }

            //Search for customer information
            CustomerInfo(searchNumber);
        }

        private void AccountCreateMenu()
        {
            Console.Clear();

            //Get customernumber from console
            Console.WriteLine("* Skapa konto för kund *");
            Console.Write("Kundnummer? ");
            string strNumber = Console.ReadLine();

            //If input could be parsed to int and the number is four digits
            if (int.TryParse(strNumber, out int custumerNumber) && IsCustomerNumberFormat(custumerNumber))
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
        }

        private void CustomerCreateMenu()
        {
            Console.Clear();
            Console.WriteLine("* Skapa ny kund *");

            //Get customer number info
            Console.WriteLine("Organisationsnummer?");
            string orgNumber = ReadStringFromKeyboard();

            Console.WriteLine("Namn?");
            string name = ReadStringFromKeyboard();

            Console.WriteLine("Adress?");
            string adress = ReadStringFromKeyboard();

            Console.WriteLine("Stad?");
            string city = ReadStringFromKeyboard();

            //It's optional the input region
            Console.WriteLine("Region?");
            string region = Console.ReadLine();

            Console.WriteLine("Postnummer?");
            string postNumber = ReadStringFromKeyboard();

            Console.WriteLine("Land?");
            string country = Console.ReadLine();

            Console.WriteLine("Telefon?");
            string phone = Console.ReadLine();

            //Create the customer
            CreateCustomer(orgNumber, name, adress, city, region, postNumber, country, phone);
        }

        private void CustomerRemoveMenu()
        {
            Console.Clear();
            Console.WriteLine("* Ta bort kund *");
            
            //Get info on which client to remove
            Console.WriteLine("Ange kundnummer");
            int customerNum = ReadIntFromKeyboard();

            //If a correct customer number has been added
            if (IsCustomerNumberFormat(customerNum))
            {
                RemoveCustomer(customerNum);
            }
            else
            {
                Console.WriteLine("\nEtt korrekt kundnummer angavs ej, kundnummer består av fyra stycken siffror (xxxx)");
            }

        }

        private void AccountRemoveMenu()
        {
            Console.Clear();
            Console.WriteLine("* Ta bort konto *");

            //Get info on which account to remove
            Console.WriteLine("Ange kontonummer");
            int accountNum = ReadIntFromKeyboard();

            //If a correct customer number has been added, try to remove account
            if (IsAccountNumberValid(accountNum))
            {
                RemoveAccount(accountNum);
            }
        }

        private void DepositMenu()
        {
            Console.Clear();
            Console.WriteLine("* Insättning *");

            Console.WriteLine("Sätt in pengar till konto, ange konto:");
            int accountNumber = ReadIntFromKeyboard();

            //check if account number is valid && Exsits
            if (IsAccountNumberValid(accountNumber) && AccountExist(accountNumber))
            {
                //Get the account
                Account account = GetAccountByNumber(accountNumber);

                //Get the amount to deposit
                Console.WriteLine("Insättningsbelopp?");
                decimal amount = ReadDecimalFromKeyboard();
                

                if (account.Deposit(amount))
                {
                    //Log the transaction
                    journal.Deposit(account.AccountNumber, amount, account.Balance);

                    Console.WriteLine("\nEn insättning på {0} kr till konto {1} lyckades.", amount, accountNumber);
                }
                else
                {
                    Console.WriteLine("\nEn insättning på {0} kr till konto {1} lyckades inte.", amount, accountNumber);
                }
                
            }

        }

        private void WithdrawalMenu()
        {
            Console.Clear();
            Console.WriteLine("* Uttag *");

            Console.WriteLine("Ta ut pengar ifrån konto, ange konto:");
            int accountNumber = ReadIntFromKeyboard();

            //check if account number is valid && Exsits
            if (IsAccountNumberValid(accountNumber) && AccountExist(accountNumber))
            {
                //Get the account
                Account account = GetAccountByNumber(accountNumber);

                //Get the amount to deposit
                Console.WriteLine("Hur mycket vill du ta ut? (saldo = {0})", account.Balance);
                decimal amount = ReadDecimalFromKeyboard();

                //How much money that were recived
                decimal recivedAmmount = account.WithdrawRequest(amount);

                //If withdrawal was accepted
                if (recivedAmmount > 0)
                {
                    //Log the transaction
                    journal.Withdrawal(account.AccountNumber, recivedAmmount, account.Balance);

                    Console.WriteLine("\nEtt uttag på {0} kr från konto {1} genomfördes.", recivedAmmount, accountNumber);
                }
            }
        }

        private void TransferMoneyMenu()
        {
            Console.Clear();
            Console.WriteLine("* Överföring mellan konton *");

            //User need to specify from which account
            Console.WriteLine("Flytta pengar från konto, ange konto: ");
            int fromAccountNumber = ReadIntFromKeyboard();

            //check if it's a valid account number
            if (IsAccountNumberValid(fromAccountNumber))
            {
                Console.WriteLine("Flytta pengar till konto, ange konto: ");
                int toAccountNumber = ReadIntFromKeyboard();

                //if the account is valid
                if (IsAccountNumberValid(toAccountNumber))
                {
                    Console.WriteLine("Vilken summa vill du överföra från konto {0} till konto {1}", fromAccountNumber, toAccountNumber);
                    decimal amount = ReadDecimalFromKeyboard();

                    TransferBetweenAccounts(fromAccountNumber, toAccountNumber, amount);
                }
            }
        }

        private void DailyTransactionMenu()
        {
            Console.Clear();
            Console.WriteLine("* Transaktionshistorik *\n");

            journal.PrintDailyTransactions();

        }

        private void AccountTransactionMenu()
        {
            Console.Clear();
            Console.WriteLine("* Transaktioner för konto *");

            Console.WriteLine("Se transaktioner för konto, ange konto:");
            int accountNumber = ReadIntFromKeyboard();

            Console.WriteLine();
            //check if the accountnumber is valid an exists
            if (IsAccountNumberValid(accountNumber) && AccountExist(accountNumber))
            {
                journal.PrintTransactions(accountNumber);
            }

        }

        private void AccountSetInterestMenu()
        {
            Console.Clear();
            Console.WriteLine("* Bestäm sparränta *");

            Console.WriteLine("Ändra ränta på konto:");
            int accountNumber = ReadIntFromKeyboard();
            Console.WriteLine("Sätt årlig sparränta till:");
            decimal interest = ReadDecimalFromKeyboard();

            if (IsAccountNumberValid(accountNumber) && AccountExist(accountNumber))
            {
                //Get account
                Account account = GetAccountByNumber(accountNumber);
                //Change interest
                account.SetSavingInterest(interest);
            }

        }

        private void AddDaliyInterestMenu()
        {
            Console.Clear();
            Console.WriteLine("* Daglig sparränta *");

            int i = 0;
            foreach (var account in _accounts)
            {
                Transaction transaction = account.AddDailyInterest();
                if (transaction != null)
                {
                    journal.AddTransaction(transaction);
                    i++;
                }
            }

            Console.WriteLine("\nDaglig sparränta tillagd på {0} konton.", i);
        }

        private void AccountSetCreditMenu()
        {
            Console.Clear();
            Console.WriteLine("* Kreditgräns *");

            Console.WriteLine("Ändra kredit på konto:");
            int accountNumber = ReadIntFromKeyboard();

            Console.WriteLine("Vilken kreditgräns ska kontot ha:");
            decimal creditLimt = ReadDecimalFromKeyboard();

            Console.WriteLine("Volken skuldränta ska kontot ha:");
            decimal debtInterest = ReadDecimalFromKeyboard();

            if (IsAccountNumberValid(accountNumber))
            {
                //Get account
                Account account = GetAccountByNumber(accountNumber);
                //Change creditlimit
                account.SetCreditLimit(creditLimt);
                //Change interest
                account.SetDebtInterest(debtInterest);
            }
        }

        private void TransferBetweenAccounts(int fromAccount, int toAccount, decimal amount)
        {
            //Control that it's different accounts
            if (fromAccount == toAccount)
            {
                Console.WriteLine("\nDu måste ange olika kontonummer");
                return;
            }

            //Get accounts
            Account accountSender = GetAccountByNumber(fromAccount);
            Account accountReciving = GetAccountByNumber(toAccount);
            
            //Control that sending and reciving account exist
            if (accountSender == null || accountReciving == null)
            {
                if(accountSender == null)
                    Console.WriteLine("\nKonto {0} finns inte.", fromAccount);
                if (accountReciving == null)
                    Console.WriteLine("\nKonto {0} finns inte.", toAccount);
                return;
            }

            decimal money = accountSender.WithdrawRequest(amount);

            if (money > 0)
            {
                accountReciving.Deposit(money);

                //create transaction journal
                journal.Transfer(money, accountReciving.AccountNumber, accountReciving.Balance,
                    accountSender.AccountNumber, accountSender.Balance);

                Console.WriteLine("\nÖverföring lyckades.");
            }
        }

        //Returns true if number is of customer number format
        private bool IsCustomerNumberFormat(int number)
        {
            return number > 999 && number < 10000;
        }

        public bool IsAccountNumberValid(int accountNumber)
        {
            if (accountNumber > 9999 && accountNumber < 100_000)
            {
                return true;
            }

            Console.WriteLine("\nEtt korrekt kontonummer angavs ej, kontonummer består av fem stycken siffror (xxxxx)");
            return false;
        }

        //Print information about customer based on the customer number
        private void CustomerInfo(int customerNumber)
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
                    Console.Write("{0}: {1} kr", account.AccountNumber, account.Balance);
                    if(account.SaveInterest > 0)
                        Console.Write(", sparränta {0}%", account.SaveInterest);

                    Console.WriteLine();
                    sum += account.Balance;
                }
                Console.WriteLine("\nTotalt på alla konton: {0} kr.", sum);

            }
            else
            {
                Console.WriteLine("\nKunde inte hitta kund.");
            }
        }

        //Forces the user to input a number
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

        //Force the user to input a decimalnumber
        private decimal ReadDecimalFromKeyboard()
        {

            do
            {
                Console.Write("> ");
                string ammountStr = Console.ReadLine();

                if (decimal.TryParse(ammountStr, out decimal amount))
                    return amount;

            } while (true);
        }

        //Infomessage and wait for a keypress to continue
        private void WaitForKey()
        {
            Console.WriteLine("\nTryck valfri tanget för att gå tillbaka till meny...");
            Console.ReadKey();
        }

        //forces the user to input non-empty string
        private string ReadStringFromKeyboard()
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

        //Create new account
        private void CreateAccount(Customer customer)
        {
            //Order the accounts based on account number
            OrderAccounts();

            //Get the number of the last account if there are any accounts, otherwise put last to 9999
            int lastAccountNumber = _accounts.Count > 0 ? _accounts.LastOrDefault().AccountNumber : 9999;

            //If the last account number has been reached, look for free spaces in list
            if (lastAccountNumber >= 99999)
            {
                int addToNumber = 10000; //What you need to add to i to get the equivalent account number in the loop
                
                //loop through all account numbers to see if spot is free
                for (int i = 0; i+addToNumber < 100_000; i++)
                {
                    //there is a free spot, store the number before the free spot
                    if (_accounts[i].AccountNumber != i+addToNumber)
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
            int lastCustomerNumber = _customers.Count > 0 ? _customers.LastOrDefault().CustomerNumber : 999;

            //If the last customer number has been reached, look for free spaces in list
            if (lastCustomerNumber >= 9999)
            {
                int addToNumber = 1000; //What you need to add to i to get the equivalent customer number in the loop

                //loop through all customer numbers to see if spot is free
                for (int i = 0; i + addToNumber < 100_00; i++)
                {
                    //there is a free spot, store the number before the free spot
                    if (_customers[i].CustomerNumber != i + addToNumber)
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
            _accounts.Add(new Account(accountNumber, ownersCustomerNumber, balance));
        }
        
        //Add customer to the list of customers
        private void AddCustomer(int customerNumber, string organisationNumber, string name, string adress,
            string city, string region, string postNumber, string country, string phoneNumber)
        {
            _customers.Add(new Customer(customerNumber, organisationNumber, name, adress, city,
                region, postNumber, country, phoneNumber));
        }

        //Try to remove a specific customer
        private void RemoveCustomer(int customerNr)
        {
            //First see if the customer exist
            Customer customer = GetCustomerByNumber(customerNr);

            if (customer == null)
            {
                Console.WriteLine("Kunde inte hitta angiven kund");
            }

            //Check that all the accounts of the customer have a zero balance
            List<Account> customerAccounts = AccountsByOwnerNumber(customerNr);

            //Check if it's possible to remove all customer accounts
            foreach (var account in customerAccounts)
            {
                if (account.Balance != 0)
                {
                    Console.WriteLine("\nKundens konton har inte 0 i saldo. Kan ej ta bort kund.");
                    return;
                }
            }

            //It's okay to reomve customer and all its accounts
            foreach (var account in customerAccounts)
            {
                RemoveAccount(account.AccountNumber);
            }

            _customers = _customers.Where(c => c.CustomerNumber != customerNr).ToList();

            Console.WriteLine("\nKund {0} har raderats", customerNr);
        }

        //Try to remove a specific account
        private void RemoveAccount(int accountNumber)
        {
            //If account exsits
            if (AccountExist(accountNumber))
            {
                int numberOfAccounts = _accounts.Count;

                //Keep all accounts except if it has the specified account number and zero balance
                _accounts = _accounts.Where(a => a.AccountNumber != accountNumber || a.Balance != 0).ToList();

                if (numberOfAccounts > _accounts.Count)
                {
                    Console.WriteLine("\nKonto {0} raderades.", accountNumber);
                }
                else
                {
                    Console.WriteLine("\nKunde inte ta bort konto {0}, kontots saldo är ej 0.", accountNumber);
                }
            }
            /*else
            {
                Console.WriteLine("\nKonto {0} existerar inte.", accountNumber);
            }*/

        }

        //Filter the customer list based on if name or city contains the string
        private List<Customer> CustomersByNameOrCity(string customerInfo)
        {
            //Make customerinfo to lower
            customerInfo = customerInfo.ToLower();

            //Filter on name and city (The search is lowercase)
            var filtredCustomers = _customers.Where(c => c.Name.ToLower().Contains(customerInfo) ||
                                                        c.City.ToLower().Contains(customerInfo));

            //Return the filtred customers as a list
            return filtredCustomers.ToList();
        }

        //Filter the accounts list by owner
        private List<Account> AccountsByOwnerNumber(int number)
        {
            //search for the accounts based on criteria
            var filtredAccounts = _accounts.Where(a => a.OwnersCustomerNumber == number).OrderBy(a => a.AccountNumber);
            return filtredAccounts.ToList();
        }

        //Get customer from customers based on customer number
        private Customer GetCustomerByNumber(int number)
        {
            Customer customer = _customers.SingleOrDefault(c => c.CustomerNumber == number);
            return customer;
        }

        //Get specific account based on accountnumber
        private Account GetAccountByNumber(int number)
        {
            return _accounts.SingleOrDefault(a => a.AccountNumber == number);
        }

        //Order customers by customer number
        private void OrderCustomers()
        {
            _customers = _customers.OrderBy(c => c.CustomerNumber).ToList();
        }

        //Order accounts by account number
        private void OrderAccounts()
        {
            _accounts = _accounts.OrderBy(a => a.AccountNumber).ToList();
        }

        //Get the owner of an account
        private int? GetOwnerOfAccount(int accountNumber)
        {
            Account account = _accounts.SingleOrDefault(a => a.AccountNumber == accountNumber);
            return account?.OwnersCustomerNumber;
        }

        //Returns true if an account exsits
        private bool AccountExist(int accountNumber)
        {
            foreach (var account in _accounts)
            {
                if (account.AccountNumber == accountNumber)
                    return true;
            }
            Console.WriteLine("\nKonto {0} existerar inte.", accountNumber);
            return false;
        }
    }
}
