using System;
using BankApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankAppTests
{
    [TestClass]
    public class InterestTest
    {
        [TestMethod]
        public void Daily_Saving_Interest()
        {
            Account account = new Account(0,0,10_000);

            account.SetSavingInterest(10);

            account.AddDailyInterest();

            //Numbers from excel, 10002.6115787607m 
            decimal expectedBalance = 10002.61m;

            Assert.AreEqual(expectedBalance, account.Balance);
        }

        [TestMethod]
        public void Daily_Saving_Intrest_Negative_Account()
        {
            Account account = new Account(0, 0, -10_000);

            account.SetSavingInterest(10);

            account.AddDailyInterest();

            decimal expectedBalance = -10_000;

            Assert.AreEqual(expectedBalance, account.Balance);
        }

        [TestMethod]
        public void Daily_Debt_Interest()
        {
            Account account = new Account(0, 0, -10_000);

            account.SetDebtInterest(10);

            account.AddDailyInterest();

            //Numbers from excel, -10002.6115787607m 
            decimal expectedBalance = -10002.61m;

            Assert.AreEqual(expectedBalance, account.Balance);
        }

        [TestMethod]
        public void Daily_Debt_Interest_Positive_Balance()
        {
            Account account = new Account(0, 0, 10_000);

            account.SetDebtInterest(10);

            account.AddDailyInterest();

            decimal expectedBalance = 10000;

            Assert.AreEqual(expectedBalance, account.Balance);
        }

        [TestMethod]
        public void Daily_Debt_Interest_Below_CreditLimit()
        {
            Account account = new Account(0, 0, -10_000);

            account.SetDebtInterest(10);
            account.SetCreditLimit(10000);

            account.AddDailyInterest();

            //Numbers from excel, -10002.6115787607m 
            decimal expectedBalance = -10002.61m;

            Assert.AreEqual(expectedBalance, account.Balance);
        }
    }
}
