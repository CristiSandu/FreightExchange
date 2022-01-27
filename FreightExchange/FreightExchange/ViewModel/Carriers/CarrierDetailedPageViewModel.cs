using Esri.ArcGISRuntime.Geometry;
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

                MapPoint point1 = await Services.RoutGeneratorService.SearchAddress(SelectedElement.StartPlace);
                MapPoint point2 = await Services.RoutGeneratorService.SearchAddress(SelectedElement.EndPlace);
                double dist = Services.RoutGeneratorService.GetDistanceBetweenTwoPoints(point1, point2);

                double price = Order.FullPrice * Convert.ToInt32(dist);

                bool answer = await App.Current.MainPage.DisplayAlert("Question?", $"Price {price} \nBudget: {SelectedElement.PriceRange}", "Yes", "No");

                if (answer)
                {
                    ContractModel carrier = new ContractModel
                    {
                        Sender = await Services.FirestoreServiceProvider.GetFirestoreUser(SelectedElement.IdClient),
                        Transporter = await Services.FirestoreServiceProvider.GetFirestoreUser(Order.IdCarrier),
                        StartPlace = SelectedElement.StartPlace,
                        EndPlace = SelectedElement.EndPlace,
                        Transporter_Id = Order.IdCarrier,
                        Sender_Id = SelectedElement.IdClient,
                        Price = $"{Order.FullPrice * Convert.ToInt32(dist)}",
                        FreightDetailes = $"{ SelectedElement.MerchType.Name } {SelectedElement.ShowValue}",
                        TruckDetailes = $"Distance: {dist} Type:{Order.TruckType}\n Volume:{Order.Volume}\n Weight:{Order.Weight}\n L-H-W:{Order.Dimentions.Lenght}-{Order.Dimentions.Height}-{Order.Dimentions.Width}\n"
                    };

                    if (!await Services.FirestoreServiceProvider.InsertContractInFirestore(carrier))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error on save", "A error occured", "Ok");
                        return;
                    }

                    await Services.FirestoreServiceProvider.DeleteCarrierOffAsync(Order);
                    await Services.FirestoreServiceProvider.DeleteOrderAsync(SelectedElement);

                    await Application.Current.MainPage.Navigation.PopAsync();
                }

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
