using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class ProductCategory
    {
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
