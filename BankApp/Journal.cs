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

    }
}
