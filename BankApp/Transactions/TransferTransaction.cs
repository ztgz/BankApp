using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Transactions
{
    class TransferTransaction : Transaction
    {
        public TransferTransaction(decimal amount, int recivingAccount, decimal recivingAccountBalance,
            int sendingAccount, decimal sendingAccountBalance)
        {
            _date = DateTime.Now;
            _amount = amount;

            _recivingAccount = recivingAccount;
            _recivingAccountBalance = recivingAccountBalance;

            _sendingAccount = sendingAccount;
            _sendingAccountBalance = sendingAccountBalance;

            SaveTransaction();
        }

        public override string GetInfo()
        {
            string text = "";

            text += "Datum: " + _date.ToString("yyyyMMdd-HHmm");
            text += " | Summa:" + _amount;
            text += " | Från konto: " + _sendingAccount;
            text += " , Saldo: " + _sendingAccountBalance;
            text += " | Till konto: " + _recivingAccount;
            text += " , Saldo: " + _recivingAccountBalance;

            return text;
        }
    }
}
