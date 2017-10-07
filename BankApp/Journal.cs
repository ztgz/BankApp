using System;
using System.Collections.Generic;
using System.Linq;
using BankApp.Transactions;

namespace BankApp
{
    class Journal
    {
        private List<Transaction> _dailyTransactions;

        public Journal()
        {
            _dailyTransactions = new List<Transaction>();
        }

        //Store the transaction in the journal
        public void AddTransaction(Transaction transaction)
        {
            _dailyTransactions.Add(transaction);
            transaction.SaveTransaction();
        }

        public void PrintDailyTransactions()
        {
            DateTime date = DateTime.Now;
            
            //Filter it incase that the system has been running for more than one day
            _dailyTransactions = _dailyTransactions.Where(t => t.GetDate().Day == date.Day
                                                             && t.GetDate().Month == date.Month &&
                                                             t.GetDate().Year == date.Year).ToList();
            Console.WriteLine();
            foreach (var trans in _dailyTransactions)
            {
                Console.WriteLine(trans.GetInfo());
            }
        }

        public void PrintTransactions(int accountNumber)
        {
            /* Om bara dagliga transaktioner ska användas
            var transactions = _dailyTransactions.Where(t => t.RecivingAccount == accountNumber || t.SendingAccount == accountNumber);
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
