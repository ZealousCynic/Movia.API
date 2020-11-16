using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Abstract;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public enum RouteBusRepositoryType
    {
        EF,
        Redis
    }
    public delegate IRouteBusRepository RouteBusFactory(RouteBusRepositoryType routeBusRepositoryType);
}
