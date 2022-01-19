using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FreightExchange.ViewModel.Carriers
{
    public class OrderFormPageViewModel : BaseViewModel
    {
        public Models.OrderModel Order { get; set; } = new Models.OrderModel { StartDate = DateTime.Now, EndDate = DateTime.Now, MaxStartDate = DateTime.Now, MaxEndDate = DateTime.Now };

        public ICommand SaveCommand { get; set; }

        public OrderFormPageViewModel()
        {
            SaveCommand = new Command(async () =>
            {
                try
                {
                    Order.IdClient = Xamarin.Essentials.Preferences.Get("IdUser", "");
                    await Services.FirestoreServiceProvider.CreateOrderAsync(Order);
                    await App.Current.MainPage.DisplayAlert("Success", "Some error occured", "Ok", "Cancel");

                    await App.Current.MainPage.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Some error occured", "Ok", "Cancel");
                }
            });
        }
    }
}
