using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNinjectStudio.V1.Dtos
{
    public class BusDriverDto
    {
    }

    public class ReturnBusDriverDto
    {
        public int ID { get; set; }
        public string PersonnelNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class CreateBusDriverDto
    {
        [Required]
        public string PersonnelNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UpdateBusDriverDto
    {
        [Required]
        public string PersonnelNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
