using System.Web.Mvc;
using Ontwikkelopdracht.Models;
using Util;

namespace Ontwikkelopdracht.Controllers
{
    public class FilmController : EntityController<Film>
    {
        public ActionResult Index()
        {
            return View(Repository.FindAll());
        }

        public ActionResult Details(int id) => View(Repository.FindOne(id));

        public ActionResult Save() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Film film)
        {
            Film updated = Repository.Save(film);

            return RedirectToAction("Details", new {id = updated.Id});
        }
    }
}