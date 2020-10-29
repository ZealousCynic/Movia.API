using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Filter
{
    public class BusParameters : QueryStringParameters
    {
        public string RegistrationNumber { get; set; }
        public string Manufacturer { get; set; }
    }
}
