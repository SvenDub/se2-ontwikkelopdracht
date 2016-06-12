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

        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Search(string query)
            => View("Index", Repository.FindAllWhere(show => show.Film.Title.ContainsIgnoreCase(query)));

        public ActionResult Details(int id) => View(Repository.FindOne(id));

        public ActionResult Add()
        {
            ViewBag.Film = new SelectList(_filmRepository.FindAll(), "Id", "Title");
            return View();
        }

        public ActionResult Edit(int id)
        {
            Show show = Repository.FindOne(id);
            ViewBag.Film = new SelectList(_filmRepository.FindAll(), "Id", "Title", show.Film.Id);
            return View(show);
        }

        [HttpPost]
        public ActionResult Save(Show show)
        {
            show.Film = _filmRepository.FindOne(show.Film.Id);
            Show saved = Repository.Save(show);

            return RedirectToAction("Details", new {id = saved.Id});
        }

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