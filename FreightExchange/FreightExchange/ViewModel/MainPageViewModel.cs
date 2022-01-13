using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FreightExchange.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public string APIKey = "AIzaSyDnIz9wcDVHpA-lYDiqIx_O840pjs264Ps";
        public ICommand LogOutCommand { get; set; }
        public string UID { get; set; }
        public string Email { get; set; }

        public Models.UserModel User { get; set; }
        public MainPageViewModel()
        {
            GetRefreshToken();

            LogOutCommand = new Command(async () =>
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(APIKey));
                try
                {
                    Preferences.Remove("FirebaseRefreshToken");
                    Application.Current.MainPage = new NavigationPage(new Views.LoginRegister.LoginPage());
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Some fields are empty", "Ok", "Cancel");
                }
            });
        }

        async private void GetRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(APIKey));
            try
            {
                var savedfirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Xamarin.Essentials.Preferences.Get("FirebaseRefreshToken", ""));
                var RefreshContent = await authProvider.RefreshAuthAsync(savedfirebaseAuth);
                Xamarin.Essentials.Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(RefreshContent));
                var user = savedfirebaseAuth.User;

                UID = savedfirebaseAuth.User.LocalId;
                Email = savedfirebaseAuth.User.Email;

                User = await Services.FirestoreServiceProvider.GetFirestoreUser(UID);
                Xamarin.Essentials.Preferences.Set("Role", User.Role);



                OnPropertyChanged(nameof(UID));
                OnPropertyChanged(nameof(User));
                OnPropertyChanged(nameof(Email));
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Token Expired", "Ok", "Cancel");
            }
        }
    }
}
