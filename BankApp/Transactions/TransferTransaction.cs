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
    }
}
