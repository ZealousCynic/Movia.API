using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace WebApiNinjectStudio.V1.Dtos
{
    public class ProductDto
    {
    }

    #region Create Product Dto
    public class CreateProductDto
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public CreateProductImageDto ProductImage { get; set; }
        public List<CreateProductTagDto> ProductTag { get; set; }
    }

    public class CreateProductImageDto
    {
        [Required]
        public string Url { get; set; }
    }

    public class CreateProductTagDto
    {
        [Required]
        public string Name { get; set; }
    }
    #endregion

    #region Update Product Dto
    public class UpdateProductDto
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public UpdateProductImageDto ProductImage { get; set; }
        public List<UpdateProductTagDto> ProductTag { get; set; }
    }

    public class UpdateProductImageDto
    {        
        public string Url { get; set; }
    }

    public class UpdateProductTagDto
    {
        public string Name { get; set; }
    }
    #endregion


    #region Return Product Dto

    public class ReturnProductDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ReturnProductImageDto ProductImage { get; set; }
        public List<ReturnProductTagDto> ProductTag { get; set; }
        public List<ReturnProductCategoryDto> ProductCategories { get; set; }
    }

    public class ReturnProductImageDto
    {
        public int ProductImageId { get; set; }
        public string Url { get; set; }
    }

    public class ReturnProductTagDto
    {
        public int ProductTagID { get; set; }
        public string Name { get; set; }
    }

    public class ReturnProductCategoryDto
    {
        public ReturnCategoryDto Category { get; set; }
    }
    #endregion
}
