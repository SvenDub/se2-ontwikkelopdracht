﻿using System;
using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;
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

        [HttpPost]
        [Authentication(Admin = true)]
        public ActionResult Save(Blog blog)
        {
            blog.Date = DateTime.Now;
            blog.Author = (User) Session[SessionVars.User];
            Log.D("BLOG", blog.ToString());
            Log.D("BLOG", blog.Author.ToString());
            Log.D("BLOG", blog.Author.Name);
            Blog saved = Repository.Save(blog);

            return RedirectToAction("Details", new {id = saved.Id});
        }

        [HttpDelete]
        [Authentication(Admin = true)]
        public ActionResult Delete(int id)
        {
            Repository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}