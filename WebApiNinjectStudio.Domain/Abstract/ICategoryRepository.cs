using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Entities;


namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
        int SaveCategory(Category category);
        int DelCategory(int categoryId);
    }
}
