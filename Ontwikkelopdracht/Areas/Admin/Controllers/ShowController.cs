using System.Web.Mvc;
using Ontwikkelopdracht.Models;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    public class ShowController : EntityController<Show>
    {
        public ActionResult Index() => View(Repository.FindAll());
    }
}