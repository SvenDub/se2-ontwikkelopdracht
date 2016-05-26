using System;
using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;
using Util;

namespace Ontwikkelopdracht.Controllers
{
    public class FilmController : EntityController<Film>
    {
        private readonly IRepository<Show> _showRepository = Injector.Resolve<IRepository<Show>>();

        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Search(string query)
            => View("Index", Repository.FindAllWhere(film => film.Title.ContainsIgnoreCase(query)));

        public ActionResult Details(int id)
        {
            ViewBag.Shows = _showRepository.FindAllWhere(show => show.Film.Id == id);

            return View(Repository.FindOne(id));
        }
    }
}