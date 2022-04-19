using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaxCalc.Interfaces;
using TaxCalc.Models;
using TaxCalc.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxCalc.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewOrderPage : ContentPage
    {
        private List<AddressRef> CompanyAddresses { get; set; }
        private Order _Order { get; set; } = new Order();

        private List<Item> InventoryItems { get; set; }
        private FirebaseDBService FirebaseDB { get; set; }


        private ObservableCollection<OrderLineItem> LineItems { get; set; } = new ObservableCollection<OrderLineItem>();
        public NewOrderPage()
        {
            InitializeComponent();
            FirebaseDB = DependencyService.Get<FirebaseDBService>();
            BindingContext = this;
            LineItemsListView.ItemsSource = LineItems;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            CompanyAddresses = await FirebaseDB.GetCompanyAddresses();
            CompanyAddressesPicker.ItemsSource = CompanyAddresses;

            InventoryItems = await FirebaseDB.GetInventoryItems();
            ItemPicker.ItemsSource = InventoryItems;
        }

        private async void UpdateOrderTotals()
        {
            var Subtotal = LineItems != null ? LineItems.Sum(i => (i.OrderQty * i.LineItem.UnitPrice)) : 0;
            OrderSubtotalLabel.Text = Subtotal.ToString("C");

            if (!String.IsNullOrEmpty(ShippingCostInput.Text))
            {
                _Order.ShippingCost = Convert.ToDecimal(ShippingCostInput.Text);
                OrderShippingLabel.Text = _Order.ShippingCost.ToString("C");
            }
            else
            {
                _Order.ShippingCost = 0;
                OrderShippingLabel.Text = _Order.ShippingCost.ToString("C");
            }
        }

        private bool OrderCanCalculateTaxes()
        {

            if (_Order.Items == null || _Order.Items.Count == 0)
            {
                return false;
            }
            if (_Order.PurchaserAddress == null || String.IsNullOrEmpty(_Order.PurchaserAddress.Country))
            {
                return false;
            }

            return true;
        }

        private void AddNewLineItemBtn_Clicked(object sender, EventArgs e)
        {
            ModalBackdrop.IsVisible = true;
            ModalContainer.IsVisible = true;
        }

        private void CancelAddLineBtn_Clicked(object sender, EventArgs e)
        {
            ModalBackdrop.IsVisible = false;
            ModalContainer.IsVisible = false;
        }

        private string ValidateAddLineInputs()
        {
            string message = "";
            if (ItemPicker.SelectedIndex == -1)
            {
                message += "- No Item Selected\n"; 
            }
            if (String.IsNullOrEmpty(QtyInput.Text))
            {
                message += "- No Quantity Entered\n";
            }

            return message;
        }

        private void SaveLineBtn_Clicked(object sender, EventArgs e)
        {
            //Add Line Item
            string ValidationErrors = ValidateAddLineInputs();

            if (String.IsNullOrEmpty(ValidationErrors))
            {

                var Line = new OrderLineItem()
                {
                    LineItem = ItemPicker.SelectedItem as Item,
                    OrderQty = Convert.ToInt32(QtyInput.Text)
                };

                LineItems.Add(Line);

                ModalBackdrop.IsVisible = false;
                ModalContainer.IsVisible = false;

                ItemPicker.SelectedItem = null;
                QtyInput.Text = "";
            }
            else
            {
                UserDialogs.Instance.Alert("Missing Data:\n" + ValidationErrors, "Required Data Missing", "Close");
            }
            UpdateOrderTotals();
            _Order.Items = LineItems.ToList();
        }

        private void DeleteLineItemBtn_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            OrderLineItem itemToDelete = menuItem.CommandParameter as OrderLineItem;

            LineItems.Remove(itemToDelete);
        }

        private void ShippingCostInput_Unfocused(object sender, FocusEventArgs e)
        {
            UpdateOrderTotals();
        }

        private void CompanyAddressesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CompanyAddressesPicker.SelectedIndex != -1)
            {
                var SelectedAddress = CompanyAddresses[CompanyAddressesPicker.SelectedIndex];

                _Order.SellerAddress = SelectedAddress;
            }
        }

        private async void ReviewOrderBtn_Clicked(object sender, EventArgs e)
        {
            if (OrderCanCalculateTaxes())
            {
                UserDialogs.Instance.ShowLoading("Crunching Numbers...");
                var _taxService = new TaxService(new TaxJarCalculator());

                var orderTaxes = await _taxService.CalculateTaxesForOrder(_Order);

                UserDialogs.Instance.HideLoading();

                NumItemsOnOrderLabel.Text = _Order.Items.Count + " Items";
                var itemsSum = _Order.Items.Sum(i => i.OrderQty * i.LineItem.UnitPrice);
                NumItemsCostLabel.Text = itemsSum.ToString("C");
                ReviewShippingCostsLabel.Text = _Order.ShippingCost.ToString("C");
                ReviewOrderTaxesLabel.Text = orderTaxes.ToString("C");
                TotalCostDueLabel.Text = (itemsSum + _Order.ShippingCost + orderTaxes).ToString("C");

                ModalBackdrop.IsVisible = true;
                ReviewOrderModalContainer.IsVisible = true;
            }
            else
            {
                UserDialogs.Instance.Toast("Error: Order Incomplete", TimeSpan.FromSeconds(3));
            }
        }

        private void FirstNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            _Order.CustomerFirstName = e.NewTextValue;
        }

        private void LastNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            _Order.CustomerLastName = e.NewTextValue;
        }

        private void Street1Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_Order.PurchaserAddress == null) _Order.PurchaserAddress = new AddressRef();
            _Order.PurchaserAddress.Street1 = e.NewTextValue;
        }

        private void Street2Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_Order.PurchaserAddress == null) _Order.PurchaserAddress = new AddressRef();
            _Order.PurchaserAddress.Street2 = e.NewTextValue;
        }

        private void CityInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_Order.PurchaserAddress == null) _Order.PurchaserAddress = new AddressRef();
            _Order.PurchaserAddress.City = e.NewTextValue;
        }

        private void StateInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_Order.PurchaserAddress == null) _Order.PurchaserAddress = new AddressRef();
            _Order.PurchaserAddress.State = e.NewTextValue;
        }

        private void ZipInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_Order.PurchaserAddress == null) _Order.PurchaserAddress = new AddressRef();
            _Order.PurchaserAddress.Zip = e.NewTextValue;
        }

        private void CountryInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_Order.PurchaserAddress == null) _Order.PurchaserAddress = new AddressRef();
            _Order.PurchaserAddress.Country = e.NewTextValue;
        }

        private void CloseReviewOrderModalBtn_Clicked(object sender, EventArgs e)
        {
            ReviewOrderModalContainer.IsVisible = false;
            ModalBackdrop.IsVisible = false;
        }
    }
}