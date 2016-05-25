using System;
using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    public class ShowController : EntityController<Show>
    {
        private readonly IRepository<Film> _filmRepository = Injector.Resolve<IRepository<Film>>();

        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Search(string query)
            =>
            View("Index",
                Repository
                    .FindAllWhere(
                        show =>
                            show.Film.Title.ToLower().IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0));

        public ActionResult Details(int id) => View(Repository.FindOne(id));

        public ActionResult Add()
        {
            ViewBag.Film = new SelectList(_filmRepository.FindAll(), "Id", "Title");
            return View();
        }

        [HttpPost]
        public ActionResult Save(Show show)
        {
            show.Film = _filmRepository.FindOne(show.Film.Id);
            Show saved = Repository.Save(show);

            return RedirectToAction("Details", new {id = saved.Id});
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Repository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}