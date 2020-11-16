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

        public void FlushDb()
        {
            this._RedisCacheClient.Db0.FlushDbAsync();
        }
        public void CreateRunningBussesRedisCache(List<RouteBus> routeBusses)
        {
            var routeBusKeyNameTemplate = @"RouteBus:{0}";
            var tasks = new Task[routeBusses.Count];
            for (var stepTask = 0; stepTask < routeBusses.Count; stepTask++)
            {
                var hashKey = string.Format(routeBusKeyNameTemplate, routeBusses[stepTask].ID.ToString());
                var hashDictionary = new Dictionary<string, string>
                {
                    { "ID" , routeBusses[stepTask].ID.ToString() },
                    { "RouteID" , routeBusses[stepTask].RouteID.ToString() },
                    { "BusID" , routeBusses[stepTask].BusID.ToString() },
                    { "BusDriverID" , routeBusses[stepTask].BusDriverID.ToString() },
                    { "Status" , routeBusses[stepTask].Status.ToString() },
                    { "Longitude" , routeBusses[stepTask].Longitude.ToString() },
                    { "Latitude" , routeBusses[stepTask].Latitude.ToString() }
                };

                tasks[stepTask] = this._RedisCacheClient.Db0.HashSetAsync(hashKey, hashDictionary);
            }
            Task.WaitAll(tasks);
        }

        public void CreateBusRedisCache(List<Bus> busses)
        {
            var busKeyNameTemplate = @"Bus:{0}";
            var tasks = new Task[busses.Count];
            for (var stepTask = 0; stepTask < busses.Count; stepTask++)
            {
                var hashKey = string.Format(busKeyNameTemplate, busses[stepTask].ID.ToString());
                var hashDictionary = new Dictionary<string, string>
                {
                    { "ID" , busses[stepTask].ID.ToString() },
                    { "RegistrationNumber" , busses[stepTask].RegistrationNumber.ToString() },
                    { "CapacityBoundary" , busses[stepTask].CapacityBoundary.ToString() },
                    { "SeatingPlace" , busses[stepTask].StandingPlace.ToString() },
                    { "StandingPlace" , busses[stepTask].StandingPlace.ToString() },
                    { "BusModelID" , busses[stepTask].BusModelID.ToString() }
                };

                tasks[stepTask] = this._RedisCacheClient.Db0.HashSetAsync(hashKey, hashDictionary);
            }
            Task.WaitAll(tasks);
        }

        public void CreateBusModelRedisCache(List<BusModel> busModels)
        {
            var busModelKeyNameTemplate = @"BusModel:{0}";
            var tasks = new Task[busModels.Count];
            for (var stepTask = 0; stepTask < busModels.Count; stepTask++)
            {
                var hashKey = string.Format(busModelKeyNameTemplate, busModels[stepTask].ID.ToString());
                var hashDictionary = new Dictionary<string, string>
                {
                    { "ID" , busModels[stepTask].ID.ToString() },
                    { "Manufacturer" , busModels[stepTask].Manufacturer.ToString() },
                    { "Model" , busModels[stepTask].Model.ToString() },
                    { "Length" , busModels[stepTask].Length.ToString() },
                    { "Width" , busModels[stepTask].Width.ToString() },
                    { "Height" , busModels[stepTask].Height.ToString() },
                    { "PowerTrain" , busModels[stepTask].PowerTrain.ToString() }
                };

                tasks[stepTask] = this._RedisCacheClient.Db0.HashSetAsync(hashKey, hashDictionary);
            }
            Task.WaitAll(tasks);
        }

        public void CreateBusDriverRedisCache(List<BusDriver> busDrivers)
        {
            var busDriverKeyNameTemplate = @"BusDriver:{0}";
            var tasks = new Task[busDrivers.Count];
            for (var stepTask = 0; stepTask < busDrivers.Count; stepTask++)
            {
                var hashKey = string.Format(busDriverKeyNameTemplate, busDrivers[stepTask].ID.ToString());
                var hashDictionary = new Dictionary<string, string>
                {
                    { "ID" , busDrivers[stepTask].ID.ToString() },
                    { "PersonnelNumber" , busDrivers[stepTask].PersonnelNumber.ToString() },
                    { "FirstName" , busDrivers[stepTask].FirstName.ToString() },
                    { "LastName" , busDrivers[stepTask].LastName.ToString() },
                    { "PhoneNumber" , busDrivers[stepTask].PhoneNumber.ToString() }
                };

                tasks[stepTask] = this._RedisCacheClient.Db0.HashSetAsync(hashKey, hashDictionary);
            }
            Task.WaitAll(tasks);
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

        public async Task<string> TestRead()
        {
            var key = "Bus:1";            
            var itemUserDetail = await this._RedisCacheClient.Db0.HashGetAllAsync<string>(key);
            var itemUserDetailaaa = await this._RedisCacheClient.GetDb(0).SearchKeysAsync("Bus:*");
            var xxx = "abc";
            xxx += 'a';
            return xxx;
        }
    }
}
