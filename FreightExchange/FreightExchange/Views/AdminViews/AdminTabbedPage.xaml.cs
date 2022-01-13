using FreightExchange.ViewModel.Admin;
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
    public partial class AdminTabbedPage : TabbedPage
    {
        public ListOfTransportators ListOfTransportators { get; set; } = new ListOfTransportators();
        public ListOfExpeditor ListOfExpeditor { get; set; } = new ListOfExpeditor();
        public ListOfGoods ListOfGoods { get; set; } = new ListOfGoods();
        public AdminTabbedPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Views.LoginRegister.LoginPage());
        }
    }
}