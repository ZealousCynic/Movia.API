using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;


namespace WebApiNinjectStudio.Domain.Concrete
{
    public class RedisUserDetailRepository : IUserDetailRepository
    {
        private readonly EFDbContext _Context;
        private readonly IRedisCacheClient _RedisCacheClient;


        public RedisUserDetailRepository(EFDbContext context, IRedisCacheClient redisCacheClient)
        {
            this._Context = context;
            this._RedisCacheClient = redisCacheClient;
        }

        public IEnumerable<UserDetail> UserDetails => this._Context.UserDetails;

        public async Task<int> SaveUserDetail(UserDetail userDetail)
        {
            var hashKey = "UserDetail:" + userDetail.UserName.ToString();

            var myDictionary = new Dictionary<string, string>
            {
                { "ID" , userDetail.ID.ToString() },
                {"UserName" , userDetail.UserName },
                {"FirstName" , userDetail.FirstName },
                {"LastName" , userDetail.LastName },
                {"Gender" , userDetail.Gender },
                {"Password" , userDetail.Password },
                {"Status" , userDetail.Status.ToString()}
            };

            var t1 = this._RedisCacheClient.Db0.HashSetAsync(hashKey, myDictionary);
            //Task t1 = this._RedisCacheClient.Db0.AddAsync(hashKey, userDetail);
            Task.WaitAll(t1);

            var a = 898989;
            //var isAdded = await this._RedisCacheClient.Db0.AddAsync("test1", a.ToString());
            return a;
        }
        public async Task<UserDetail> GetUserDetailByUserNamePassword(string userName, string password)
        {
            if (this._RedisCacheClient.Db0.Database.IsConnected("ConnectTest") == false)
            {
                return null;
            }

            var key = "UserDetail:" + userName;            
            var itemUserDetail = await this._RedisCacheClient.Db0.HashGetAllAsync<string>(key);

            if (itemUserDetail.Count > 0)
            {
                var returnItem = new UserDetail();
                foreach (var hashItem in itemUserDetail)
                {
                    switch (hashItem.Key)
                    {
                        case "ID":
                            returnItem.ID = int.Parse(hashItem.Value);
                            break;
                        case "UserName":
                            returnItem.UserName = hashItem.Value;
                            break;
                        case "FirstName":
                            returnItem.FirstName = hashItem.Value;
                            break;
                        case "LastName":
                            returnItem.LastName = hashItem.Value;
                            break;
                        case "Gender":
                            returnItem.Gender = hashItem.Value;
                            break;
                        case "Password":
                            returnItem.Password = hashItem.Value;
                            break;
                        case "Status":
                            returnItem.Status = (byte)int.Parse(hashItem.Value);
                            break;
                        default:
                            break;
                    }
                }
                return returnItem;
            }
            else
            {
                return null;
            }

        }
    }
}
