using FreightExchange.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace FreightExchange.ViewModel.Carriers
{
    public class OrdersOffertsListViewModel : BaseViewModel
    {
        public ObservableCollection<Models.OrderModel> ListOf { get; set; }
        public Command<Models.OrderModel> Delete { get; private set; }

        private OrderModel _selectedElement;
        public OrderModel SelectedElement
        {
            get { return _selectedElement; }
            set
            {
                if (_selectedElement != value)
                {
                    _selectedElement = value;
                    OnPropertyChanged(nameof(SelectedElement));
                    OpenPage.Execute(_selectedElement);
                }
            }
        }

        public string PageTitle { get; set; }
        public Command RefreshCommand { get; set; }
        public Command<OrderModel> OpenPage { get; set; }
        public Command AddNewElementCommand { get; set; }

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
        public bool IsAddVisible { get; set; } = true;

        public OrdersOffertsListViewModel()
        {
            GetData();
            Delete = new Command<Models.OrderModel>(async good =>
            {
                if (await Services.FirestoreServiceProvider.DeleteOrderAsync(good))
                {
                    ListOf.Remove(good);
                }
            });

            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                string userId = Xamarin.Essentials.Preferences.Get("IdUser", "");
                List<Models.OrderModel> orders = await Services.FirestoreServiceProvider.GetFirestoreAllOrdersForUserAsync(userId);
                ListOf = new ObservableCollection<OrderModel>(orders);
                OnPropertyChanged(nameof(ListOf));
                PageTitle = $"List of Orders - {ListOf.Count}";
                OnPropertyChanged(nameof(PageTitle));
                IsRefreshing = false;
            });

            AddNewElementCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.Orders.OrdersFormPage());
            });

            OpenPage = new Command<Models.OrderModel>(async order =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.Orders.SelectValuePage { BindingContext = new ViewModel.Carriers.OrderDetailedPageViewModel(order) });
            });
        }

        public async void GetData()
        {
            string userId = Xamarin.Essentials.Preferences.Get("IdUser", "");
            List<Models.OrderModel> orders = await Services.FirestoreServiceProvider.GetFirestoreAllOrdersForUserAsync(userId);
            ListOf = new ObservableCollection<OrderModel>(orders);
            OnPropertyChanged(nameof(ListOf));

            PageTitle = $"List of Orders - {ListOf.Count}";
            OnPropertyChanged(nameof(PageTitle));

        }
    }
}
