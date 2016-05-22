using System.Web.Mvc;
using Ontwikkelopdracht.Models;

namespace Ontwikkelopdracht.Controllers
{
    public class BlogController : EntityController<Blog>
    {
        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Details(int id) => View(Repository.FindOne(id));

        [HttpPost]
        public ActionResult Save(Blog blog)
        {
            Blog saved = Repository.Save(blog);

            return RedirectToAction("Details", saved.Id);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Repository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}