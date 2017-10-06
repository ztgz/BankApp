using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Transactions
{
    public class WithdrawalTransaction : Transaction
    {
        public WithdrawalTransaction(DateTime date, int account, decimal amount, decimal accountBalance)
        {
            _date = date;
            _amount = amount;

            RecivingAccount = noAccount;
            _recivingAccountBalance = 0;

            SendingAccount = account;
            _sendingAccountBalance = accountBalance;
        }

        public override string GetInfo()
        {
            string text = "";

            text += "Datum: " + _date.ToString("yyyyMMdd-HHmm");
            text += " | Summa:" + _amount;
            text += " | Från konto: " + SendingAccount;
            text += " , Saldo: " + _sendingAccountBalance;
            text += " | (Uttag från konto)";

            return text;
        }
    }
}
