using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNinjectStudio.V1.Dtos
{
    public class BusDto
    {
    }
    public class ReturnBusDto
    {
        public int ID { get; set; }
        public string RegistrationNumber { get; set; }
        public int CapacityBoundary { get; set; }
        public int SeatingPlace { get; set; }
        public int StandingPlace { get; set; }        
        public ReturnBusModelDto BusModel { get; set; }
    }
}
