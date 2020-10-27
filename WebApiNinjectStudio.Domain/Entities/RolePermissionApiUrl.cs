using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class RolePermissionApiUrl
    {
        public int ApiUrlID { get; set; }
        public ApiUrl ApiUrl { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
    }
}
