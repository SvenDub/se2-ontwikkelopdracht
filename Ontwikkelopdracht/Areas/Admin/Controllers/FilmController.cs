using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    [Authentication(Admin = true)]
    public class FilmController : Ontwikkelopdracht.Controllers.FilmController
    {
        private readonly IRepository<Genre> _genreRepository = Injector.Resolve<IRepository<Genre>>();

        public ActionResult Add()
        {
            ViewBag.Genre = new SelectList(_genreRepository.FindAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Film film)
        {
            film.Genre = _genreRepository.FindOne(film.Genre.Id);
            Film saved = Repository.Save(film);

            return RedirectToAction("Details", new {id = saved.Id});
        }
    }
}