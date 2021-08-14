using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuizbeePlus.Commons
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        private readonly string[] allowedRoles;
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //if not logged, it will work as normal Authorize and redirect to the Login
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                //logged and wihout the role to access it - redirect to the custom controller action
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Base", action = "UnAuthorized" }));
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;

            foreach(var roles in allowedRoles)
            {
                if (httpContext.User.IsInRole(roles))
                {
                    authorize = true;
                }
                return base.AuthorizeCore(httpContext);
            }
            return authorize;
        }

        public CustomAuthorize(params string[] roles)
        {
            this.allowedRoles = roles;
        }


    }
}