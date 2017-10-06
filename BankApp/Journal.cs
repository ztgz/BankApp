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

        public void Deposit(int toAccount, decimal amount, decimal accountBalance)
        {
            Transaction transaction = new DepositTransaction(toAccount, amount, accountBalance);
            dailyTransactions.Add(transaction);
        }

        public void Withdrawal(int fromAccount, decimal amount, decimal accountBalance)
        {
            Transaction transaction = new WithdrawalTransaction(fromAccount, amount, accountBalance);
            dailyTransactions.Add(transaction);
        }

        public void Transfer(decimal amount, int recivingAccount, decimal recivingAccountBalance,
            int sendingAccount, decimal sendingAccountBalance)
        {
            Transaction transaction = new TransferTransaction(amount, recivingAccount, recivingAccountBalance,
                sendingAccount, sendingAccountBalance);
            dailyTransactions.Add(transaction);
        }

        public void PrintDailyTransactions()
        {
            DateTime date = DateTime.Now;
            
            //Filter it incase a new day started
            dailyTransactions = dailyTransactions.Where(t => t.GetDate().Day == date.Day
                                                             && t.GetDate().Month == date.Month &&
                                                             t.GetDate().Year == date.Year).ToList();
            
            foreach (var trans in dailyTransactions)
            {
                Console.WriteLine(trans.GetInfo());
            }
        }

    }
}
