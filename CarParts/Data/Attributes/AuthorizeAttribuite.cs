using CarParts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace CarParts.Attributes
{
    public class AuthRoleAttribute : AuthorizeAttribute
    {
        new public Role[] Roles { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            string userRole = "";

            if (HttpContext.Current.User != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                // see if this user is authenticated, any authenticated cookie (ticket) exists for this user
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    // see if the authentication is done using FormsAuthentication
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;

                        userRole = identity.Ticket.UserData;
                    }
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary
                                   {
                                       { "action", "Index" },
                                       { "controller", "Home" }
                                   });
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary
                                   {
                                       { "action", "Index" },
                                       { "controller", "Home" }
                                   });
            }


            if (string.IsNullOrEmpty(userRole))
            {
                filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary
                                   {
                                       { "action", "Index" },
                                       { "controller", "Home" }
                                   });
            }
            else
            {
                int role = int.Parse(userRole.ToString());

                if(Roles == null || Roles.Length == 0 || Roles.Any(x => x == (Role)role))
                {
                    //ok
                }
                else
                { 
                    filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary
                                   {
                                       { "action", "Index" },
                                       { "controller", "Home" }
                                   });
                }
            }
        }
    } 
}