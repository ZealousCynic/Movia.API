using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class Bus
    {
        public int ID { get; set; }
        public string RegistrationNumber { get; set; }
        public int CapacityBoundary { get; set; }
        public int SeatingPlace { get; set; }
        public int StandingPlace { get; set; }

        public int BusModelID { get; set; }
        public BusModel BusModel { get; set; }

        public ICollection<RouteBus> RouteBusses { get; set; }
    }
}
