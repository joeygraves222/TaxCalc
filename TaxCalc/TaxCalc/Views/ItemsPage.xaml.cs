using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Models;
using TaxCalc.Services;
using TaxCalc.ViewModels;
using TaxCalc.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxCalc.Views
{
    public partial class ItemsPage : ContentPage
    {
        private List<Item> InventoryItems { get; set; }
        private FirebaseDBService FirebaseDB { get; set; }
        public ItemsPage()
        {
            InitializeComponent();

            FirebaseDB = DependencyService.Get<FirebaseDBService>();

            ItemsListView_Refreshing(null, null);
        }

        
        
        private async void ItemsListView_Refreshing(object sender, EventArgs e)
        {
            ItemsListView.IsRefreshing = true;

            InventoryItems = await FirebaseDB.GetInventoryItems();

            ItemsListView.ItemsSource = InventoryItems;
            ItemsListView.IsRefreshing = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }

        private async void AddItemBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewItemPage());
        }

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}