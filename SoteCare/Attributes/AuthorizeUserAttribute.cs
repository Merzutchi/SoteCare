using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SoteCare.Attributes
{
    public class AuthorizeUserAttribute : ActionFilterAttribute
    {
        public string Role { get; set; } // Role to check, e.g., "Doctor" or "Nurse"

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if the user is logged in
            if (HttpContext.Current.Session["Role"] == null)
            {
                // Redirect to Login page if not logged in
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Account", action = "Login" })
                );
                return;
            }

            // Check if a role is specified and if the user's role matches
            if (!string.IsNullOrEmpty(Role))
            {
                var userRole = HttpContext.Current.Session["Role"].ToString();
                if (!userRole.Equals(Role, StringComparison.OrdinalIgnoreCase))
                {
                    // Redirect to an Unauthorized or Home page if the role does not match
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "Unauthorized" })
                    );
                    return;
                }
            }

            // Call base method to continue with the action execution
            base.OnActionExecuting(filterContext);
        }
    }
}