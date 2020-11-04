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
    public class BusModelsController : ControllerBase
    {
        private readonly IBusModelRepository _BusModelRepository;
        private readonly IMapper _Mapper;

        public BusModelsController(IBusModelRepository busModelRepository, IMapper mapper)
        {
            this._BusModelRepository = busModelRepository;
            this._Mapper = mapper;
        }

        // GET: api/v1/busmodels/
        /// <summary>
        /// Get All bus models;
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ReturnBusModelDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetBusModel()
        { 
            var busModels = this._BusModelRepository.BusModels;

            return Ok(
                this._Mapper.Map<List<BusModel>, List<ReturnBusModelDto>>(busModels.ToList())
                );
        }

        // GET: api/v1/busmodels/1
        /// <summary>
        /// Get a bus model by id;
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnBusModelDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{busModelId}")]
        public IActionResult GetBusModelById(int busModelId)
        {
            var busModels = this._BusModelRepository.BusModels.Where(o => o.ID == busModelId).ToList();

            if (busModels.Count > 0)
            {
                return Ok(
                    this._Mapper.Map<BusModel, ReturnBusModelDto>(busModels.First())
                    );
            }

            return BadRequest(new BadRequestMessage
            {
                Message = new string[] {
                        "Find not bus model.",
                        "Tip: ID does not exist"
                        }
            });
        }

        // POST: /​api​/v1​/busmodels​/
        /// <summary>
        /// Create a bus model
        /// </summary>
        /// <param name="createBusModelDto">Object bus model</param>
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnBusModelDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] CreateBusModelDto createBusModelDto)
        {
            try
            {
                var newBusModels = this._Mapper.Map<CreateBusModelDto, BusModel>(createBusModelDto);
                if (this._BusModelRepository.SaveBusModel(newBusModels) > 0)
                {
                    return Ok(
                        this._Mapper.Map<BusModel, ReturnBusModelDto>(
                            this._BusModelRepository.BusModels
                                .Where(o => o.ID == newBusModels.ID).First()
                            )
                        );
                }

                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "Bus model fails to create."
                    }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: /api/v1/busmodel/1
        /// <summary>
        /// Update a bus model by id
        /// </summary>
        /// <param name="busModelId">The ID of a bus model</param>
        /// <param name="updateBusModelDto">Object bus model</param>
        [HttpPut]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ReturnBusModelDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{busModelId}")]
        public IActionResult Put(int busModelId, [FromBody] UpdateBusModelDto updateBusModelDto)
        {
            try
            {
                var updateBusModel = this._Mapper.Map<UpdateBusModelDto, BusModel>(updateBusModelDto);
                updateBusModel.ID = busModelId;

                if (this._BusModelRepository.SaveBusModel(updateBusModel) > 0)
                {
                    return Ok(
                        this._Mapper.Map<BusModel, ReturnBusModelDto>(updateBusModel)
                        );
                }
                return BadRequest(new BadRequestMessage
                {
                    Message = new string[] {
                        "Bus driver fails to update.",
                        "Tip: ID of bus model does not exist"
                    }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: /v1/busmodels/1
        /// <summary>
        /// Remove a bus model by id
        /// </summary>
        /// <param name="busModelId">The ID of a bus model</param>
        [HttpDelete]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{busModelId}")]
        public IActionResult Delete(int busModelId)
        {
            try
            {
                if (this._BusModelRepository.DelBusModel(busModelId) > 0)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest(new BadRequestMessage
                    {
                        Message = new string[] {
                        "Bus model fails to delete.",
                        "Tip: ID does not exist",
                        "The Bus model is being used"
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
