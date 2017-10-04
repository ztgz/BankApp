using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    class Account
    {
        public int AccountNumber { get; private set; }
        
        public int OwnersCustomerNumber { get; private set; }

        public decimal Balance { get; private set; }

        private decimal _saveInterest;

        private decimal _debtInterest;

        private decimal _creditLimit;

        private decimal _debtLimit;

        public Account(int accountNumber, int ownersCustomerNumber, decimal balance)
        {
            AccountNumber = accountNumber;
            OwnersCustomerNumber = ownersCustomerNumber;
            Balance = balance;
        }
    }
}
