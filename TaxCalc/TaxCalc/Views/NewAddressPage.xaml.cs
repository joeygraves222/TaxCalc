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
    public partial class NewAddressPage : ContentPage
    {
        private AddressRef NewAddress { get; set; }
        private List<AddressRef> Addresses { get; set; }
        private FirebaseDBService FirebaseDB { get; set; }
        private bool UpdateExistingAddress { get; set; }
        private int IndexToUpdate { get; set; }
        public NewAddressPage()
        {
            InitializeComponent();
            NewAddress = new AddressRef();

            FirebaseDB = DependencyService.Get<FirebaseDBService>();
            UpdateExistingAddress = false;
        }

        public NewAddressPage(AddressRef AddrToUpdate)
        {
            InitializeComponent();
            NewAddress = AddrToUpdate;

            FirebaseDB = DependencyService.Get<FirebaseDBService>();
            UpdateExistingAddress = true;

            NameInput.Text = NewAddress.Name;
            Street1Input.Text = NewAddress.Street1;
            Street2Input.Text = NewAddress.Street2;
            CityInput.Text = NewAddress.City;
            StateInput.Text = NewAddress.State;
            ZipInput.Text = NewAddress.Zip;
            CountryInput.Text = NewAddress.Country;
        }

        private void NameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            NewAddress.Name = e.NewTextValue;
        }

        private void Street1Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            NewAddress.Street1 = e.NewTextValue;
        }

        private void Street2Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            NewAddress.Street2 = e.NewTextValue;
        }

        private void CityInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            NewAddress.City = e.NewTextValue;
        }

        private void StateInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            NewAddress.State = e.NewTextValue;
        }

        private void ZipInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            NewAddress.Zip = e.NewTextValue;
        }

        private void CountryInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            NewAddress.Country = e.NewTextValue;
        }

        private async void SaveAddressBtn_Clicked(object sender, EventArgs e)
        {
            var MessageString = ValidateInputs();

            if (String.IsNullOrEmpty(MessageString))
            {
                UserDialogs.Instance.ShowLoading("Adding Address...");
                if (UpdateExistingAddress)
                {
                    Addresses[IndexToUpdate] = NewAddress;
                }
                else
                {
                    Addresses.Add(NewAddress);
                }

                await FirebaseDB.UpdateCompanyAddresses(Addresses);
                UserDialogs.Instance.HideLoading();
                await Navigation.PopAsync();
            }
            else
            {
                UserDialogs.Instance.Alert("The following fields are required:\n" + MessageString, "Missing Data", "Close");
            }
        }

        private string ValidateInputs()
        {
            string message = "";

            if (String.IsNullOrEmpty(NewAddress.Name))
            {
                message += "- Name\n";
            }
            if (String.IsNullOrEmpty(NewAddress.Street1))
            {
                message += "- Street Address\n";
            }
            if (String.IsNullOrEmpty(NewAddress.City))
            {
                message += "- City\n";
            }
            if (String.IsNullOrEmpty(NewAddress.State))
            {
                message += "- State\n";
            }
            if (String.IsNullOrEmpty(NewAddress.Zip))
            {
                message += "- Zip\n";
            }
            if (String.IsNullOrEmpty(NewAddress.Country))
            {
                message += "- Country\n";
            }

            return message;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Addresses = await FirebaseDB.GetCompanyAddresses();

            if (UpdateExistingAddress)
            {
                foreach (var addr in Addresses)
                {
                    if (addr.Name == NewAddress.Name)
                    {
                        IndexToUpdate = Addresses.IndexOf(addr);
                    }
                }
            }
        }
    }
}