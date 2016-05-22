using System.Web.Mvc;
using Ontwikkelopdracht.Models;

namespace Ontwikkelopdracht.Controllers
{
    public class FilmController : EntityController<Film, int>
    {
        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Details(int id) => View(Repository.FindOne(id));
    }
}