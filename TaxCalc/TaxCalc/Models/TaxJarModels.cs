using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaxCalc.Models
{
    public class TaxJarTaxesPayload
    {
        public string from_country { get; set; }
        public string from_zip { get; set; }
        public string from_state { get; set; }
        public string from_city { get; set; }
        public string from_street { get; set; }
        public string to_country { get; set; }
        public string to_zip { get; set; }
        public string to_state { get; set; }
        public string to_city { get; set; }
        public string to_street { get; set; }
        public float amount { get; set; }
        public float shipping { get; set; }
        public List<TaxJarNexusAddress> nexus_addresses { get; set; }
        public List<TaxJarRequestLineItem> line_items { get; set; }

        public TaxJarTaxesPayload()
        {

        }
        public TaxJarTaxesPayload(Order order)
        {
            if (order.SellerAddress != null)
            {
                from_street = order.SellerAddress.Street1;
                from_city = order.SellerAddress.City;
                from_state = order.SellerAddress.State;
                from_zip = order.SellerAddress.Zip;
                from_country = order.SellerAddress.Country;
            }

            to_street = order.PurchaserAddress.Street1;
            to_city = order.PurchaserAddress.City;
            to_state = order.PurchaserAddress.State;
            to_zip = order.PurchaserAddress.Zip;
            to_country = order.PurchaserAddress.Country;
            amount = order.Items != null ? Convert.ToSingle(order.Items.Sum(i => (i.OrderQty * i.LineItem.UnitPrice))) : 0;
            shipping = Convert.ToSingle(order.ShippingCost);

            if (line_items == null)
            {
                line_items = new List<TaxJarRequestLineItem>();
            }

            foreach (var item in order.Items)
            {
                line_items.Add(new TaxJarRequestLineItem(item));
            }
        }


    }
    public class TaxJarNexusAddress
    {
        public string id { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string street { get; set; }
    }

    public class TaxJarRequestLineItem
    {
        public string id { get; set; }
        public int quantity { get; set; }
        public string product_tax_code { get; set; }
        public decimal unit_price { get; set; }
        public float discount { get; set; }

        public TaxJarRequestLineItem()
        {

        }

        public TaxJarRequestLineItem(OrderLineItem lineItem)
        {
            id = lineItem.LineItem.Name;
            quantity = lineItem.OrderQty;
            unit_price = lineItem.LineItem.UnitPrice;
            discount = 0;
        }
    }

    public class TaxJarJurisdictions
    {
        public string country { get; set; }
        public string state { get; set; }
        public string county { get; set; }
        public string city { get; set; }
    }

    public class TaxJarResponseLineItem
    {
        public string id { get; set; }
        public double taxable_amount { get; set; }
        public double tax_collectable { get; set; }
        public double combined_tax_rate { get; set; }
        public double state_taxable_amount { get; set; }
        public double state_sales_tax_rate { get; set; }
        public double state_amount { get; set; }
        public double county_taxable_amount { get; set; }
        public double county_tax_rate { get; set; }
        public double county_amount { get; set; }
        public double city_taxable_amount { get; set; }
        public double city_tax_rate { get; set; }
        public double city_amount { get; set; }
        public double special_district_taxable_amount { get; set; }
        public double special_tax_rate { get; set; }
        public double special_district_amount { get; set; }
    }

    public class TaxJarBreakdown
    {
        public double taxable_amount { get; set; }
        public double tax_collectable { get; set; }
        public double combined_tax_rate { get; set; }
        public double state_taxable_amount { get; set; }
        public double state_tax_rate { get; set; }
        public double state_tax_collectable { get; set; }
        public double county_taxable_amount { get; set; }
        public double county_tax_rate { get; set; }
        public double county_tax_collectable { get; set; }
        public double city_taxable_amount { get; set; }
        public double city_tax_rate { get; set; }
        public double city_tax_collectable { get; set; }
        public double special_district_taxable_amount { get; set; }
        public double special_tax_rate { get; set; }
        public double special_district_tax_collectable { get; set; }
        public List<TaxJarResponseLineItem> line_items { get; set; }
    }

    public class TaxJarTax
    {
        public double order_total_amount { get; set; }
        public double shipping { get; set; }
        public double taxable_amount { get; set; }
        public double amount_to_collect { get; set; }
        public double rate { get; set; }
        public bool has_nexus { get; set; }
        public bool freight_taxable { get; set; }
        public string tax_source { get; set; }
        public TaxJarJurisdictions jurisdictions { get; set; }
        public TaxJarBreakdown breakdown { get; set; }
    }

    public class TaxJarTaxesResponse
    {
        public TaxJarTax tax { get; set; }
    }

    public class TaxJarRate
    {
        public string zip { get; set; }
        public string country { get; set; }
        public string country_rate { get; set; }
        public string state { get; set; }
        public string state_rate { get; set; }
        public string county { get; set; }
        public string county_rate { get; set; }
        public string city { get; set; }
        public string city_rate { get; set; }
        public string combined_district_rate { get; set; }
        public string combined_rate { get; set; }
        public bool freight_taxable { get; set; }
    }

    public class TaxJarRatesResponse
    {
        public TaxJarRate rate { get; set; }
    }
}
