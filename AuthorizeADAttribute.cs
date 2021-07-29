using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Linq;

//Custom Filter Attribute for GroupBased Authnetication in Azure AD
//Do not forget tp add [Add Group Claim[ under app registration > Token Authentication 
namespace Application.Filters
{
    public class CustomAuthorizationFilterAttribute : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUserRole = context.HttpContext.User;

            var claimList = currentUserRole.Claims;
            var isInAdminGrp = claimList.Any(c => c.Type == "groups" && c.Value == "c5ddef1e-b982-4d5b-ae04-ed4fe058790f");

            if (!isInAdminGrp)
            {
                //throw new UnauthorizedAccessException("Access Denied!");
                context.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                     { "controller", "Account" },
                     { "action", "Login" }
                });
            }
        }
       
    }
}

//Usage:
//[TypeFilter(typeof(CustomAuthorizationFilterAttribute))]
//public class HomeController : Controller
