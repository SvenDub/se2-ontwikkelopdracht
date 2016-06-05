using System;
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

            ticket.Order = order.Id;
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
            // TODO Calculate cost
            Order saved = Repository.Save(order);

            return View(saved);
        }

        private Order GetOrder()
        {
            // Start a new order if no order exists yet
            if (Session[SessionVars.Order] == null)
            {
                Order order = Repository.Save(new Order
                {
                    User = (User) Session[SessionVars.User]
                });

                Session[SessionVars.Order] = order;
                return order;
            }
            return (Order) Session[SessionVars.Order];
        }
    }
}