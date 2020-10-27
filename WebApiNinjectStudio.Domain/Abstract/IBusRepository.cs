using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface IBusRepository
    {
        IEnumerable<Bus> Busses { get; }
        int SaveBus(Bus bus);
        int DelBus(int busId);
    }
}
