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

namespace FreightExchange.ViewModel.MapViewModel
{
    public class MapViewModel : BaseViewModel
    {
        public ICommand GetYourLocationCommand { get; set; }

        public MapViewModel()
        {
            SetupMap();
            CreateGraphics(-118.8066, 34.0006);

            GetYourLocationCommand = new Command(async () =>
            {
                try
                {
                    var location = await Geolocation.GetLastKnownLocationAsync();
                    if (location != null)
                    {
                        CreateGraphics(location.Latitude, location.Longitude);
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

        private void SetupMap()
        {
            // Create a new map with a 'topographic vector' basemap.
            Map = new Esri.ArcGISRuntime.Mapping.Map(BasemapStyle.ArcGISTopographic);
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
