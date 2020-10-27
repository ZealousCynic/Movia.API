using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface IRouteBusStopRepository
    {
        IEnumerable<RouteBusStop> RouteBusStops { get; }
        int SaveRouteBusStop(RouteBusStop routeBusStop);
        int DelRouteBusStop(int routeBusStopId);
    }
}
