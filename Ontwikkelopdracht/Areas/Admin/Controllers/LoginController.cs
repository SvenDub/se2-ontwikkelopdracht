using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ontwikkelopdracht.Models;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    public class LoginController : EntityController<User>
    {
        public ActionResult Index(string returnUrl, bool failed = false)
        {
            if (Session[SessionVars.User] == null)
            {
                ViewBag.RedirectUrl = returnUrl;
                ViewBag.Failed = failed;
                return View();
            }
            else
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(returnUrl);
            }
        }

        [HttpPost]
        public ActionResult Index(User model, string returnUrl)
        {
            if (Session[SessionVars.User] == null)
            {
                List<User> users =
                    Repository.FindAllWhere(user => user.Email == model.Email && user.Password == model.Password);
                if (users.Count == 1)
                {
                    User loggedIn = users.First();

                    Session[SessionVars.User] = loggedIn;

                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", new { returnUrl = returnUrl, failed = true });
                }
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        public ActionResult Logout()
        {
            Session.Remove(SessionVars.User);

            return RedirectToAction("Index");
        }

        public ActionResult Unauthorized(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }
    }
}