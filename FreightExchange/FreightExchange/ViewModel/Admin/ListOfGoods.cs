using FreightExchange.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace FreightExchange.ViewModel.Admin
{
    public class ListOfGoods : BaseViewModel
    {
        public ObservableCollection<Models.GoodsModel> ListOf { get; set; }
        public string PageTitle { get; set; }

        public Command<Models.GoodsModel> Delete { get; private set; }
        public Command RefreshCommand { get; set; }
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


        public ListOfGoods()
        {
            GetData();
            Delete = new Command<Models.GoodsModel>(async good =>
            {
                if (await Services.FirestoreServiceProvider.DeleteGoodAsync(good))
                {
                    ListOf.Remove(good);
                    PageTitle = $"Count {ListOf.Count}";
                    OnPropertyChanged(nameof(PageTitle));
                }
            });

            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                List<Models.GoodsModel> goods = await Services.FirestoreServiceProvider.GetFirestoreAllGoodsAsync();
                ListOf = new ObservableCollection<GoodsModel>(goods);
                OnPropertyChanged(nameof(ListOf));
                PageTitle = $"Count {ListOf.Count}";
                OnPropertyChanged(nameof(PageTitle));
                IsRefreshing = false;
            });

            AddNewElementCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.AdminViews.AddGoodsCategoryPage());
            });
        }

        public async void GetData()
        {
            List<Models.GoodsModel> goods = await Services.FirestoreServiceProvider.GetFirestoreAllGoodsAsync();
            ListOf = new ObservableCollection<GoodsModel>(goods);
            OnPropertyChanged(nameof(ListOf));
            PageTitle = $"Count {ListOf.Count}";
            OnPropertyChanged(nameof(PageTitle));

        }
    }
}
