using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FreightExchange.ViewModel.LoginRegister
{
    public class RegisterPageViewModel : BaseViewModel
    {
        public string APIKey = "AIzaSyDnIz9wcDVHpA-lYDiqIx_O840pjs264Ps";

        public List<string> Roles { get; set; } = new List<string> { "Admin", "Transportator", "Expeditor" };
        public Models.UserModel User { get; set; } = new Models.UserModel();
        public string Password { get; set; }

        public ICommand RegisterCommand { get; set; }

        public RegisterPageViewModel()
        {

            RegisterCommand = new Command(async () =>
            {
                if (string.IsNullOrEmpty(User.Role) ||
                    string.IsNullOrEmpty(Password) ||
                    string.IsNullOrEmpty(User.Name) ||
                    string.IsNullOrEmpty(User.SurName) ||
                    Password.Length <= 6)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Some fields are empty", "Ok", "Cancel");
                }
                else
                {
                    try
                    {
                        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(APIKey));
                        var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(User.Email, Password);
                        string getToken = auth.FirebaseToken;

                        var user = auth.User;

                        await App.Current.MainPage.DisplayAlert("Error", getToken, "Ok", "Cancel");
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Some Error, retry", "Ok", "Cancel");
                    }
                }
            });
        }
    }
}
