using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreightExchange.Models
{
    public class RouteModel
    {
        public Graphic StartGraphic { get; set; }
        public Graphic EndGraphic { get; set; }
        public Graphic RouteGraphic { get; set; }
        public Graphic TruckPosition{ get; set; }

    }


}
