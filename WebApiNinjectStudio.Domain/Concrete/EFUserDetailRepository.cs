using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFUserDetailRepository : IUserDetailRepository
    {
        private readonly EFDbContext _Context;

        public EFUserDetailRepository(EFDbContext context)
        {
            this._Context = context;
        }
        public IEnumerable<UserDetail> UserDetails => this._Context.UserDetails;

        public async Task<int> SaveUserDetail(UserDetail userDetail)
        {
            if (userDetail.ID == 0)
            {
                this._Context.UserDetails.Add(userDetail);
            }
            else
            {
                var dbEntry = this._Context.UserDetails
                  .Where(u => u.ID == userDetail.ID).First();

                if (dbEntry != null)
                {
                    dbEntry.UserName = userDetail.UserName;
                    dbEntry.FirstName = userDetail.FirstName;
                    dbEntry.LastName = userDetail.LastName;
                    dbEntry.Gender = userDetail.Gender;
                    dbEntry.Password = userDetail.Password;
                    dbEntry.Status = userDetail.Status;
                }
            }            
            return await this._Context.SaveChangesAsync();
        }

        public async Task<UserDetail> GetUserDetailByUserNamePassword(string userName, string password)
        {
            var itemUserDetail = await this._Context.UserDetails.Where(item => item.UserName == userName && item.Password == password).ToListAsync();
            if (itemUserDetail.Count > 0)
            { return itemUserDetail.First(); }
            else
            { return null; }            
        }


    }
}
