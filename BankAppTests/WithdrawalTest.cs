using System;
using BankApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankAppTests
{
    [TestClass]
    public class WithdrawalTest
    {
        [TestMethod]
        public void Withdraw_Insufficient_Balance_No_Balance_Change()
        {
            Account account = new Account(0, 0, 100);
            decimal expectedBalance = 100;

            account.Withdraw(150);

            Assert.AreEqual(expectedBalance, account.Balance);
        }

        [TestMethod]
        public void Withdraw_Money_Change_Balance()
        {
            Account account = new Account(0, 0, 100);

            decimal expectedBalance = 40;

            account.Withdraw(60);

            Assert.AreEqual(expectedBalance, account.Balance);
        }

        [TestMethod]
        public void Withdraw_Nonpositive_Amount_No_Change()
        {
            Account account = new Account(0, 0, 100);

            decimal expectedBalance = 100;

            account.Withdraw(-60);

            Assert.AreEqual(expectedBalance, account.Balance);
        }

        [TestMethod]
        public void Withdrawal_With_Creditlimit()
        {
            Account account = new Account(10000, 1000, 100);
            account.SetCreditLimit(100);

            decimal expected = -50;

            account.Withdraw(150);

            Assert.AreEqual(expected, account.Balance);
        }

        [TestMethod]
        public void Withdrawal_With_Creditlimit_Insufficient_Balance()
        {
            Account account = new Account(10000, 1000, 100);
            account.SetCreditLimit(100);

            decimal expected = 100;

            account.Withdraw(250);

            Assert.AreEqual(expected, account.Balance);
        }
    }
}
