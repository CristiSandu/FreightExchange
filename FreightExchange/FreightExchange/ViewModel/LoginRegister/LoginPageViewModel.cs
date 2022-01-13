using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FreightExchange.ViewModel.LoginRegister
{
    public class LoginPageViewModel : BaseViewModel
    {
        public string APIKey = "AIzaSyDnIz9wcDVHpA-lYDiqIx_O840pjs264Ps";
        public string Email { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; set; }

        public LoginPageViewModel()
        {
            LoginCommand = new Command(async () =>
            {
                if (string.IsNullOrEmpty(Email) ||
                    string.IsNullOrEmpty(Password) ||
                    Password.Length <= 6)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Some fields are empty", "Ok", "Cancel");
                }
                else
                {
                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(APIKey));
                    try
                    {
                        var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);
                        var content = await auth.GetFreshAuthAsync();
                        var serializer = JsonConvert.SerializeObject(content);

                        var user = auth.User;
                        Xamarin.Essentials.Preferences.Set("FirebaseRefreshToken", serializer);
                        
                        Models.UserModel User = await Services.FirestoreServiceProvider.GetFirestoreUser(user.LocalId);
                        Xamarin.Essentials.Preferences.Set("Role", User.Role);

                        switch (User.Role)
                        {
                            case "Transportator":
                                Application.Current.MainPage = new NavigationPage(new Views.MainPage());
                                break;
                            case "Expeditor":
                                Application.Current.MainPage = new NavigationPage(new Views.MainPage());
                                break;
                            case "Admin":
                                Application.Current.MainPage = new NavigationPage(new Views.AdminViews.AdminTabbedPage());
                                break;
                            default:
                                Application.Current.MainPage = new NavigationPage(new Views.MainPage());
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Some fields are empty", "Ok", "Cancel");
                    }
                }
            });
        }

    }
}
