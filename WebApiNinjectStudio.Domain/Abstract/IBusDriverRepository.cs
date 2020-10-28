using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface IBusDriverRepository
    {
        IEnumerable<BusDriver> BusDrivers { get; }
        int SaveBusDriver(BusDriver busDriver);
        int DelBusDriver(int busDriverId);
    }
}
