using FreightExchange.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace FreightExchange.ViewModel.Carriers
{
    public class OrderDetailedPageViewModel : BaseViewModel
    {
        public ObservableCollection<Models.CarrierModel> ListOf { get; set; }

        private CarrierModel _selectedElement;
        public CarrierModel SelectedElement
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
        public Command SelectionOrderChanges { get; set; }

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

        public OrderModel Order { get; set; }

        public OrderDetailedPageViewModel(OrderModel orderModel)
        {
            Order = orderModel;
            GetData();

            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                List<Models.CarrierModel> orders = await Services.FirestoreServiceProvider.GetFirestoreAllCarrierOffertsAsync();
                ListOf = new ObservableCollection<CarrierModel>(orders);
                OnPropertyChanged(nameof(ListOf));
                PageTitle = $"Order {Order.Id}";
                OnPropertyChanged(nameof(PageTitle));
                IsRefreshing = false;
            });

            SelectionOrderChanges = new Command(async () =>
            {
                var selected = SelectedElement;
                int i = 0;
            });

            SaveOffertValueCommand = new Command(async () =>
            {
                Order.Id_value = Order.Id;
                if (SelectedElement.ReservedList != null)
                {
                    SelectedElement.ReservedList.Add(Order);
                }
                else
                {
                    SelectedElement.ReservedList = new List<OrderModel> { Order};
                }

                if (!await Services.FirestoreServiceProvider.UpdateCarrierOffertAsync(SelectedElement))
                {
                    await Application.Current.MainPage.DisplayAlert("Error on save", "A error occured", "Ok");
                    return;
                }

                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        public async void GetData()
        {
            List<Models.CarrierModel> orders = await Services.FirestoreServiceProvider.GetFirestoreAllCarrierOffertsAsync();
            ListOf = new ObservableCollection<CarrierModel>(orders);
            OnPropertyChanged(nameof(ListOf));

            PageTitle = $"Order {Order.Id}";
            OnPropertyChanged(nameof(PageTitle));
        }
    }
}
