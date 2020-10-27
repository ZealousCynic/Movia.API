using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private readonly EFDbContext _Context;

        public EFProductRepository(EFDbContext context)
        {
            this._Context = context;
        }

        public IEnumerable<Product> Products => this._Context.Products
                  .Include(p => p.ProductImage)
                  .Include(p => p.ProductTag)
                  .Include(p => p.ProductCategories).ThenInclude(c=>c.Category);

        public int SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                this._Context.Products.Add(product);
            }
            else
            {
                var dbEntry = this._Context.Products
                  .Include(p => p.ProductImage)
                  .Include(p => p.ProductTag)
                  .Where(p => p.ProductID == product.ProductID).First();

                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.ProductImage.Url = product.ProductImage.Url;
                    dbEntry.ProductTag = product.ProductTag;
                }
            }
            return this._Context.SaveChanges();
        }

        public int DelProduct(int productId)
        {
            if (productId <= 0)
            {
                return 0;
            }
            else
            {
                var dbEntry = this._Context.Products
                  .Include(p => p.ProductImage)
                  .Include(p => p.ProductTag)
                  .Include(p => p.ProductCategories)
                  .Where(p => p.ProductID == productId);                
                if (dbEntry.Any())
                {
                    this._Context.Remove(dbEntry.First());
                }
            }
            return this._Context.SaveChanges();
        }

    }
}
