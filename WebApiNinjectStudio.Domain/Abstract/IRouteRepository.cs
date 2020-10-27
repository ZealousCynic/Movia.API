using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface IRouteRepository
    {
        IEnumerable<Route> Routes { get; }
        int SaveRoute(Route route);
        int DelRoute(int routeId);
    }
}
