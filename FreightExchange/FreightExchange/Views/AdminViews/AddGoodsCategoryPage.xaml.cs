﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreightExchange.Views.AdminViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddGoodsCategoryPage : ContentPage
    {
        public AddGoodsCategoryPage()
        {
            InitializeComponent();
            BindingContext = new ViewModel.Admin.AddGoodsCategoryPageViewModel();
        }
    }
}