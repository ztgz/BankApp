using System;

namespace BankApp.Transactions
{
    public class TransferTransaction : Transaction
    {
        public TransferTransaction(DateTime date, decimal amount, 
            int recivingAccount, decimal recivingAccountBalance,
            int sendingAccount, decimal sendingAccountBalance)
        {
            base.date = date;
            base.amount = amount;

            RecivingAccount = recivingAccount;
            base.recivingAccountBalance = recivingAccountBalance;

            SendingAccount = sendingAccount;
            base.sendingAccountBalance = sendingAccountBalance;
        }

        public override string GetInfo()
        {
            string text = "";

            text += "Datum: " + date.ToString("yyyyMMdd-HHmm");
            text += " | Summa:" + amount;
            text += " | Från konto: " + SendingAccount;
            text += " , Saldo: " + sendingAccountBalance;
            text += " | Till konto: " + RecivingAccount;
            text += " , Saldo: " + recivingAccountBalance;

            return text;
        }
    }
}
