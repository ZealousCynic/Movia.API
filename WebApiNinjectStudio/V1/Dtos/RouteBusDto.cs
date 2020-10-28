using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace WebApiNinjectStudio.V1.Dtos
{
    public class RouteBusDto
    {
    }

    public class ReturnRouteBusDto
    {
        public int ID { get; set; }
        public int RouteID { get; set; }        
        public ReturnBusDto Bus { get; set; }
        public ReturnBusDriverDto BusDriver { get; set; }
        public int Status { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class ReturnBusAndDriverInRouteDto
    {
        public ReturnBusDto Bus { get; set; }
        public ReturnBusDriverDto BusDriver { get; set; }
        public int Status { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class BusWithDriverDto
    {
        public int BusID { get; set; }
        public int BusDriverID { get; set; }
    }
}
