using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductImage ProductImage { get; set; }
        public ICollection<ProductTag> ProductTag { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
