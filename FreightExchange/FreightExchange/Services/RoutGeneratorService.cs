using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using Esri.ArcGISRuntime.Tasks.NetworkAnalysis;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightExchange.Services
{
    public static class RoutGeneratorService
    {
        public async static Task<Dictionary<string, Models.RouteModel>> GetAllPointForContracts(MapView mapView)
        {
            Random rnd = new Random();


            Dictionary<string, Models.RouteModel> points = new Dictionary<string, Models.RouteModel>();
            var contract_list = await Services.FirestoreServiceProvider.GetFirestoreContractsLessThen();

            var startOutlineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Red, 2);
            var endOutlineSymbol = new SimpleLineSymbol(style: SimpleLineSymbolStyle.Solid, color: System.Drawing.Color.Red, width: 2);

            foreach (var contract in contract_list)
            {
                points.Add(contract.Id, new Models.RouteModel
                {
                    StartGraphic =
                   new Graphic(null, new SimpleMarkerSymbol
                   {
                       Style = SimpleMarkerSymbolStyle.Diamond,
                       Color = System.Drawing.Color.Red,
                       Size = 8,
                       Outline = startOutlineSymbol
                   }),
                    EndGraphic = new Graphic(null, new SimpleMarkerSymbol
                    {
                        Style = SimpleMarkerSymbolStyle.Square,
                        Color = System.Drawing.Color.Red,
                        Size = 8,
                        Outline = endOutlineSymbol
                    }),
                    RouteGraphic = new Graphic(null, new SimpleLineSymbol(
                       style: SimpleLineSymbolStyle.Solid,
                       color: Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)),
                       width: 4
                   )),
                    TruckPosition = new Graphic(null, new SimpleMarkerSymbol
                    {
                        Style = SimpleMarkerSymbolStyle.Circle,
                        Color = System.Drawing.Color.Red,
                        Size = 15,
                        Outline = endOutlineSymbol
                    })
                });

                MapPoint point1 = await SearchAddress(contract.StartPlace);
                MapPoint point2 = await SearchAddress(contract.EndPlace);

                var diff = GetDistanceBetweenTwoPoints(point1, point2);

                points[contract.Id].StartGraphic.Geometry = point1;
                points[contract.Id].EndGraphic.Geometry = point2;


                Polyline densifiedLine = await FindRoute(points[contract.Id].StartGraphic, points[contract.Id].EndGraphic);
                points[contract.Id].RouteGraphic.Geometry = densifiedLine;

                var points2 = densifiedLine.Parts.SelectMany(part => part.Points).ToList();

                int index = rnd.Next(0, points2.Count);
                points[contract.Id].TruckPosition.Geometry = points2[index];
            }

            return points;
        }


        public async static Task<Dictionary<string, Models.RouteModel>> GetAllPointForOfferts(MapView mapView)
        {
            Random rnd = new Random();

            Dictionary<string, Models.RouteModel> points = new Dictionary<string, Models.RouteModel>();
            var offerts_list = await Services.FirestoreServiceProvider.GetFirestoreAllCarrierOffertsAsync();

            var startOutlineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Green, 2);
            var endOutlineSymbol = new SimpleLineSymbol(style: SimpleLineSymbolStyle.Solid, color: System.Drawing.Color.Green, width: 2);

            foreach (var offert in offerts_list)
            {
                points.Add(offert.Id, new Models.RouteModel
                {
                    StartGraphic =
                   new Graphic(null, new SimpleMarkerSymbol
                   {
                       Style = SimpleMarkerSymbolStyle.Diamond,
                       Color = System.Drawing.Color.Green,
                       Size = 8,
                       Outline = startOutlineSymbol
                   }),
                    EndGraphic = new Graphic(null, new SimpleMarkerSymbol
                    {
                        Style = SimpleMarkerSymbolStyle.Square,
                        Color = System.Drawing.Color.Green,
                        Size = 8,
                        Outline = endOutlineSymbol
                    }),
                    RouteGraphic = new Graphic(null, new SimpleLineSymbol(
                       style: SimpleLineSymbolStyle.Solid,
                       color: Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)),
                       width: 4
                   )),
                    TruckPosition = new Graphic(null, new SimpleMarkerSymbol
                    {
                        Style = SimpleMarkerSymbolStyle.Circle,
                        Color = System.Drawing.Color.Green,
                        Size = 15,
                        Outline = endOutlineSymbol
                    })
                });

                MapPoint point1 = await SearchAddress(offert.StartPlace);
                MapPoint point2 = await SearchAddress(offert.EndPlace);

                var diff = GetDistanceBetweenTwoPoints(point1, point2);

                points[offert.Id].StartGraphic.Geometry = point1;
                points[offert.Id].EndGraphic.Geometry = point2;


                try
                {
                    Polyline densifiedLine = await FindRoute(points[offert.Id].StartGraphic, points[offert.Id].EndGraphic);
                    points[offert.Id].RouteGraphic.Geometry = densifiedLine;

                    var points2 = densifiedLine.Parts.SelectMany(part => part.Points).ToList();
                    points[offert.Id].TruckPosition.Geometry = points2[0];
                }
                catch (Exception ex)
                {

                }

            }

            return points;
        }

        public static async Task<Polyline> FindRoute(Graphic startGraf, Graphic endGrad)
        {
            var stops = new[] { startGraf, endGrad }.Select(graphic => new Stop(graphic.Geometry as MapPoint));
            var routeTask = await RouteTask.CreateAsync(new Uri("https://route-api.arcgis.com/arcgis/rest/services/World/Route/NAServer/Route_World"));
            RouteParameters parameters = await routeTask.CreateDefaultParametersAsync();
            parameters.SetStops(stops);
            parameters.ReturnDirections = true;
            parameters.ReturnRoutes = true;

            var result = await routeTask.SolveRouteAsync(parameters);

            if (result?.Routes?.FirstOrDefault() is Route routeResult)
            {
                return routeResult.RouteGeometry;
            }
            else
            {
                return null;
            }
        }

        public static async Task<MapPoint> SearchAddress(string address)
        {
            MapPoint addressLocation = null;

            try
            {
                // Create a locator task.
                LocatorTask locatorTask = new LocatorTask(new Uri("https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer"));

                // Define geocode parameters: limit the results to one and get all attributes.
                GeocodeParameters geocodeParameters = new GeocodeParameters();
                geocodeParameters.ResultAttributeNames.Add("*");
                geocodeParameters.MaxResults = 1;

                // Geocode the address string and get the first (only) result.
                IReadOnlyList<GeocodeResult> results = await locatorTask.GeocodeAsync(address, geocodeParameters);
                GeocodeResult geocodeResult = results.FirstOrDefault();


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

        public static double GetDistanceBetweenTwoPoints(MapPoint startPlace, MapPoint endPlace)
        {
            // Get the points that define the route polyline.
            PointCollection polylinePoints = new PointCollection(SpatialReferences.Wgs84)
            {
                startPlace,
                endPlace
            };

            // Create the polyline for the two points.
            Polyline routeLine = new Polyline(polylinePoints);

            // Densify the polyline to show the geodesic curve.
            Geometry pathGeometry = GeometryEngine.DensifyGeodetic(routeLine, 1, LinearUnits.Kilometers, GeodeticCurveType.Geodesic);
            // Calculate and show the distance.

            double distance = GeometryEngine.LengthGeodetic(pathGeometry, LinearUnits.Kilometers, GeodeticCurveType.Geodesic);

            return distance;
        }
    }
}
