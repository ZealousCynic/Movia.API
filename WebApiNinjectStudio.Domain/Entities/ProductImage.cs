using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public string Url { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
