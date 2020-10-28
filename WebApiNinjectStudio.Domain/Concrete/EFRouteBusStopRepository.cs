using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFRouteBusStopRepository : IRouteBusStopRepository
    {
        private readonly EFDbContext _Context;

        public EFRouteBusStopRepository(EFDbContext context)
        {
            this._Context = context;
        }

        public IEnumerable<RouteBusStop> RouteBusStops => this._Context.RouteBusStops
                    .Include(o => o.BusStop);

        public int SaveRouteBusStop(RouteBusStop routeBusStop)
        {
            //Is the bus stop already in the route.
            //if (
            //    this._Context.RouteBusStops.Where(o => o.BusStopID == routeBusStop.BusStopID && o.RouteID == routeBusStop.RouteID).Any()
            //    )
            //{
            //    return 0;
            //}

            //Is bus and route already exist.
            if (
                (!this._Context.BusStops.Where(o => o.ID == routeBusStop.BusStopID).Any())
                ||
                (!this._Context.Routes.Where(o => o.ID == routeBusStop.RouteID).Any())
                )
            {
                return 0;
            }

            var isNew = true;
            var routeBusStops = this._Context.RouteBusStops.Where(o => o.BusStopID == routeBusStop.BusStopID && o.RouteID == routeBusStop.RouteID).ToList();

            if (routeBusStops.Count > 0)
            {
                isNew = false;
            }

            if (isNew)
            {
                this._Context.RouteBusStops.Add(routeBusStop);
            }
            else
            {
                var dbEntry = routeBusStops.First();

                if (dbEntry != null)
                {                    
                    dbEntry.BusStopID = routeBusStop.BusStopID;
                    dbEntry.RouteID = routeBusStop.RouteID;
                    dbEntry.Order = routeBusStop.Order;
                }
            }
            return this._Context.SaveChanges();
        }

        public int DelRouteBusStop(int routeId, int busStopId)
        {
            var routeBus = this._Context.RouteBusStops
                .Where(o => o.BusStopID == busStopId && o.RouteID == routeId);

            if (! routeBus.Any())
            {
                return 0;
            }
            var dbEntry = routeBus.First();
            this._Context.Remove(dbEntry);

            return this._Context.SaveChanges();
        }


    }
}
