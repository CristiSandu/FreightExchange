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
using Esri.ArcGISRuntime.Xamarin.Forms;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping.Popups;

namespace FreightExchange.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public double Latitude { get; set; } = 26.10;
        public double Longitude { get; set; } = 44.42;

        public string Role { get; set; }

        public MapPage(string role)
        {
            InitializeComponent();

            Role = role;
            MapPoint mapCenterPoint = new MapPoint(Latitude, Longitude, SpatialReferences.Wgs84);
            MainMapView.SetViewpoint(new Viewpoint(mapCenterPoint, 144_447.638572));
            BindingContext = new MapViewModel(MainMapView);
        }

        public async void MainMapView_GeoViewTapped(object sender, GeoViewInputEventArgs e)
        {
            try
            {
                // Get the feature layer from the map.
                FeatureLayer incidentLayer = MainMapView.Map.OperationalLayers.First() as FeatureLayer;

                // Identify the tapped on feature.
                IdentifyLayerResult result = await MainMapView.IdentifyLayerAsync(incidentLayer, e.Position, 12, true);

                if (result?.Popups?.FirstOrDefault() is Popup popup)
                {
                    // Remove the instructions label.
                    MyPopupViewer.IsVisible = true;

                    // Create a new popup manager for the popup.
                    MyPopupViewer.PopupManager = new PopupManager(popup);

                    QueryParameters queryParams = new QueryParameters
                    {
                        // Set the geometry to selection envelope for selection by geometry.
                        Geometry = new Envelope((MapPoint)popup.GeoElement.Geometry, 6, 6)
                    };

                    // Select the features based on query parameters defined above.
                    await incidentLayer.SelectFeaturesAsync(queryParams, Esri.ArcGISRuntime.Mapping.SelectionMode.New);
                }
            }
            catch (Exception ex)
            {
            }
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
            MapViewModel viewModel = BindingContext as MapViewModel;

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
            if (Role == "Transportator")
                await Navigation.PushAsync(new Views.Carriers.CarrierFormPage());
            else
                await Navigation.PushAsync(new Views.Orders.OrdersFormPage());
        }

        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            if (Role == "Transportator")
                await Navigation.PushAsync(new Views.Orders.OrdersOffertsList { BindingContext = new ViewModel.Carriers.CarrierOffertsListViewModel() });
            else
                await Navigation.PushAsync(new Views.Orders.OrdersOffertsList { BindingContext = new ViewModel.Carriers.OrdersOffertsListViewModel() });
        }

        private async void Button_Clicked_4(object sender, EventArgs e)
        {
            if (Role == "Transportator")
                await Navigation.PushAsync(new Contracts.ContractsListPage { BindingContext = new ViewModel.Contract.ContractTransporterPageViewModel() });
            else
                await Navigation.PushAsync(new Contracts.ContractsListPage { BindingContext = new ViewModel.Contract.ContractPageViewModel() });
        }
    }
}