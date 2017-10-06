using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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

        /*
        public void Deposit(int toAccount, decimal amount, decimal accountBalance)
        {
            Transaction transaction = new DepositTransaction(DateTime.Now, toAccount, amount, accountBalance);
            AddTransaction(transaction);
        }

        public void Withdrawal(int fromAccount, decimal amount, decimal accountBalance)
        {
            Transaction transaction = new WithdrawalTransaction(DateTime.Now, fromAccount, amount, accountBalance);
            AddTransaction(transaction);
        }
        */
        public void Transfer(decimal amount, int recivingAccount, decimal recivingAccountBalance,
            int sendingAccount, decimal sendingAccountBalance)
        {
            Transaction transaction = new TransferTransaction(DateTime.Now, amount, recivingAccount, recivingAccountBalance,
                sendingAccount, sendingAccountBalance);
            AddTransaction(transaction);
        }
        /*
        public void AddInterest(int toAccount, decimal amount, decimal accountBalance)
        {
            Transaction transaction = new SaveInterestTransaction(DateTime.Now, toAccount, amount, accountBalance);
            transaction.SaveTransaction();
            dailyTransactions.Add(transaction);
        }*/

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
