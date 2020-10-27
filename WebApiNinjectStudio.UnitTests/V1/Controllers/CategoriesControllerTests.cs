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
using Microsoft.AspNetCore.Mvc;
using WebApiNinjectStudio.UnitTests.Extension;

namespace WebApiNinjectStudio.UnitTests.V1.Controllers
{
    [TestCaseOrderer("WebApiNinjectStudio.UnitTests.Extension.PriorityOrderer", "WebApiNinjectStudio.UnitTests")]

    public class CategoriesControllerTests
    {
        private readonly ICategoryRepository _EFCategoryRepository;
        private readonly IProductCategoryRepository _EFProductCategoryRepository;
        private readonly IMapper _MockMapper;

        public CategoriesControllerTests()
        {
            var dbOptions = new DbContextOptionsBuilder<EFDbContext>()
                    .UseInMemoryDatabase(databaseName: "WebApiNinjectStudioDbInMemory")
                    .Options;
            var context = new EFDbContext(dbOptions);
            context.Database.EnsureCreated();
            this._EFProductCategoryRepository = new EFProductCategoryRepository(context);
            this._EFCategoryRepository = new EFCategoryRepository(context);            
            this._MockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()))
                    .CreateMapper();
        }

        [Fact, TestPriority(1)]
        public void Post()
        {
            var target = new CategoriesController(this._EFCategoryRepository, this._EFProductCategoryRepository, this._MockMapper);
          
            var newCategory = new CreateCategoryDto
            {
                CategoryName = "TestCategory01"
            };
            var result = target.Post(newCategory);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(9, this._EFCategoryRepository.Categories.Count());
            Assert.Equal("TestCategory01", this._EFCategoryRepository.Categories.Where(c => c.CategoryName == "TestCategory01").FirstOrDefault().CategoryName);

            newCategory = new CreateCategoryDto
            {
                CategoryName = "TestCategory02"
            };
            result = target.Post(newCategory);
            okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(10, this._EFCategoryRepository.Categories.Count());
            Assert.Equal("TestCategory02", this._EFCategoryRepository.Categories.Where(c => c.CategoryName == "TestCategory02").FirstOrDefault().CategoryName);
        }

        [Fact, TestPriority(2)]
        public void Get()
        {
            var target = new CategoriesController(this._EFCategoryRepository, this._EFProductCategoryRepository, this._MockMapper);

            var result = target.Get();

            var okResult = result as OkObjectResult;
            var categories = (List<ReturnCategoryDto>)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(10, categories.Count());
            Assert.Equal("Kontor", categories.Where(c=>c.CategoryId == 3).FirstOrDefault().CategoryName);
        }

        [Fact, TestPriority(3)]
        public void GetCategoryByID()
        {
            var target = new CategoriesController(this._EFCategoryRepository, this._EFProductCategoryRepository, this._MockMapper);

            var result = target.Get(3);

            var okResult = result as OkObjectResult;
            var category = (ReturnCategoryDto)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Kontor", category.CategoryName.ToString());
        }


        [Fact, TestPriority(4)]
        public void Delete()
        {
            var target = new CategoriesController(this._EFCategoryRepository, this._EFProductCategoryRepository, this._MockMapper);

            var result = target.Delete(10);
            var okResult = result as OkObjectResult;
            var returnResult = (bool)okResult.Value;
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(returnResult);
            Assert.Equal(9, this._EFCategoryRepository.Categories.Count());

            result = target.Delete(12);
            var badResult = result as BadRequestObjectResult;
            Assert.Equal(400, badResult.StatusCode);
            Assert.Equal(9, this._EFCategoryRepository.Categories.Count());

            //Cannot to delete category if it has products
            result = target.Delete(3);
            badResult = result as BadRequestObjectResult;
            Assert.Equal(400, badResult.StatusCode);
            Assert.Equal(9, this._EFCategoryRepository.Categories.Count());
        }

        [Fact, TestPriority(5)]
        public void Put()
        {
            var target = new CategoriesController(this._EFCategoryRepository, this._EFProductCategoryRepository, this._MockMapper);

            var modifyCategory = new UpdateCategoryDto
            {
                CategoryName = "TestCategory01_Modified"
            };
            var result = target.Put(9, modifyCategory);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(9, this._EFCategoryRepository.Categories.Count());
            Assert.Equal("TestCategory01_Modified", this._EFCategoryRepository.Categories.Where(c=>c.CategoryId == 9).FirstOrDefault().CategoryName.ToString());
        }

        [Fact, TestPriority(6)]
        public void GetProductsAssignedToCategory()
        {
            var target = new CategoriesController(this._EFCategoryRepository, this._EFProductCategoryRepository, this._MockMapper);

            var result = target.GetProductsAssignedToCategory(3);
            var okResult = result as OkObjectResult;
            var productLinks = (List<ProductLinkDto>)okResult.Value;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Single(productLinks);
            Assert.Equal(1, productLinks.FirstOrDefault().ProductID);

            result = target.GetProductsAssignedToCategory(5);
            okResult = result as OkObjectResult;
            productLinks = (List<ProductLinkDto>)okResult.Value;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(2, productLinks.Count());
            Assert.Equal(1, productLinks.FirstOrDefault().ProductID);
        }

        [Fact, TestPriority(7)]
        public void AssignProductCategory()
        {
            var target = new CategoriesController(this._EFCategoryRepository, this._EFProductCategoryRepository, this._MockMapper);

            var productLinkDto = new ProductLinkDto
            {
                ProductID = 2
            };
            var result = target.AssignProductCategory(8, productLinkDto);
            var okResult = result as OkObjectResult;
            var returnResult = (bool)okResult.Value;
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(returnResult);
            Assert.Equal(2, this._EFProductCategoryRepository.ProductCategories.Where(pc=>pc.ProductID == 2).ToList().Count);


            // Is product not exists
            productLinkDto = new ProductLinkDto
            {
                ProductID = 22
            };
            result = target.AssignProductCategory(8, productLinkDto);
            var badResult = result as BadRequestObjectResult;
            Assert.Equal(400, badResult.StatusCode);


            // Is category not exists
            productLinkDto = new ProductLinkDto
            {
                ProductID = 2
            };
            result = target.AssignProductCategory(18, productLinkDto);
            badResult = result as BadRequestObjectResult;
            Assert.Equal(400, badResult.StatusCode);

        }

        [Fact, TestPriority(8)]
        public void RemoveProductAssignment()
        {
            var target = new CategoriesController(this._EFCategoryRepository, this._EFProductCategoryRepository, this._MockMapper);

            var productLinkDto = new ProductLinkDto
            {
                ProductID = 2
            };
            var result = target.RemoveProductAssignment(8, productLinkDto);            
            var okResult = result as OkObjectResult;
            var returnResult = (bool)okResult.Value;
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(returnResult);
            Assert.Single(this._EFProductCategoryRepository.ProductCategories.Where(pc => pc.ProductID == 2).ToList());

            // Is product and category not exists
            productLinkDto = new ProductLinkDto
            {
                ProductID = 22
            };
            result = target.AssignProductCategory(8, productLinkDto);
            var badResult = result as BadRequestObjectResult;
            Assert.Equal(400, badResult.StatusCode);

        }

    }
}
