using System.Collections.Generic;
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
        private readonly IRepository<Genre> _genreRepository = Injector.Resolve<IRepository<Genre>>();

        public ActionResult Index()
        {
            ViewBag.Genres = _genreRepository.FindAll();

            return View(Repository.FindAll());
        }

        public ActionResult Search(string query, int genre = -1)
        {
            ViewBag.Genres = _genreRepository.FindAll();
            ViewBag.Genre = genre;

            List<Film> films = Repository.FindAllWhere(film => film.Title.ContainsIgnoreCase(query));
            if (genre > -1)
            {
                films.RemoveAll(film => film.Genre.Id != genre);
            }
            return View("Index", films);
        }

        public ActionResult Details(int id)
        {
            ViewBag.Shows = _showRepository.FindAllWhere(show => show.Film.Id == id);

            return View(Repository.FindOne(id));
        }
    }
}