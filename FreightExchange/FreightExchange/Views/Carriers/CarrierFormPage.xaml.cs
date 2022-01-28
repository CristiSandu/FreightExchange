using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreightExchange.Views.Carriers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarrierFormPage : ContentPage
    {
        public CarrierFormPage()
        {
            InitializeComponent();
            BindingContext = new ViewModel.Carriers.CarrierFormPageViewModel();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}