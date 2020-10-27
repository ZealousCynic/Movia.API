using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface IProductCategoryRepository
    {
        IEnumerable<ProductCategory> ProductCategories { get; }
        int SaveProductCategory(ProductCategory productCategory);
        int DelProductCategory(ProductCategory productCategory);
    }
}
