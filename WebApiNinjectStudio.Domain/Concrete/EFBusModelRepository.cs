using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFBusModelRepository : IBusModelRepository
    {
        private readonly EFDbContext _Context;

        public EFBusModelRepository(EFDbContext context)
        {
            this._Context = context;
        }

        public IEnumerable<BusModel> BusModels => this._Context.BusModels;

        public int SaveBusModel(BusModel busModel)
        {
            if (busModel.ID == 0)
            {
                this._Context.BusModels.Add(busModel);
                this._Context.SaveChanges();
                return 1;
            }
            else
            {
                var busModels = this._Context.BusModels
                        .Where(o => o.ID == busModel.ID)
                        .ToList();
                if (busModels.Count <= 0)
                {
                    return 0;
                }
                var dbEntry = busModels.First();
                if (dbEntry != null)
                {
                    dbEntry.Manufacturer = busModel.Manufacturer;
                    dbEntry.Model = busModel.Model;
                    dbEntry.Length = busModel.Length;
                    dbEntry.Height = busModel.Height;
                    dbEntry.PowerTrain = busModel.PowerTrain;
                    this._Context.SaveChanges();
                    return 1;
                }
            }
            return 0;
        }

        public int DelBusModel(int busModelId)
        {
            //Is bus model being used
            var busses = this._Context.Busses
                .Where(o => o.BusModel.ID == busModelId)
                .ToList();
            if (busses.Count > 0)
            {
                return 0;
            }

            var busModels = this._Context.BusModels
                       .Where(o => o.ID == busModelId).ToList();
            if (busModels.Count <= 0)
            {
                return 0;
            }
            var dbEntry = busModels.First();
            this._Context.BusModels.Remove(dbEntry);
            return this._Context.SaveChanges();            
        }
    }
}
