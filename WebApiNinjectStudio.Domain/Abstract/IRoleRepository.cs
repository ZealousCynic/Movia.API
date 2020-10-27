using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface IRoleRepository
    {
        IEnumerable<Role> Roles { get; }
        int SaveRole(Role role);
        int DelRole(int roleId);
    }
}
