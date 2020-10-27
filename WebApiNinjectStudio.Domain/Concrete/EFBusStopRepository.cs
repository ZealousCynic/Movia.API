using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFBusStopRepository : IBusStopRepository
    {
        private readonly EFDbContext _Context;

        public EFBusStopRepository(EFDbContext context)
        {
            this._Context = context;
        }
        public IEnumerable<BusStop> BusStops => this._Context.BusStops;

        public int SaveBusStop(BusStop busStop)
        {
            if (busStop.ID == 0)
            {
                //Is StopNumber exist
                if (
                    this._Context.BusStops.Where(o => o.StopNumber == busStop.StopNumber).Any()
                    )
                {
                    return 0;
                }
                this._Context.BusStops.Add(busStop);
            }
            else
            {
                //Is StopNumber exist, except for itself
                if (
                    this._Context.BusStops
                        .Where(o => o.StopNumber == busStop.StopNumber && o.ID != busStop.ID)
                        .Any()
                    )
                {
                    return 0;
                }
                var dbEntry = this._Context.BusStops.Where(o => o.ID == busStop.ID).First();

                if (dbEntry != null)
                {
                    dbEntry.ID = busStop.ID;
                    dbEntry.StopNumber = busStop.StopNumber;
                    dbEntry.Label = busStop.Label;
                    dbEntry.Longitude = busStop.Longitude;
                    dbEntry.Latitude = busStop.Latitude;
                    dbEntry.Zone = busStop.Zone;
                }
            }
            return this._Context.SaveChanges();

        }
        public int DelBusStop(int busStopId)
        {
            //Is the bus stop in the route
            if (this._Context.RouteBusStops.Where(o => o.BusStopID == busStopId).Any())
            {
                return 0;
            }
            if (busStopId <= 0)
            {
                return 0;
            }
            else
            {
                var dbEntry = this._Context.BusStops
                  .Where(o => o.ID == busStopId);
                if (dbEntry.Any())
                {
                    this._Context.Remove(dbEntry.First());
                }
            }
            return this._Context.SaveChanges();
        }
    }
}
