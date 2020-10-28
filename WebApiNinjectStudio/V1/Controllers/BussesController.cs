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
    public class BussesController : ControllerBase
    {
        private readonly IBusRepository _BusRepository;                
        private readonly IMapper _Mapper;

        public BussesController(IBusRepository busRepository, IMapper mapper)
        {
            this._BusRepository = busRepository;
            this._Mapper = mapper;
        }

        // GET: api/v1/busses/
        /// <summary>
        /// Get All busses;
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ReturnBusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetBusses([FromQuery] BusParameters parameters)
        {

            var busses = this._BusRepository.Busses;

            #region Search
            if (busses.Any())
            {
                //RegistrationNumber
                if (!string.IsNullOrWhiteSpace(parameters.RegistrationNumber))
                {
                    busses = busses.Where(o => o.RegistrationNumber.ToLower().Contains(parameters.RegistrationNumber.ToLower()));
                }

                //Manufacturer
                if (!string.IsNullOrWhiteSpace(parameters.Manufacturer))
                {
                    busses = busses.Where(o => o.BusModel.Manufacturer.ToLower().Contains(parameters.Manufacturer.ToLower()));
                }

            }
            #endregion

            // Pagination
            var bussesWithPageList = PagedList<Bus>.ToPagedList(
                busses.AsQueryable(),
                parameters.PageNumber,
                parameters.PageSize);

            var paginationMetaDataOfHead = new
            {
                bussesWithPageList.CurrentPage,
                bussesWithPageList.TotalPages,
                bussesWithPageList.PageSize,
                bussesWithPageList.TotalCount,
                bussesWithPageList.HasPrevious,
                bussesWithPageList.HasNext
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetaDataOfHead));

            return Ok(
                this._Mapper.Map<List<Bus>, List<ReturnBusDto>>(bussesWithPageList.ToList())
                );

        }

        // GET: api/v1/busses/1
        /// <summary>
        /// Get a bus by id;
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ReturnBusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{busId}")]
        public IActionResult GetBusById(int busId)
        {
            var busses = this._BusRepository.Busses.Where(o => o.ID == busId).ToList();

            if (busses.Count > 0)
            {
                return Ok(
                    this._Mapper.Map<Bus, ReturnBusDto>(busses.First())
                    );
            }

            return BadRequest(new BadRequestMessage
            {
                Message = new string[] {
                        "Find not bus.",
                        "Tip: ID does not exist"
                        }
            });
        }

        // POST: /​api​/v1​/busses​/
        /// <summary>
        /// Create a bus 
        /// </summary>
        /// <param name="createBusDto">Object bus</param>
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnBusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] CreateBusDto createBusDto)
        {
            try
            {
                var newBus = this._Mapper.Map<CreateBusDto, Bus>(createBusDto);
                if (this._BusRepository.SaveBus(newBus) > 0)
                {
                    return Ok(                        
                        this._Mapper.Map<Bus, ReturnBusDto>(
                            this._BusRepository.Busses.Where(o => o.ID == newBus.ID).First()
                            )
                        );
                }

                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "Bus fails to create.",
                        "Tip: RegistrationNumber is already exists",
                        "ID of busmodel does not exist"
                    }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: /api/v1/busses/1
        /// <summary>
        /// Update a bus by id
        /// </summary>
        /// <param name="busId">The ID of a bus</param>
        /// <param name="updateBusDto">Object bus</param>
        [HttpPut]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnBusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{busId}")]
        public IActionResult Put(int busId, [FromBody] UpdateBusDto updateBusDto)
        {
            try
            {
                var updateBus = this._Mapper.Map<UpdateBusDto, Bus>(updateBusDto);
                updateBus.ID = busId;
                if (this._BusRepository.SaveBus(updateBus) > 0)
                {
                    return Ok(
                        this._Mapper.Map<Bus, ReturnBusDto>(
                          this._BusRepository.Busses.Where(o => o.ID == updateBus.ID).First()
                            )
                        );
                }
                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "Bus fails to update.",
                        "Tip: RegistrationNumber is already exists",
                        "ID of bus does not exist",
                        "ID of busmodel does not exist"
                        }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: /v1/busses/1
        /// <summary>
        /// Remove a bus by id
        /// </summary>
        /// <param name="busId">The ID of a bus</param>
        [HttpDelete]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{busId}")]
        public IActionResult Delete(int busId)
        {
            try
            {
                if (this._BusRepository.DelBus(busId) > 0)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                        "Bus fails to delete.",
                        "Tip: ID does not exist",
                        "The Bus is in route"
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
