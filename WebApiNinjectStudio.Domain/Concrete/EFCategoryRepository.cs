using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly EFDbContext _Context;

        public EFCategoryRepository(EFDbContext context)
        {
            this._Context = context;
        }

        public IEnumerable<Category> Categories => this._Context.Categories;

        public int SaveCategory(Category category)
        {
            if (category.CategoryId == 0)
            {
                this._Context.Categories.Add(category);
            }
            else
            {
                var dbEntry = this._Context.Categories.Find(category.CategoryId);
                if (dbEntry != null)
                {
                    dbEntry.CategoryName = category.CategoryName;
                }
            }
            return this._Context.SaveChanges();
        }

        public int DelCategory(int categoryId)
        {
            //Cannot to delete category if it has products
            if (
                (categoryId <= 0) ||
                this._Context.ProductCategories.Where(pc => pc.CategoryId == categoryId).Any()
                )
            {
                 return 0;
            }
            else
            {
                var dbEntry = this._Context.Categories.Find(categoryId);

                if (dbEntry != null)
                {
                    this._Context.Remove(dbEntry);
                }
            }
            return this._Context.SaveChanges();
        }

    }
}
