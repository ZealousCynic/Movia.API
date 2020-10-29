using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class CreateBusDto
    {
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public int CapacityBoundary { get; set; }
        [Required]
        public int SeatingPlace { get; set; }
        [Required]
        public int StandingPlace { get; set; }
        [Required]
        public int BusModelID { get; set; }
    }

    public class UpdateBusDto
    {
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public int CapacityBoundary { get; set; }
        [Required]
        public int SeatingPlace { get; set; }
        [Required]
        public int StandingPlace { get; set; }
        [Required]
        public int BusModelID { get; set; }
    }
}
