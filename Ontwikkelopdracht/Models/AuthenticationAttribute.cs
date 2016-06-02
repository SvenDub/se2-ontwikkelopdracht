using System.Web.Mvc;
using System.Web.Routing;
using Inject;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        private readonly IRepository<User> _userRepository = Injector.Resolve<IRepository<User>>();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session?["user"] == null)
            {
                RedirectToLogin(filterContext);
            }
            else
            {
                User user = filterContext.HttpContext.Session["user"] as User;
                if (user != null)
                {
                    // Get latest user info from db
                    User dbUser = _userRepository.FindOne(user.Id);
                    filterContext.HttpContext.Session["user"] = dbUser;

                    if (Admin && !dbUser.Admin)
                    {
                        RedirectToUnauthorized(filterContext);
                    }
                }
                else
                {
                    filterContext.HttpContext.Session.Remove("user");
                }
            }
        }

        private static void RedirectToLogin(ActionExecutingContext filterContext)
        {
            filterContext.Result =
                new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Login",
                            action = "Index",
                            returnUrl = filterContext.HttpContext.Request.RawUrl
                        }));
        }

        private static void RedirectToUnauthorized(ActionExecutingContext filterContext)
        {
            filterContext.Result =
                new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Login",
                            action = "Unauthorized",
                            returnUrl = filterContext.HttpContext.Request.RawUrl
                        }));
        }

        public bool Admin { get; set; } = false;
    }
}