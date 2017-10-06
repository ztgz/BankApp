﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Transactions
{
    class DepositTransaction : Transaction
    {
        public DepositTransaction(int account, decimal amount, decimal accountBalance)
        {
            _date = DateTime.Now;
            _amount = amount;

            _recivingAccount = account;
            _recivingAccountBalance = accountBalance;

            _sendingAccount = noAccount;
            _sendingAccountBalance = 0;

            SaveTransaction();
        }

        public override string GetInfo()
        {
            string text = "";

            text += "Datum: " + _date.ToString("yyyyMMdd-HHmm");
            text += " | Summa:" + _amount;
            text += " | (Insättning till konto)";
            text += " | Till konto: " + _recivingAccount;
            text += " , Saldo: " + _recivingAccountBalance;

            return text;
        }
    }
}
