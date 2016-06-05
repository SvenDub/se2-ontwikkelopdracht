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

        public ActionResult Index(int show)
        {
            Show dbShow = _showRepository.FindOne(show);
            return View(new Ticket
            {
                Show = dbShow
            });
        }

        [HttpPost]
        public ActionResult Add(Ticket ticket)
        {
            Order order = GetOrder();

            ticket.Show = _showRepository.FindOne(ticket.Show.Id);
            order.Tickets.Add(ticket);

            return View(ticket);
        }

        public ActionResult Save()
        {
            // Do not save order if not yet created
            if (Session[SessionVars.Order] == null)
            {
                return View();
            }

            Order order = (Order) Session[SessionVars.Order];
            order.Date = DateTime.Now;
            order.Tickets.ForEach(ticket => ticket.Order = order.Id);
            order.Cost = order.Tickets.Select(GetTicketCost).Sum();
            Order saved = Repository.Save(order);

            return View(saved);
        }

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

        private int GetTicketCost(Ticket ticket) => 1200;
    }
}