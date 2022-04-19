using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalc.Models
{
    public class TaxRatesModel
    {
        public double CombinedRate { get; set; }
        public double CityRate { get; set; }
        public double CountyRate { get; set; }
        public double StateRate { get; set; }
        public double CountryRate { get; set; }

        public string ToDisplayString()
        {
            return $"City Rate: {CityRate * 100}%\nCounty Rate: {CountyRate * 100}%\nState Rate: {StateRate * 100}%\nCountry Rate: {CountryRate * 100}%\nTotal Rate: {CombinedRate * 100}%";
        }
    }
}
