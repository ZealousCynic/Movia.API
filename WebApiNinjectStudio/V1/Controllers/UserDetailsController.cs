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
    public class UserDetailsController : ControllerBase
    {        
        private readonly IUserDetailRepository _EFUserDetailRepository;
        private readonly IUserDetailRepository _RedisUserDetailRepository;
        private readonly IMapper _Mapper;

        public UserDetailsController(UserDetailFactory userDetailFactory, IMapper mapper)
        {
            this._EFUserDetailRepository = userDetailFactory(UserDetailRepositoryType.EF);
            this._RedisUserDetailRepository = userDetailFactory(UserDetailRepositoryType.Redis);
            this._Mapper = mapper;           
        }

        // GET: /v1/userdetails/GetUserDetailWithRedis
        /// <summary>
        /// Get info about userdetail by username and password with Redis
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [Route("GetUserDetailWithRedis")]
        public async Task<IActionResult> GetUserDetailWithRedis(string userName, string password)
        {
            //try
            {
                var userdetails = await this._RedisUserDetailRepository.GetUserDetailByUserNamePassword(userName, password);
                //string userName, string password
                //var userName = "morgan17-999055";
                //var password = "helloworld";
                return Ok(userdetails);
            }
            //catch (Exception)
            //{
            //    return StatusCode(500, "Internal server error");
            //}
        }

        // GET: /v1/userdetails/GetUserDetailByDB
        /// <summary>
        /// Get info about userdetail by username and password with database
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Produces("application/json")]
        [Route("GetUserDetailWithDB")]
        public async Task<IActionResult> GetUserDetailByDB(string userName, string password)
        {
            //try
            {
                //var userName = "morgan17-999055";
                //var password = "helloworld";

                var userdetails = await Task.Run(() => this._EFUserDetailRepository.GetUserDetailByUserNamePassword(userName, password));
                return Ok(userdetails);
            }
            //catch (Exception)
            //{
            //    return StatusCode(500, "Internal server error");
            //}
        }
    }

}
