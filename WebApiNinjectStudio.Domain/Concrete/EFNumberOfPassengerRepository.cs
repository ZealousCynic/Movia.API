using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFNumberOfPassengerRepository : INumberOfPassengerRepository
    {
        private readonly EFDbContext _Context;

        public EFNumberOfPassengerRepository(EFDbContext context)
        {
            this._Context = context;
        }
        public IEnumerable<NumberOfPassenger> NumberOfPassengers => this._Context.NumberOfPassengers;

        public int SaveNumberOfPassenger(NumberOfPassenger numberOfPassenger)
        {
            var routeBusItem = this._Context.RouteBusses.Find(numberOfPassenger.RouteBusID);
            if (routeBusItem == null)
            {
                return 0;
            }
            if (numberOfPassenger.ID == 0)
            {
                this._Context.NumberOfPassengers.Add(numberOfPassenger);
            }
            return this._Context.SaveChanges();            
        }

        public int DelNumberOfPassenger(int numberOfPassengerId)
        {
            return 1;
        }
    }
}
