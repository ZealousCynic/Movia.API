using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiNinjectStudio.Domain.Entities
{
    public class ApiUrl
    {
        public int ApiUrlID { get; set; }
        public string ApiUrlString { get; set; }
        public string ApiRequestMethod { get; set; }
        public string ApiUrlRegex { get; set; }
        public string Description { get; set; }
        public ICollection<RolePermissionApiUrl> RolePermissionApiUrls { get; set; }
    }
}
