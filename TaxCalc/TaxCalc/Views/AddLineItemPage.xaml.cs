using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxCalc.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddLineItemPage : ContentPage
    {
        public AddLineItemPage()
        {
            InitializeComponent();
        }

        private async void CancelBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void AddLineBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}