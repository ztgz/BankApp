using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.Extensions;

namespace BankApp
{
    class Menu
    {
        private Bank bank;

        public Menu()
        {
            bank = new Bank();
        }

        public void StartMenu()
        {
            //Becomes true when application is exiting
            bool exitApp = false;

            //While bank is running
            do
            {
                Console.Clear();

                PrintMainMenu();

                //What is choosen in the main menu
                int choise = ReadFromKeyboard.GetInt();

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
                        DaliyInterestMenu();
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
                if (!exitApp)
                {
                    Console.WriteLine("\nTryck valfri tangent för att gå tillbaka till meny...");
                    Console.ReadKey();
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
            Console.WriteLine("4) Ta bort kund");
            Console.WriteLine("5) Lägg till konto");
            Console.WriteLine("6) Ta bort konto");
            Console.WriteLine("7) Insättning på konto");
            Console.WriteLine("8) Uttag från konto");
            Console.WriteLine("9) Överföring mellan konton");
            Console.WriteLine("10) Daglig transaktions historik");
            Console.WriteLine("11) Visa transaktioner för konto");
            Console.WriteLine("12) Ange sparränta på konto");
            Console.WriteLine("13) Daglig ränta");
            Console.WriteLine("14) Ange kreditgräns och skuldränta på konto");
            Console.WriteLine();
        }

        private void AccountCreateMenu()
        {
            Console.Clear();

            //Get customernumber from console
            Console.WriteLine("* Skapa konto för kund *");
            Console.WriteLine("Kundnummer: ");
            int customerNumber = ReadFromKeyboard.GetInt();

            bank.AccountCreate(customerNumber);
        }

        private void AccountRemoveMenu()
        {
            Console.Clear();
            Console.WriteLine("* Ta bort konto *");

            //Get info on which account to remove
            Console.WriteLine("Ange kontonummer:");
            int accountNum = ReadFromKeyboard.GetInt();

            bank.AccountRemove(accountNum);
        }

        private void AccountSetCreditMenu()
        {
            Console.Clear();
            Console.WriteLine("* Kreditgräns *");

            Console.WriteLine("Ändra kredit på konto:");
            int accountNumber = ReadFromKeyboard.GetInt();

            Console.WriteLine("Vilken kreditgräns ska kontot ha:");
            decimal creditLimt = ReadFromKeyboard.GetInt();

            Console.WriteLine("Vilken skuldränta ska kontot ha:");
            decimal debtInterest = ReadFromKeyboard.GetInt();

            bank.AccountSetCredit(accountNumber, creditLimt, debtInterest);
        }

        private void AccountSetInterestMenu()
        {
            Console.Clear();
            Console.WriteLine("* Bestäm sparränta *");

            Console.WriteLine("Ändra ränta på konto:");
            int accountNumber = ReadFromKeyboard.GetInt();
            Console.WriteLine("Sätt årlig sparränta till:");
            decimal interest = ReadFromKeyboard.GetDecimal();

            bank.AccountSetSavingInterest(accountNumber, interest);
        }

        private void AccountTransactionMenu()
        {
            Console.Clear();
            Console.WriteLine("* Transaktioner för konto *");

            Console.WriteLine("Se transaktioner för konto, ange konto:");
            int accountNumber = ReadFromKeyboard.GetInt();

            bank.PrintTransactions(accountNumber);
        }

        private void CustomerCreateMenu()
        {
            Console.Clear();
            Console.WriteLine("* Skapa ny kund *");

            //Get customer number info
            Console.WriteLine("Organisationsnummer?");
            string orgNumber = ReadFromKeyboard.GetString();

            Console.WriteLine("Namn?");
            string name = ReadFromKeyboard.GetString();

            Console.WriteLine("Adress?");
            string adress = ReadFromKeyboard.GetString();

            Console.WriteLine("Stad?");
            string city = ReadFromKeyboard.GetString();

            //It's optional the input region
            Console.Write("Region?\n> ");
            string region = Console.ReadLine();

            Console.WriteLine("Postnummer?");
            string postNumber = ReadFromKeyboard.GetString();

            Console.Write("Land?\n> ");
            string country = Console.ReadLine();

            Console.Write("Telefon?\n> ");
            string phone = Console.ReadLine();

            //Create the customer
            bank.CustomerCreate(orgNumber, name, adress, city, region, postNumber, country, phone);
        }

        private void CustomerInfoMenu()
        {
            Console.Clear();

            //Get input from user
            Console.WriteLine("* Visa kundbild *");
            Console.WriteLine("Ange kundnummer eller kontonummer: ");

            int searchNumber = ReadFromKeyboard.GetInt();

            //Search for the customer
            bank.CustomerInfo(searchNumber);
        }

        private void CustomerRemoveMenu()
        {
            Console.Clear();
            Console.WriteLine("* Ta bort kund *");

            //Get info on which customer to remove
            Console.WriteLine("Ange kundnummer:");
            int customerNum = ReadFromKeyboard.GetInt();

            bank.CustomerRemove(customerNum);
        }

        private void CustomerSearchMenu()
        {
            Console.Clear();
            Console.WriteLine("* Sök kund *");

            //Get search string from keyboard
            Console.Write("Namn eller postort: \n> ");
            string customerSearch = Console.ReadLine();

            bank.SearchCustomers(customerSearch);
        }

        private void DaliyInterestMenu()
        {
            Console.Clear();
            Console.WriteLine("* Daglig ränta *");

            bank.DailyInterest();           
        }

        private void DailyTransactionMenu()
        {
            Console.Clear();
            Console.WriteLine("* Transaktionshistorik *\n");

            bank.PrintDailyTransactions();
        }

        private void DepositMenu()
        {
            Console.Clear();
            Console.WriteLine("* Insättning *");

            Console.WriteLine("Sätt in pengar till konto, ange konto:");
            int accountNumber = ReadFromKeyboard.GetInt();

            Console.WriteLine("Insättningsbelopp:");
            decimal amount = ReadFromKeyboard.GetDecimal();

            bank.Deposit(accountNumber, amount);
        }

        private void TransferMoneyMenu()
        {
            Console.Clear();
            Console.WriteLine("* Överföring mellan konton *");

            Console.WriteLine("Flytta pengar från konto, ange konto: ");
            int fromAccountNumber = ReadFromKeyboard.GetInt();

            Console.WriteLine("Flytta pengar till konto, ange konto: ");
            int toAccountNumber = ReadFromKeyboard.GetInt();

            Console.WriteLine("Vilken summa vill du överföra från konto {0} till konto {1}", 
                fromAccountNumber, toAccountNumber);
            decimal amount = ReadFromKeyboard.GetDecimal();

            //Try to transfer amount
            bank.Transfer(fromAccountNumber, toAccountNumber, amount);

        }

        private void WithdrawalMenu()
        {
            Console.Clear();
            Console.WriteLine("* Uttag *");

            Console.WriteLine("Ta ut pengar ifrån konto, ange konto:");
            int accountNumber = ReadFromKeyboard.GetInt();

            Console.WriteLine("Hur mycket vill du ta ut:");
            decimal amount = ReadFromKeyboard.GetDecimal();

            bank.Withdraw(accountNumber, amount);
        }

        
    }
}
