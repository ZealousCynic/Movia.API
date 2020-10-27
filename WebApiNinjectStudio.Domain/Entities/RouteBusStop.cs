using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class RouteBusStop
    {
        public int ID { get; set; }
        public int BusStopID { get; set; }
        public BusStop BusStop { get; set; }

        public int RouteID { get; set; }
        public Route Route { get; set; }

        public int Order { get; set; }
    }
}
