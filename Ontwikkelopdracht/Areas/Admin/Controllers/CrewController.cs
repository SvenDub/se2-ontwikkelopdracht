using System.Web.Mvc;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence.Exception;
using Util;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    [Authentication(Admin = true)]
    public class CrewController : EntityController<Crewmember>
    {
        /// <summary>
        ///     Show list of all crew.
        /// </summary>
        public ActionResult Index() => View(Repository.FindAll());

        /// <summary>
        ///     Show list of crew.
        /// </summary>
        /// <param name="query">Query string to search for.</param>
        public ActionResult Search(string query)
            => View("Index", Repository.FindAllWhere(crewmember => crewmember.Name.ContainsIgnoreCase(query)));

        /// <summary>
        ///     Show individual crewmember.
        /// </summary>
        /// <param name="id">Id of the crewmember.</param>
        public ActionResult Details(int id) => View(Repository.FindOne(id));

        /// <summary>
        ///     Show form for creating new crewmember.
        /// </summary>
        public ActionResult Add() => View();

        /// <summary>
        ///     Edit a crewmember.
        /// </summary>
        /// <param name="id">Id of the crewmember.</param>
        public ActionResult Edit(int id) => View(Repository.FindOne(id));

        /// <summary>
        ///     Create or update a crewmember.
        /// </summary>
        /// <param name="crew">The crewmember to save.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Crewmember crew)
        {
            Crewmember saved = Repository.Save(crew);

            return RedirectToAction("Details", new {id = saved.Id});
        }

        /// <summary>
        ///     Delete a crewmember.
        /// </summary>
        /// <param name="id">Id of the crewmember.</param>
        public ActionResult Delete(int id)
        {
            try
            {
                Repository.Delete(id);
            }
            catch (DataSourceException ex)
            {
                Log.E("CREW", $"Delete failed. {ex}");
                throw;
            }

            return RedirectToAction("Index");
        }
    }
}