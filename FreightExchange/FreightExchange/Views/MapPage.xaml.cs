using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Xamarin.Essentials;
using FreightExchange.ViewModel.MapViewModel;

namespace FreightExchange.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public double Latitude { get; set; }       
        public double Longitude { get; set; }

        public MapPage()
        {
            InitializeComponent();
            BindingContext = new MapViewModel();

            MapPoint mapCenterPoint = new MapPoint(-118.805, 34.027, SpatialReferences.Wgs84);
            MainMapView.SetViewpoint(new Viewpoint(mapCenterPoint, 100000));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MapPoint mapCenterPoint = new MapPoint(Longitude, Latitude, SpatialReferences.Wgs84);
            MainMapView.SetViewpoint(new Viewpoint(mapCenterPoint, 100000));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Views.LoginRegister.LoginPage());
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    OnAppearing();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        private async void SearchAddressButton_Clicked(object sender, EventArgs e)
        {
            // Get the MapViewModel from the page (defined as a static resource).
            MapViewModel viewModel =  BindingContext as MapViewModel;

            // Call SearchAddress on the view model, pass the address text and the map view's spatial reference.
            Esri.ArcGISRuntime.Geometry.MapPoint addressPoint = await viewModel.SearchAddress(AddressTextBox.Text, MainMapView.SpatialReference);

            // If a result was found, center the display on it.
            if (addressPoint != null)
            {
                await MainMapView.SetViewpointCenterAsync(addressPoint);
            }
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Carriers.CarrierFormPage());
        }
    }
}