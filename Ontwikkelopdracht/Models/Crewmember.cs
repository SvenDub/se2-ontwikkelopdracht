using System;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "crew")]
    public class Crewmember
    {
        [Identity(Column = "crew_id")]
        public int Id { get; set; }

        [DataMember(Column = "naam")]
        public string Name { get; set; }

        [DataMember(Column = "geboortedatum")]
        public DateTime Birth { get; set; }

        [DataMember(Column = "geslacht")]
        public Gender Gender { get; set; }

        [DataMember(Column = "bio")]
        public string Bio { get; set; }
    }
}