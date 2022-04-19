using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalc.Models
{
    public class AddressRef
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

        public string GetAddressAsURLParameters()
        {
            List<string> results = new List<string>();

            if (!String.IsNullOrEmpty(Street1))
            {
                results.Add($"street={Street1}");
            }
            if (!String.IsNullOrEmpty(City))
            {
                results.Add($"city={City}");
            }
            if (!String.IsNullOrEmpty(State))
            {
                results.Add($"state={State}");
            }
            if (!String.IsNullOrEmpty(Zip))
            {
                results.Add($"zip={Zip}");
            }
            if (!String.IsNullOrEmpty(Country))
            {
                results.Add($"country={Country}");
            }
            

            return String.Join("&", results);
        }

        public string GetAddressDisplay()
        {
            string result = "";
            result += !String.IsNullOrEmpty(Name) ? $"{Name}\n" : "";
            result += !String.IsNullOrEmpty(Street1) ? $"{Street1}\n" : "";
            result += !String.IsNullOrEmpty(Street2) ? $"{Street2}\n" : "";
            result += !String.IsNullOrEmpty(City) ? $"{City}, " : "";
            result += !String.IsNullOrEmpty(State) ? $"{State}\n" : "";
            result += !String.IsNullOrEmpty(Zip) ? $"{Zip}\n" : "";
            result += !String.IsNullOrEmpty(Country) ? $"{Country}" : "";
            return result;
        }
    }
}
