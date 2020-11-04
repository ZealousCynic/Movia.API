using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface IBusModelRepository
    {
        IEnumerable<BusModel> BusModels { get; }
        int SaveBusModel(BusModel busModel);
        int DelBusModel(int busModelId);
    }
}
