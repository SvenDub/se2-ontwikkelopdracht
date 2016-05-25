using System;
using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Controllers
{
    public class FilmController : EntityController<Film>
    {
        private readonly IRepository<Show> _showRepository = Injector.Resolve<IRepository<Show>>();

        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Search(string query)
            =>
            View("Index",
                Repository
                    .FindAllWhere(film => film.Title.ToLower().IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0));

        public ActionResult Details(int id)
        {
            ViewBag.Shows = _showRepository.FindAllWhere(show => show.Film.Id == id);

            return View(Repository.FindOne(id));
        }
    }
}