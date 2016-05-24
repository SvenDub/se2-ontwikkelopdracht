using System;
using System.Collections.Generic;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "BESTELLING")]
    public class Order
    {
        [Identity(Column = "BESTELNR")]
        public int Id { get; set; }

        [DataMember(Column = "USER_ID", Type = DataType.Entity)]
        public User User { get; set; }

        [DataMember(Column = "DATUM")]
        public DateTime Date { get; set; }

        [DataMember(Column = "PRIJS")]
        public int Cost { get; set; }

        public List<Ticket> Tickets { get; private set; }
    }
}