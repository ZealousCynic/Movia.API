using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNinjectStudio.V1.Dtos
{
    public class NumberOfPassengerDto
    {
    }

    public class ReturnNumberOfPassengerDto
    {
        public int ID { get; set; }
        public int Total { get; set; }
        public DateTime CreateDT { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class CreateNumberOfPassengerDto
    {
        [Required]        
        public int Total { get; set; }
        [Required]
        public DateTime CreateDT { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }
    }
}
