using System;

namespace TaxCalc.Models
{
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public string UnitPriceDisplay { get { return UnitPrice.ToString("C"); } }
    }
}