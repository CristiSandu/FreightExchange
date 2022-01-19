using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FreightExchange.ViewModel.Carriers
{
    public class CarrierFormPageViewModel : BaseViewModel
    {
        public Models.CarrierModel CarrierModel { get; set; } = new Models.CarrierModel { StartDate = DateTime.Now, EndDate = DateTime.Now, Dimentions = new Models.Dimensions() };

        public ICommand SaveCommand { get; set; }

        public CarrierFormPageViewModel()
        {
            SaveCommand = new Command(async () =>
            {
                try
                {
                    CarrierModel.IdCarrier = Xamarin.Essentials.Preferences.Get("IdUser", "");
                    await Services.FirestoreServiceProvider.CreateCarrierOffertAsync(CarrierModel);
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
