using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class Route
    {
        public int ID { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }

        public ICollection<RouteBusStop> RouteBusStops { get; set; }
        public ICollection<RouteBus> RouteBusses { get; set; }

    }
}
