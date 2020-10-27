using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNinjectStudio.V1.Dtos
{
    public class BusStopDto
    {
    }

    public class ReturnBusStopDto
    {
        public int ID { get; set; }
        public string StopNumber { get; set; }
        public string Label { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Zone { get; set; }
    }

    public class ReturnBusStopWithOrderDto
    {
        public int Order { get; set; }
        public ReturnBusStopDto BusStop { get; set; }
    }

    public class CreateBusStopDto
    {
        [Required]
        public string StopNumber { get; set; }
        [Required]
        public string Label { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Zone { get; set; }
    }
    public class UpdateBusStopDto
    {
        [Required]
        public string StopNumber { get; set; }
        [Required]
        public string Label { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Zone { get; set; }
    }
}
