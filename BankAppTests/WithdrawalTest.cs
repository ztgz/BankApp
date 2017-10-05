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
            Account account = new Account(10000, 1000, 100);
            decimal expectedBalance = 100;

            account.WithdrawRequest(150);

            Assert.AreEqual(expectedBalance, account.Balance);
        }

        [TestMethod]
        public void Withdraw_Money_Change_Saldo()
        {
            Account account = new Account(10000, 1000, 100);

            decimal expectedBalance = 40;

            account.WithdrawRequest(60);

            Assert.AreEqual(expectedBalance, account.Balance);
        }

        [TestMethod]
        public void Withdraw_Nonpositive_Amount()
        {
            Account account = new Account(10000, 1000, 100);

            decimal expectedBalance = 100;

            account.WithdrawRequest(-60);

            Assert.AreEqual(expectedBalance, account.Balance);
        }

        [TestMethod]
        public void Withdraw_Nonpositive_Amount_No_Withdrawal()
        {
            Account account = new Account(10000, 1000, 100);

            decimal expected = 0;

            decimal actual = account.WithdrawRequest(-60);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Withdraw_Insufficient_Balance_No_Withdrawal()
        {
            Account account = new Account(10000, 1000, 100);

            decimal expected = 0;

            decimal actual = account.WithdrawRequest(160);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Withdraw_Withdrawal()
        {
            Account account = new Account(10000, 1000, 100);

            decimal expected = 100;

            decimal actual = account.WithdrawRequest(100);

            Assert.AreEqual(expected, actual);
        }
    }
}
