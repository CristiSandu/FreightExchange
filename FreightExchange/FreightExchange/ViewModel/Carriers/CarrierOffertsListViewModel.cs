using FreightExchange.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace FreightExchange.ViewModel.Carriers
{
    public class CarrierOffertsListViewModel : BaseViewModel
    {
        public ObservableCollection<Models.CarrierModel> ListOf { get; set; }
        public Command<Models.CarrierModel> Delete { get; private set; }

        private CarrierModel _selectedElement;
        public CarrierModel SelectedElement
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
        public Command AddNewElementCommand { get; set; }
        public Command OpenPage { get; set; }


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

        public CarrierOffertsListViewModel()
        {
            GetData();
            Delete = new Command<Models.CarrierModel>(async carr =>
            {
                if (await Services.FirestoreServiceProvider.DeleteCarrierOffAsync(carr))
                {
                    ListOf.Remove(carr);
                    PageTitle = $"List of Transporter Offers - {ListOf.Count}";
                    OnPropertyChanged(nameof(PageTitle));
                }
            });

            OpenPage = new Command<Models.CarrierModel>(async carr =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.Orders.SelectValuePage { BindingContext = new ViewModel.Carriers.CarrierDetailedPageViewModel(carr) });
            });

            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                string userId = Xamarin.Essentials.Preferences.Get("IdUser", "");
                List<Models.CarrierModel> orders = await Services.FirestoreServiceProvider.GetFirestoreAllCarrierOffertsForUserAsync(userId);
                ListOf = new ObservableCollection<CarrierModel>(orders);
                OnPropertyChanged(nameof(ListOf));
                PageTitle = $"List of Transporter Offers - {ListOf.Count}";
                OnPropertyChanged(nameof(PageTitle));
                IsRefreshing = false;
            });

            AddNewElementCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.Carriers.CarrierFormPage());
            });
        }

        public async void GetData()
        {
            string userId = Xamarin.Essentials.Preferences.Get("IdUser", "");
            List<Models.CarrierModel> orders = await Services.FirestoreServiceProvider.GetFirestoreAllCarrierOffertsForUserAsync(userId);
            ListOf = new ObservableCollection<CarrierModel>(orders);
            OnPropertyChanged(nameof(ListOf));

            PageTitle = $"List of Transporter Offers - {ListOf.Count}";
            OnPropertyChanged(nameof(PageTitle));

        }
    }
}
