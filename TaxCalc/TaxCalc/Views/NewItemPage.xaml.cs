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
    public partial class NewItemPage : ContentPage
    {
        private Item NewItem { get; set; }
        private List<Item> Items { get; set; }
        private FirebaseDBService FirebaseDB { get; set; }
        private bool UpdateExistingItem { get; set; }
        private int IndexToUpdate { get; set; }
        public NewItemPage()
        {
            InitializeComponent();
            NewItem = new Item();

            FirebaseDB = DependencyService.Get<FirebaseDBService>();
            UpdateExistingItem = false;
        }

        public NewItemPage(Item item)
        {
            InitializeComponent();
            NewItem = item;

            FirebaseDB = DependencyService.Get<FirebaseDBService>();
            UpdateExistingItem = true;

            NameInput.Text = NewItem.Name;
            DescriptionInput.Text = NewItem.Description;
            UnitPriceInput.Text = NewItem.UnitPrice.ToString();
        }

        private string ValidateInputs()
        {
            string message = "";

            if (String.IsNullOrEmpty(NewItem.Name))
            {
                message += "- Name\n";
            }
            if (String.IsNullOrEmpty(NewItem.Description))
            {
                message += "- Description\n";
            }

            return message;
        }

        private void NameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            NewItem.Name = e.NewTextValue;
        }

        private void DescriptionInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            NewItem.Description = e.NewTextValue;
        }

        private void UnitPriceInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            NewItem.UnitPrice = Convert.ToDecimal(e.NewTextValue);
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            var MessageString = ValidateInputs();

            if (String.IsNullOrEmpty(MessageString))
            {
                UserDialogs.Instance.ShowLoading("Adding Address...");
                if (UpdateExistingItem)
                {
                    Items[IndexToUpdate] = NewItem;
                }
                else
                {
                    Items.Add(NewItem);
                }

                await FirebaseDB.UpdateInventoryItems(Items);
                UserDialogs.Instance.HideLoading();
                await Navigation.PopAsync();
            }
            else
            {
                UserDialogs.Instance.Alert("The following fields are required:\n" + MessageString, "Missing Data", "Close");
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Items = await FirebaseDB.GetInventoryItems();

            if (UpdateExistingItem)
            {
                foreach (var item in Items)
                {
                    if (item.Name == NewItem.Name)
                    {
                        IndexToUpdate = Items.IndexOf(item);
                    }
                }
            }
        }
    }
}