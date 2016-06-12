using System.Web.Mvc;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence.Exception;
using Util;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    [Authentication(Admin = true)]
    public class UserController : EntityController<User>
    {
        /// <summary>
        ///     Show list of all users.
        /// </summary>
        public ActionResult Index() => View(Repository.FindAll());

        /// <summary>
        ///     Show list of users.
        /// </summary>
        /// <param name="query">Query string to search for.</param>
        public ActionResult Search(string query)
            => View("Index", Repository.FindAllWhere(user => user.Name.ContainsIgnoreCase(query)));

        /// <summary>
        ///     Show individual user.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        public ActionResult Details(int id) => View(Repository.FindOne(id));

        /// <summary>
        ///     Show form for creating new user.
        /// </summary>
        public ActionResult Add() => View();

        /// <summary>
        ///     Edit a user.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        public ActionResult Edit(int id) => View(Repository.FindOne(id));

        /// <summary>
        ///     Create or update a user.
        /// </summary>
        /// <param name="user">The user to save.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(User user)
        {
            User saved = Repository.Save(user);

            return RedirectToAction("Details", new {id = saved.Id});
        }

        /// <summary>
        ///     Delete a user.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        public ActionResult Delete(int id)
        {
            try
            {
                Repository.Delete(id);
            }
            catch (DataSourceException ex)
            {
                Log.E("USER", $"Delete failed. {ex}");
                throw;
            }

            return RedirectToAction("Index");
        }
    }
}