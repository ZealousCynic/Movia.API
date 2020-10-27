using System;
using System.Collections.Generic;
using System.Text;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<User> Users { get; }
        int SaveUser(User user);
        int DelUser(int userId);
    }
}
