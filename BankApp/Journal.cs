using System;
using System.Collections.Generic;
using System.Linq;
using BankApp.Transactions;

namespace BankApp
{
    class Journal
    {
        private List<Transaction> dailyTransactions;
        public Journal()
        {
            dailyTransactions = new List<Transaction>();
        }

        public void AddTransaction(Transaction transaction)
        {
            dailyTransactions.Add(transaction);
            transaction.SaveTransaction();
        }

        public void PrintDailyTransactions()
        {
            DateTime date = DateTime.Now;
            
            //Filter it incase that the system has been running for more than one day
            dailyTransactions = dailyTransactions.Where(t => t.GetDate().Day == date.Day
                                                             && t.GetDate().Month == date.Month &&
                                                             t.GetDate().Year == date.Year).ToList();
            Console.WriteLine();
            foreach (var trans in dailyTransactions)
            {
                Console.WriteLine(trans.GetInfo());
            }
        }

        public void PrintTransactions(int accountNumber)
        {
            /* Om bara dagliga transaktioner ska användas
            var transactions = dailyTransactions.Where(t => t.RecivingAccount == accountNumber || t.SendingAccount == accountNumber);
            */

            var transactions = Transaction.GetTransactionsHistory()
                .Where(t => t.RecivingAccount == accountNumber || t.SendingAccount == accountNumber);

            Console.WriteLine("\n-Transactions-");
            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction.GetInfo());
            }
        }
        
    }
}
