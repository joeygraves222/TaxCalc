using Acr.UserDialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Models;
using TaxCalc.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxCalc.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnitTestingPage : ContentPage
    {
        public UnitTestingPage()
        {
            InitializeComponent();
        }

        private async void RunOrderTaxesTestBtn_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Running Order Taxes Test...");

            //This test case will call the API with the sample data provided in the API documentation

            var sampleData = new TaxJarTaxesPayload
            {
                from_country = "US",
                from_zip = "92093",
                from_state = "CA",
                from_city = "La Jolla",
                from_street = "9500 Gilman Drive",
                to_country = "US",
                to_zip = "90002",
                to_state = "CA",
                to_city = "Los Angeles",
                to_street = "1335 E 103rd St",
                amount = 15,
                shipping = 1.5f,
                nexus_addresses = new List<TaxJarNexusAddress>
                {
                      new TaxJarNexusAddress
                      {
                        id = "Main Location",
                        country = "US",
                        zip = "92093",
                        state = "CA",
                        city = "La Jolla",
                        street = "9500 Gilman Drive",
                      }
                },
                line_items = new List<TaxJarRequestLineItem>
                {
                      new TaxJarRequestLineItem
                      {
                        id = "1",
                        quantity = 1,
                        product_tax_code = "20010",
                        unit_price = 15,
                        discount = 0
                      }
                }
            };

            var taxService = new TaxService(new TaxJarCalculator());
            var response = await taxService.CalculateTaxesForOrder(sampleData);

            //var expectedResponseSerialized = "{\r\n  \"tax\": {\r\n    \"order_total_amount\": 16.5,\r\n    \"shipping\": 1.5,\r\n    \"taxable_amount\": 15,\r\n    \"amount_to_collect\": 1.35,\r\n    \"rate\": 0.09,\r\n    \"has_nexus\": true,\r\n    \"freight_taxable\": false,\r\n    \"tax_source\": \"destination\",\r\n    \"jurisdictions\": {\r\n      \"country\": \"US\",\r\n      \"state\": \"CA\",\r\n      \"county\": \"LOS ANGELES\",\r\n      \"city\": \"LOS ANGELES\"\r\n    },\r\n    \"breakdown\": {\r\n      \"taxable_amount\": 15,\r\n      \"tax_collectable\": 1.35,\r\n      \"combined_tax_rate\": 0.09,\r\n      \"state_taxable_amount\": 15,\r\n      \"state_tax_rate\": 0.0625,\r\n      \"state_tax_collectable\": 0.94,\r\n      \"county_taxable_amount\": 15,\r\n      \"county_tax_rate\": 0.0025,\r\n      \"county_tax_collectable\": 0.04,\r\n      \"city_taxable_amount\": 0,\r\n      \"city_tax_rate\": 0,\r\n      \"city_tax_collectable\": 0,\r\n      \"special_district_taxable_amount\": 15,\r\n      \"special_tax_rate\": 0.025,\r\n      \"special_district_tax_collectable\": 0.38,\r\n      \"line_items\": [\r\n        {\r\n          \"id\": \"1\",\r\n          \"taxable_amount\": 15,\r\n          \"tax_collectable\": 1.35,\r\n          \"combined_tax_rate\": 0.09,\r\n          \"state_taxable_amount\": 15,\r\n          \"state_sales_tax_rate\": 0.0625,\r\n          \"state_amount\": 0.94,\r\n          \"county_taxable_amount\": 15,\r\n          \"county_tax_rate\": 0.0025,\r\n          \"county_amount\": 0.04,\r\n          \"city_taxable_amount\": 0,\r\n          \"city_tax_rate\": 0,\r\n          \"city_amount\": 0,\r\n          \"special_district_taxable_amount\": 15,\r\n          \"special_tax_rate\": 0.025,\r\n          \"special_district_amount\": 0.38\r\n        }\r\n      ]\r\n    }\r\n  }\r\n}";

            var expectedResponseSerialized = "{\r\n    \"tax\": {\r\n        \"amount_to_collect\": 1.43,\r\n        \"breakdown\": {\r\n            \"city_tax_collectable\": 0.0,\r\n            \"city_tax_rate\": 0.0,\r\n            \"city_taxable_amount\": 0.0,\r\n            \"combined_tax_rate\": 0.095,\r\n            \"county_tax_collectable\": 0.15,\r\n            \"county_tax_rate\": 0.01,\r\n            \"county_taxable_amount\": 15.0,\r\n            \"line_items\": [\r\n                {\r\n                    \"city_amount\": 0.0,\r\n                    \"city_tax_rate\": 0.0,\r\n                    \"city_taxable_amount\": 0.0,\r\n                    \"combined_tax_rate\": 0.095,\r\n                    \"county_amount\": 0.15,\r\n                    \"county_tax_rate\": 0.01,\r\n                    \"county_taxable_amount\": 15.0,\r\n                    \"id\": \"1\",\r\n                    \"special_district_amount\": 0.34,\r\n                    \"special_district_taxable_amount\": 15.0,\r\n                    \"special_tax_rate\": 0.0225,\r\n                    \"state_amount\": 0.94,\r\n                    \"state_sales_tax_rate\": 0.0625,\r\n                    \"state_taxable_amount\": 15.0,\r\n                    \"tax_collectable\": 1.43,\r\n                    \"taxable_amount\": 15.0\r\n                }\r\n            ],\r\n            \"special_district_tax_collectable\": 0.34,\r\n            \"special_district_taxable_amount\": 15.0,\r\n            \"special_tax_rate\": 0.0225,\r\n            \"state_tax_collectable\": 0.94,\r\n            \"state_tax_rate\": 0.0625,\r\n            \"state_taxable_amount\": 15.0,\r\n            \"tax_collectable\": 1.43,\r\n            \"taxable_amount\": 15.0\r\n        },\r\n        \"freight_taxable\": false,\r\n        \"has_nexus\": true,\r\n        \"jurisdictions\": {\r\n            \"city\": \"LOS ANGELES\",\r\n            \"country\": \"US\",\r\n            \"county\": \"LOS ANGELES COUNTY\",\r\n            \"state\": \"CA\"\r\n        },\r\n        \"order_total_amount\": 16.5,\r\n        \"rate\": 0.095,\r\n        \"shipping\": 1.5,\r\n        \"tax_source\": \"destination\",\r\n        \"taxable_amount\": 15.0\r\n    }\r\n}";


            var expectedResponse = JsonConvert.DeserializeObject<TaxJarTaxesResponse>(expectedResponseSerialized);

            var mismatches = CompareResults(response, expectedResponse);

            TestResultsListView.ItemsSource = mismatches;

            TestNameLabel.Text = "Order Taxes Test";

            UserDialogs.Instance.HideLoading();

            var mismatchCount = mismatches.Where(m => m.ExpectedValue != m.ReceivedValue).ToList().Count;
            if (mismatchCount > 0)
            {
                TestStatusLabel.Text = "Failed";
                TestStatusLabel.BackgroundColor = Color.Red;
                TestStatusLabel.TextColor = Color.White;
                NumFailedProperties.Text = mismatchCount.ToString();
            }
            else
            {
                TestStatusLabel.Text = "Succeeded";
                TestStatusLabel.BackgroundColor = Color.Green;
                TestStatusLabel.TextColor = Color.White;
                NumFailedProperties.Text = "None";
            }

            ModalBackdrop.IsVisible = true;
            ResultsModalContainer.IsVisible = true;
            
        }

        private async void RunLocationRatesTestBtn_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Running Location Rates Test...");

            var sampleData = new AddressRef
            {
                Street1 = "312 Hurricane Lane",
                City = "Williston",
                State = "VT",
                Country = "US",
                Zip = "05495-2086"
            };

            var taxService = new TaxService(new TaxJarCalculator());
            var response = await taxService.CalculateTaxRateReportForLocationRawResponse(sampleData);

            var expectedResponseSerialized = "{\r\n    \"rate\": {\r\n        \"city\": \"WILLISTON\",\r\n        \"city_rate\": \"0.01\",\r\n        \"combined_district_rate\": \"0.0\",\r\n        \"combined_rate\": \"0.07\",\r\n        \"country\": \"US\",\r\n        \"country_rate\": \"0.0\",\r\n        \"county\": \"CHITTENDEN\",\r\n        \"county_rate\": \"0.0\",\r\n        \"freight_taxable\": true,\r\n        \"state\": \"VT\",\r\n        \"state_rate\": \"0.06\",\r\n        \"zip\": \"05495-2086\"\r\n    }\r\n}";

            var expectedResponse = JsonConvert.DeserializeObject<TaxJarRatesResponse>(expectedResponseSerialized);

            var mismatches = CompareResults(response, expectedResponse);

            TestResultsListView.ItemsSource = mismatches;

            TestNameLabel.Text = "Location Rates Test";

            UserDialogs.Instance.HideLoading();

            var mismatchCount = mismatches.Where(m => m.ExpectedValue != m.ReceivedValue).ToList().Count;
            if (mismatchCount > 0)
            {
                TestStatusLabel.Text = "Failed";
                TestStatusLabel.BackgroundColor = Color.Red;
                TestStatusLabel.TextColor = Color.White;
                NumFailedProperties.Text = mismatchCount.ToString();
            }
            else
            {
                TestStatusLabel.Text = "Succeeded";
                TestStatusLabel.BackgroundColor = Color.Green;
                TestStatusLabel.TextColor = Color.White;
                NumFailedProperties.Text = "None";
            }

            ModalBackdrop.IsVisible = true;
            ResultsModalContainer.IsVisible = true;
        }

        private List<TestCaseMismatch> CompareResults(TaxJarRatesResponse testCase, TaxJarRatesResponse expectedResult)
        {
            List<TestCaseMismatch> Mismatches = new List<TestCaseMismatch>();

           
             Mismatches.Add(new TestCaseMismatch { PropertyName = "Zip", ExpectedValue = expectedResult.rate.zip.ToString(), ReceivedValue = testCase.rate.zip.ToString() });
           
           
             Mismatches.Add(new TestCaseMismatch { PropertyName = "Country", ExpectedValue = expectedResult.rate.country.ToString(), ReceivedValue = testCase.rate.country.ToString() });
           
           
             Mismatches.Add(new TestCaseMismatch { PropertyName = "Country Rate", ExpectedValue = expectedResult.rate.country_rate.ToString(), ReceivedValue = testCase.rate.country_rate.ToString() });
           
           
             Mismatches.Add(new TestCaseMismatch { PropertyName = "State", ExpectedValue = expectedResult.rate.state.ToString(), ReceivedValue = testCase.rate.state.ToString() });
           
           
             Mismatches.Add(new TestCaseMismatch { PropertyName = "State Rate", ExpectedValue = expectedResult.rate.state_rate.ToString(), ReceivedValue = testCase.rate.state_rate.ToString() });
           
           
             Mismatches.Add(new TestCaseMismatch { PropertyName = "County", ExpectedValue = expectedResult.rate.county.ToString(), ReceivedValue = testCase.rate.county.ToString() });
           
           
             Mismatches.Add(new TestCaseMismatch { PropertyName = "County Rate", ExpectedValue = expectedResult.rate.county_rate.ToString(), ReceivedValue = testCase.rate.county_rate.ToString() });
           
           
             Mismatches.Add(new TestCaseMismatch { PropertyName = "City", ExpectedValue = expectedResult.rate.city.ToString(), ReceivedValue = testCase.rate.city.ToString() });
           
           
             Mismatches.Add(new TestCaseMismatch { PropertyName = "City Rate", ExpectedValue = expectedResult.rate.city_rate.ToString(), ReceivedValue = testCase.rate.city_rate.ToString() });
           
           
             Mismatches.Add(new TestCaseMismatch { PropertyName = "Combined District Rate", ExpectedValue = expectedResult.rate.combined_district_rate.ToString(), ReceivedValue = testCase.rate.combined_district_rate.ToString() });
           
           
             Mismatches.Add(new TestCaseMismatch { PropertyName = "Combined Rate", ExpectedValue = expectedResult.rate.combined_rate.ToString(), ReceivedValue = testCase.rate.combined_rate.ToString() });
           
           
             Mismatches.Add(new TestCaseMismatch { PropertyName = "Freight Taxable", ExpectedValue = expectedResult.rate.freight_taxable.ToString(), ReceivedValue = testCase.rate.freight_taxable.ToString() });
           

            return Mismatches;
        }

        private List<TestCaseMismatch> CompareResults(TaxJarTaxesResponse testCase, TaxJarTaxesResponse expectedResult)
        {
            List<TestCaseMismatch> Mismatches = new List<TestCaseMismatch>();

            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Order Total Amount", ExpectedValue = expectedResult.tax.order_total_amount.ToString(), ReceivedValue = testCase.tax.order_total_amount.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Shipping", ExpectedValue = expectedResult.tax.shipping.ToString(), ReceivedValue = testCase.tax.shipping.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Taxable Amount", ExpectedValue = expectedResult.tax.taxable_amount.ToString(), ReceivedValue = testCase.tax.taxable_amount.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Amount To Collect", ExpectedValue = expectedResult.tax.amount_to_collect.ToString(), ReceivedValue = testCase.tax.amount_to_collect.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Rate", ExpectedValue = expectedResult.tax.rate.ToString(), ReceivedValue = testCase.tax.rate.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Has Nexus", ExpectedValue = expectedResult.tax.has_nexus.ToString(), ReceivedValue = testCase.tax.has_nexus.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Freight Taxable", ExpectedValue = expectedResult.tax.freight_taxable.ToString(), ReceivedValue = testCase.tax.freight_taxable.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Tax Source", ExpectedValue = expectedResult.tax.tax_source.ToString(), ReceivedValue = testCase.tax.tax_source.ToString() });
            

            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Jurisdictions City", ExpectedValue = expectedResult.tax.jurisdictions.city.ToString(), ReceivedValue = testCase.tax.jurisdictions.city.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Jurisdictions State", ExpectedValue = expectedResult.tax.jurisdictions.state.ToString(), ReceivedValue = testCase.tax.jurisdictions.state.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Jurisdictions County", ExpectedValue = expectedResult.tax.jurisdictions.county.ToString(), ReceivedValue = testCase.tax.jurisdictions.county.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Jurisdictions Country", ExpectedValue = expectedResult.tax.jurisdictions.country.ToString(), ReceivedValue = testCase.tax.jurisdictions.country.ToString() });
            

            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Taxable Amount", ExpectedValue = expectedResult.tax.breakdown.taxable_amount.ToString(), ReceivedValue = testCase.tax.breakdown.taxable_amount.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Tax Collectable", ExpectedValue = expectedResult.tax.breakdown.tax_collectable.ToString(), ReceivedValue = testCase.tax.breakdown.tax_collectable.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Combined Tax Rate", ExpectedValue = expectedResult.tax.breakdown.combined_tax_rate.ToString(), ReceivedValue = testCase.tax.breakdown.combined_tax_rate.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "State Taxable Amount", ExpectedValue = expectedResult.tax.breakdown.state_taxable_amount.ToString(), ReceivedValue = testCase.tax.breakdown.state_taxable_amount.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "State Tax Rate", ExpectedValue = expectedResult.tax.breakdown.state_tax_rate.ToString(), ReceivedValue = testCase.tax.breakdown.state_tax_rate.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "State Tax Collectable", ExpectedValue = expectedResult.tax.breakdown.state_tax_collectable.ToString(), ReceivedValue = testCase.tax.breakdown.state_tax_collectable.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "County Taxable Amount", ExpectedValue = expectedResult.tax.breakdown.county_taxable_amount.ToString(), ReceivedValue = testCase.tax.breakdown.county_taxable_amount.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "County Tax Rate", ExpectedValue = expectedResult.tax.breakdown.county_tax_rate.ToString(), ReceivedValue = testCase.tax.breakdown.county_tax_rate.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "County Tax Collectable", ExpectedValue = expectedResult.tax.breakdown.county_tax_collectable.ToString(), ReceivedValue = testCase.tax.breakdown.county_tax_collectable.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "City Taxable Amount", ExpectedValue = expectedResult.tax.breakdown.city_taxable_amount.ToString(), ReceivedValue = testCase.tax.breakdown.city_taxable_amount.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "City Tax Rate", ExpectedValue = expectedResult.tax.breakdown.city_tax_rate.ToString(), ReceivedValue = testCase.tax.breakdown.city_tax_rate.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "City Tax Collectable", ExpectedValue = expectedResult.tax.breakdown.city_tax_collectable.ToString(), ReceivedValue = testCase.tax.breakdown.city_tax_collectable.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Special District Taxable Amount", ExpectedValue = expectedResult.tax.breakdown.special_district_taxable_amount.ToString(), ReceivedValue = testCase.tax.breakdown.special_district_taxable_amount.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Special Tax Rate", ExpectedValue = expectedResult.tax.breakdown.special_tax_rate.ToString(), ReceivedValue = testCase.tax.breakdown.special_tax_rate.ToString() });
            
            
              Mismatches.Add(new TestCaseMismatch { PropertyName = "Special District Tax Collectible", ExpectedValue = expectedResult.tax.breakdown.special_district_tax_collectable.ToString(), ReceivedValue = testCase.tax.breakdown.special_district_tax_collectable.ToString() });
            

            
            Mismatches.Add(new TestCaseMismatch { PropertyName = "Breakdown Line Items", ExpectedValue = expectedResult.tax.breakdown.line_items.Count.ToString() + " Items", ReceivedValue = testCase.tax.breakdown.line_items.Count.ToString() + " Items" });
            
            
            
            for (int i = 0; i < expectedResult.tax.breakdown.line_items.Count; i++)
            {
                var expectedLine = expectedResult.tax.breakdown.line_items[i];
                var testCaseLine = testCase.tax.breakdown.line_items[i];

                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} Id", ExpectedValue = expectedLine.id.ToString(), ReceivedValue = testCaseLine.id.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} Taxable Amount", ExpectedValue = expectedLine.taxable_amount.ToString(), ReceivedValue = testCaseLine.taxable_amount.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} Tax Collectable", ExpectedValue = expectedLine.tax_collectable.ToString(), ReceivedValue = testCaseLine.tax_collectable.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} Combined Tax Rate", ExpectedValue = expectedLine.combined_tax_rate.ToString(), ReceivedValue = testCaseLine.combined_tax_rate.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} State Taxable Amount", ExpectedValue = expectedLine.state_taxable_amount.ToString(), ReceivedValue = testCaseLine.state_taxable_amount.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} State Sales Tax Rate", ExpectedValue = expectedLine.state_sales_tax_rate.ToString(), ReceivedValue = testCaseLine.state_sales_tax_rate.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} State Amount", ExpectedValue = expectedLine.state_amount.ToString(), ReceivedValue = testCaseLine.state_amount.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1}  County Taxable Amount", ExpectedValue = expectedLine.county_taxable_amount.ToString(), ReceivedValue = testCaseLine.county_taxable_amount.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} County Tax Rate", ExpectedValue = expectedLine.county_tax_rate.ToString(), ReceivedValue = testCaseLine.county_tax_rate.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} County Amount", ExpectedValue = expectedLine.county_amount.ToString(), ReceivedValue = testCaseLine.county_amount.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} City Taxable Amount", ExpectedValue = expectedLine.city_taxable_amount.ToString(), ReceivedValue = testCaseLine.city_taxable_amount.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} City Tax Rate", ExpectedValue = expectedLine.city_tax_rate.ToString(), ReceivedValue = testCaseLine.city_tax_rate.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} City Amount", ExpectedValue = expectedLine.city_amount.ToString(), ReceivedValue = testCaseLine.city_amount.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} Special District Taxable Amount", ExpectedValue = expectedLine.special_district_taxable_amount.ToString(), ReceivedValue = testCaseLine.special_district_taxable_amount.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} Special Tax Rate", ExpectedValue = expectedLine.special_tax_rate.ToString(), ReceivedValue = testCaseLine.special_tax_rate.ToString() });
                
                
                  Mismatches.Add(new TestCaseMismatch { PropertyName = $"Line {i + 1} Special District Amount", ExpectedValue = expectedLine.special_district_amount.ToString(), ReceivedValue = testCaseLine.special_district_amount.ToString() });
                
            }
            

            return Mismatches;
        }

        private void CloseResultsModalBtn_Clicked(object sender, EventArgs e)
        {
            ModalBackdrop.IsVisible = false;
            ResultsModalContainer.IsVisible = false;
        }

        private void HelpBtn_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.Alert("These test cases report successful tests if the response comes back and matches an expected response. The expected responses were generated by executing the sample data in the Tax Jar API Sandbox. If my call to their API returns the same response, I consider this to be a successful test case since the API call worked as expected.", "What is Happening?", "Close");
        }
    }
}