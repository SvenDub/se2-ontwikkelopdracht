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

        [DataMember(Column = "gebruiker_email", Type = DataType.Entity)]
        public User User { get; set; }

        [DataMember(Column = "datum")]
        public DateTime Date { get; set; }

        [DataMember(Column = "prijs")]
        public int Cost { get; set; }

        public List<Ticket> Tickets { get; private set; }
    }
}