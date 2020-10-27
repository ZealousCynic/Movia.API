using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Concrete
{
    public class EFRoleRepository : IRoleRepository
    {
        private readonly EFDbContext _Context;

        public EFRoleRepository(EFDbContext context)
        {
            this._Context = context;
        }

        public IEnumerable<Role> Roles => this._Context.Roles.
                    Include(r => r.RolePermissionApiUrls).ThenInclude(a => a.ApiUrl);

        public int SaveRole(Role role)
        {
            return 0;
        }

        public int DelRole(int rolelId)
        {
            return 0;
        }
    }
}
