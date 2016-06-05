using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ontwikkelopdracht.Persistence;
using DataType = Ontwikkelopdracht.Persistence.DataType;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "BESTELLING")]
    public class Order
    {
        [Identity(Column = "BESTELNR")]
        [DisplayName("Bestel nummer")]
        public int Id { get; set; }

        [DataMember(Column = "USER_ID", Type = DataType.Entity)]
        [DisplayName("Gebruiker")]
        public User User { get; set; }

        [DataMember(Column = "DATUM")]
        [DisplayName("Datum")]
        [DisplayFormat(DataFormatString = "{0:D}")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime Date { get; set; }

        [DataMember(Column = "PRIJS")]
        [DisplayName("Prijs")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public int Cost { get; set; }

        [DisplayName("Tickets")]
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}