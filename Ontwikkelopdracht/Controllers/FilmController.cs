using System;
using System.Linq;
using System.Web.Mvc;
using Ontwikkelopdracht.Models;

namespace Ontwikkelopdracht.Controllers
{
    public class FilmController : EntityController<Film>
    {
        public ActionResult Index()
        {
            return View(Repository.FindAll());
        }

        public ActionResult Search(string query)
            =>
            View("Index",
                Repository.FindAll()
                    .Where(film => film.Title.ToLower().IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    .ToList());

        public ActionResult Details(int id) => View(Repository.FindOne(id));

        /*public ActionResult Save() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Film film)
        {
            Film updated = Repository.Save(film);

            return RedirectToAction("Details", new {id = updated.Id});
        }*/
    }
}