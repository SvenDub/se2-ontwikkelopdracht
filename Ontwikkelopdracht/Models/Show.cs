using System;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "voorstelling")]
    public class Show
    {
        [Identity(Column = "voorstelling_id")]
        public int Id { get; set; }

        [DataMember(Column = "film_id", Type = DataType.Entity)]
        public Film Film { get; set; }

        [DataMember(Column = "datum")]
        public DateTime Date { get; set; }
    }
}