﻿using OnlineShop.Model.Models;
using OnlineShop.Service.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Json;
using System.Security.Policy;

namespace LoginService.Attributes
{
    [System.AttributeUsage(AttributeTargets.Method)]
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string? groupName, string? role, string? claimTypes = null, string? claimValues = null) : base(typeof(ClaimRequirementFilter))
        {
            if (string.IsNullOrEmpty(groupName)) groupName = Group.Customer;
            if (string.IsNullOrEmpty(role)) role = Role.BasicMember;

            if (string.IsNullOrEmpty(claimTypes) || string.IsNullOrEmpty(claimValues))
            {
                Arguments = new object[] { groupName, role };
                return;
            }
            var claimTypeArr = claimTypes.Split(',').ToList();
            var claimValueArr = claimValues.Split(',').ToList();
            List<Claim> claimList = new List<Claim>();
            if (claimTypeArr.Count == 0 || claimValueArr.Count == 0)
            {
                Arguments = new object[] { groupName, role };
                return;
            }
            for (int i = 0; i < claimTypeArr.Count; i++)
            {
                claimList.Add(new Claim(claimTypeArr[i].ToString(), claimValues[i].ToString()));
            }
            Arguments = new object[] { groupName, role, claimList };
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly List<Claim>? _claims;
        readonly string _groupName;
        readonly string _role;

        public ClaimRequirementFilter(string groupName, string role, List<Claim>? claims = null)
        {
            _groupName = groupName;
            _role = role;
            _claims = claims;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            if(_claims != null)
            {
                foreach (var claim in _claims)
                {
                    var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value);
                    if (!hasClaim)
                    {
                        //context.Result = new ForbidResult();
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                }
            }
            // check group
            var groupClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == nameof(AccessToken.GroupName));
            if (groupClaim == null)
            {
                //context.Result = new UnauthorizedObjectResult("Not found group in user info");
                context.Result = new RedirectToRouteResult(new { page = "/Account/Error", area = "Identity" , title = "Error", errormsg = "Not-found-group-in-user-info" });
                return;
            }

            if (groupClaim.Value != _groupName)
            {
                //context.Result = new UnauthorizedObjectResult("Unauthorized");
                //return;
                context.Result = new RedirectToRouteResult(new {  page = "/Account/AccessDenied", area = "Identity" });
                return;
            }

            //// check role
            //var roleClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == nameof(AccessToken.RoleName));
            //if (roleClaim == null)
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}

            //if (roleClaim.Value != _role)
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}  

            // you can also use registered services
            //var userManager = context.HttpContext.RequestServices.GetService<UserManager<BaseUser>>();
            //var listService = context.HttpContext.RequestServices.GetService<Login.Data.DbContext>();
        }
    }
}
