using System;
using System.Globalization;
using System.Text;
using BankApp.Transactions;

namespace BankApp
{
    public class Account
    {
        public int AccountNumber { get;}

        public int OwnersCustomerNumber { get;}

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

        public string SetCreditLimit(decimal limit)
        {
            if (limit < 0)
            {
                return "\nKreditgräns måste anges som en icke-negativ siffra.";
            }

            _creditLimit = limit;
            return String.Format("\nKonto {0} har nu en kreditgräns på {1} kr.", AccountNumber, _creditLimit);

        }

        public string SetDebtInterest(decimal interest)
        {
            if (interest < 0)
            {
                return String.Format("\nSkuldräntan kan inte vara negativ. Skuldränta {0}% kan inte sättas till konto {1}.",
                    interest, AccountNumber);
            }

            _debtInterest = interest;
            return String.Format("\nSkuldräntan är nu {0}% på konto {1}.", _debtInterest, AccountNumber);
        }

        public string SetSavingInterest(decimal interest)
        {
            if (interest <= 0)
            {
                return String.Format("\nSparräntan måste vara positiv. Ränta {0}% kan inte sättas till konto {1}.",
                    interest, AccountNumber);
            }

            _saveInterest = interest;
            return String.Format("\nSparräntan är nu {0}% på konto {1}.", _saveInterest, AccountNumber);
        }

        public InterestTransaction AddDailyInterest()
        {
            //Calculate the interest
            //If debt
            if (Balance < 0 && _debtInterest > 0)
            {
                //Yearly interest to daily interest
                decimal dailyInterest = (decimal)Math.Pow(1 + (double)_debtInterest / 100.0, 1.0 / 365.0) - 1;

                //How much to remove from the account (Balance is negative => amount is negative)
                decimal amount = Math.Round(Balance * dailyInterest, 2);

                //Remove the rent to the account
                Balance = Balance + amount;

                Console.WriteLine("Skuldränta har tagits ifrån konto {0}.", AccountNumber);

                InterestTransaction transaction = new InterestTransaction(DateTime.Now, AccountNumber, +amount, Balance);
                return transaction;
            }

            //If savings
            if (Balance > 0 && _saveInterest > 0)
            {
                //Yearly interest to daily interest
                decimal dailyInterest = (decimal)Math.Pow(1 + (double)_saveInterest / 100.0, 1.0 / 365.0) - 1;

                //How much to add
                decimal amount = Math.Round(Balance * dailyInterest, 2);

                //Add the rent to the account
                Balance = Balance + amount;

                Console.WriteLine("Sparränta har adderats till konto {0}.", AccountNumber);

                InterestTransaction transaction = new InterestTransaction(DateTime.Now, AccountNumber, amount, Balance);
                return transaction;
            }

            return null;
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
            Console.Write("{0}: {1:0.00} kr", AccountNumber, Balance);
            if (_saveInterest > 0)
                Console.Write(", sparränta {0}%", _saveInterest);
            if (_creditLimit > 0)
                Console.Write(", kreditgräns {0:0.00} kr", _creditLimit);
            if (_debtInterest > 0)
                Console.Write(", skuldränta {0}%", _debtInterest);

            Console.WriteLine();
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
                sb.Append(";" + _saveInterest.ToString(CultureInfo.InvariantCulture));
                sb.Append(";" + _debtInterest.ToString(CultureInfo.InvariantCulture));
                sb.Append(";" + _creditLimit.ToString(CultureInfo.InvariantCulture));
            }

            return sb.ToString();
        }

        public decimal MaxPossibleWithdraw()
        {
            return Balance + _creditLimit;
        }

    }
}
