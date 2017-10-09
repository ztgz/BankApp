using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    class Filehandler
    {
        //private const string fileName = @"files\bankdata-small.txt";
        //private const string fileName = @"files\bankdata.txt";
        private const string fileName = @"files\20171009-1711.txt";
        private Encoding win1252;

        private bool _detailed; //If saving in detailed (new format) or not detailed (old format)

        public Filehandler()
        {
            win1252 = Encoding.GetEncoding("Windows-1252");

            _detailed = false;
        }

        public void LoadData(List<Customer> customers, List<Account> accounts)
        {
            //Removev all current customers and accounts
            customers.Clear();
            accounts.Clear();

            StreamReader reader = null;

            try
            {
                Console.WriteLine("Läser in data ifrån {0}", fileName.Substring(6));

                reader = new StreamReader(fileName, win1252);

                //Number of customers from first line
                int numCustomers = int.Parse(reader.ReadLine());

                //Load all customers
                for (int i = 0; i < numCustomers; i++)
                {
                    string line = reader.ReadLine();

                    customers.Add(CustomerCreate(line));
                }
                //Number of accounts from line
                int numAccounts = int.Parse(reader.ReadLine());

                //Load account
                for (int i = 0; i < numAccounts; i++)
                {
                    string line = reader.ReadLine();
                    accounts.Add(AccountCreate(line));
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Kan inte ladda data. Fil kunde inte hittas.");
                Console.ReadLine();
            }
            catch (IOException)
            {
                Console.WriteLine("Kan inte ladda data. Fil kunde inte öppnas.");
                Console.ReadLine();
            }
            catch (Exception)
            {
                Console.WriteLine("Något gick fel när data laddades.");
                Console.ReadLine();
            }
            finally
            {
                reader?.Close();
            }

            PrintStatistics(customers, accounts);

        }

        //public void SaveData(List<Customer> customers, List<Account> accounts)
        //{
        //    string file = @"files\" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".txt";

        //    Console.WriteLine("Sparar " + file.Substring(6));

        //    using (StreamWriter writer = new StreamWriter(file, false, win1252))
        //    {
        //        int numberOfCustomers = customers.Count;
        //        int numberOfAccounts = accounts.Count;

        //        writer.WriteLine(numberOfCustomers);

        //        foreach (var customer in customers)
        //        {
        //            writer.WriteLine(customer.ToString());
        //        }

        //        writer.WriteLine(numberOfAccounts);

        //        foreach (var account in accounts)
        //        {
        //            writer.WriteLine(account.ToSaveFormat(_detailed));
        //        }

        //        /*for (int i = 0; i < customers.Count; i++)
        //        {
        //            writer.WriteLine(customers[i].ToString());
        //        }

        //        writer.WriteLine(numberOfAccounts);

        //        for (int i = 0; i < accounts.Count; i++)
        //        {
        //            writer.WriteLine(accounts[i].ToSaveFormat(_detailed));
        //        }*/

        //    }

        //    PrintStatistics(customers, accounts);
        //}

        public void SaveData(List<Customer> customers, List<Account> accounts)
        {
            string file = @"files\" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".txt";

            StreamWriter writer = null;

            try
            {
                Console.WriteLine("Sparar " + file.Substring(6));

                writer = new StreamWriter(file, false, win1252);

                int numberOfCustomers = customers.Count;
                int numberOfAccounts = accounts.Count;

                writer.WriteLine(numberOfCustomers);

                foreach (var customer in customers)
                {
                    writer.WriteLine(customer.ToString());
                }

                writer.WriteLine(numberOfAccounts);

                foreach (var account in accounts)
                {
                    writer.WriteLine(account.ToSaveFormat(_detailed));
                }

            }
            catch (Exception)
            {
                Console.WriteLine("\nNågot gick fel när fil sparades.");
            }
            finally
            {
                writer?.Close();
            }

            PrintStatistics(customers, accounts);

        }

        public void ChangeFormat()
        {
            _detailed = !_detailed;
            if (_detailed)
                Console.WriteLine("\nDu sparar nu i det nya och mer detaljerade formatet.");
            else
                Console.WriteLine("\nDu sparar nu i det gamla formatet.");
        }

        private Customer CustomerCreate(string line)
        {            
            string[] parameters = line.Split(';');

            //What is loaded
            int customerNumber = int.Parse(parameters[0]);
            string orgNumber = parameters[1];
            string name = parameters[2];
            string adress = parameters[3];
            string city = parameters[4];
            string region = parameters[5];
            string postNumber = parameters[6];
            string country = parameters[7];
            string phoneNumber = parameters[8];

            return new Customer(customerNumber, orgNumber, name, adress, city, 
                region, postNumber, country, phoneNumber);
        }

        private Account AccountCreate(string line)
        {
            string[] parameters = line.Split(';');

            //What is loaded
            int accountNumber = int.Parse(parameters[0]);
            int ownersCustomerNumber = int.Parse(parameters[1]);
            decimal balance = decimal.Parse(parameters[2], CultureInfo.InvariantCulture);

            Account account = new Account(accountNumber, ownersCustomerNumber, balance);

            //if data is in detailed format
            if (parameters.Length == 6)
            {
                decimal saveInterest = decimal.Parse(parameters[3], CultureInfo.InvariantCulture);
                decimal debtInterest = decimal.Parse(parameters[4], CultureInfo.InvariantCulture);
                decimal creditLimit = decimal.Parse(parameters[5], CultureInfo.InvariantCulture);


                account.SetSavingInterest(saveInterest);
                account.SetDebtInterest(debtInterest);
                account.SetCreditLimit(creditLimit);
            }

            return account;
        }

        private void PrintStatistics(List<Customer> customers, List<Account> accounts)
        {

            decimal sum = 0;
            foreach (var account in accounts)
            {
                sum += account.Balance;
            }

            Console.WriteLine("\nAntal kunder: {0}", customers.Count);
            Console.WriteLine("Antal konton: {0}", accounts.Count);
            Console.WriteLine("Total balans på konton: {0} kr", sum);

            Console.WriteLine("\nTryck på valfri tangent för att gå vidare...");
            Console.ReadKey();
        }

    }
}
