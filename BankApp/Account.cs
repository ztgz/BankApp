using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.Transactions;

namespace BankApp
{
    public class Account
    {
        public int AccountNumber { get; private set; }

        public int OwnersCustomerNumber { get; private set; }

        public decimal Balance { get; private set; }

        public decimal SaveInterest { get; private set; }

        private decimal _debtInterest;

        private decimal _creditLimit;

        public Account(int accountNumber, int ownersCustomerNumber, decimal balance)
        {
            AccountNumber = accountNumber;
            OwnersCustomerNumber = ownersCustomerNumber;
            Balance = balance;

            _creditLimit = 0;
        }

        public void SetSavingInterest(decimal interest)
        {
            if (interest <= 0)
            {
                Console.WriteLine("\nSparräntan måste vara positiv. Ränta {0}% kan inte sättas till konto {1}.",
                    interest, AccountNumber);
                return;
            }

            SaveInterest = interest;
            Console.WriteLine("\nSparräntan är nu {0}% på konto {1}", interest, AccountNumber);
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
            {
                Console.WriteLine("\nKan enbart ta ut positiva belopp från kontot.");
                return 0.0m;
            }

            //Cannot withdraw, balance would be to low
            if (Balance - amount < 0 - _creditLimit)
            {
                Console.WriteLine("\nFinns ej tillräckligt med pengar på konto {0} för begärd överföring ({1} kr).", AccountNumber, amount);
                return 0.0m;
            }

            //It's possible to withdraw the amount
            Balance -= amount;
            return amount;
        }

        public SaveInterestTransaction AddDailyInterest()
        {
            if (SaveInterest > 0)
            {
                //Yearly interest to daily interest eg
                decimal dailyInterest = (decimal)Math.Pow(1 + (double)SaveInterest / 100.0, 1.0 / 365.0)-1;
                decimal amount = Balance * dailyInterest;
                //Add the rent to the account
                Balance += amount;
                Console.WriteLine("Sparränta har adderats till konto {0}.", AccountNumber);

                SaveInterestTransaction transaction = new SaveInterestTransaction(DateTime.Now, AccountNumber, amount, Balance);
                return transaction;
            }

            return null;
        }
    }
}
