using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNinjectStudio.V1.Dtos
{
    public class RouteDto
    {
    }

    public class CreateRouteDto
    {
        [Required]
        public string Label { get; set; }
        [Required]
        public string Description { get; set; }
    }
    public class ReturnRouteDto
    {
        public int ID { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
    }
}
