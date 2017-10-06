using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Transactions
{
    public abstract class Transaction
    {
        private const string fileName = @"files\transactionLogg.txt";

        protected DateTime _date;

        public int RecivingAccount { get; protected set; }

        public int SendingAccount { get; protected set; }

        protected decimal _recivingAccountBalance;

        protected decimal _sendingAccountBalance;

        protected decimal _amount;

        protected const int noAccount = 0; //If it was an deposit or withdrawal from outside of the bank

        protected const int fromBank = -1; //If the bank recived or payed interest

        public abstract string GetInfo();

        public DateTime GetDate()
        {
            return _date;
        }

        public void SaveTransaction()
        {
            Encoding win1252 = Encoding.GetEncoding("Windows-1252");
            using( StreamWriter writer = new StreamWriter(fileName, true, win1252))
            {
                writer.Write(_date.ToString("yyyyMMdd-HHmm"));
                writer.Write(";" + _amount);

                writer.Write(";" + RecivingAccount);
                writer.Write(";" + _recivingAccountBalance);

                writer.Write(";" + SendingAccount);
                writer.Write(";" + _sendingAccountBalance);

                writer.WriteLine();
            }
        }

        public static List<Transaction> GetTransactionsHistory()
        {
            Encoding win1252 = Encoding.GetEncoding("Windows-1252");

            List<Transaction> transactions = new List<Transaction>();

            using (StreamReader reader = new StreamReader(fileName, win1252))
            {
                string line = reader.ReadLine();

                while (line != null)
                {
                    string[] transInfo = line.Split(';');

                    //Correct rows vill contain 6 variables
                    if (transInfo.Length == 6)
                    {
                        //Get the date
                        int year = int.Parse(transInfo[0].Substring(0, 4));
                        int month = int.Parse(transInfo[0].Substring(4, 2));
                        int day = int.Parse(transInfo[0].Substring(6, 2));

                        int hour = int.Parse(transInfo[0].Substring(9, 2));
                        int minute = int.Parse(transInfo[0].Substring(11, 2));

                        DateTime date = new DateTime(year, month, day, hour, minute, 00);

                        Transaction transaction;
                        //Test type of transaction
                        //if it's daily savings interestRate
                        if (transInfo[4] == "-1")
                        {
                            transaction = new SaveInterestTransaction(date, int.Parse(transInfo[2]),
                                decimal.Parse(transInfo[1]), decimal.Parse(transInfo[3]));
                        }
                        //It's a whitdrawal
                        else if (transInfo[2] == "0")
                        {
                            transaction = new WithdrawalTransaction(date, int.Parse(transInfo[4])
                                , decimal.Parse(transInfo[1]), decimal.Parse(transInfo[5]));
                        }
                        //it's a deposit
                        else if (transInfo[4] == "0")
                        {
                            transaction = new DepositTransaction(date, int.Parse(transInfo[2]),
                                decimal.Parse(transInfo[1]), decimal.Parse(transInfo[3]));
                        }
                        //Otherwise it's a transaction between accounts
                        else
                        {
                            transaction = new TransferTransaction(date, decimal.Parse(transInfo[1]), int.Parse(transInfo[2]),
                                decimal.Parse(transInfo[3]), int.Parse(transInfo[4]), decimal.Parse(transInfo[5]));
                        }

                        transactions.Add(transaction);
                    }
                        //read new line from file
                        line = reader.ReadLine();
                }
            }

            return transactions;
        }

    }
}
