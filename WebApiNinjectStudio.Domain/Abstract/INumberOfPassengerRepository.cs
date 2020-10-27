using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface INumberOfPassengerRepository
    {
        IEnumerable<NumberOfPassenger> NumberOfPassengers { get; }
        int SaveNumberOfPassenger(NumberOfPassenger numberOfPassenger);
        int DelNumberOfPassenger(int numberOfPassengerId);
    }
}
