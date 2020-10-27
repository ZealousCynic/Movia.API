using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using WebApiNinjectStudio.Domain.Abstract;
using WebApiNinjectStudio.Domain.Concrete;
using WebApiNinjectStudio.Domain.Entities;

namespace WebApiNinjectStudio.Services
{
    public class AuthPolicyHandler : AuthorizationHandler<AuthPolicyRequirement>
    {
        private readonly IHttpContextAccessor _HttpContextAccessor = null;

        public AuthPolicyHandler(IHttpContextAccessor httpContextAccessor)
        {
            this._HttpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthPolicyRequirement requirement)
        {                        
            //Valid token
            var isAuthenticated = context.User.Identity.IsAuthenticated;

            if (isAuthenticated && (requirement.RolePermissions != null))
            {
                //Get request url from context
                //var requestUrl = "/" + (context.Resource as Microsoft.AspNetCore.Routing.RouteEndpoint).RoutePattern.RawText;
                var requestUrl = this._HttpContextAccessor.HttpContext.Request.Path.ToString();
                //Get request medtode from context, for example: Get Post...
                //var requestType = (context.Resource as Microsoft.AspNetCore.Routing.RouteEndpoint).RoutePattern.RequiredValues.Values.First().ToString();
                var requestType = this._HttpContextAccessor.HttpContext.Request.Method.ToString();

                if (
                    context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier) &&
                    context.User.HasClaim(c => c.Type == ClaimTypes.Role)
                    )
                {
                    //Get id of user and role permission of user from token
                    var userID = int.Parse(
                        context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value
                        );
                    var userRole = context.User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

                    //Valid permission
                    var apiUrlsOfRole = requirement.RolePermissions.First(r => r.Name == userRole).RolePermissionApiUrls;
                    if (apiUrlsOfRole == null)
                    {
                        context.Fail();
                    }

                    var isRequestUrlMatch = false;
                    var isRequestTypeMatch = false;
                    foreach (var itemApiUrl in apiUrlsOfRole)
                    {   
                        //isRequestUrlMatch = Regex.IsMatch(requestUrl.ToLower(), "");
                        isRequestUrlMatch = Regex.Match(requestUrl.ToLower(), itemApiUrl.ApiUrl.ApiUrlRegex??"".ToLower())?.Value == requestUrl.ToLower();
                        isRequestTypeMatch =
                            requestType.ToLower() == itemApiUrl.ApiUrl.ApiRequestMethod.ToLower() ? true : false;
                        if (isRequestUrlMatch && isRequestTypeMatch)
                        {
                            break;
                        }
                    }

                    //if (apiUrlsOfRole.FirstOrDefault(
                    //    u => u.ApiUrl.ApiUrlString.ToLower() == requestUrl.ToLower() && u.ApiUrl.ApiRequestMethod.ToLower() == requestType.ToLower()) != null)
                    if (isRequestUrlMatch && isRequestTypeMatch)
                    {
                        context.Succeed(requirement);
                    }

                }
            }
            return Task.CompletedTask;
        }
    }
}
