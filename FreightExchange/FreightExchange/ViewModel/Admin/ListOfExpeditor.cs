using FreightExchange.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreightExchange.ViewModel.Admin
{
    public class ListOfExpeditor : BaseViewModel
    {
        public ObservableCollection<Models.UserModel> ListOf { get; set; }
        public string PageTitle { get; set; }


        public Command<Models.UserModel> Delete { get; private set; }
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
        public bool IsAddVisible { get; set; } = false;


        public ListOfExpeditor()
        {
            GetData();

            Delete = new Command<Models.UserModel>(async user =>
            {
                if (await Services.FirestoreServiceProvider.DeleteUser(user))
                {
                    ListOf.Remove(user);
                    PageTitle = $"Count {ListOf.Count}";
                    OnPropertyChanged(nameof(PageTitle));
                }
            });

            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                List<Models.UserModel> users = await Services.FirestoreServiceProvider.GetFirestoreAllUser("Expeditor");
                ListOf = new ObservableCollection<UserModel>(users);
                OnPropertyChanged(nameof(ListOf));
                PageTitle = $"Count {ListOf.Count}";
                OnPropertyChanged(nameof(PageTitle));
                IsRefreshing = false;
            });
        }


        public async void GetData()
        {
            List<Models.UserModel> users = await Services.FirestoreServiceProvider.GetFirestoreAllUser("Expeditor");
            ListOf = new ObservableCollection<UserModel>(users);
            OnPropertyChanged(nameof(ListOf));
            PageTitle = $"Count {ListOf.Count}";
            OnPropertyChanged(nameof(PageTitle));
        }
    }
}
