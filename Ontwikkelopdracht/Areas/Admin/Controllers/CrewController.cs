using System.Web.Mvc;
using Ontwikkelopdracht.Models;
using Util;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    public class CrewController : EntityController<Crewmember>
    {
        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Search(string query)
            => View("Index", Repository.FindAllWhere(crewmember => crewmember.Name.ContainsIgnoreCase(query)));
    }
}