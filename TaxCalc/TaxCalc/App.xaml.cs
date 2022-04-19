using System;
using TaxCalc.Services;
using TaxCalc.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxCalc
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();

            DependencyService.RegisterSingleton(new FirebaseDBService("https://taxcalculator-347420-default-rtdb.firebaseio.com/"));
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
