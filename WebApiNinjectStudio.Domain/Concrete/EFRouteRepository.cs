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
                this._Context.SaveChanges();
                return 1;
            }
            else
            {
                var routeItem = this._Context.Routes.Where(o => o.ID == route.ID).ToList();
                if (routeItem.Count > 0)
                {
                    var dbEntry = routeItem.First();
                    if (dbEntry != null)
                    {
                        dbEntry.Label = route.Label;
                        dbEntry.Description = route.Description;

                        this._Context.SaveChanges();
                        return 1;
                    }
                }
            }
            return 0;
        }

        public int DelRoute(int routeId)
        {
            return 0;
        }
    }
}
