using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class RouteBus
    {
        public int ID { get; set; }

        public int RouteID { get; set; }
        public Route Route { get; set; }

        public int BusID { get; set; }
        public Bus Bus { get; set; }

        public int BusDriverID { get; set; }
        public BusDriver BusDriver { get; set; }

        public int Status { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public ICollection<NumberOfPassenger> NumberOfPassengers { get; set; }
    }
}
