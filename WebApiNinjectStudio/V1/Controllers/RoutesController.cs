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
        private readonly IBusStopRepository _BusStopRepository;
        private readonly IRouteBusStopRepository _RouteBusStopRepository;
        private readonly IRouteBusRepository _RouteBusRepository;
        private readonly IMapper _Mapper;

        public RoutesController(IRouteRepository routeRepository, IRouteBusStopRepository routeBusStopRepository, IBusStopRepository busStopRepository, IRouteBusRepository routeBusRepository, IMapper mapper)
        {
            this._RouteRepository = routeRepository;
            this._RouteBusStopRepository = routeBusStopRepository;
            this._RouteBusRepository = routeBusRepository;
            this._BusStopRepository = busStopRepository;
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

        // GET: api/v1/Routes/1
        /// <summary>
        /// Get a route by id;
        /// </summary>
        /// <param name="routeId">The id of a route</param>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnRouteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{routeId}")]
        public IActionResult GetRouteById(int routeId)
        {
            try
            {
                var route = this._RouteRepository.Routes.FirstOrDefault(c => c.ID == routeId);
                if (route == null)
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {"Find not route."}
                    });
                }
                return Ok(
                    this._Mapper.Map<Route, ReturnRouteDto>(route)
                    );
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: /v1/Routes/1
        /// <summary>
        /// Update the route by id
        /// </summary>
        /// <param name="routeId">The ID of route</param>
        /// <param name="updateRouteDto">Object route</param>
        [HttpPut]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{routeId}")]
        public IActionResult Put(int routeId, [FromBody] UpdateRouteDto updateRouteDto)
        {
            //try
            {
                var updateRoute = this._Mapper.Map<UpdateRouteDto, Route>(updateRouteDto);
                updateRoute.ID = routeId;
                if (this._RouteRepository.SaveRoute(updateRoute) > 0)
                {
                    return Ok(
                        this._Mapper.Map<Route, ReturnRouteDto>(updateRoute)
                        );
                }
                else
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                            "Route fails to update.",
                            "ID does not exist"
                        }
                    });
                }
            }
            //catch (Exception)
            //{
            //    return StatusCode(500, "Internal server error");
            //}
        }

        // POST: /​api​/v1​/Routes​/
        /// <summary>
        /// Create a route 
        /// </summary>
        /// <param name="createRouteDto">Object route</param>
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnRouteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] CreateRouteDto createRouteDto)
        {
            try
            {
                //Is already exist
                if (this._RouteRepository.Routes
                    .Where(
                        o => o.Label.Trim().ToLower() == createRouteDto.Label.Trim().ToLower() &&
                        o.Description.Trim().ToLower() == createRouteDto.Description.Trim().ToLower())
                    .Any())
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                        "Route is already exists.",
                        "Tip: Label and Description is exactly the same as the existing route"
                        }
                    });
                }

                var newRoute = this._Mapper.Map<CreateRouteDto, Route>(createRouteDto);
                if (this._RouteRepository.SaveRoute(newRoute) > 0)
                {
                    return Ok(
                        this._Mapper.Map<Route, ReturnRouteDto>(newRoute)
                        );
                }
                else
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                        "Route fails to create."}
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Delete: api/v1/Routes/1/
        /// <summary>
        /// Remove a route;
        /// </summary>
        /// <param name="routeId">The id of a route</param>
        [HttpDelete]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{routeId}")]
        public IActionResult DeleteRoute(int routeId)
        {
            try
            {
                if (this._RouteRepository.DelRoute(routeId) > 0)
                {
                    return Ok(true);
                }
                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "The route fails to remove.",
                        "Tip: The route must be existed."
                        }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/v1/Routes/1/BusStops
        /// <summary>
        /// Get all bus stops of a route by id;
        /// </summary>
        /// <param name="routeId">The id of a route</param>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<ReturnBusStopWithOrderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{routeId}/BusStops")]
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
                    .OrderBy(o => o.Order)
                    //.Select(o => o.BusStop)
                    .ToList();

                return Ok(
                    this._Mapper.Map<List<RouteBusStop>, List<ReturnBusStopWithOrderDto>>(routeBusStopItems)
                    );
                
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/v1/Routes/1/BusStops/1
        /// <summary>
        /// Add a bus stop to a route;
        /// </summary>
        /// <param name="routeId">The id of a route</param>
        /// <param name="busStopId">The id of a stop</param>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{routeId}/BusStops/{busStopId}")]
        public IActionResult PostAddBusStopToRoute(int routeId, int busStopId)
        {
            try
            {            

                var newRouteBusStop = new RouteBusStop { BusStopID = busStopId, RouteID = routeId, Order = 0};
                if (this._RouteBusStopRepository.SaveRouteBusStop(newRouteBusStop) > 0)
                {
                    return Ok(true);
                }
                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "The bus stop fails to add to the route.",
                        "Tip: The bus stop is already in the route.",
                        "The bus must already exist.",
                        "The route must already exist."
                        }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/v1/Routes/1/BusStops/1/2
        /// <summary>
        /// Update order to bus stop And add a bus stop to a route;
        /// </summary>
        /// <param name="routeId">The id of a route</param>
        /// <param name="busStopId">The id of a stop</param>
        /// <param name="order">Sequence number for stop</param>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{routeId}/BusStops/{busStopId}/{order}")]
        public IActionResult PostOrderOfBusStop(int routeId, int busStopId, int order)
        {
            try
            {
                var newRouteBusStop = new RouteBusStop { BusStopID = busStopId, RouteID = routeId, Order = order };
                if (this._RouteBusStopRepository.SaveRouteBusStop(newRouteBusStop) > 0)
                {
                    return Ok(true);
                }
                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "Sequence number fails to update.",
                        "Tip: The bus must be already in the route.",
                        "The bus must already exist.",
                        "The route must already exist."
                        }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Delete: api/v1/Routes/1/BusStops/1
        /// <summary>
        /// Remove a bus stop from a route;
        /// </summary>
        /// <param name="routeId">The id of a route</param>
        /// <param name="busStopId">The id of a stop</param>
        [HttpDelete]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{routeId}/BusStops/{busStopId}")]
        public IActionResult DeleteBusStopFromRoute(int routeId, int busStopId)
        {
            try
            {
                if (this._RouteBusStopRepository.DelRouteBusStop(routeId, busStopId) > 0)
                {
                    return Ok(true);
                }
                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "The bus stop fails to remove from the route.",
                        "Tip: The bus stop must be in the route."
                        }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/v1/Routes/1/Bus/
        /// <summary>
        /// Add a bus with driver to a route;
        /// </summary>
        /// <param name="routeId">The id of a route</param>
        /// <param name="busWithDriverDto">ID of bus and driver</param>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{routeId}/Busses")]
        public IActionResult PostAddBusWithDriverToRoute(int routeId, BusWithDriverDto busWithDriverDto)
        {
            try
            {
                var newRouteBus = new RouteBus { BusID = busWithDriverDto.BusID, RouteID = routeId, BusDriverID = busWithDriverDto.BusDriverID, Status = 0 };

                if (this._RouteBusRepository.SaveRouteBus(newRouteBus) > 0)
                {
                    return Ok(true);
                }
                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "The bus fails to add to the route.",
                        "Tip: The bus must already exist.",
                        "The bus driver must already exist.",
                        "The route must already exist.",
                        "The bus and bus driver is already in other rute"
                        }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/v1/Routes/1/Bus
        /// <summary>
        /// Get all busses of a route by id;
        /// </summary>
        /// <param name="routeId">The id of a route</param>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<ReturnBusAndDriverInRouteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{routeId}/Busses")]
        public IActionResult GetAllBussesOfRoute(int routeId)
        {
            try
            {
                //Is route exist
                if (this._RouteRepository.Routes.Where(o => o.ID == routeId).ToList().Count
                    <= 0)
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                        "Find not route."}
                    });

                }
                var routeBusItems = this._RouteBusRepository.RouteBusses
                    .Where(o => o.RouteID == routeId)
                    .ToList();
                
                return Ok(
                    this._Mapper.Map<List<RouteBus>, List<ReturnBusAndDriverInRouteDto>>(routeBusItems)
                    );
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Delete: api/v1/Routes/1/Busses/1
        /// <summary>
        /// Remove a bus from a route;
        /// </summary>
        /// <param name="routeId">The id of a route</param>
        /// <param name="busId">The id of a bus</param>
        [HttpDelete]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{routeId}/Busses/{busId}")]
        public IActionResult DeleteBusFromRoute(int routeId, int busId)
        {
            try
            {
                if (this._RouteBusRepository.DelRouteBus(routeId, busId) > 0)
                {
                    return Ok(true);
                }
                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "The bus fails to remove from the route.",
                        "Tip: The bus must be in the route."
                        }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
