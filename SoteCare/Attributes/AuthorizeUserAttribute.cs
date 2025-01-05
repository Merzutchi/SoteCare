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
        // This method will be triggered before any action method is executed
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if the user is logged in by checking the session
            if (HttpContext.Current.Session["Role"] == null)
            {
                // If not logged in, redirect to the Login page
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Account", action = "Login" })
                );
            }

            // Call base method to continue with the action execution
            base.OnActionExecuting(filterContext);
        }
    }
}