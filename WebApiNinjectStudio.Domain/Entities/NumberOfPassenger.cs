using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class NumberOfPassenger
    {
        public int ID { get; set; }        
        public int Total { get; set; }
        public DateTime CreateDT { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public int RouteBusID { get; set; }
        public RouteBus RouteBus { get; set; }        

    }
}
