using System;
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

            _givingAccount = noAccount;
            _givingAccountBalance = 0;

            SaveTransaction();
        }
    }
}
