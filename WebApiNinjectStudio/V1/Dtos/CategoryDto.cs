using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace WebApiNinjectStudio.V1.Dtos
{
    public class CategoryDto
    {
    }

    public class CreateCategoryDto
    {
        [Required]
        [StringLength(30)]
        public string CategoryName { get; set; }
    }

    public class UpdateCategoryDto
    {
        [Required]
        [StringLength(30)]
        public string CategoryName { get; set; }
    }

    public class ReturnCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class ProductLinkDto
    {
        [Required]
        public int ProductID { get; set; }
    }
}
