using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Extensions
{
    static class BankValidationExstentions
    {
        private const int AccountNumberFirst = 10_000;
        private const int AccountNumberLast = 99_999;

        private const int CustomerNumberFirst = 1_000;
        private const int CustomerNumberLast = 9_999;

        public static bool ValidAccountNumber(this int number)
        {
            if(number >= AccountNumberFirst && number <= AccountNumberLast)
                return true;

            return false;
        }

        public static bool ValidCustomerNumber(this int number)
        {
            if (number >= CustomerNumberFirst && number <= CustomerNumberLast)
                return true;

            return false;
        }
    }
}
