using System;
using System.Collections.Generic;
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
            return 0;
        }

        public int DelRouteBusStop(int routeBusStopId)
        {
            return 0;
        }


    }
}
