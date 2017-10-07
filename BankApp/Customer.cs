using System;

namespace BankApp
{
    public class Customer
    {
        public int CustomerNumber { get; private set; }

        public string OrganisationNumber { get; private set; }

        public string Name { get; private set; }

        public string Address { get; private set; }
        
        public string City { get; private set; }

        public string Region { get; private set; }

        public string PostNumber { get; private set; }

        public string Country { get; private set; }

        public string PhoneNumber { get; private set; }

        public Customer(int customerNumber, string organisationNumber, string name, string adress, 
            string city, string region, string postNumber, string country, string phoneNumber)
        {
            CustomerNumber = customerNumber;
            OrganisationNumber = organisationNumber;
            Name = name;

            Address = adress;
            City = city;
            Region = region;

            PostNumber = postNumber;
            Country = country;
            PhoneNumber = phoneNumber;
        }

        public void PrintCustomer()
        {
            Console.WriteLine("\nOrganistionsnummer: {0}", OrganisationNumber);
            Console.WriteLine("Namn: {0}", Name);
            Console.WriteLine("Adress: {0}, {1}, {2}, {3}{4}", Address, PostNumber, City,
                (Region != "" ? $"{Region}, " : ""), Country);
            Console.WriteLine("Telefonnummer: {0}", PhoneNumber);
        }
    }
}
