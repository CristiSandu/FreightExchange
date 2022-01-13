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
                switch (Xamarin.Essentials.Preferences.Get("Role", ""))
                {
                    case "Transportator":
                        MainPage = new NavigationPage(new Views.MainPage());
                        break;
                    case "Expeditor":
                        MainPage = new NavigationPage(new Views.MainPage());
                        break;
                    case "Admin":
                        MainPage = new NavigationPage(new Views.AdminViews.AdminTabbedPage());
                        break;
                    default:
                        MainPage = new NavigationPage(new Views.MainPage());
                        break;
                }
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
