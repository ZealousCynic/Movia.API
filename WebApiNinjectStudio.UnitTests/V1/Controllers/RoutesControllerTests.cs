using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApiNinjectStudio.V1.Dtos;
using WebApiNinjectStudio.Domain.Concrete;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.V1.Controllers;
using Xunit;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.UnitTests.Extension;

namespace WebApiNinjectStudio.UnitTests.V1.Controllers
{
    [TestCaseOrderer("WebApiNinjectStudio.UnitTests.Extension.PriorityOrderer", "WebApiNinjectStudio.UnitTests")]

    public class RoutesControllerTests
    {
        private readonly IRouteRepository _EFRouteRepository;
        private readonly IBusStopRepository _EFBusStopRepository;
        private readonly IRouteBusStopRepository _EFRouteBusStopRepository;
        private readonly IRouteBusRepository _EFRouteBusRepository;
        private readonly IMapper _MockMapper;

        public RoutesControllerTests()
        {
            var dbOptions = new DbContextOptionsBuilder<EFDbContext>()
                    .UseInMemoryDatabase(databaseName: "WebApiNinjectStudioDbInMemory")
                    .Options;
            var context = new EFDbContext(dbOptions);
            context.Database.EnsureCreated();

            this._EFRouteRepository = new EFRouteRepository(context);
            this._EFBusStopRepository = new EFBusStopRepository(context);
            this._EFRouteBusStopRepository = new EFRouteBusStopRepository(context);
            this._EFRouteBusRepository = new EFRouteBusRepository(context);

            this._MockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()))
                    .CreateMapper();
        }

        /// <summary>
        /// Create route
        /// </summary>
        [Fact, TestPriority(1)]
        public void CreateRoute()
        {
            var target = new RoutesController(this._EFRouteRepository, this._EFRouteBusStopRepository, this._EFBusStopRepository, this._EFRouteBusRepository, this._MockMapper);

            var newRoute = new CreateRouteDto
            {
                Label = "Test-Route",
                Description = "Against Test Route"
            };

            var result = target.Post(newRoute);
            var okResult = result as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(7, this._EFRouteRepository.Routes.Count());
            Assert.Equal("Test-Route", this._EFRouteRepository.Routes
                .Where(o => o.ID == 7).FirstOrDefault().Label);
        }

        /// <summary>
        /// Get bus stops of route
        /// </summary>
        [Fact, TestPriority(2)]
        public void GetBusStopsOfRoute()
        {
            var target = new RoutesController(this._EFRouteRepository, this._EFRouteBusStopRepository, this._EFBusStopRepository, this._EFRouteBusRepository, this._MockMapper);

            var okResult = target.GetAllBusStopsOfRoute(3) as OkObjectResult;
            var busStops = (List<ReturnBusStopWithOrderDto>)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(13, busStops.Count);
            Assert.Equal("Allerød St.", busStops[0].BusStop.Label.ToString());

        }

        /// <summary>
        /// Get all busses of a route by id
        /// </summary>
        [Fact, TestPriority(3)]
        public void GetBusOfRoute()
        {
            var target = new RoutesController(this._EFRouteRepository, this._EFRouteBusStopRepository, this._EFBusStopRepository, this._EFRouteBusRepository, this._MockMapper);

            var okResult = target.GetAllBussesOfRoute(3) as OkObjectResult;
            var busses = (List<ReturnBusAndDriverInRouteDto>)okResult.Value;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("AF22454", busses[0].Bus.RegistrationNumber.ToString());
            Assert.Equal(2, busses.Count);
        }

        /// <summary>
        /// Handle bus and busstop to a route
        /// </summary>
        [Fact, TestPriority(4)]
        public void HandleBusAndBusStopToRoute()
        {
            var target = new RoutesController(this._EFRouteRepository, this._EFRouteBusStopRepository, this._EFBusStopRepository, this._EFRouteBusRepository, this._MockMapper);

            //Add bus to route
            //Remove bus from other route first
            target.DeleteBusFromRoute(3, 5);
            target.PostAddBusWithDriverToRoute(
                7,
                new BusWithDriverDto { BusID = 5, BusDriverID = 8}
            );

            //Add busstop with order to route
            target.PostOrderOfBusStop(7, 1, 2);
            target.PostOrderOfBusStop(7, 2, 1);

            var okBusStopsResult = target.GetAllBusStopsOfRoute(7) as OkObjectResult;
            var busStops = (List<ReturnBusStopWithOrderDto>)okBusStopsResult.Value;
            Assert.Equal(200, okBusStopsResult.StatusCode);
            Assert.Equal(2, busStops.Count);
            Assert.Equal("5733", busStops[1].BusStop.StopNumber.ToString());
            Assert.Equal("3306", busStops[0].BusStop.StopNumber.ToString());

            var okBusResult = target.GetAllBussesOfRoute(7) as OkObjectResult;
            var busses = (List<ReturnBusAndDriverInRouteDto>)okBusResult.Value;
            Assert.Equal(200, okBusResult.StatusCode);
            Assert.Equal("AF22454", busses[0].Bus.RegistrationNumber.ToString());
            Assert.Single(busses);

            //Remove busstop from the route
            var okRemoveBusStopResult = target.DeleteBusStopFromRoute(7, 1) as OkObjectResult;
            var removeBusStop = (bool)okRemoveBusStopResult.Value;
            Assert.Equal(200, okRemoveBusStopResult.StatusCode);
            Assert.True(removeBusStop);
            okBusStopsResult = target.GetAllBusStopsOfRoute(7) as OkObjectResult;
            busStops = (List<ReturnBusStopWithOrderDto>)okBusStopsResult.Value;
            Assert.Single(busStops);

        }

    }
}
