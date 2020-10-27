using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Domain.Abstract
{
    public interface IUserDetailRepository
    {
        IEnumerable<UserDetail> UserDetails { get; }
        Task<int> SaveUserDetail(UserDetail userDetail);
        Task<UserDetail> GetUserDetailByUserNamePassword(string userName, string password);
        //int DelUser(int userDetailID);
    }
}
