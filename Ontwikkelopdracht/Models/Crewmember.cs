using System;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "CREW")]
    public class Crewmember
    {
        [Identity(Column = "CREW_ID")]
        public int Id { get; set; }

        [DataMember(Column = "NAAM")]
        public string Name { get; set; }

        [DataMember(Column = "GEBOORTEDATUM")]
        public DateTime Birth { get; set; }

        [DataMember(Column = "GESLACHT")]
        public Gender Gender { get; set; }

        [DataMember(Column = "BIO")]
        public string Bio { get; set; }
    }
}