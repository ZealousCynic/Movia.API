using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFBusRepository : IBusRepository
    {
        private readonly EFDbContext _Context;

        public EFBusRepository(EFDbContext context)
        {
            this._Context = context;
        }

        public IEnumerable<Bus> Busses => this._Context.Busses.Include(o => o.BusModel);

        public int SaveBus(Bus bus)
        {
            //Is bus model exist
            if(
                ! this._Context.BusModels.Where(o => o.ID == bus.BusModelID).Any()
                )
            {
                return 0;
            }

            if (bus.ID == 0)
            {
                //Is RegistrationNumber exist
                if (
                    this._Context.Busses.Where(o => o.RegistrationNumber == bus.RegistrationNumber).Any()
                    )
                {
                    return 0;
                }
                this._Context.Busses.Add(bus);
            }
            else
            {
                //Is RegistrationNumber exist, except for itself
                if (
                    this._Context.Busses
                        .Where(o => o.RegistrationNumber == bus.RegistrationNumber && o.ID != bus.ID)
                        .Any()
                    )
                {
                    return 0;
                }
                var dbEntry = this._Context.Busses.Where(o => o.ID == bus.ID).First();

                if (dbEntry != null)
                {
                    dbEntry.RegistrationNumber = bus.RegistrationNumber;
                    dbEntry.CapacityBoundary = bus.CapacityBoundary;
                    dbEntry.SeatingPlace = bus.SeatingPlace;
                    dbEntry.StandingPlace = bus.StandingPlace;
                    dbEntry.BusModelID = bus.BusModelID;
                }
            }
            return this._Context.SaveChanges();

        }

        public int DelBus(int busId)
        {
            //Is the bus running in the route
            if (this._Context.RouteBusses.Where(o => o.BusID == busId).Any())
            {
                return 0;
            }
            var dbEntry = this._Context.Busses.Where(o => o.ID == busId);
            if (dbEntry.Any())
            {
                this._Context.Remove(dbEntry.First());
            }
            return this._Context.SaveChanges();
        }

    }
}
