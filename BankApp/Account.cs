using System;
using System.Globalization;
using System.Text;
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
                Console.WriteLine("\nKreditgräns måste anges som en icke-negativ siffra.");
            }
            else
            {
                CreditLimit = limit;
                Console.WriteLine("\nKonto {0} har nu en kreditgräns på {1} kr.", AccountNumber, CreditLimit);
            }
        }

        public DepositTransaction Deposit(decimal amount)
        {
            //Cannot deposit a nonpositive number
            if (amount <= 0)
                return null;

            Balance += amount;
            return new DepositTransaction(DateTime.Now, AccountNumber, amount, Balance);
        }

        public WithdrawalTransaction Withdraw(decimal amount)
        {
            //No negative amounts cannot be withdrawn from account (or zero)
            //or balance would be to low
            if (amount <= 0.0m || amount > MaxPossibleWithdraw())
            {
                return null;
            }

            //It's possible to withdraw the amount
            Balance -= amount;
            return new WithdrawalTransaction(DateTime.Now, AccountNumber, amount, Balance);
        }

        public InterestTransaction AddDailyInterest()
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

        public TransferTransaction Transfer(Account sendingAccount, decimal amount)
        {
            //Try to withdraw from sendningAccount & if withdrawal from account was succesfull
            if (sendingAccount.Withdraw(amount) != null)
            {
                Deposit(amount);
                
                return new TransferTransaction(DateTime.Now, amount, AccountNumber, Balance,
                    sendingAccount.AccountNumber, sendingAccount.Balance);
            }

            //Transfer failed
            return null;
        }

        public void PrintAccount()
        {
            Console.Write("{0}: {1} kr", AccountNumber, Balance);
            if (SaveInterest > 0)
                Console.Write(", sparränta {0}%", SaveInterest);
            if (CreditLimit > 0)
                Console.Write(", kreditgräns {0} kr", CreditLimit);
            if (DebtInterest > 0)
                Console.Write(", skuldränta {0}%", DebtInterest);

            Console.WriteLine();
        }

        public decimal MaxPossibleWithdraw()
        {
            return Balance + CreditLimit;
        }

        public string ToSaveFormat(bool detailed)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(AccountNumber);
            sb.Append(";" + OwnersCustomerNumber);
            sb.Append(";" + Balance.ToString(CultureInfo.InvariantCulture));

            //If data is saved in detailed format
            if (detailed)
            {
                sb.Append(";" + SaveInterest.ToString(CultureInfo.InvariantCulture));
                sb.Append(";" + DebtInterest.ToString(CultureInfo.InvariantCulture));
                sb.Append(";" + CreditLimit.ToString(CultureInfo.InvariantCulture));
            }

            return sb.ToString();
        }
    }
}
