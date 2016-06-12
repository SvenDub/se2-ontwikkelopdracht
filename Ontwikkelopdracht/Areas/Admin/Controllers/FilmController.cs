using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;
using Ontwikkelopdracht.Persistence.Exception;
using Util;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    [Authentication(Admin = true)]
    public class FilmController : Ontwikkelopdracht.Controllers.FilmController
    {
        private readonly IRepository<Genre> _genreRepository = Injector.Resolve<IRepository<Genre>>();

        /// <summary>
        ///     Show form for creating new film.
        /// </summary>
        public ActionResult Add()
        {
            ViewBag.Genre = new SelectList(_genreRepository.FindAll(), "Id", "Name");
            return View();
        }

        /// <summary>
        ///     Edit a film.
        /// </summary>
        /// <param name="id">Id of the film.</param>
        public ActionResult Edit(int id)
        {
            Film model = Repository.FindOne(id);
            ViewBag.Genre = new SelectList(_genreRepository.FindAll(), "Id", "Name", model.Genre.Id);
            return View(model);
        }

        /// <summary>
        ///     Create or update a film.
        /// </summary>
        /// <param name="film">The film to save.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Film film)
        {
            film.Genre = _genreRepository.FindOne(film.Genre.Id);
            Film saved = Repository.Save(film);

            return RedirectToAction("Details", new {id = saved.Id});
        }

        /// <summary>
        ///     Delete a film.
        /// </summary>
        /// <param name="id">Id of the film.</param>
        public ActionResult Delete(int id)
        {
            try
            {
                Repository.Delete(id);
            }
            catch (DataSourceException ex)
            {
                Log.E("FILM", $"Delete failed. {ex}");
                throw;
            }

            return RedirectToAction("Index");
        }
    }
}