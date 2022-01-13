using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreightExchange.Views.AdminViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenericElementsListPage : ContentPage
    {
        public GenericElementsListPage()
        {
            InitializeComponent();
        }

        private void accountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}