﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Transactions
{
    public class SaveInterestTransaction : Transaction
    {
        public SaveInterestTransaction(DateTime date, int account, decimal amount, decimal accountBalance)
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
            text += " | (Sparränta till konto)";
            text += " | Till konto: " + RecivingAccount;
            text += " , Saldo: " + _recivingAccountBalance;

            return text;
        }
    }
}
