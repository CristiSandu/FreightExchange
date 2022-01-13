using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FreightExchange.ViewModel.Admin
{
    public class AddGoodsCategoryPageViewModel : BaseViewModel
    {
        public string Name { get; set; }

        public Command SaveCommand { get; set; }
        public AddGoodsCategoryPageViewModel()
        {
            SaveCommand = new Command(async () =>
            {
                if (string.IsNullOrEmpty(Name))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Some fields are empty", "Ok", "Cancel");
                    return;
                }

                try
                {
                    if (!await Services.FirestoreServiceProvider.ExistGood(Name))
                    {
                        await Services.FirestoreServiceProvider.CreateGoodAsync(new Models.GoodsModel { Id = Name, Name = Name });
                    }else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "This good exist in catalog", "Ok", "Cancel");
                        return ;
                    }

                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Something whent wrong", "Ok", "Cancel");
                }
                await App.Current.MainPage.Navigation.PopAsync();
            });
        }
    }
}
