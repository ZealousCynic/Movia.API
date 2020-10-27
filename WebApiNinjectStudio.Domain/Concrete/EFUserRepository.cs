using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFUserRepository : IUserRepository
    {
        private readonly EFDbContext _Context;

        public EFUserRepository(EFDbContext context)
        {
            this._Context = context;
        }

        public IEnumerable<User> Users => this._Context.Users
                    .Include(u => u.Role).ThenInclude(r => r.RolePermissionApiUrls);

        public int SaveUser(User user)
        {
            if (user.UserID == 0)
            {
                this._Context.Users.Add(user);
            }
            else
            {
                var dbEntry = this._Context.Users.Find(user.UserID);
                if (dbEntry != null)
                {
                    dbEntry.Email = user.Email;
                    dbEntry.Password = user.Password;
                }
            }
            return this._Context.SaveChanges();
        }


        public int DelUser(int userId)
        {
            if (userId <= 0)
            {
                return 0;
            }
            else
            {
                var dbEntry = this._Context.Users.Find(userId);
                if (dbEntry != null)
                {
                    this._Context.Remove(dbEntry);
                }
            }
            return this._Context.SaveChanges();
        }
    }
}
