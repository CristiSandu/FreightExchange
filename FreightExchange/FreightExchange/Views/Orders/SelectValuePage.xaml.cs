using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreightExchange.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectValuePage : ContentPage
    {
        public SelectValuePage()
        {
            InitializeComponent();
        }

        private void ordersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}