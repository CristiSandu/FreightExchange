using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreightExchange
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(Xamarin.Essentials.Preferences.Get("FirebaseRefreshToken", "")))
            {
                MainPage = new NavigationPage(new Views.MainPage());
            }
            else
            {
                MainPage = new NavigationPage(new Views.LoginRegister.LoginPage());
            }
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
