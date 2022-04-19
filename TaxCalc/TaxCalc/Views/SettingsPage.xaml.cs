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
    public partial class SettingsPage : ContentPage
    {
        private List<AddressRef> CompanyAddresses { get; set; }
        private FirebaseDBService FirebaseDB { get; set; }
        public SettingsPage()
        {
            InitializeComponent();
            FirebaseDB = DependencyService.Get<FirebaseDBService>();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            CompanyAddresses = await FirebaseDB.GetCompanyAddresses();
            AddressesListView.ItemsSource = CompanyAddresses;
        }

        private async void AddressesListView_Refreshing(object sender, EventArgs e)
        {
            AddressesListView.IsRefreshing = true;
            CompanyAddresses = await FirebaseDB.GetCompanyAddresses();
            AddressesListView.IsRefreshing = false;
        }

        private async void AddAddressBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewAddressPage());
        }

        private async void AddressesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var addr = e.Item as AddressRef;

            await Navigation.PushAsync(new NewAddressPage(addr));
        }
    }
}