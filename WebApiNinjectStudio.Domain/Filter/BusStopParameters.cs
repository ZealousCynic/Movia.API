using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Filter
{
    public class BusStopParameters : QueryStringParameters
    {
        public string StopNumber { get; set; }
        public string Label { get; set; }
    }
}
