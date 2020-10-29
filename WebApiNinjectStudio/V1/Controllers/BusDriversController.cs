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
    public class BusDriversController : ControllerBase
    {
        private readonly IBusDriverRepository _BusDriverRepository;
        private readonly IMapper _Mapper;

        public BusDriversController(IBusDriverRepository busDriverRepository, IMapper mapper)
        {
            this._BusDriverRepository = busDriverRepository;
            this._Mapper = mapper;
        }

        // GET: api/v1/busdrivers/
        /// <summary>
        /// Get All bus drivers;
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ReturnBusDriverDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetBusses([FromQuery] BusDriverParameters parameters)
        {

            var busDrivers = this._BusDriverRepository.BusDrivers;

            #region Search
            if (busDrivers.Any())
            {
                //PersonnelNumber
                if (!string.IsNullOrWhiteSpace(parameters.PersonnelNumber))
                {
                    busDrivers = busDrivers.Where(o => o.PersonnelNumber.ToLower().Contains(parameters.PersonnelNumber.ToLower()));
                }
                //FirstName
                if (!string.IsNullOrWhiteSpace(parameters.FirstName))
                {
                    busDrivers = busDrivers.Where(o => o.FirstName.ToLower().Contains(parameters.FirstName.ToLower()));
                }

            }
            #endregion

            // Pagination
            var busDriversWithPageList = PagedList<BusDriver>.ToPagedList(
                busDrivers.AsQueryable(),
                parameters.PageNumber,
                parameters.PageSize);

            var paginationMetaDataOfHead = new
            {
                busDriversWithPageList.CurrentPage,
                busDriversWithPageList.TotalPages,
                busDriversWithPageList.PageSize,
                busDriversWithPageList.TotalCount,
                busDriversWithPageList.HasPrevious,
                busDriversWithPageList.HasNext
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetaDataOfHead));

            return Ok(
                this._Mapper.Map<List<BusDriver>, List<ReturnBusDriverDto>>(busDriversWithPageList.ToList())
                );
        }

        // GET: api/v1/busdrivers/1
        /// <summary>
        /// Get a bus driver by id;
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ReturnBusDriverDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{busDriverId}")]
        public IActionResult GetBusDriverById(int busDriverId)
        {
            var busDriver = this._BusDriverRepository.BusDrivers
                .Where(o => o.ID == busDriverId).ToList();

            if (busDriver.Count > 0)
            {
                return Ok(
                    this._Mapper.Map<BusDriver, ReturnBusDriverDto>(busDriver.First())
                    );
            }

            return BadRequest(new BadRequestMessage
            {
                Message = new string[] {
                        "Find not bus driver.",
                        "Tip: ID does not exist"
                        }
            });
        }

        // POST: /​api​/v1​/busdrivers​/
        /// <summary>
        /// Create a bus driver
        /// </summary>
        /// <param name="createBusDriverDto">Object busdrivers</param>
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnBusDriverDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] CreateBusDriverDto createBusDriverDto)
        {
            try
            {
                var newBusDriver = this._Mapper.Map<CreateBusDriverDto, BusDriver>(createBusDriverDto);
                if (this._BusDriverRepository.SaveBusDriver(newBusDriver) > 0)
                {
                    return Ok(
                        this._Mapper.Map<BusDriver, ReturnBusDriverDto>(newBusDriver)
                        );
                }

                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "Bus driver fails to create.",
                        "Tip: PersonnelNumber is already exists"
                    }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: /api/v1/busdrivers/1
        /// <summary>
        /// Update a bus driver by id
        /// </summary>
        /// <param name="busDriverId">The ID of a bus driver</param>
        /// <param name="updateBusDriverDto">Object bus driver</param>
        [HttpPut]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnBusDriverDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{busDriverId}")]
        public IActionResult Put(int busDriverId, [FromBody] UpdateBusDriverDto updateBusDriverDto)
        {
            //try
            {
                var updateBusDriver = this._Mapper.Map<UpdateBusDriverDto, BusDriver>(updateBusDriverDto);
                updateBusDriver.ID = busDriverId;

                if (this._BusDriverRepository.SaveBusDriver(updateBusDriver) > 0)
                {
                    return Ok(
                        this._Mapper.Map<BusDriver, ReturnBusDriverDto>(updateBusDriver)
                        );
                }
                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "Bus driver fails to update.",
                        "Tip: PersonnelNumber is already exists",
                        "ID of bus dirver does not exist"
                        }
                });
            }
            //catch (Exception)
            //{
            //    return StatusCode(500, "Internal server error");
            //}
        }

        // DELETE: /v1/busdrivers/1
        /// <summary>
        /// Remove a bus driver by id
        /// </summary>
        /// <param name="busDriverId">The ID of a bus driver</param>
        [HttpDelete]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{busDriverId}")]
        public IActionResult Delete(int busDriverId)
        {
            try
            {
                if (this._BusDriverRepository.DelBusDriver(busDriverId) > 0)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                        "Bus driver fails to delete.",
                        "Tip: ID does not exist",
                        "The Bus driver is in route"
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
