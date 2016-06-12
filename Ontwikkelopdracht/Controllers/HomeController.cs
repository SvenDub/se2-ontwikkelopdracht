using System.Web.Mvc;

namespace Ontwikkelopdracht.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        ///     Show homepage.
        /// </summary>
        public ActionResult Index() => View();
    }
}