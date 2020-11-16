using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis.Extensions.Core.Abstractions;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class RedisRouteBusRepository : IRouteBusRepository
    {
        private readonly EFDbContext _Context;
        private readonly IRedisCacheClient _RedisCacheClient;

        public RedisRouteBusRepository(EFDbContext context, IRedisCacheClient redisCacheClient)
        {
            this._Context = context;
            this._RedisCacheClient = redisCacheClient;
        }

        public IEnumerable<RouteBus> RouteBusses
        {
            get
            {
                var routeBusKeyNameTemplate = @"RouteBus:*";
                var routeBusses = new List<RouteBus>();
                var cacheRouteBusses = this._RedisCacheClient.GetDb(0).SearchKeysAsync(routeBusKeyNameTemplate).Result;
                foreach (var cacheItemKeyName in cacheRouteBusses)
                {
                    var itemRouteBusCache = this._RedisCacheClient.Db0.HashGetAllAsync<string>(cacheItemKeyName).Result;
                    if (itemRouteBusCache.Count > 0)
                    {
                        var returnItem = new RouteBus();
                        //Create route bus 
                        foreach (var hashItem in itemRouteBusCache)
                        {
                            switch (hashItem.Key)
                            {
                                case "ID":
                                    returnItem.ID = int.Parse(hashItem.Value);
                                    break;
                                case "RouteID":
                                    returnItem.RouteID = int.Parse(hashItem.Value);
                                    break;
                                case "BusID":
                                    returnItem.BusID = int.Parse(hashItem.Value);
                                    break;
                                case "BusDriverID":
                                    returnItem.BusDriverID = int.Parse(hashItem.Value);
                                    break;
                                case "Status":
                                    returnItem.Status = int.Parse(hashItem.Value);
                                    break;
                                case "Longitude":
                                    returnItem.Longitude = float.Parse(hashItem.Value);
                                    break;
                                case "Latitude":
                                    returnItem.Latitude = float.Parse(hashItem.Value);
                                    break;
                                default:
                                    break;
                            }
                        }

                        //Create bus                        
                        var itemBusCache = this._RedisCacheClient.GetDb(0).HashGetAllAsync<string>(
                            string.Format(@"Bus:{0}", returnItem.BusID)
                         ).Result;
                        if (itemBusCache.Count > 0)
                        {
                            returnItem.Bus = new Bus();
                            foreach (var hashItem in itemBusCache)
                            {
                                switch (hashItem.Key)
                                {
                                    case "ID":
                                        returnItem.Bus.ID = int.Parse(hashItem.Value);
                                        break;
                                    case "RegistrationNumber":
                                        returnItem.Bus.RegistrationNumber = hashItem.Value;
                                        break;
                                    case "CapacityBoundary":
                                        returnItem.Bus.CapacityBoundary = int.Parse(hashItem.Value);
                                        break;
                                    case "SeatingPlace":
                                        returnItem.Bus.SeatingPlace = int.Parse(hashItem.Value);
                                        break;
                                    case "StandingPlace":
                                        returnItem.Bus.StandingPlace = int.Parse(hashItem.Value);
                                        break;
                                    case "BusModelID":
                                        returnItem.Bus.BusModelID = int.Parse(hashItem.Value);
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }

                        //Create bus model
                        var itemBusModelCache = this._RedisCacheClient.GetDb(0).HashGetAllAsync<string>(
                            string.Format(@"BusModel:{0}", returnItem.Bus.BusModelID)
                         ).Result;
                        if (itemBusModelCache.Count > 0)
                        {
                            returnItem.Bus.BusModel = new BusModel();
                            foreach (var hashItem in itemBusModelCache)
                            {
                                switch (hashItem.Key)
                                {
                                    case "ID":
                                        returnItem.Bus.BusModel.ID = int.Parse(hashItem.Value);
                                        break;
                                    case "Manufacturer":
                                        returnItem.Bus.BusModel.Manufacturer = hashItem.Value;
                                        break;
                                    case "Model":
                                        returnItem.Bus.BusModel.Model = hashItem.Value;
                                        break;
                                    case "Length":
                                        returnItem.Bus.BusModel.Length = hashItem.Value;
                                        break;
                                    case "Width":
                                        returnItem.Bus.BusModel.Width = hashItem.Value;
                                        break;
                                    case "Height":
                                        returnItem.Bus.BusModel.Height = hashItem.Value;
                                        break;
                                    case "PowerTrain":
                                        returnItem.Bus.BusModel.PowerTrain = hashItem.Value;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        //Create bus driver
                        var itemBusDriverCache = this._RedisCacheClient.GetDb(0).HashGetAllAsync<string>(
                            string.Format(@"BusDriver:{0}", returnItem.BusDriverID)
                         ).Result;
                        if (itemBusDriverCache.Count > 0)
                        {
                            returnItem.BusDriver = new BusDriver();
                            foreach (var hashItem in itemBusDriverCache)
                            {
                                switch (hashItem.Key)
                                {
                                    case "ID":
                                        returnItem.BusDriver.ID = int.Parse(hashItem.Value);
                                        break;
                                    case "PersonnelNumber":
                                        returnItem.BusDriver.PersonnelNumber = hashItem.Value;
                                        break;
                                    case "FirstName":
                                        returnItem.BusDriver.FirstName = hashItem.Value;
                                        break;
                                    case "LastName":
                                        returnItem.BusDriver.LastName = hashItem.Value;
                                        break;
                                    case "PhoneNumber":
                                        returnItem.BusDriver.PhoneNumber = hashItem.Value;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        routeBusses.Add(returnItem);
                    }
                }
                return routeBusses.AsEnumerable();
            }
        }

        public int SaveRouteBus(RouteBus routeBus)
        {
            return 0;
        }

        public int DelRouteBus(int routeId, int busId)
        {
            return 0;
        }
    }
}
