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

        public ActionResult Details(int id) => View(Repository.FindOne(id));

        public ActionResult Add() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Crewmember crew)
        {
            Crewmember saved = Repository.Save(crew);

            return RedirectToAction("Details", new {id = saved.Id});
        }
    }
}