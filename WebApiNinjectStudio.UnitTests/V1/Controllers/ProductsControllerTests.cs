using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using WebApiNinjectStudio.V1.Controllers;
using WebApiNinjectStudio.V1.Dtos;
using WebApiNinjectStudio.Domain.Abstract;
using Xunit;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Concrete;
using WebApiNinjectStudio.Domain.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiNinjectStudio.UnitTests.Extension;

namespace WebApiNinjectStudio.UnitTests.V1.Controllers
{
    [TestCaseOrderer("WebApiNinjectStudio.UnitTests.Extension.PriorityOrderer", "WebApiNinjectStudio.UnitTests")]
    public class ProductsControllerTests
    {

        private readonly IMapper _MockMapper;
        private readonly IProductRepository _EFProductRepository;

        public ProductsControllerTests()
        {
            var dbOptions = new DbContextOptionsBuilder<EFDbContext>()
                    .UseInMemoryDatabase(databaseName: "WebApiNinjectStudioDbInMemory")
                    .Options;
            var context = new EFDbContext(dbOptions);
            context.Database.EnsureCreated();
            this._EFProductRepository = new EFProductRepository(context);

            this._MockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()))
                    .CreateMapper();
        }

        /// <summary>
        /// Create Product
        /// </summary>
        [Fact, TestPriority(1)]
        public void Post()
        {
            var target = new ProductsController(this._EFProductRepository, this._MockMapper);
            var newProduct = new CreateProductDto
            {
                Name = "TestProduct01",
                Price = 23M,
                Description = "",
                ProductTag = new List<CreateProductTagDto>
                {
                    new CreateProductTagDto { Name = "TestProduct01-Tag01" },
                    new CreateProductTagDto { Name = "TestProduct01-Tag02" }
                },
                ProductImage = new CreateProductImageDto { Url = "jysk.dk/TestProduct01/" }
            };
            var result = target.Post(newProduct);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(3, this._EFProductRepository.Products.Count());
            Assert.Equal(23M, this._EFProductRepository.Products.Where(p => p.Name == "TestProduct01").FirstOrDefault().Price);
        }

        [Fact, TestPriority(2)]
        public void Get()
        {
            var target = new ProductsController(this._EFProductRepository, this._MockMapper);

            // The HeaderDictionary is needed for adding HTTP headers to the response.                
            var headerDictionary = new HeaderDictionary();
            var response = new Mock<HttpResponse>();
            response.SetupGet(r => r.Headers).Returns(headerDictionary);
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(a => a.Response).Returns(response.Object);

            target.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object
            };

            var okResult = target.Get(new ProductParameters() { }) as OkObjectResult;
            var products = (List<ReturnProductDto>)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(3, products.Count());
        }

        /// <summary>
        /// Valid paginering
        /// </summary>
        [Fact, TestPriority(3)]
        public void GetWithPaginering()
        {
            var target = new ProductsController(this._EFProductRepository, this._MockMapper);

            // The HeaderDictionary is needed for adding HTTP headers to the response.                
            var headerDictionary = new HeaderDictionary();
            var response = new Mock<HttpResponse>();
            response.SetupGet(r => r.Headers).Returns(headerDictionary);
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(a => a.Response).Returns(response.Object);

            target.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object
            };

            var productParameters = new ProductParameters { PageNumber = 2, PageSize = 2 };
            var okResult = target.Get(productParameters) as OkObjectResult;
            var products = (List<ReturnProductDto>)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("TestProduct01", products.FirstOrDefault().Name.ToString());
        }

        [Fact, TestPriority(4)]
        public void GetProductByID()
        {
            var target = new ProductsController(this._EFProductRepository, this._MockMapper);

            var okResult = target.Get(3) as OkObjectResult;
            var product = (ReturnProductDto)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("TestProduct01", product.Name.ToString());

            var badRequestResult = target.Get(4) as BadRequestObjectResult;
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact, TestPriority(5)]
        public void Put()
        {
            var target = new ProductsController(this._EFProductRepository, this._MockMapper);
            var newProduct = new UpdateProductDto
            {
                Name = "TestProduct01-Edit",
                Price = 20M,
                Description = "Add description edit",
                ProductTag = new List<UpdateProductTagDto>
                {
                    new UpdateProductTagDto { Name = "TestProduct01-Tag01-Edit" },
                    new UpdateProductTagDto { Name = "TestProduct01-Tag02-Edit" }
                },
                ProductImage = new UpdateProductImageDto { Url = "jysk.dk/TestProduct01-Edit/" }
            };

            var okResult = target.Put(3, newProduct) as OkObjectResult;
            var product = (ReturnProductDto)okResult.Value;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("TestProduct01-Edit", product.Name.ToString());
            Assert.Equal(20M, product.Price);
            Assert.Equal("Add description edit", product.Description.ToString());
            Assert.Equal("TestProduct01-Tag01-Edit", product.ProductTag[0].Name.ToString());
            Assert.Equal(2, product.ProductTag.Count());
        }

        [Fact, TestPriority(6)]
        private void Delete()
        {
            var target = new ProductsController(this._EFProductRepository, this._MockMapper);
            var result = target.Delete(3);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(true, okResult.Value);

            result = target.Delete(3);
            var badResult = result as BadRequestObjectResult;
            Assert.Equal(400, badResult.StatusCode);
            Assert.Equal(2, this._EFProductRepository.Products.Count());
        }

    }
}
