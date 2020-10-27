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
    public class RoutesController : ControllerBase
    {
        private readonly IRouteRepository _RouteRepository;
        private readonly IRouteBusStopRepository _RouteBusStopRepository;
        private readonly IMapper _Mapper;

        public RoutesController(IRouteRepository routeRepository, IRouteBusStopRepository routeBusStopRepository, IMapper mapper)
        {
            this._RouteRepository = routeRepository;
            this._RouteBusStopRepository = routeBusStopRepository;
            this._Mapper = mapper;
        }

        // GET: api/v1/Routes/
        /// <summary>
        /// Get All routes;
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ReturnRouteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("routes")]
        public IActionResult GetRoutes()
        {
            try
            {                
                return Ok(
                    this._Mapper.Map<List<Route>, List<ReturnRouteDto>>(this._RouteRepository.Routes.ToList())
                    );
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/v1/Routes/1/BusStops
        /// <summary>
        /// Get all busstops of a route by id of route;
        /// </summary>
        /// <param name="routeId">The id of a route</param>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<BusStopDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("routes/{routeId}/BusStops")]
        public IActionResult GetAllBusStopsOfRoute(int routeId)
        {
            try
            {
                //Is route exist
                if (this._RouteRepository.Routes.Where(o => o.ID == routeId).ToList().Count
                    <= 0 )
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                        "Find not route."}
                    });

                }
                var routeBusStopItems = this._RouteBusStopRepository.RouteBusStops
                    .Where(o => o.RouteID == routeId)
                    .OrderByDescending(o => o.Order)
                    .Select(o => o.BusStop)
                    .ToList();

                return Ok(
                    this._Mapper.Map<List<BusStop>, List<ReturnBusStopDto>>(routeBusStopItems)
                    );
                
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/v1/Routes/1/BusStops/1
        /// <summary>
        /// Add a busstop to a route;
        /// </summary>
        /// <param name="routeId">The id of a route</param>
        /// <param name="busStopId">The id of a stop</param>
        [HttpPost]
        [AllowAnonymous]
        [Route("routes/{routeId}/BusStops/{busStopId}")]
        public IActionResult PostAddBusStopToRoute(int routeId, int busStopId)
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // HttpDelete: api/v1/Routes/1/BusStops/1
        /// <summary>
        /// Remove a busstop from a route;
        /// </summary>
        /// <param name="routeId">The id of a route</param>
        /// <param name="busStopId">The id of a stop</param>
        [HttpDelete]
        [AllowAnonymous]
        [Route("routes/{routeId}/BusStops/{busStopId}")]
        public IActionResult DeleteAddBusStopToRoute(int routeId, int busStopId)
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
