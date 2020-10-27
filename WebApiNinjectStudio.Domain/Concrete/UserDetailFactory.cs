using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public enum UserDetailRepositoryType
    {
        EF,
        Redis
    }
    public delegate IUserDetailRepository UserDetailFactory(UserDetailRepositoryType userDetailRepositoryType);

    //public class UserDetailFactory
    //{
    //    private readonly IUserDetailRepository _EFUserDetailRepository;
    //    private readonly IUserDetailRepository _RedisUserDetailRepository;

    //    public UserDetailFactory(EFUserDetailRepository efUserDetailRepository, RedisUserDetailRepository redisUserDetailRepository)
    //    {
    //        this._EFUserDetailRepository = efUserDetailRepository;
    //        this._RedisUserDetailRepository = redisUserDetailRepository;
    //    }

    //    public IUserDetailRepository GetUserDetailRepository(string userSelection)
    //    {            
    //        if (userSelection == "ef")
    //        {
    //            return this._EFUserDetailRepository;
    //        }
    //        else
    //        {
    //            return this._RedisUserDetailRepository;
    //        }
    //    }
    //}
}
