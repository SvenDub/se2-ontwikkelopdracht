using System.Web.Mvc;
using Ontwikkelopdracht.Models;
using Util;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    public class UserController : EntityController<User>
    {
        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Search(string query)
            => View("Index", Repository.FindAllWhere(user => user.Name.ContainsIgnoreCase(query)));
    }
}