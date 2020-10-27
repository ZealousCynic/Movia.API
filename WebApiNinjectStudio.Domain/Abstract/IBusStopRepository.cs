using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface IBusStopRepository
    {
        IEnumerable<BusStop> BusStops { get; }
        int SaveBusStop(BusStop busStop);
        int DelBusStop(int busStopId);
    }
}
