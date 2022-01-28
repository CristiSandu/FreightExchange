using System;
using System.Collections.Generic;
using System.Text;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Linq;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using Esri.ArcGISRuntime.Tasks.NetworkAnalysis;
using Esri.ArcGISRuntime.Xamarin.Forms;

namespace FreightExchange.ViewModel.MapViewModel
{
    public class MapViewModel : BaseViewModel
    {
        public ICommand GetYourLocationCommand { get; set; }

        enum RouteBuilderStatus
        {
            NotStarted, // No locations have been defined.
            SelectedStart, // Origin point exists.
            SelectedStartAndEnd // Origin and destination exist.
        }
        private RouteBuilderStatus _currentState = RouteBuilderStatus.NotStarted;

        private Graphic _startGraphic;
        private Graphic _endGraphic;
        private Graphic _routeGraphic;

        public MapViewModel(MapView mapView)
        {
            SetupMap(mapView);
            //CreateGraphics(-118.8066, 34.0006);

            GetYourLocationCommand = new Command(async () =>
            {
                try
                {
                    var location = await Geolocation.GetLastKnownLocationAsync();
                    if (location != null)
                    {
                        //CreateGraphics(location.Latitude, location.Longitude);
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
            });
        }

        private Esri.ArcGISRuntime.Mapping.Map _map;
        public Esri.ArcGISRuntime.Mapping.Map Map
        {
            get { return _map; }
            set
            {
                _map = value;
                OnPropertyChanged(nameof(Map));
            }
        }

        private GraphicsOverlayCollection _graphicsOverlays;
        public GraphicsOverlayCollection GraphicsOverlays
        {
            get { return _graphicsOverlays; }
            set
            {
                _graphicsOverlays = value;
                OnPropertyChanged(nameof(GraphicsOverlays));
            }
        }

        private List<string> _directions;
        public List<string> Directions
        {
            get { return _directions; }
            set
            {
                _directions = value;
                OnPropertyChanged(nameof(Directions));
            }
        }

        private async void SetupMap(MapView mapView)
        {
            // Create a new map with a 'topographic vector' basemap.
            Map = new Esri.ArcGISRuntime.Mapping.Map(BasemapStyle.ArcGISStreets);
            // Set the view model "GraphicsOverlays" property.

            GraphicsOverlay routeAndStopsOverlay = new GraphicsOverlay();
            this.GraphicsOverlays = new GraphicsOverlayCollection
            {
                routeAndStopsOverlay
            };

            Dictionary<string, Models.RouteModel> points = await Services.RoutGeneratorService.GetAllPointForContracts(mapView);
            Dictionary<string, Models.RouteModel> points_2 = await Services.RoutGeneratorService.GetAllPointForOfferts(mapView);

            foreach (KeyValuePair<string, Models.RouteModel> entry in points)
            {
                routeAndStopsOverlay.Graphics.AddRange(new[] { entry.Value.StartGraphic, entry.Value.EndGraphic, entry.Value.RouteGraphic, entry.Value.TruckPosition });
            }

            foreach (KeyValuePair<string, Models.RouteModel> entry in points_2)
            {
                routeAndStopsOverlay.Graphics.AddRange(new[] { entry.Value.StartGraphic, entry.Value.EndGraphic, entry.Value.RouteGraphic, entry.Value.TruckPosition });
            }
           
        }

        private void ResetState()
        {
            _startGraphic.Geometry = null;
            _endGraphic.Geometry = null;
            _routeGraphic.Geometry = null;
            Directions = null;
            _currentState = RouteBuilderStatus.NotStarted;
        }

        public async Task FindRoute()
        {
            var stops = new[] { _startGraphic, _endGraphic }.Select(graphic => new Stop(graphic.Geometry as MapPoint));
            var routeTask = await RouteTask.CreateAsync(new Uri("https://route-api.arcgis.com/arcgis/rest/services/World/Route/NAServer/Route_World"));
            RouteParameters parameters = await routeTask.CreateDefaultParametersAsync();
            parameters.SetStops(stops);
            parameters.ReturnDirections = true;
            parameters.ReturnRoutes = true;

            var result = await routeTask.SolveRouteAsync(parameters);

            if (result?.Routes?.FirstOrDefault() is Route routeResult)
            {
                _routeGraphic.Geometry = routeResult.RouteGeometry;
                Directions = routeResult.DirectionManeuvers.Select(maneuver => maneuver.DirectionText).ToList();
                _currentState = RouteBuilderStatus.NotStarted;
            }
            else
            {
                ResetState();
                throw new Exception("Route not found");
            }
        }

        public async Task HandleTap(MapPoint tappedPoint)
        {
            switch (_currentState)
            {
                case RouteBuilderStatus.NotStarted:
                    ResetState();
                    _startGraphic.Geometry = tappedPoint;
                    _currentState = RouteBuilderStatus.SelectedStart;
                    break;
                case RouteBuilderStatus.SelectedStart:
                    _endGraphic.Geometry = tappedPoint;
                    _currentState = RouteBuilderStatus.SelectedStartAndEnd;
                    await FindRoute();
                    break;
                case RouteBuilderStatus.SelectedStartAndEnd:
                    // Ignore map clicks while routing is in progress
                    break;
            }
        }

        public async Task<MapPoint> SearchAddress(string address, SpatialReference spatialReference)
        {
            MapPoint addressLocation = null;

            try
            {
                // Get the first graphics overlay from the GraphicsOverlays and remove any previous result graphics.
                GraphicsOverlay graphicsOverlay = this.GraphicsOverlays.FirstOrDefault();
                graphicsOverlay.Graphics.Clear();

                // Create a locator task.
                LocatorTask locatorTask = new LocatorTask(new Uri("https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer"));

                // Define geocode parameters: limit the results to one and get all attributes.
                GeocodeParameters geocodeParameters = new GeocodeParameters();
                geocodeParameters.ResultAttributeNames.Add("*");
                geocodeParameters.MaxResults = 1;
                geocodeParameters.OutputSpatialReference = spatialReference;

                // Geocode the address string and get the first (only) result.
                IReadOnlyList<GeocodeResult> results = await locatorTask.GeocodeAsync(address, geocodeParameters);
                GeocodeResult geocodeResult = results.FirstOrDefault();
                await App.Current.MainPage.DisplayAlert("Debug", $"{ geocodeResult.InputLocation.X}-{geocodeResult.InputLocation.Y}", "OK");

                if (geocodeResult == null) { throw new Exception("No matches found."); }

                // Create a graphic to display the result location.
                SimpleMarkerSymbol markerSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Square, Color.Blue, 12);
                Graphic markerGraphic = new Graphic(geocodeResult.DisplayLocation, geocodeResult.Attributes, markerSymbol);

                // Create a graphic to display the result address label.
                TextSymbol textSymbol = new TextSymbol(geocodeResult.Label, Color.Red, 18, HorizontalAlignment.Center, VerticalAlignment.Bottom);
                Graphic textGraphic = new Graphic(geocodeResult.DisplayLocation, textSymbol);

                // Add the location and label graphics to the graphics overlay.
                graphicsOverlay.Graphics.Add(markerGraphic);
                graphicsOverlay.Graphics.Add(textGraphic);

                // Set the address location to return from the function.
                addressLocation = geocodeResult.DisplayLocation;

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Couldn't find address: " + ex.Message, "Ok", "Cancel");
            }

            // Return the location of the geocode result.
            return addressLocation;
        }

        private void CreateGraphics(double latitude, double logitude)
        {
            // Create a new graphics overlay to contain a variety of graphics.
            var malibuGraphicsOverlay = new GraphicsOverlay();

            // Add the overlay to a graphics overlay collection.
            GraphicsOverlayCollection overlays = new GraphicsOverlayCollection
            {
                malibuGraphicsOverlay
            };

            // Set the view model's "GraphicsOverlays" property (will be consumed by the map view).
            this.GraphicsOverlays = overlays;

            // Create a point geometry.
            var dumeBeachPoint = new MapPoint(logitude, latitude, SpatialReferences.Wgs84);

            // Create a symbol to define how the point is displayed.
            var pointSymbol = new SimpleMarkerSymbol
            {
                Style = SimpleMarkerSymbolStyle.Circle,
                Color = System.Drawing.Color.Red,
                Size = 10.0
            };

            // Add an outline to the symbol.
            pointSymbol.Outline = new SimpleLineSymbol
            {
                Style = SimpleLineSymbolStyle.Solid,
                Color = System.Drawing.Color.Black,
                Width = 2.0
            };

            // Create a point graphic with the geometry and symbol.
            var pointGraphic = new Graphic(dumeBeachPoint, pointSymbol);

            // Add the point graphic to graphics overlay.
            malibuGraphicsOverlay.Graphics.Add(pointGraphic);
        }

    }
}
