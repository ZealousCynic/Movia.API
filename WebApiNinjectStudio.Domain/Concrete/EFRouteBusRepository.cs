using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFRouteBusRepository : IRouteBusRepository
    {
        private readonly EFDbContext _Context;

        public EFRouteBusRepository(EFDbContext context)
        {
            this._Context = context;
        }

        public IEnumerable<RouteBus> RouteBusses => this._Context.RouteBusses
            .Include(o => o.Bus).ThenInclude(o => o.BusModel)
            .Include(o => o.BusDriver);
        
        public int SaveRouteBus(RouteBus routeBus)
        {
            //Is bus,driver and route exist.
            if (
                (!this._Context.Busses.Where(o => o.ID == routeBus.BusID).Any())
                ||
                (!this._Context.Routes.Where(o => o.ID == routeBus.RouteID).Any())
                ||
                (!this._Context.BusDrivers.Where(o => o.ID == routeBus.BusDriverID).Any())
                )
            {
                return 0;
            }
            //Is bus and driver is already in other route
            if (this._Context.RouteBusses.Where(o=> o.BusDriverID == routeBus.BusDriverID || o.BusID == routeBus.BusID).Any())
            {
                return 0;
            }
            this._Context.RouteBusses.Add(routeBus);
            return this._Context.SaveChanges();
        }

        public int DelRouteBus(int routeId, int busId)
        {
            var routeBus = this._Context.RouteBusses
                .Where(o => o.RouteID == routeId && o.BusID == busId).ToList();
            if (routeBus.Count <= 0)
            {
                return 0;
            }
            this._Context.Remove(routeBus.First());            
            return this._Context.SaveChanges();
        }
    }
}
