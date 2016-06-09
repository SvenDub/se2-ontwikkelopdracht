using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence.Exception;
using Util;

namespace Ontwikkelopdracht.Controllers
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
                    return RedirectToAction("Index", new {returnUrl = returnUrl, failed = true});
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

        public ActionResult Signup(string returnUrl, bool failed = false)
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
        public ActionResult Signup(User model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Make sure the user can not create an admin account or override an existing one.
                model.Admin = false;
                model.Id = 0;

                try
                {
                    Repository.Save(model);
                }
                catch (DataSourceException ex)
                {
                    Log.E("SIGNUP", ex.ToString());
                    return RedirectToAction("Signup", new {returnUrl = returnUrl, failed = true});
                }

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
                    return RedirectToAction("Index", new {returnUrl = returnUrl, failed = true});
                }
            }
            else
            {
                // Make sure the user can not create an admin account or override an existing one.
                model.Admin = false;
                model.Id = 0;
                ViewBag.Failed = true;

                return View(model);
            }
        }

        public ActionResult Unauthorized(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }
    }
}