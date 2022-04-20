using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Interfaces;
using TaxCalc.Models;

namespace TaxCalc.Services
{
    public class TaxJarCalculator : ITaxCalculator
    {
        private static string APIKey = "5da2f821eee4035db4771edab942a4cc";
        private static string BaseURL = "https://api.taxjar.com/v2/";
        private HttpClient Client { get; set; }

        public TaxJarCalculator()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(BaseURL);
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + APIKey);
        }
        public async Task<decimal> CalculateTaxesForOrderAsync(Order order)
        {
            var payload = new TaxJarTaxesPayload(order);

            var response = await Client.PostAsJsonAsync("taxes", payload);

            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TaxJarTaxesResponse>(responseText);

                return Convert.ToDecimal(result.tax.amount_to_collect);
            }
            else
            {
                return 0;
            }
        }

        public async Task<double> GetRatesByLocationAsync(AddressRef Addr)
        {
            if (!String.IsNullOrEmpty(Addr.Zip))
            {
                
                var response = await Client.GetAsync($"rates/{Addr.GetAddressAsURLParameters()}");

                if (response.IsSuccessStatusCode)
                {
                    var responseText = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<TaxJarRatesResponse>(responseText);

                    return Convert.ToDouble(result.rate.combined_rate);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public async Task<TaxRatesModel> GetTaxRatesReportByLocation(AddressRef Addr)
        {
            TaxRatesModel results = new TaxRatesModel();
            if (!String.IsNullOrEmpty(Addr.Zip))
            {
                string Parameters = Addr.GetAddressAsURLParameters();
                var response = await Client.GetAsync($"rates?{Parameters}");

                if (response.IsSuccessStatusCode)
                {
                    var responseText = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<TaxJarRatesResponse>(responseText);

                    results.CityRate = Convert.ToDouble(result.rate.city_rate);
                    results.CountyRate = Convert.ToDouble(result.rate.county_rate);
                    results.StateRate = Convert.ToDouble(result.rate.state_rate);
                    results.CountryRate = Convert.ToDouble(result.rate.country_rate);
                    results.CombinedRate = Convert.ToDouble(result.rate.combined_rate);

                    return results;
                }
                else
                {
                    return results;
                }
            }
            else
            {
                return results;
            }
        }

        public async Task<TaxJarRatesResponse> GetTaxRatesReportByLocationRawResponse(AddressRef Addr)
        {
            if (!String.IsNullOrEmpty(Addr.Zip))
            {
                string Parameters = Addr.GetAddressAsURLParameters();
                var response = await Client.GetAsync($"rates?{Parameters}");

                if (response.IsSuccessStatusCode)
                {
                    var responseText = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<TaxJarRatesResponse>(responseText);

                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<TaxJarTaxesResponse> CalculateTaxesForOrderAsync(TaxJarTaxesPayload payload)
        {
            var response = await Client.PostAsJsonAsync("taxes", payload);

            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TaxJarTaxesResponse>(responseText);

                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
