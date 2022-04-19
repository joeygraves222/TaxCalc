using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Models;

namespace TaxCalc.Interfaces
{
    public interface ITaxCalculator
    {
        Task<double> GetRatesByLocationAsync(AddressRef Addr);
        Task<TaxRatesModel> GetTaxRatesReportByLocation(AddressRef Addr);
        Task<TaxJarRatesResponse> GetTaxRatesReportByLocationRawResponse(AddressRef Addr);
        Task<decimal> CalculateTaxesForOrderAsync(Order order);
        Task<TaxJarTaxesResponse> CalculateTaxesForOrderAsync(TaxJarTaxesPayload payload);
    }
}
