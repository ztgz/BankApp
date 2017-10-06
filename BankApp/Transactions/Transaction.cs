using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Transactions
{
    abstract class Transaction
    {
        private const string filePath = @"files\transactionLogg.txt";

        protected DateTime _date;

        protected int _recivingAccount;

        protected decimal _recivingAccountBalance;

        protected int _sendingAccount;

        protected decimal _sendingAccountBalance;

        protected decimal _amount;

        protected const int noAccount = 0; //If it was an deposit or withdrawal from outside of the bank

        protected void SaveTransaction()
        {
            Encoding win1252 = Encoding.GetEncoding("Windows-1252");
            using( StreamWriter writer = new StreamWriter(filePath, true, win1252))
            {
                writer.Write(_date.ToString("yyyyMMdd-HHmm"));
                writer.Write(";" + _amount);

                writer.Write(";" + _recivingAccount);
                writer.Write(";" + _recivingAccountBalance);

                writer.Write(";" + _sendingAccount);
                writer.Write(";" + _sendingAccountBalance);

                writer.WriteLine();
            }
        }

        public DateTime GetDate()
        {
            return _date;
        }

        public abstract string GetInfo();

    }
}
