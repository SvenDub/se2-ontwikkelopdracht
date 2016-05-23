using System.Web.Mvc;
using Ontwikkelopdracht.Models;

namespace Ontwikkelopdracht.Controllers
{
    public class FilmController : EntityController<Film>
    {
        public ActionResult Index() => View(Repository.FindAll());

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