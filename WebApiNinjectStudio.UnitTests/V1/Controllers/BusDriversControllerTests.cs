using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiNinjectStudio.V1.Dtos;
using WebApiNinjectStudio.Domain.Concrete;
using WebApiNinjectStudio.Domain.Abstract;
using Moq;
using WebApiNinjectStudio.V1.Controllers;
using Xunit;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Filter;
using WebApiNinjectStudio.UnitTests.Extension;

namespace WebApiNinjectStudio.UnitTests.V1.Controllers
{
    [TestCaseOrderer("WebApiNinjectStudio.UnitTests.Extension.PriorityOrderer", "WebApiNinjectStudio.UnitTests")]
    public class BusDriversControllerTests
    {
        private readonly IMapper _MockMapper;
        private readonly IBusDriverRepository _EFBusDriverRepository;


        public BusDriversControllerTests()
        {
            var dbOptions = new DbContextOptionsBuilder<EFDbContext>()
                    .UseInMemoryDatabase(databaseName: "WebApiNinjectStudioDbInMemory")
                    .Options;
            var context = new EFDbContext(dbOptions);
            context.Database.EnsureCreated();

            this._EFBusDriverRepository = new EFBusDriverRepository(context);

            this._MockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()))
                    .CreateMapper();
        }

        /// <summary>
        /// Create bus driver
        /// </summary>
        [Fact, TestPriority(1)]
        public void CreateBusDriver()
        {
            var target = new BusDriversController(this._EFBusDriverRepository, this._MockMapper);

            var newBusDriver = new CreateBusDriverDto
            {
                PersonnelNumber = "Test-0001",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                PhoneNumber = "12345678"
            };

            var result = target.Post(newBusDriver);
            var okResult = result as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(13, this._EFBusDriverRepository.BusDrivers.Count());
            Assert.Equal("TestFirstName", this._EFBusDriverRepository.BusDrivers
                .Where(o => o.PersonnelNumber == "Test-0001").FirstOrDefault().FirstName);
        }

        /// <summary>
        /// Get bus driver
        /// </summary>
        [Fact, TestPriority(2)]
        public void GetBusDrivers()
        {
            var target = new BusDriversController(this._EFBusDriverRepository, this._MockMapper);

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

            var okResult = target.GetBusses(new BusDriverParameters() { PageNumber = 2 }) as OkObjectResult;
            var busDrivers = (List<ReturnBusDriverDto>)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(3, busDrivers.Count());
        }

        /// <summary>
        /// Update Bus Driver
        /// </summary>
        [Fact, TestPriority(3)]
        public void UpdateBusDriver()
        {
            var target = new BusDriversController(this._EFBusDriverRepository, this._MockMapper);

            var newBusDriver = new UpdateBusDriverDto
            {
                PersonnelNumber = "Updated-Test-0001",
                FirstName = "Updated-TestFirstName",
                LastName = "Updated-TestLastName",
                PhoneNumber = "012345678"
            };

            var okResult = target.Put(13, newBusDriver) as OkObjectResult;
            var busDriver = (ReturnBusDriverDto)okResult.Value;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Updated-Test-0001", busDriver.PersonnelNumber.ToString());
            Assert.Equal("Updated-TestFirstName", busDriver.FirstName.ToString());           
        }

        /// <summary>
        /// Get bus driver by Id
        /// </summary>
        [Fact, TestPriority(4)]
        public void GetBusDriverById()
        {
            var target = new BusDriversController(this._EFBusDriverRepository, this._MockMapper);

            var okResult = target.GetBusDriverById(13) as OkObjectResult;
            var busDriver = (ReturnBusDriverDto)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Updated-TestFirstName", busDriver.FirstName.ToString());

            var badRequestResult = target.GetBusDriverById(14) as BadRequestObjectResult;
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        /// <summary>
        /// Delete bus driver by Id
        /// </summary>
        [Fact, TestPriority(5)]
        private void DelBusDriver()
        {
            var target = new BusDriversController(this._EFBusDriverRepository, this._MockMapper);
            var result = target.Delete(13);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(true, okResult.Value);

            result = target.Delete(13);
            var badResult = result as BadRequestObjectResult;
            Assert.Equal(400, badResult.StatusCode);
            Assert.Equal(12, this._EFBusDriverRepository.BusDrivers.Count());

            //Can't delete the bus driver which is in route
            result = target.Delete(1);
            badResult = result as BadRequestObjectResult;
            Assert.Equal(400, badResult.StatusCode);
            Assert.Equal(12, this._EFBusDriverRepository.BusDrivers.Count());
        }

    }
}
