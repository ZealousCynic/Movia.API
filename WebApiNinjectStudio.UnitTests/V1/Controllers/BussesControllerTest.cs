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
    public class BussesControllerTest
    {
        private readonly IMapper _MockMapper;
        private readonly IBusRepository _EFBusRepository;

        public BussesControllerTest()
        {
            var dbOptions = new DbContextOptionsBuilder<EFDbContext>()
                    .UseInMemoryDatabase(databaseName: "WebApiNinjectStudioDbInMemory")
                    .Options;
            var context = new EFDbContext(dbOptions);
            context.Database.EnsureCreated();

            this._EFBusRepository = new EFBusRepository(context);

            this._MockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()))
                    .CreateMapper();
        }

        /// <summary>
        /// Create bus
        /// </summary>
        [Fact, TestPriority(1)]
        public void CreateBus()
        {
            var target = new BussesController(this._EFBusRepository, this._MockMapper);

            var newBus = new CreateBusDto
            {
                RegistrationNumber = "TEST123",
                CapacityBoundary = 10,
                SeatingPlace = 15,
                StandingPlace = 20,
                BusModelID = 1
            };

            var result = target.Post(newBus);
            var okResult = result as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(13, this._EFBusRepository.Busses.Count());
            Assert.Equal(20, this._EFBusRepository.Busses
                .Where(o => o.RegistrationNumber == "TEST123").FirstOrDefault().StandingPlace);

            //ID of busmodel does not exist
            newBus = new CreateBusDto
            {
                RegistrationNumber = "TEST456",
                CapacityBoundary = 10,
                SeatingPlace = 15,
                StandingPlace = 20,
                BusModelID = 100
            };

            result = target.Post(newBus);
            var badResult = result as BadRequestObjectResult;

            Assert.Equal(400, badResult.StatusCode);

            //RegistrationNumber is already exists"
            newBus = new CreateBusDto
            {
                RegistrationNumber = "TEST123",
                CapacityBoundary = 10,
                SeatingPlace = 15,
                StandingPlace = 20,
                BusModelID = 1
            };

            result = target.Post(newBus);
            badResult = result as BadRequestObjectResult;
            Assert.Equal(400, badResult.StatusCode);

        }

        /// <summary>
        /// Get bus
        /// </summary>
        [Fact, TestPriority(2)]
        public void GetBusses()
        {
            var target = new BussesController(this._EFBusRepository, this._MockMapper);


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

            var okResult = target.GetBusses(new BusParameters() { PageNumber = 2 }) as OkObjectResult;
            var busses = (List<ReturnBusDto>)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(3, busses.Count());
        }

        /// <summary>
        /// Update Bus
        /// </summary>
        [Fact, TestPriority(3)]
        public void UpdateBus()
        {
            var target = new BussesController(this._EFBusRepository, this._MockMapper);

            var newBus = new UpdateBusDto
            {
                RegistrationNumber = "UpdateTEST123",
                CapacityBoundary = 1,
                SeatingPlace = 2,
                StandingPlace = 2,
                BusModelID = 4
            };

            var okResult = target.Put(13, newBus) as OkObjectResult;
            var bus = (ReturnBusDto)okResult.Value;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("UpdateTEST123", bus.RegistrationNumber.ToString());
            Assert.Equal(1, bus.CapacityBoundary);
        }

        /// <summary>
        /// Get bus by Id
        /// </summary>
        [Fact, TestPriority(4)]
        public void GetBusById()
        {
            var target = new BussesController(this._EFBusRepository, this._MockMapper);

            var okResult = target.GetBusById(13) as OkObjectResult;
            var bus = (ReturnBusDto)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("UpdateTEST123", bus.RegistrationNumber.ToString());

            var badRequestResult = target.GetBusById(14) as BadRequestObjectResult;
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        /// <summary>
        /// Delete bus by Id
        /// </summary>
        [Fact, TestPriority(5)]
        private void DelBusDriver()
        {
            var target = new BussesController(this._EFBusRepository, this._MockMapper);
            var result = target.Delete(13);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(true, okResult.Value);

            result = target.Delete(13);
            var badResult = result as BadRequestObjectResult;
            Assert.Equal(400, badResult.StatusCode);
            Assert.Equal(12, this._EFBusRepository.Busses.Count());

            //Can't delete the bus which is in route
            result = target.Delete(1);
            badResult = result as BadRequestObjectResult;
            Assert.Equal(400, badResult.StatusCode);
            Assert.Equal(12, this._EFBusRepository.Busses.Count());
        }
    }
}
