using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalc.Models
{
    public class OrderLineItem
    {
        public Item LineItem { get; set; }
        public int OrderQty { get; set; }
        public string LineQtyDisplay
        {
            get
            {
                return LineItem.UnitPrice.ToString("C") + " X " + OrderQty + " = " + (LineItem.UnitPrice * OrderQty).ToString("C");
            }
        }
    }
}
