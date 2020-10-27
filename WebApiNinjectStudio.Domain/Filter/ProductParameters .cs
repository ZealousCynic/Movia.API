using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Filter
{
    public class ProductParameters:QueryStringParameters
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? MinOfPrice { get; set; } = 0;
        public decimal? MaxOfPrice { get; set; }

    }
}
