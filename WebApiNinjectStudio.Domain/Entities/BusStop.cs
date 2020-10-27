using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class BusStop
    {
        public int ID { get; set; }
        public string StopNumber { get; set; }
        public string Label { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Zone { get; set; }

        public ICollection<RouteBusStop> RouteBusStops { get; set; }

    }
}
