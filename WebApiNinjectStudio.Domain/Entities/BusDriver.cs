using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class BusDriver
    {
        public int ID { get; set; }
        public string PersonnelNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<RouteBus> RouteBusses { get; set; }
    }
}
