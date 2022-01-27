using FreightExchange.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace FreightExchange.ViewModel.Contract
{
    public class ContractPageViewModel : BaseViewModel
    {

        public ObservableCollection<Models.ContractModel> ListOfContracts { get; set; }

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

        private ContractModel _selectedElement;

        public Command<ContractModel> OpenPage { get; set; }

        public ContractModel SelectedElement
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

        public ContractPageViewModel()
        {
            GetData();
            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                string userId = Xamarin.Essentials.Preferences.Get("IdUser", "");
                List<Models.ContractModel> contracts = await Services.FirestoreServiceProvider.GetFirestoreContractsForAClient(userId, "sender_id");
                ListOfContracts = new ObservableCollection<ContractModel>(contracts);
                OnPropertyChanged(nameof(ListOfContracts));
                PageTitle = $"List of Contracts - {ListOfContracts.Count}";
                OnPropertyChanged(nameof(PageTitle));
                IsRefreshing = false;
            });

            OpenPage = new Command<Models.ContractModel>(async contract =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.Contracts.ContractViewPage { BindingContext = contract });
            });
        }

        public async void GetData()
        {
            string userId = Xamarin.Essentials.Preferences.Get("IdUser", "");
            List<Models.ContractModel> contracts = await Services.FirestoreServiceProvider.GetFirestoreContractsForAClient(userId, "sender_id");
            ListOfContracts = new ObservableCollection<ContractModel>(contracts);
            OnPropertyChanged(nameof(ListOfContracts));
            PageTitle = $"List of Contracts - {ListOfContracts.Count}";
            OnPropertyChanged(nameof(PageTitle));
        }
    }
}
