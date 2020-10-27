using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiNinjectStudio.Services;
using WebApiNinjectStudio.V1.Dtos;
using WebApiNinjectStudio.Domain.Concrete;
using Microsoft.AspNetCore.Authorization;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;
using WebApiNinjectStudio.Domain.Helpers;
using WebApiNinjectStudio.Domain.Filter;
using Newtonsoft.Json;
using AutoMapper;

namespace WebApiNinjectStudio.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _ProductRepository;
        private readonly IMapper _Mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            this._ProductRepository = productRepository;
            this._Mapper = mapper;
        }

        // POST: /​api​/v1​/products​/
        /// <summary>
        /// Create product 
        /// </summary>
        [HttpPost]
        [Authorize("Permission")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IDictionary<string, Array>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] CreateProductDto createProductDto)
        {
            try
            {
                var newProduct = this._Mapper.Map<CreateProductDto, Product>(createProductDto);
                if (this._ProductRepository.SaveProduct(newProduct) > 0)
                {
                    return Ok(
                        this._Mapper.Map<Product, ReturnProductDto>(newProduct)
                        );
                }
                else
                {
                    return BadRequest(new { Message = "Product fails to create." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: /v1/products
        /// <summary>
        /// Get product list
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ReturnProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IDictionary<string, Array>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get([FromQuery] ProductParameters parameters)
        {
            try
            {
                var products = this._ProductRepository.Products;

                #region Search
                if (products.Any())
                {
                    //Name
                    if (!string.IsNullOrWhiteSpace(parameters.Name))
                    {
                        products = products.Where(p => p.Name.ToLower().Contains(parameters.Name.ToLower()));
                    }
                    //Description
                    if (!string.IsNullOrWhiteSpace(parameters.Description))
                    {
                        products = products.Where(p => p.Description.ToLower().Contains(parameters.Description.ToLower()));
                    }
                    //Price rang
                    if (parameters.MinOfPrice.HasValue && parameters.MaxOfPrice.HasValue)
                    {
                        if (parameters.MinOfPrice <= parameters.MaxOfPrice)
                        {
                            products = products.Where(p => p.Price >= parameters.MinOfPrice && p.Price <= parameters.MaxOfPrice);
                        }
                        else
                        {
                            return BadRequest(new { Message = "Max Price must be more than min price." });
                        }
                    }
                }
                #endregion

                // Pagination
                var productsWithPageList = PagedList<Product>.ToPagedList(
                    products.AsQueryable(),
                    parameters.PageNumber,
                    parameters.PageSize);

                var paginationMetaDataOfHead = new
                {
                    productsWithPageList.CurrentPage,
                    productsWithPageList.TotalPages,
                    productsWithPageList.PageSize,
                    productsWithPageList.TotalCount,
                    productsWithPageList.HasPrevious,
                    productsWithPageList.HasNext
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetaDataOfHead));

                return Ok(this._Mapper.Map<List<Product>, List<ReturnProductDto>>(productsWithPageList.ToList()));
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: /v1/products/{productId}
        /// <summary>
        /// Get info about product by product id
        /// </summary>
        /// <param name="productId">The ID of a product</param>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IDictionary<string, Array>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{productId}")]
        public IActionResult Get(int productId)
        {
            try
            {
                var product = this._ProductRepository.Products.FirstOrDefault(c => c.ProductID == productId);
                if (product == null)
                {
                    return BadRequest(new { Message = "Find not product." });
                }
                return Ok(this._Mapper.Map<Product, ReturnProductDto>(product));
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: /v1/products/1
        /// <summary>
        /// Remove the product by product id
        /// </summary>
        /// <param name="productId">The ID of a product</param>
        [HttpDelete]
        [Authorize("Permission")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IDictionary<string, Array>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{productId}")]
        public IActionResult Delete(int productId)
        {
            try
            {
                if (this._ProductRepository.DelProduct(productId) > 0)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest(new { Message = "Product fails to delete. ID does not exist" });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        // PUT: /v1/products/1
        /// <summary>
        /// Update the product by product id
        /// </summary>
        /// <param name="productId">The ID of a category</param>
        /// <param name="updateProductDto">Object category</param>
        [HttpPut]
        [Authorize("Permission")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IDictionary<string, Array>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{productId}")]
        public IActionResult Put(int productId, [FromBody] UpdateProductDto updateProductDto)
        {
            try
            {
                var updateProduct = this._Mapper.Map<UpdateProductDto, Product>(updateProductDto);
                updateProduct.ProductID = productId;
                if (this._ProductRepository.SaveProduct(updateProduct) > 0)
                {
                    return Ok(
                        this._Mapper.Map<Product, ReturnProductDto>(updateProduct)
                        );
                }
                else
                {
                    return BadRequest(new { Message = "Product fails to update. ID does not exist" });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
