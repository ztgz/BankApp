using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Transactions
{
    public class DepositTransaction : Transaction
    {
        public DepositTransaction(DateTime date, int account, decimal amount, decimal accountBalance)
        {
            _date = date;
            _amount = amount;

            RecivingAccount = account;
            _recivingAccountBalance = accountBalance;

            SendingAccount = noAccount;
            _sendingAccountBalance = 0;
        }

        public override string GetInfo()
        {
            string text = "";

            text += "Datum: " + _date.ToString("yyyyMMdd-HHmm");
            text += " | Summa:" + _amount;
            text += " | (Insättning till konto)";
            text += " | Till konto: " + RecivingAccount;
            text += " , Saldo: " + _recivingAccountBalance;

            return text;
        }
    }
}
