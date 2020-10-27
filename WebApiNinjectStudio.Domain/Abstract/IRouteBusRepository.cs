using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface IRouteBusRepository
    {
        IEnumerable<RouteBus> RouteBusses { get; }
        int SaveRouteBus(RouteBus routeBus);
        int DelRouteBus(int routeBusId);
    }
}
