using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;
using Util;

namespace Ontwikkelopdracht.Controllers
{
    public class BlogController : EntityController<Blog>
    {
        private readonly IRepository<Author> _authorRepository = Injector.Resolve<IRepository<Author>>();

        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Details(int id) => View(Repository.FindOne(id));

        public ActionResult Add()
        {
            ViewBag.Author = new SelectList(_authorRepository.FindAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Save(Blog blog)
        {
            blog.Date = DateTime.Now;
            blog.Author = _authorRepository.FindOne(blog.Author.Id);
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