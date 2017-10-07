using System;

namespace BankApp.Transactions
{
    public class InterestTransaction : Transaction
    {
        public InterestTransaction(DateTime date, int account, decimal amount, decimal accountBalance)
        {
            _date = date;
            _amount = amount;

            RecivingAccount = account;
            _recivingAccountBalance = accountBalance;

            SendingAccount = fromBank;
            _sendingAccountBalance = 0;
        }

        public override string GetInfo()
        {
            string text = "";

            text += "Datum: " + _date.ToString("yyyyMMdd-HHmm");
            text += " | Summa:" + _amount;
            text += " | (Daglig ränta)";
            text += " | Konto: " + RecivingAccount;
            text += " , Saldo: " + _recivingAccountBalance;

            return text;
        }
    }
}
