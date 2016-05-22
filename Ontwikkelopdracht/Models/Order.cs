using System;
using System.Collections.Generic;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "bestelling")]
    public class Order
    {
        [Identity(Column = "bestelnr")]
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public int Cost { get; set; }
        public List<Ticket> Tickets { get; private set; }
    }
}