using System;
using System.Linq;
using System.Web.Mvc;
using Inject;
using Ontwikkelopdracht.Models;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Controllers
{
    [Authentication(Admin = false)]
    public class OrderController : EntityController<Order>
    {
        private readonly IRepository<Show> _showRepository = Injector.Resolve<IRepository<Show>>();
        private readonly IRepository<Ticket> _ticketRepository = Injector.Resolve<IRepository<Ticket>>();

        /// <summary>
        ///     Create a new order.
        /// </summary>
        /// <param name="show">The show for which to create the order.</param>
        public ActionResult Index(int show)
        {
            Show dbShow = _showRepository.FindOne(show);
            return View(new Ticket
            {
                Show = dbShow
            });
        }

        /// <summary>
        ///     Add a new ticket.
        /// </summary>
        /// <param name="ticket">The ticket to add.</param>
        [HttpPost]
        public ActionResult Add(Ticket ticket)
        {
            Order order = GetOrder();

            ticket.Show = _showRepository.FindOne(ticket.Show.Id);
            ticket.Seat = _ticketRepository
                              .FindAllWhere(t => t.Show.Id == ticket.Show.Id)
                              .Concat(order.Tickets.Where(t => t.Show.Id == ticket.Show.Id))
                              .Select(t => t.Seat)
                              .DefaultIfEmpty(0)
                              .Max() + 1;
            order.Tickets.Add(ticket);
            order.Cost = order.Tickets.Select(GetTicketCost).Sum();

            return View(ticket);
        }

        /// <summary>
        ///     Show overview of pending order.
        /// </summary>
        public ActionResult Cart() => View(GetOrder());

        /// <summary>
        ///     Confirm order.
        /// </summary>
        public ActionResult Save()
        {
            // Do not save order if not yet created
            if (Session[SessionVars.Order] == null)
            {
                return RedirectToAction("Cart");
            }

            Order order = (Order) Session[SessionVars.Order];

            if (order.Tickets.Count == 0)
            {
                return RedirectToAction("Cart");
            }

            order.Date = DateTime.Now;
            order.Cost = order.Tickets.Select(GetTicketCost).Sum();
            Order saved = Repository.Save(order);

            Clear();

            return View(saved);
        }

        /// <summary>
        ///     Clear pending order.
        /// </summary>
        public ActionResult Clear()
        {
            Session[SessionVars.Order] = null;

            return RedirectToAction("Cart");
        }

        /// <summary>
        ///     Get the pending order or create a new one if none exist.
        /// </summary>
        /// <returns>The pending order.</returns>
        private Order GetOrder()
        {
            // Start a new order if no order exists yet
            if (Session[SessionVars.Order] == null)
            {
                Order order = new Order
                {
                    User = (User) Session[SessionVars.User]
                };

                Session[SessionVars.Order] = order;
                return order;
            }
            return (Order) Session[SessionVars.Order];
        }

        /// <summary>
        ///     Get the cost for a ticket.
        /// </summary>
        private int GetTicketCost(Ticket ticket) => 1200;
    }
}