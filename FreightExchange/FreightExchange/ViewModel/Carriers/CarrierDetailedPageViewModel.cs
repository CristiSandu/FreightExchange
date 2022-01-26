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
        public Command SaveOffertValueCommand { get; set; }


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

        public CarrierModel Order { get; set; }

        public CarrierDetailedPageViewModel(CarrierModel carrierModel)
        {
            Order = carrierModel;
            GetData();

            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                List<Models.OrderModel> orders = Order.ReservedList != null ? Order.ReservedList : new List<OrderModel>();
                ListOf = new ObservableCollection<OrderModel>(orders);
                OnPropertyChanged(nameof(ListOf));
                PageTitle = $"List of Transporter Offers - {ListOf.Count}";
                OnPropertyChanged(nameof(PageTitle));
                IsRefreshing = false;
            });

            SaveOffertValueCommand = new Command(async () =>
            {
                SelectedElement.Transport = Order;
                if (!await Services.FirestoreServiceProvider.UpdateOrderAsync(SelectedElement))
                {
                    await Application.Current.MainPage.DisplayAlert("Error on save", "A error occured", "Ok");
                    return;
                }

                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        public async void GetData()
        {
            List<Models.OrderModel> orders = Order.ReservedList != null ? Order.ReservedList : new List<OrderModel>();
            ListOf = new ObservableCollection<OrderModel>(orders);
            OnPropertyChanged(nameof(ListOf));

            PageTitle = $"List of Transporter Offers - {ListOf.Count}";
            OnPropertyChanged(nameof(PageTitle));
        }
    }
}
