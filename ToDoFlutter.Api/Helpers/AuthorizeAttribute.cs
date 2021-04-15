

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoFlutter.core.Models;

namespace ToDoFlutter.Api.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter 
    {
        private readonly IList<AppUserRole> _roles;

        public AuthorizeAttribute(params AppUserRole[] roles)
        {
            _roles = roles ?? new AppUserRole[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var appuser = (AppUser)context.HttpContext.Items["AppUser"];
            if (appuser == null || (_roles.Any() && !_roles.Contains(appuser.UserRole)))
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
