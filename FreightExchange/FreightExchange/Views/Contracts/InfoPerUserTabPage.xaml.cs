using FreightExchange.ViewModel.Carriers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreightExchange.Views.Contracts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPerUserTabPage : TabbedPage
    {

        public InfoPerUserTabPage()
        {
            InitializeComponent();


            BindingContext = this;
        }
    }
}