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
    public class BusStopsControllerTests
    {
        private readonly IRouteBusRepository _EFRouteBusRepository;
        private readonly IBusStopRepository _EFBusStopRepository;
        private readonly IMapper _MockMapper;

        public BusStopsControllerTests()
        {
            var dbOptions = new DbContextOptionsBuilder<EFDbContext>()
                    .UseInMemoryDatabase(databaseName: "WebApiNinjectStudioDbInMemory")
                    .Options;
            var context = new EFDbContext(dbOptions);
            context.Database.EnsureCreated();

            this._EFRouteBusRepository = new EFRouteBusRepository(context);
            this._EFBusStopRepository = new EFBusStopRepository(context);

            this._MockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()))
                    .CreateMapper();
        }

        /// <summary>
        /// Create bus stop
        /// </summary>
        [Fact, TestPriority(1)]
        public void CreateBusStop()
        {
            var target = new BusStopsController(this._EFRouteBusRepository, this._EFBusStopRepository, this._MockMapper);

            var newBusStop = new CreateBusStopDto
            {
                StopNumber = "Test-001",
                Label = "Test Bus Stop",
                Longitude = 12.0,
                Latitude = 13.2,
                Zone = 1
            };

            var result = target.Post(newBusStop);
            var okResult = result as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(93, this._EFBusStopRepository.BusStops.Count());
            Assert.Equal("Test Bus Stop", this._EFBusStopRepository.BusStops
                .Where(o => o.StopNumber == "Test-001").FirstOrDefault().Label);
        }

        /// <summary>
        /// Get bus stop
        /// </summary>
        [Fact, TestPriority(2)]
        public void GetBusStops()
        {
            var target = new BusStopsController(this._EFRouteBusRepository, this._EFBusStopRepository, this._MockMapper);

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

            var okResult = target.GetBusStops(new BusStopParameters() { StopNumber = "3306" }) as OkObjectResult;
            var busStops = (List<ReturnBusStopDto>)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Single(busStops);
        }

        /// <summary>
        /// Update Bus Stop
        /// </summary>
        [Fact, TestPriority(3)]
        public void UpdateBusStop()
        {
            var target = new BusStopsController(this._EFRouteBusRepository, this._EFBusStopRepository, this._MockMapper);

            var newBusStop = new UpdateBusStopDto
            {
                StopNumber = "UTest-001",
                Label = "Update-Test Bus Stop",
                Longitude = 1.0,
                Latitude = 2.0,
                Zone = 2
            };

            var okResult = target.Put(93, newBusStop) as OkObjectResult;
            var busStop = (ReturnBusStopDto)okResult.Value;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("UTest-001", busStop.StopNumber.ToString());
            Assert.Equal(1.0, busStop.Longitude);
        }

        /// <summary>
        /// Get bus stop by Id
        /// </summary>
        [Fact, TestPriority(4)]
        public void GetBusStopById()
        {
            var target = new BusStopsController(this._EFRouteBusRepository, this._EFBusStopRepository, this._MockMapper);

            var okResult = target.GetBusStopById(93) as OkObjectResult;
            var busDriver = (ReturnBusStopDto)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Update-Test Bus Stop", busDriver.Label.ToString());

            var badRequestResult = target.GetBusStopById(200) as BadRequestObjectResult;
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        /// <summary>
        /// Delete bus stop by Id
        /// </summary>
        [Fact, TestPriority(5)]
        private void DelBusStop()
        {
            var target = new BusStopsController(this._EFRouteBusRepository, this._EFBusStopRepository, this._MockMapper);

            var result = target.Delete(93);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(true, okResult.Value);

            result = target.Delete(93);
            var badResult = result as BadRequestObjectResult;
            Assert.Equal(400, badResult.StatusCode);
            Assert.Equal(92, this._EFBusStopRepository.BusStops.Count());
        }

    }
}
