using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    class Journal
    {
        public int AccountNr { get; private set; }

        public int Amount { get; private set; }

        public string Notes { get; private set; }
    }
}
