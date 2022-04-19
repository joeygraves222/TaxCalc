using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Interfaces;
using TaxCalc.Models;

namespace TaxCalc.Services
{
    public class TaxService
    {
        ITaxCalculator CalcToUse { get; set; }
        public TaxService(ITaxCalculator calcToUse)
        {
            CalcToUse = calcToUse;
        }

        public async Task<decimal> CalculateTaxesForOrder(Order order)
        {
            return await CalcToUse.CalculateTaxesForOrderAsync(order);
        }

        public async Task<TaxJarTaxesResponse> CalculateTaxesForOrder(TaxJarTaxesPayload payload)
        {
            return await CalcToUse.CalculateTaxesForOrderAsync(payload);
        }

        public async Task<double> CalculateTaxRateForLocation(AddressRef addr)
        {
            return await CalcToUse.GetRatesByLocationAsync(addr);
        }

        public async Task<TaxRatesModel> CalculateTaxRateReportForLocation(AddressRef addr)
        {
            return await CalcToUse.GetTaxRatesReportByLocation(addr);
        }

        public async Task<TaxJarRatesResponse> CalculateTaxRateReportForLocationRawResponse(AddressRef addr)
        {
            return await CalcToUse.GetTaxRatesReportByLocationRawResponse(addr);
        }

    }
}
