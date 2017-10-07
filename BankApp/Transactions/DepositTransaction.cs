using System;

namespace BankApp.Transactions
{
    public class DepositTransaction : Transaction
    {
        public DepositTransaction(DateTime date, int account, decimal amount, decimal accountBalance)
        {
            base.date = date;
            base.amount = amount;

            RecivingAccount = account;
            recivingAccountBalance = accountBalance;

            SendingAccount = noAccount;
            sendingAccountBalance = 0;
        }

        public override string GetInfo()
        {
            string text = "";

            text += "Datum: " + date.ToString("yyyyMMdd-HHmm");
            text += " | Summa:" + amount;
            text += " | (Insättning till konto)";
            text += " | Till konto: " + RecivingAccount;
            text += " , Saldo: " + recivingAccountBalance;

            return text;
        }
    }
}
