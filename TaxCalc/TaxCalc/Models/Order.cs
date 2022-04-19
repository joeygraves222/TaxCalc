using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalc.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public AddressRef PurchaserAddress { get; set; }
        public AddressRef SellerAddress { get; set; }
        public List<OrderLineItem> Items { get; set; }
        public decimal ShippingCost { get; set; }

    }
}
