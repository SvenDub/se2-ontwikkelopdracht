using System.Web.Mvc;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        ///     Show homepage.
        /// </summary>
        public ActionResult Index() => View();
    }
}