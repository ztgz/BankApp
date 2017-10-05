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

        public void Deposit(int toAccount, decimal amount, decimal balance)
        {
            Transaction transaction = new DepositTransaction(toAccount, amount, balance);
            dailyTransactions.Add(transaction);
        }
    }
}
