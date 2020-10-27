using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Services
{
    public class AuthPolicyRequirement : IAuthorizationRequirement
    {
        public List<Role> RolePermissions { get; set; }
    }
}
