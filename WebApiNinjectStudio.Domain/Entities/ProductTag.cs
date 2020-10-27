using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class ProductTag
    {
        public int ProductTagID { get; set; }
        public string Name { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
