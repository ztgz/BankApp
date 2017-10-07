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
        private const string fileName = @"files\bankdata.txt";

        public void LoadData(List<Customer> customers, List<Account> accounts)
        {
            //Removev all current customers and accounts
            customers.Clear();
            accounts.Clear();

            Encoding win1252 = Encoding.GetEncoding("Windows-1252");

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

                PrintStatistics(customers,accounts);

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

            return new Account(accountNumber, ownersCustomerNumber, balance);
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
