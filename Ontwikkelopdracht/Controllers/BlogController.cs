using System;
using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Controllers
{
    public class BlogController : EntityController<Blog>
    {
        private readonly IRepository<User> _userRepository = Injector.Resolve<IRepository<User>>();

        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Details(int id) => View(Repository.FindOne(id));

        public ActionResult Add()
        {
            ViewBag.Author = new SelectList(_userRepository.FindAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Save(Blog blog)
        {
            blog.Date = DateTime.Now;
            blog.Author = _userRepository.FindOne(blog.Author.Id);
            Blog saved = Repository.Save(blog);

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