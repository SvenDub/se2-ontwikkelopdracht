using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;
using Util;

namespace Ontwikkelopdracht.Areas.Admin.Controllers
{
    [Authentication(Admin = true)]
    public class OrderController : EntityController<Order>
    {
        private readonly IRepository<Ticket> _ticketRepository = Injector.Resolve<IRepository<Ticket>>();

        public ActionResult Index() => View(Repository.FindAll());

        public ActionResult Search(string query)
            => View("Index", Repository.FindAllWhere(order => order.User.Name.ContainsIgnoreCase(query)));

        public ActionResult Details(int id)
        {
            Order order = Repository.FindOne(id);
            order.Tickets = _ticketRepository.FindAllWhere(ticket => ticket.Order == id);

            return View(order);
        }
    }
}