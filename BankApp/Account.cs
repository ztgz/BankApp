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

        public decimal DebtInterest { get; private set; }

        public decimal CreditLimit { get; private set; }

        public Account(int accountNumber, int ownersCustomerNumber, decimal balance)
        {
            AccountNumber = accountNumber;
            OwnersCustomerNumber = ownersCustomerNumber;
            Balance = balance;

            CreditLimit = 0;
        }

        public void SetSavingInterest(decimal interest)
        {
            if (interest <= 0)
            {
                Console.WriteLine("\nSparräntan måste vara positiv. Ränta {0}% kan inte sättas till konto {1}.",
                    interest, AccountNumber);
            }
            else
            {
                SaveInterest = interest;
                Console.WriteLine("\nSparräntan är nu {0}% på konto {1}", SaveInterest, AccountNumber);
            }

        }

        public void SetDebtInterest(decimal interest)
        {
            if (interest < 0)
            {
                Console.WriteLine("\nSkuldräntan kan inte vara negativ. Skuldränta {0}% kan inte sättas till konto {1}.",
                    interest, AccountNumber);
            }
            else {
                DebtInterest = interest;
                Console.WriteLine("\nSkuldräntan är nu {0}% på konto {1}", DebtInterest, AccountNumber);
            }
        }

        public void SetCreditLimit(decimal limit)
        {
            if (limit < 0)
            {
                Console.WriteLine("Kreditgräns måste anges som en icke-negativ siffra.");
            }
            else
            {
                CreditLimit = limit;
                Console.WriteLine("Konto {0} har nu en kreditgräns på {1} kr.", AccountNumber, CreditLimit);
            }

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
            if (Balance - amount < 0 - CreditLimit)
            {
                Console.WriteLine("\nFinns ej tillräckligt med pengar på konto {0} för begärd överföring ({1} kr).", AccountNumber, amount);
                return 0.0m;
            }

            //It's possible to withdraw the amount
            Balance -= amount;
            return amount;
        }

        public Transaction AddDailyInterest()
        {
            //Calculate the interest
            //If debt
            if (Balance < 0 && DebtInterest > 0)
            {
                //Yearly interest to daily interest
                decimal dailyInterest = (decimal)Math.Pow(1 + (double)DebtInterest / 100.0, 1.0 / 365.0) - 1;

                //How much to remove from the account (Balance is negative => amount is negativ)
                decimal amount = Balance * dailyInterest;
                //Remove the rent to the account
                Balance = Balance + amount;

                Console.WriteLine("Skuldränta har tagits ifrån konto {0}.", AccountNumber);

                InterestTransaction transaction = new InterestTransaction(DateTime.Now, AccountNumber, +amount, Balance);
                return transaction;
            }

            //If savings
            if (Balance > 0 && SaveInterest > 0)
            {
                //Yearly interest to daily interest
                decimal dailyInterest = (decimal)Math.Pow(1 + (double)SaveInterest / 100.0, 1.0 / 365.0)-1;

                //How much to add
                decimal amount = Balance * dailyInterest;

                //Add the rent to the account
                Balance = Balance + amount;

                Console.WriteLine("Sparränta har adderats till konto {0}.", AccountNumber);

                InterestTransaction transaction = new InterestTransaction(DateTime.Now, AccountNumber, amount, Balance);
                return transaction;
            }

            return null;
        }
    }
}
