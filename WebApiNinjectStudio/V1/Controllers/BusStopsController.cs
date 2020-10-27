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
    public class BusStopsController : ControllerBase
    {
        private readonly IRouteBusRepository _RouteBusRepository;
        private readonly IBusStopRepository _BusStopRepository;
        private readonly IMapper _Mapper;

        public BusStopsController(IRouteBusRepository routeBusRepository, IBusStopRepository busStopRepository, IMapper mapper)
        {
            this._RouteBusRepository = routeBusRepository;
            this._BusStopRepository = busStopRepository;
            this._Mapper = mapper;
        }

        // GET: api/v1/BusStops/
        /// <summary>
        /// Get All BusStops;
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ReturnBusStopDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("busstops")]
        public IActionResult GetBusStops([FromQuery] BusStopParameters parameters)
        {

            var busStops = this._BusStopRepository.BusStops;

            #region Search
            if (busStops.Any())
            {
                //StopNumber
                if (!string.IsNullOrWhiteSpace(parameters.StopNumber))
                {
                    busStops = busStops.Where(o => o.StopNumber.ToLower().Contains(parameters.StopNumber.ToLower()));
                }
                //Label
                if (!string.IsNullOrWhiteSpace(parameters.Label))
                {
                    busStops = busStops.Where(o => o.Label.ToLower().Contains(parameters.Label.ToLower()));
                }
            }
            #endregion

            // Pagination
            var busStopsWithPageList = PagedList<BusStop>.ToPagedList(
                busStops.AsQueryable(),
                parameters.PageNumber,
                parameters.PageSize);

            var paginationMetaDataOfHead = new
            {
                busStopsWithPageList.CurrentPage,
                busStopsWithPageList.TotalPages,
                busStopsWithPageList.PageSize,
                busStopsWithPageList.TotalCount,
                busStopsWithPageList.HasPrevious,
                busStopsWithPageList.HasNext
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetaDataOfHead));

            return Ok(this._Mapper.Map<List<BusStop>, List<ReturnBusStopDto>>(busStopsWithPageList.ToList()));

        }

        // POST: /​api​/v1​/BusStops​/
        /// <summary>
        /// Create a BusStop 
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnBusStopDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] CreateBusStopDto createBusStopDto)
        {
            try
            {
                if (this._BusStopRepository.BusStops.Where(o => o.StopNumber == createBusStopDto.StopNumber).Any())
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                        "StopNumber is already exists."}
                    });
                }
                var newBusStop = this._Mapper.Map<CreateBusStopDto, BusStop>(createBusStopDto);
                if (this._BusStopRepository.SaveBusStop(newBusStop) > 0)
                {
                    return Ok(
                        this._Mapper.Map<BusStop, ReturnBusStopDto>(newBusStop)
                        );
                }
                else
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                        "BusStop fails to create."}
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: /v1/busstop/1
        /// <summary>
        /// Remove the BusStop by stop id
        /// </summary>
        /// <param name="busStopId">The ID of a BusStop</param>
        [HttpDelete]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{busStopId}")]
        public IActionResult Delete(int busStopId)
        {
            try
            {
                if (this._BusStopRepository.DelBusStop(busStopId) > 0)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                        "BusStop fails to delete.",
                        "Tip: ID does not exist",
                        "The BusStop is in route"
                        }
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
