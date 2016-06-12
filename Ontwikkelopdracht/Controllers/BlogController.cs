using System;
using System.Web.Mvc;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence.Exception;
using Util;

namespace Ontwikkelopdracht.Controllers
{
    public class BlogController : EntityController<Blog>
    {
        /// <summary>
        ///     Show list of all blogs.
        /// </summary>
        public ActionResult Index() => View(Repository.FindAll());

        /// <summary>
        ///     Show individual blog.
        /// </summary>
        /// <param name="id">Id of the blog.</param>
        public ActionResult Details(int id) => View(Repository.FindOne(id));

        /// <summary>
        ///     Show form for creating new blog.
        /// </summary>
        [Authentication(Admin = true)]
        public ActionResult Add()
        {
            return View(new Blog());
        }

        /// <summary>
        ///     Edit a blog.
        /// </summary>
        /// <param name="id">Id of the blog.</param>
        [Authentication(Admin = true)]
        public ActionResult Edit(int id) => View(Repository.FindOne(id));

        /// <summary>
        ///     Create or update a blog.
        /// </summary>
        /// <param name="blog">The blog to save.</param>
        [HttpPost]
        [Authentication(Admin = true)]
        public ActionResult Save(Blog blog)
        {
            blog.Date = DateTime.Now;
            blog.Author = (User) Session[SessionVars.User];
            Blog saved = Repository.Save(blog);

            return RedirectToAction("Details", new {id = saved.Id});
        }

        /// <summary>
        ///     Delete a blog.
        /// </summary>
        /// <param name="id">Id of the blog.</param>
        [Authentication(Admin = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                Repository.Delete(id);
            }
            catch (DataSourceException ex)
            {
                Log.E("BLOG", $"Delete failed. {ex}");
                throw;
            }

            return RedirectToAction("Index");
        }
    }
}