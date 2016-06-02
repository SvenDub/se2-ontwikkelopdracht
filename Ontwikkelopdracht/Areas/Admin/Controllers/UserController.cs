using System.Web.Mvc;
using Ontwikkelopdracht.Models;
using Util;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    [Authentication(Admin = true)]
    public class UserController : EntityController<User>
    {
        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Search(string query)
            => View("Index", Repository.FindAllWhere(user => user.Name.ContainsIgnoreCase(query)));

        public ActionResult Details(int id) => View(Repository.FindOne(id));

        public ActionResult Add() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(User user)
        {
            User saved = Repository.Save(user);

            return RedirectToAction("Details", new {id = saved.Id});
        }
    }
}