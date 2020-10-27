using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFRouteRepository : IRouteRepository
    {
        private readonly EFDbContext _Context;

        public EFRouteRepository(EFDbContext context)
        {
            this._Context = context;
        }

        public IEnumerable<Route> Routes => this._Context.Routes.Include(o => o.RouteBusStops).ThenInclude(o => o.BusStop);

        public int SaveRoute(Route route)
        {
            if (route.ID == 0)
            {
                this._Context.Routes.Add(route);
            }
            else
            {
                var dbEntry = this._Context.Routes.Where(o => o.ID == route.ID).First();

                if (dbEntry != null)
                {
                    dbEntry.ID = route.ID;
                    dbEntry.Label = route.Label;
                    dbEntry.Description = route.Description;
                }
            }
            return this._Context.SaveChanges();
        }

        public int DelRoute(int routeId)
        {
            return 0;
        }
    }
}
