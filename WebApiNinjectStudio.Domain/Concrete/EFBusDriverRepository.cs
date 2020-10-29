using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFBusDriverRepository : IBusDriverRepository
    {
        private readonly EFDbContext _Context;

        public EFBusDriverRepository(EFDbContext context)
        {
            this._Context = context;
        }

        public IEnumerable<BusDriver> BusDrivers => this._Context.BusDrivers;


        public int SaveBusDriver(BusDriver busDriver)
        {

            if (busDriver.ID == 0)
            {
                //Is PersonnelNumber exist
                if (
                    this._Context.BusDrivers.Where(o => o.PersonnelNumber == busDriver.PersonnelNumber).Any()
                    )
                {
                    return 0;
                }
                this._Context.BusDrivers.Add(busDriver);
            }
            else
            {
                //Is PersonnelNumber exist, except for itself
                if (
                    this._Context.BusDrivers
                        .Where(o => o.PersonnelNumber == busDriver.PersonnelNumber && o.ID != busDriver.ID)
                        .Any()
                    )
                {
                    return 0;
                }
                var busDrivers = this._Context.BusDrivers
                    .Where(o => o.ID == busDriver.ID).ToList();
                if (busDrivers.Count <= 0)
                {
                    return 0;
                }
                var dbEntry = busDrivers.First();
                dbEntry.PersonnelNumber = busDriver.PersonnelNumber;
                dbEntry.FirstName = busDriver.FirstName;
                dbEntry.LastName = busDriver.LastName;
                dbEntry.PhoneNumber = busDriver.PhoneNumber;
            }
            return this._Context.SaveChanges();
        }

        public int DelBusDriver(int busDriverId)
        {
            //Is the bus running in the route
            if (this._Context.RouteBusses.Where(o => o.BusDriverID == busDriverId).Any())
            {
                return 0;
            }
            var dbEntry = this._Context.BusDrivers.Where(o => o.ID == busDriverId);
            if (dbEntry.Any())
            {
                this._Context.Remove(dbEntry.First());
            }
            return this._Context.SaveChanges();
        }
    }
}
