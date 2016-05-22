using System.Web.Mvc;
using Ontwikkelopdracht.Models;

namespace Ontwikkelopdracht.Controllers
{
    public class OrderController : EntityController<Order, int>
    {
        public ActionResult Index(int show) => View();

        [HttpPost]
        public ActionResult Save(Order order)
        {
            Order saved = Repository.Save(order);

            return View(saved);
        }
    }
}