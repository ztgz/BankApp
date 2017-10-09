using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BankApp.Extensions;
using BankApp.Transactions;

namespace BankApp
{
    class Bank
    {
        private List<Customer> _customers = new List<Customer>();

        private List<Account> _accounts = new List<Account>();

        private Journal _journal; //Keeps track of all transactions that happend

        private Filehandler _filehandler; //Saves and read bank data

        public Bank()
        {
            _journal = new Journal();

            _filehandler = new Filehandler();
           
            //Load data
            _filehandler.LoadData(_customers, _accounts);
        }

        //Calls to save data before bank should be stopped
        public void Close()
        {
            _filehandler.SaveData(_customers, _accounts);
        }

        //Create new account
        public void AccountCreate(int customerNumber)
        {
            if (!customerNumber.ValidCustomerNumber())
            {
                Console.WriteLine("\n{0} är inte ett riktigt kundnummer.", customerNumber);
                return;
            }

            if (GetCustomerByNumber(customerNumber) == null)
            {
                Console.WriteLine("\nKund {0} existerar inte.", customerNumber);
                return;
            }

            //Order the accounts based on account number
            _accounts = _accounts.OrderBy(a => a.AccountNumber).ToList();

            //Get the number of the last account if there are any accounts, otherwise put last to 9999
            int lastAccountNumber = _accounts.Count > 0 ? _accounts.Last().AccountNumber : 9999;

            //If the last account number has been reached, look for free spaces in list
            if (lastAccountNumber >= 99999)
            {
                int addToNumber = 10000; //What you need to add to i to get the equivalent account number in the loop

                //loop through all account numbers to see if spot is free
                for (int i = 0; i + addToNumber < 100_000; i++)
                {
                    //there is a free spot, store the number before the free spot
                    if (_accounts[i].AccountNumber != i + addToNumber)
                    {
                        lastAccountNumber = i + addToNumber - 1;
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

            //Add new account to the list
            _accounts.Add(new Account(newAccountNumber, customerNumber, 0));

            //Info that account was created
            Console.WriteLine("\nKonto {0} skapades för kund {1}", newAccountNumber, customerNumber);
        }

        //Try to remove a specific account
        public void AccountRemove(int accountNumber)
        {
            //!Valid account number or and existing account?
            if (!TestAccountValidity(accountNumber))
                return;

            for (int i = 0; i < _accounts.Count; i++)
            {
                if (_accounts[i].AccountNumber == accountNumber)
                {

                    if (_accounts[i].Balance == 0.0m)
                    {
                        _accounts.RemoveAt(i);
                        Console.WriteLine("\nKonto {0} raderades.", accountNumber);
                    }
                    else
                    {
                        Console.WriteLine("\nKunde inte ta bort konto {0}, kontots saldo är ej 0.", accountNumber);
                    }

                    break;
                }
            }

            /*
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
            */
        }

        //Set the debt and credit of an account
        public void AccountSetCredit(int accountNumber, decimal creditLimit, decimal debtInterest)
        {
            //!Valid account number or and existing account?
            if(!TestAccountValidity(accountNumber))
                return;

            //Get account
            Account account = GetAccount(accountNumber);

            //Change creditlimit
            string text = account.SetCreditLimit(creditLimit);
            Console.WriteLine(text);

            //Change interest
            text = account.SetDebtInterest(debtInterest);
            Console.WriteLine(text);

        }

        //Set saving interest for an account
        public void AccountSetSavingInterest(int accountNumber, decimal interest)
        {
            if (TestAccountValidity(accountNumber))
            {
                //Try to set saving interest to account, returns a message
                string text = GetAccount(accountNumber).SetSavingInterest(interest);
                Console.WriteLine(text);
            }
        }

        //changes the save format
        public void ChangeSaveFormat()
        {
            _filehandler.ChangeFormat();
        }

        //Create new customer
        public void CustomerCreate(string organisationNumber, string name, string adress,
            string city, string region, string postNumber, string country, string phoneNumber)
        {
            //Order customers by customer number
            _customers = _customers.OrderBy(c => c.CustomerNumber).ToList();

            //Find a free customer number, if no customers set last account to 999, to make new account to 1000
            int lastCustomerNumber = _customers.Count > 0 ? _customers.Last().CustomerNumber : 999;

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
                    Console.WriteLine("\nKontakta admin. Kundlista är full.");
                    return;
                }
            }

            int newCustomerNumber = lastCustomerNumber + 1;

            //Add the Customer to the list
            _customers.Add(new Customer(newCustomerNumber, organisationNumber, name, adress, 
                city, region, postNumber, country, phoneNumber));

            //Info that customer was created
            Console.WriteLine("\nKund {0} skapades", newCustomerNumber);

            //Skapa konto åt kund
            Customer customer = GetCustomerByNumber(newCustomerNumber);
            AccountCreate(customer.CustomerNumber);
        }

        //Get detailed info on customer
        public void CustomerInfo(int searchNumber)
        {
            //If search is an account number, look for owner of the account
            if (searchNumber.ValidAccountNumber())
            {
                //If the account dosen't exist, return
                if (!AccountExist(searchNumber))
                {
                    Console.WriteLine("\nKonto {0} existerar inte.", searchNumber);
                    return;
                }

                //Set searchnumber to the account owners customer number
                searchNumber = GetAccount(searchNumber).OwnersCustomerNumber;
            }
            //If it's neither account or customer number
            else if (!searchNumber.ValidCustomerNumber())
            {
                Console.WriteLine("\n{0} är varken ett konto eller kundnummer.", searchNumber);
                return;
            }

            //Get info based on owners customer number
            Customer customer = GetCustomerByNumber(searchNumber);

            //if a customer was found, print info
            if (customer != null)
            {
                List<Account> filtredAccounts = GetAccountsByCustomerNumber(customer.CustomerNumber);

                customer.PrintCustomer();

                decimal sum = 0; //To calulate total sum of customers accounts

                Console.WriteLine("\nKonton");
                foreach (var account in filtredAccounts)
                {
                    account.PrintAccount();
                    sum += account.Balance;
                }

                Console.WriteLine("\nTotalt på alla konton: {0:0.00} kr.", sum);
            }
            else
            {
                Console.WriteLine("\nKunde inte hitta kund.");
            }
        }

        //Remove Customer
        public void CustomerRemove(int customerNr)
        {
            //Check if it's not a valid customer number
            if (!customerNr.ValidCustomerNumber())
            {
                Console.WriteLine("\n{0} är inte ett kundnummer.", customerNr);
                return;
            }

            //Get the customer
            Customer customer = GetCustomerByNumber(customerNr);

            //If customer doesn't exist
            if (customer == null)
            {
                Console.WriteLine("\nKunde inte hitta angiven kund.");
                return;
            }

            //Check that all the accounts of the customer have a zero balance
            List<Account> customerAccounts = GetAccountsByCustomerNumber(customerNr);

            //Check if it's possible to remove all customer accounts
            foreach (var account in customerAccounts)
            {
                if (account.Balance != 0)
                {
                    Console.WriteLine("\nKundens alla konton har inte 0 kr i saldo. Kan ej ta bort kund.");
                    return;
                }
            }

            //It's okay to remove customer and all the accounts
            foreach (var account in customerAccounts)
            {
                AccountRemove(account.AccountNumber);
            }

            _customers = _customers.Where(c => c.CustomerNumber != customerNr).ToList();

            Console.WriteLine("\nKund {0} har raderats.", customerNr);
        }

        //Add/take interest from the accounts
        public void DailyInterest()
        {
            int i = 0;

            foreach (var account in _accounts)
            {
                Transaction transaction = account.AddDailyInterest();
                if (transaction != null)
                {
                    _journal.AddTransaction(transaction);
                    i++;
                }
            }

            Console.WriteLine("\nDaglig ränta utförd på {0} konton.", i);
        }

        //Deposit to account
        public void Deposit(int accountNumber, decimal amount)
        {
            //Test if valid account number and account exist
            if(!TestAccountValidity(accountNumber))
                return;

            if (amount <= 0)
            {
                Console.WriteLine("\nBelopp måste vara positivt");
                return;
            }

            //Try to deposit, if succesfull - returns an transaction
            Transaction transaction = GetAccount(accountNumber).Deposit(amount);

            if (transaction != null)
            {
                //Add to journal
                _journal.AddTransaction(transaction);
                Console.WriteLine("\nEn insättning på {0:0.00} kr till konto {1} lyckades.", amount, accountNumber);
            }
            else
            {
                Console.WriteLine("\nEn insättning på {0:0.00} kr till konto {1} lyckades inte.", amount, accountNumber);
            }
        }

        //Print the daily transactions
        public void PrintDailyTransactions()
        {
            _journal.PrintDailyTransactions();
        }

        //Print transactions from a specific account
        public void PrintTransactions(int accountNumber)
        {
            if (!accountNumber.ValidAccountNumber())
            {
                Console.WriteLine("\n{0} är inte ett korrekt kontonummer.", accountNumber);
            }
            else
            {
                _journal.PrintTransactions(accountNumber);
            }
        }

        //Get customers info based on name or city
        public void SearchCustomers(string search)
        {
            //Make search to lowercase to lower
            search = search.ToLower();

            //Filter on name and city (The search is lowercase)
            var filtredCustomers = _customers.Where(c => c.Name.ToLower().Contains(search)
                                                         || c.City.ToLower().Contains(search));
            
            //Print the filtred list
            Console.WriteLine("\nKundnr| Kund");
            foreach (var filtredCustomer in filtredCustomers)
            {
                Console.WriteLine(" {0} | {1}", filtredCustomer.CustomerNumber, filtredCustomer.Name);
            }
        }

        //Transferbetween two accounts
        public void Transfer(int fromAccountNumber, int toAccountNumber, decimal amount)
        {
            //Control that it's different accounts
            if (fromAccountNumber == toAccountNumber)
            {
                Console.WriteLine("\nDu måste ange olika kontonummer.");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("\nDu kan bara överföra positiva belopp.");
                return;
            }

            //Test account number an see if account exist
            if(!TestAccountValidity(fromAccountNumber) || !TestAccountValidity(toAccountNumber))
                return;

            //Get accounts
            Account senderAcc = GetAccount(fromAccountNumber);
            Account recivingAcc = GetAccount(toAccountNumber);

            //Try to transfer from sendingaccount to reciving account
            Transaction transaction = recivingAcc.Transfer(senderAcc, amount);

            //If withdrawal was succesfull
            if (transaction != null)
            {
                _journal.AddTransaction(transaction);

                Console.WriteLine("\nEn överföring på {0:0.00} kr från konto {1} till konto {2} lyckades",
                    amount, senderAcc.AccountNumber, recivingAcc.AccountNumber);
            }
            else
            {
                Console.WriteLine("\nTransaktion genomfördes ej.");
                Console.WriteLine("Kan max överföra {0:0.00} kr från konto {1}.", senderAcc.MaxPossibleWithdraw(), senderAcc.AccountNumber);
            }
        }

        //Withdraw from account
        public void Withdraw(int accountNumber, decimal amount)
        {
            //Test if valid account number and account exist
            if (!TestAccountValidity(accountNumber))
                return;

            if (amount <= 0)
            {
                Console.WriteLine("\nBelopp måste anges som en positiv summa.");
                return;
            }

            //Try to withdraw, if successful - returns an transaction
            Account account = GetAccount(accountNumber);
            Transaction transaction = account.Withdraw(amount);

            if (transaction != null)
            {
                //Add to journal
                _journal.AddTransaction(transaction);
                Console.WriteLine("\nEtt uttag på {0:0.00} kr från konto {1} lyckades.", amount, accountNumber);
            }
            else
            {
                Console.WriteLine("\nEtt uttag på {0:0.00} kr från konto {1} lyckades inte.", amount, accountNumber);
                Console.WriteLine("Kan max ta ut {0:0.00} kr.", account.MaxPossibleWithdraw());
            }
        }

        //Check if a specific account
        private bool AccountExist(int accountNumber)
        {
            foreach (var account in _accounts)
            {
                if (account.AccountNumber == accountNumber)
                    return true;
            }

            return false;
        }

        //Get specific account based on accountnumber
        private Account GetAccount(int accountNumber)
        {
            return _accounts.SingleOrDefault(a => a.AccountNumber == accountNumber);
        }

        //Filter the accounts list by owner
        private List<Account> GetAccountsByCustomerNumber(int customerNumber)
        {
            //search for the accounts based on criteria
            var filtredAccounts = _accounts.Where(a => a.OwnersCustomerNumber == customerNumber)
                .OrderBy(a => a.AccountNumber);
            return filtredAccounts.ToList();
        }

        //Get customer by customer number
        private Customer GetCustomerByNumber(int customerNumber)
        {
            Customer customer = _customers.SingleOrDefault(c => c.CustomerNumber == customerNumber);
            return customer;
        }

        //Test if accountnumber is valid and exist
        private bool TestAccountValidity(int accountNumber)
        {
            if (!accountNumber.ValidAccountNumber())
            {
                Console.WriteLine("\n{0} är inte ett riktigt kontonummer.", accountNumber);
                return false;
            }

            if (!AccountExist(accountNumber))
            {
                Console.WriteLine("\nKonto med kontonummer {0} existerar inte.", accountNumber);
                return false;
            }

            return true;
        }
    }
}