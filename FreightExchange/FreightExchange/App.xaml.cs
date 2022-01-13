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
                        MainPage = new NavigationPage(new Views.MapPage());
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
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "AAPKde6098c66c264fbaa5a7b5db9cc112f9FhU7-0GD2XVloSUUrOESUX3aFNOrQTgHGiHl8oCJqy-P-XgqhCMwqezY7soC6wrf";
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
