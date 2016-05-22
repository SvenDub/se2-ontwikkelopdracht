using System;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "crew")]
    public class Crewmember
    {
        [Identity(Column = "crew_id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birth { get; set; }
        public Gender Gender { get; set; }
        public string Bio { get; set; }
    }
}