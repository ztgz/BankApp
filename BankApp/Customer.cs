using System;
using System.Text;

namespace BankApp
{
    public class Customer
    {
        public int CustomerNumber { get; }

        public string Name { get; }

        public string City { get;}

        private readonly string _organisationNumber;

        private readonly string _address;

        private readonly string _region;

        private readonly string _postNumber;

        private readonly string _country;

        private readonly string _phoneNumber;

        public Customer(int customerNumber, string organisationNumber, string name, string adress, 
            string city, string region, string postNumber, string country, string phoneNumber)
        {
            CustomerNumber = customerNumber;
            _organisationNumber = organisationNumber;
            Name = name;

            _address = adress;
            City = city;
            _region = region;

            _postNumber = postNumber;
            _country = country;
            _phoneNumber = phoneNumber;
        }

        public void PrintCustomer()
        {
            Console.WriteLine("\nKundnummer: {0}", CustomerNumber);
            Console.WriteLine("Organistionsnummer: {0}", _organisationNumber);
            Console.WriteLine("Namn: {0}", Name);
            Console.WriteLine("Adress: {0}, {1}, {2}, {3}{4}", _address, _postNumber, City,
                (_region != "" ? $"{_region}, " : ""), _country);
            Console.WriteLine("Telefonnummer: {0}", _phoneNumber);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            //Build to return data
            sb.Append(CustomerNumber);
            sb.Append(";" + _organisationNumber);
            sb.Append(";" + Name);
            sb.Append(";" + _address);
            sb.Append(";" + City);
            sb.Append(";" + _region);
            sb.Append(";" + _postNumber);
            sb.Append(";" + _country);
            sb.Append(";" + _phoneNumber);

            return sb.ToString();
        }

    }
}
