using System;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "voorstelling")]
    public class Show
    {
        [Identity(Column = "voorstelling_id")]
        public int Id { get; set; }
        public Film Film { get; set; }
        public DateTime Date { get; set; }
    }
}