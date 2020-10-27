using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFProductCategoryRepository : IProductCategoryRepository
    {
        private readonly EFDbContext _Context;

        public EFProductCategoryRepository(EFDbContext context)
        {
            this._Context = context;
        }

        public IEnumerable<ProductCategory> ProductCategories => this._Context.ProductCategories
            .Include(pc => pc.Product)
            .Include(pc => pc.Category);

        public int SaveProductCategory(ProductCategory productCategory)
        {
            var dbEntrys = this._Context.ProductCategories.Where(pc =>
                           pc.CategoryId == productCategory.CategoryId && pc.ProductID == productCategory.ProductID);


            if (!dbEntrys.Any())
            {
                // Is product exists
                var dbProductEntry = this._Context.Products.Find(productCategory.ProductID);
                // Is category exists
                var dbCategoryEntry = this._Context.Categories.Find(productCategory.CategoryId);
                if (dbProductEntry != null && dbCategoryEntry != null)
                {
                    this._Context.ProductCategories.Add(productCategory);
                }
            }
            else
            {
                var dbEntry = dbEntrys.FirstOrDefault();
                if (
                    (dbEntry != null) &&
                    (dbEntry.CategoryId != productCategory.CategoryId) &&
                    (dbEntry.ProductID != productCategory.ProductID)
                    )
                {
                    dbEntry.CategoryId = productCategory.CategoryId;
                    dbEntry.ProductID = productCategory.ProductID;
                }
            }
            return this._Context.SaveChanges();
        }


        public int DelProductCategory(ProductCategory productCategory)
        {
            var dbEntry = this._Context.ProductCategories.FirstOrDefault(pc =>
                   pc.CategoryId == productCategory.CategoryId && pc.ProductID == productCategory.ProductID);
            if (dbEntry != null)
            {
                this._Context.Remove(dbEntry);
            }
            return this._Context.SaveChanges();
        }

    }
}
