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

        /// <summary>
        ///     Show list of all orders.
        /// </summary>
        public ActionResult Index() => View(Repository.FindAll());

        /// <summary>
        ///     Show list of orders.
        /// </summary>
        /// <param name="query">Query string to search for.</param>
        public ActionResult Search(string query)
            => View("Index", Repository.FindAllWhere(order => order.User.Name.ContainsIgnoreCase(query)));

        /// <summary>
        ///     Show individual order.
        /// </summary>
        /// <param name="id">Id of the order.</param>
        public ActionResult Details(int id)
        {
            Order order = Repository.FindOne(id);
            order.Tickets = _ticketRepository.FindAllWhere(ticket => ticket.Order == id);

            return View(order);
        }
    }
}