using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Account
    {
        public int AccountNumber { get; private set; }
        
        public int OwnersCustomerNumber { get; private set; }

        public decimal Balance { get; private set; }

        private decimal _saveInterest;

        private decimal _debtInterest;

        private decimal _creditLimit;

        public Account(int accountNumber, int ownersCustomerNumber, decimal balance)
        {
            AccountNumber = accountNumber;
            OwnersCustomerNumber = ownersCustomerNumber;
            Balance = balance;

            _creditLimit = 0;
        }

        public bool Deposit(decimal amount)
        {
            //Cannot deposit a nonpositive number
            if (amount <= 0)
                return false;

            Balance += amount;
            return true;
        }

        public decimal WithdrawRequest(decimal amount)
        {
            //No positive amounts cannot be withdrawn from account
            if (amount <= 0.0m)
                return 0.0m;

            //Cannot withdraw, balance would be to low
            if (Balance - amount < 0 - _creditLimit)
                return 0.0m;

            //It's possible to withdraw the amount
            Balance -= amount;
            return amount;
        }
    }
}
