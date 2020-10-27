using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class Role
    {
        public int RoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<RolePermissionApiUrl> RolePermissionApiUrls { get; set; }
    }
}
