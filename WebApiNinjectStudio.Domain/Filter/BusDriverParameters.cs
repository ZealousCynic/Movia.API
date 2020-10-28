using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Filter
{
    public class BusDriverParameters : QueryStringParameters
    {
        public string PersonnelNumber { get; set; }
        public string FirstName { get; set; }
    }
}
