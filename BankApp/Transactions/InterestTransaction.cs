using System;

namespace BankApp.Transactions
{
    public class InterestTransaction : Transaction
    {
        public InterestTransaction(DateTime date, int account, decimal amount, decimal accountBalance)
        {
            base.date = date;
            base.amount = amount;

            RecivingAccount = account;
            recivingAccountBalance = accountBalance;

            SendingAccount = fromBank;
            sendingAccountBalance = 0;
        }

        public override string GetInfo()
        {
            string text = "";

            text += "(   Ränta  ) Datum: " + date.ToString("yyyyMMdd-HHmm");
            text += " | Summa:" + amount;
            text += " | Konto: " + RecivingAccount;
            text += " , Saldo: " + recivingAccountBalance;

            return text;
        }
    }
}
