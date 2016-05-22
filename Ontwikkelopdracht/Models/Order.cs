using System;
using System.Collections.Generic;

namespace Ontwikkelopdracht.Models
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public int Cost { get; set; }
        public List<Ticket> Tickets { get; private set; }
    }
}