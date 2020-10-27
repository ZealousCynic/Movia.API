using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis.Extensions.Core.Abstractions;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Redis
{
    public class CacheInit
    {
        private readonly IRedisCacheClient _RedisCacheClient;
        public CacheInit(IRedisCacheClient redisCacheClient)
        {
            this._RedisCacheClient = redisCacheClient;
        }

        public void CreateUserDetailRedisCache(List<UserDetail> userDetails)
        {
            var tasks = new Task[userDetails.Count];
            for (var stepTask = 0; stepTask < userDetails.Count; stepTask++)
            {
                var hashKey = "UserDetail:" + userDetails[stepTask].UserName.ToString();
                var hashDictionary = new Dictionary<string, string>
                {
                    { "ID" , userDetails[stepTask].ID.ToString() },
                    { "UserName" , userDetails[stepTask].UserName },
                    { "FirstName" , userDetails[stepTask].FirstName },
                    { "LastName" , userDetails[stepTask].LastName },
                    { "Gender" , userDetails[stepTask].Gender },
                    { "Password" , userDetails[stepTask].Password },
                    { "Status" , userDetails[stepTask].Status.ToString()}
                };

                tasks[stepTask] = this._RedisCacheClient.Db0.HashSetAsync(hashKey, hashDictionary);
            }
            Task.WaitAll(tasks);
        }
    }
}
