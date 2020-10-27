using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class BusModel
    {
        public int ID { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string PowerTrain { get; set; }

        public ICollection<Bus> Bus { get; set; }
    }
}
