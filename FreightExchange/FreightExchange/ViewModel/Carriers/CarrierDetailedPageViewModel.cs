using FreightExchange.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace FreightExchange.ViewModel.Carriers
{
    public class CarrierDetailedPageViewModel : BaseViewModel
    {
        public ObservableCollection<Models.OrderModel> ListOf { get; set; }

        private OrderModel _selectedElement;
        public OrderModel SelectedElement
        {
            get 
            { 
                return _selectedElement; 
            }
            set
            {
                if (_selectedElement != value)
                {
                    _selectedElement = value;
                    OnPropertyChanged(nameof(SelectedElement));
                }
            }
        }

        public string PageTitle { get; set; }
        public Command RefreshCommand { get; set; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        public CarrierModel Carrier { get; set; }

        public CarrierDetailedPageViewModel(CarrierModel carrierModel)
        {
            Carrier = carrierModel;
            GetData();

            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                List<Models.OrderModel> orders = await Services.FirestoreServiceProvider.GetFirestoreAllOrdersForCarrierAsync(Carrier.ReservedList != null ? Carrier.ReservedList : new List<string>());
                ListOf = new ObservableCollection<OrderModel>(orders);
                OnPropertyChanged(nameof(ListOf));
                PageTitle = $"List of Transporter Offers - {ListOf.Count}";
                OnPropertyChanged(nameof(PageTitle));
                IsRefreshing = false;
            });
        }

        public async void GetData()
        {
            List<Models.OrderModel> orders = await Services.FirestoreServiceProvider.GetFirestoreAllOrdersForCarrierAsync(Carrier.ReservedList != null ? Carrier.ReservedList : new List<string>());
            ListOf = new ObservableCollection<OrderModel>(orders);
            OnPropertyChanged(nameof(ListOf));

            PageTitle = $"List of Transporter Offers - {ListOf.Count}";
            OnPropertyChanged(nameof(PageTitle));
        }
    }
}
