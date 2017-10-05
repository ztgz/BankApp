using System;
using BankApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankAppTests
{
    [TestClass]
    public class TransferTest
    {
        [TestMethod]
        public void Transer_Not_Enough_Money()
        {
            Account account = new Account(10000, 1000, 100);

            decimal expectedBalance = 40;

            account.WithdrawRequest(60);

            Assert.AreEqual(expectedBalance, account.Balance);

        }
    }
}
