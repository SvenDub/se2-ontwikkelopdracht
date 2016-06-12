using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;
using Ontwikkelopdracht.Persistence.Exception;
using Util;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    [Authentication(Admin = true)]
    public class ShowController : EntityController<Show>
    {
        private readonly IRepository<Film> _filmRepository = Injector.Resolve<IRepository<Film>>();

        /// <summary>
        ///     Show list of all shows.
        /// </summary>
        public ActionResult Index() => View(Repository.FindAll());

        /// <summary>
        ///     Show list of shows.
        /// </summary>
        /// <param name="query">Query string to search for.</param>
        public ActionResult Search(string query)
            => View("Index", Repository.FindAllWhere(show => show.Film.Title.ContainsIgnoreCase(query)));

        /// <summary>
        ///     Show individual show.
        /// </summary>
        /// <param name="id">Id of the show.</param>
        public ActionResult Details(int id) => View(Repository.FindOne(id));

        /// <summary>
        ///     Show form for creating new show.
        /// </summary>
        public ActionResult Add()
        {
            ViewBag.Film = new SelectList(_filmRepository.FindAll(), "Id", "Title");
            return View();
        }

        /// <summary>
        ///     Edit a show.
        /// </summary>
        /// <param name="id">Id of the show.</param>
        public ActionResult Edit(int id)
        {
            Show show = Repository.FindOne(id);
            ViewBag.Film = new SelectList(_filmRepository.FindAll(), "Id", "Title", show.Film.Id);
            return View(show);
        }

        /// <summary>
        ///     Create or update a show.
        /// </summary>
        /// <param name="show">The show to save.</param>
        [HttpPost]
        public ActionResult Save(Show show)
        {
            show.Film = _filmRepository.FindOne(show.Film.Id);
            Show saved = Repository.Save(show);

            return RedirectToAction("Details", new {id = saved.Id});
        }

        /// <summary>
        ///     Delete a show.
        /// </summary>
        /// <param name="id">Id of the show.</param>
        public ActionResult Delete(int id)
        {
            try
            {
                Repository.Delete(id);
            }
            catch (DataSourceException ex)
            {
                Log.E("SHOW", $"Delete failed. {ex}");
                throw;
            }

            return RedirectToAction("Index");
        }
    }
}