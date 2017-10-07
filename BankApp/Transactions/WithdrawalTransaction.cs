using System;

namespace BankApp.Transactions
{
    public class WithdrawalTransaction : Transaction
    {
        public WithdrawalTransaction(DateTime date, int account, decimal amount, decimal accountBalance)
        {
            base.date = date;
            base.amount = amount;

            RecivingAccount = noAccount;
            recivingAccountBalance = 0;

            SendingAccount = account;
            sendingAccountBalance = accountBalance;
        }

        public override string GetInfo()
        {
            string text = "";

            text += "Datum: " + date.ToString("yyyyMMdd-HHmm");
            text += " | Summa:" + amount;
            text += " | Från konto: " + SendingAccount;
            text += " , Saldo: " + sendingAccountBalance;
            text += " | (Uttag från konto)";

            return text;
        }
    }
}
