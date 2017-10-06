using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Transactions
{
    class TransferTransaction : Transaction
    {
        public TransferTransaction(DateTime date, decimal amount, 
            int recivingAccount, decimal recivingAccountBalance,
            int sendingAccount, decimal sendingAccountBalance)
        {
            _date = date;
            _amount = amount;

            RecivingAccount = recivingAccount;
            _recivingAccountBalance = recivingAccountBalance;

            SendingAccount = sendingAccount;
            _sendingAccountBalance = sendingAccountBalance;
        }

        public override string GetInfo()
        {
            string text = "";

            text += "Datum: " + _date.ToString("yyyyMMdd-HHmm");
            text += " | Summa:" + _amount;
            text += " | Från konto: " + SendingAccount;
            text += " , Saldo: " + _sendingAccountBalance;
            text += " | Till konto: " + RecivingAccount;
            text += " , Saldo: " + _recivingAccountBalance;

            return text;
        }
    }
}
