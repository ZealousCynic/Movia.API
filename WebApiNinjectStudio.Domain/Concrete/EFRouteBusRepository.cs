using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFRouteBusRepository : IRouteBusRepository
    {
        private readonly EFDbContext _Context;

        public EFRouteBusRepository(EFDbContext context)
        {
            this._Context = context;
        }

        public IEnumerable<RouteBus> RouteBusses => this._Context.RouteBusses
            .Include(o => o.Bus).ThenInclude(o => o.BusModel)
            .Include(o => o.BusDriver);
        
        public int SaveRouteBus(RouteBus routeBus)
        {
            return 1;
        }

        public int DelRouteBus(int routeBusId)
        {
            return 1;
        }
    }
}
