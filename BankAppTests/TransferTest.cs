using System;
using BankApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankAppTests
{
    [TestClass]
    public class TransferTest
    {
        [TestMethod]
        public void Transfer_Not_Enough_Money()
        {
            Account sender = new Account(0, 0, 100);
            Account reciver = new Account(1, 1, 0);

            decimal expectedBalance = 100;

            reciver.Transfer(sender, 150);

            Assert.AreEqual(expectedBalance, sender.Balance);
        }

        [TestMethod]
        public void Transfer_Not_Enough_Money_2()
        {
            Account sender = new Account(0, 0, 100);
            Account reciver = new Account(1, 1, 0);

            decimal expectedBalance = 0;

            reciver.Transfer(sender, 150);

            Assert.AreEqual(expectedBalance, reciver.Balance);
        }

        [TestMethod]
        public void Transfer_Regular()
        {
            Account sender = new Account(0, 0, 100);
            Account reciver = new Account(1, 1, 0);

            decimal expectedBalance = 40;

            reciver.Transfer(sender, 60);

            Assert.AreEqual(expectedBalance, sender.Balance);
        }

        [TestMethod]
        public void Transfer_Regular_2()
        {
            Account sender = new Account(0, 0, 100);
            Account reciver = new Account(1, 1, 0);

            decimal expectedBalance = 60;

            reciver.Transfer(sender, 60);

            Assert.AreEqual(expectedBalance, reciver.Balance);
        }

        [TestMethod]
        public void Transfer_With_Credit()
        {
            Account sender = new Account(0, 0, 100);
            sender.SetCreditLimit(300);

            Account reciver = new Account(1, 1, 0);

            decimal expectedBalance = -250;

            reciver.Transfer(sender, 350);

            Assert.AreEqual(expectedBalance, sender.Balance);
        }

        [TestMethod]
        public void Transfer_With_Credit_2()
        {
            Account sender = new Account(0, 0, 100);
            sender.SetCreditLimit(300);

            Account reciver = new Account(1, 1, 0);

            decimal expectedBalance = 350;

            reciver.Transfer(sender, 350);

            Assert.AreEqual(expectedBalance, reciver.Balance);
        }
    }
}
