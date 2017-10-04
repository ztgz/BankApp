using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create the bank
            Bank bank = new Bank();
            
            //Run the bank app
            bank.Run();
        }
    }
}
