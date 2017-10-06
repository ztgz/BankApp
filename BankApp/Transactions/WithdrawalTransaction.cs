﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Transactions
{
    class WithdrawalTransaction : Transaction
    {
        public WithdrawalTransaction(int account, decimal amount, decimal accountBalance)
        {
            _date = DateTime.Now;
            _amount = amount;

            _recivingAccount = noAccount;
            _recivingAccountBalance = 0;

            _sendingAccount = account;
            _sendingAccountBalance = accountBalance;

            SaveTransaction();
        }

        public override string GetInfo()
        {
            string text = "";

            text += "Datum: " + _date.ToString("yyyyMMdd-HHmm");
            text += " | Summa:" + _amount;
            text += " | Från konto: " + _sendingAccount;
            text += " , Saldo: " + _sendingAccountBalance;
            text += " | (Uttag från konto)";

            return text;
        }
    }
}
