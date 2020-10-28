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
    public class RunningBussesController : ControllerBase
    {
        private readonly IRouteBusRepository _RouteBusRepository;
        private readonly INumberOfPassengerRepository _NumberOfPassengerRepository;
        private readonly IMapper _Mapper;

        public RunningBussesController(IRouteBusRepository routeBusRepository, INumberOfPassengerRepository numberOfPassengerRepository, IMapper mapper)
        {
            this._RouteBusRepository = routeBusRepository;
            this._NumberOfPassengerRepository = numberOfPassengerRepository;
            this._Mapper = mapper;
        }

        // GET: api/v1/RunningBusses/
        /// <summary>
        /// Get All running busses;
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ReturnRouteBusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("runningbusses")]
        public IActionResult GetRunningBusses()
        {
            try
            {
                var routeBusses = this._RouteBusRepository.RouteBusses
                    .Where(o => o.Status == 1).ToList(); 
                return Ok(
                    this._Mapper.Map<List<RouteBus>, List<ReturnRouteBusDto>>(routeBusses)
                    );
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/v1/RunningBusses/HG30202/currentpassenger
        /// <summary>
        /// Get current situation for passenger after RegistrationNumber
        /// </summary>
        /// <param name="registrationNumber">The registration number of a bus</param>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnNumberOfPassengerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{registrationNumber}/currentpassenger")]
        public IActionResult GetCurrentPassenger(string registrationNumber)
        {
            try
            {
                // Get id of RouteBus
                var routeBus = this._RouteBusRepository.RouteBusses
                    .Where(o => o.Status == 1 && o.Bus.RegistrationNumber == registrationNumber)
                    .ToList();
                if (routeBus.Count > 0)
                {
                    var routeBusId = routeBus.First().ID;
                    var numberOfPassengers = this._NumberOfPassengerRepository.NumberOfPassengers.Where(o => o.RouteBusID == routeBusId).OrderByDescending(o => o.CreateDT).ToList();
                    if (numberOfPassengers.Count > 0)
                    {                        
                        return Ok(
                            this._Mapper.Map<NumberOfPassenger, ReturnNumberOfPassengerDto>(numberOfPassengers.First())
                            );
                    }
                }
                else
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                        "Find not bus."}
                    });                    
                }
                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "Find not information of passenger."}
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/v1/RunningBusses/HG30202/currentpassenger
        /// <summary>
        /// Create newest situation for passenger after RegistrationNumber
        /// </summary>
        /// <param name="registrationNumber">The registration number of a bus</param>
        /// <param name="createNumberOfPassengerDto">Object of newest situation for passenger</param>
        [HttpPost]
        [AllowAnonymous]
        [Route("{registrationNumber}/currentpassenger")]
        public IActionResult PostCurrentPassenger(string registrationNumber, [FromBody] CreateNumberOfPassengerDto createNumberOfPassengerDto)
        {
            try
            {
                // Get id of RouteBus
                var routeBusses = this._RouteBusRepository.RouteBusses.Where(o => o.Status == 1 && o.Bus.RegistrationNumber == registrationNumber).ToList();
                if (routeBusses.Count > 0)
                {
                    var newNumberOfPassenger = this._Mapper.Map<CreateNumberOfPassengerDto, NumberOfPassenger>(createNumberOfPassengerDto);                    
                    newNumberOfPassenger.RouteBusID = routeBusses.First().ID;

                    if (this._NumberOfPassengerRepository.SaveNumberOfPassenger(newNumberOfPassenger) > 0)
                    {
                        return Ok(
                            this._Mapper.Map<NumberOfPassenger, ReturnNumberOfPassengerDto>(newNumberOfPassenger)
                            );
                    }
                }
                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "Situation for passenger fails to create.",
                        "Tip: Find not bus, the Bus does not exist",
                        "The bus is not running"
                    }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/v1/RunningBusses/HG30202/historicalpassenger
        /// <summary>
        /// Get historical data for passenger after RegistrationNumber
        /// </summary>
        /// <param name="registrationNumber">The registration number of a bus</param>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ReturnNumberOfPassengerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{registrationNumber}/historicalpassenger")]
        public IActionResult HistoricalPassenger(string registrationNumber)
        {
            try
            {
                // Get id of RouteBus
                var routeBus = this._RouteBusRepository.RouteBusses.Where(o => o.Status == 1 && o.Bus.RegistrationNumber == registrationNumber).ToList();
                if (routeBus.Count > 0)
                {
                    var routeBusId = routeBus.First().ID;
                    var numberOfPassengers = this._NumberOfPassengerRepository.NumberOfPassengers.Where(o => o.RouteBusID == routeBusId).OrderByDescending(o => o.CreateDT).ToList();
                    if (numberOfPassengers.Count > 0)
                    {
                        return Ok(
                            this._Mapper.Map<List<NumberOfPassenger>, List<ReturnNumberOfPassengerDto>>(numberOfPassengers)
                            );
                    }
                }
                else
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                        "Find not bus."}
                    });
                }
                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "Find not information of passenger."}
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
