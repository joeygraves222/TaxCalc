using Acr.UserDialogs;
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
    public partial class TaxRatesPage : ContentPage
    {
        private AddressRef Addr { get; set; } = new AddressRef();

        public TaxRatesPage()
        {
            InitializeComponent();
        }

        private void Street1Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            Addr.Street1 = e.NewTextValue;
        }

        private void CityInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Addr.City = e.NewTextValue;
        }

        private void StateInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Addr.State = e.NewTextValue;
        }

        private void ZipInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Addr.Zip = e.NewTextValue;
        }

        private void CountryInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Addr.Country = e.NewTextValue;
        }
        private async void GetRatesBtn_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Addr.Zip))
            {
                UserDialogs.Instance.ShowLoading("Calculating...");

                var taxService = new TaxService(new TaxJarCalculator());
                UserDialogs.Instance.HideLoading();
                var results = await taxService.CalculateTaxRateReportForLocation(Addr);
                ModalBackdrop.IsVisible = true;
                ResultsModalContainer.IsVisible = true;

                ResultsAddressLabel.Text = Addr.GetAddressDisplay();
                ResultsRatesLabel.Text = results.ToDisplayString();
            }
            else
            {
                UserDialogs.Instance.Alert("Address is incomplete", "Missing Data", "Close");
            }
        }

        private void CloseResultsModalBtn_Clicked(object sender, EventArgs e)
        {
            ModalBackdrop.IsVisible = false;
            ResultsModalContainer.IsVisible = false;
        }
    }
}