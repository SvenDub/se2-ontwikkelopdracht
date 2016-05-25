using System;
using System.Web.Mvc;
using Ontwikkelopdracht.Models;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    public class ShowController : EntityController<Show>
    {
        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Search(string query)
            =>
            View("Index",
                Repository
                    .FindAllWhere(
                        show =>
                            show.Film.Title.ToLower().IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0));
    }
}