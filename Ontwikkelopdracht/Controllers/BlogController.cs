using System;
using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;
using Ontwikkelopdracht.Persistence.Exception;
using Util;

namespace Ontwikkelopdracht.Controllers
{
    public class BlogController : EntityController<Blog>
    {
        private readonly IRepository<User> _userRepository = Injector.Resolve<IRepository<User>>();

        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Details(int id) => View(Repository.FindOne(id));

        [Authentication(Admin = true)]
        public ActionResult Add()
        {
            return View(new Blog());
        }

        [Authentication(Admin = true)]
        public ActionResult Edit(int id) => View(Repository.FindOne(id));

        [HttpPost]
        [Authentication(Admin = true)]
        public ActionResult Save(Blog blog)
        {
            blog.Date = DateTime.Now;
            blog.Author = (User) Session[SessionVars.User];
            Blog saved = Repository.Save(blog);

            return RedirectToAction("Details", new {id = saved.Id});
        }

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