using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ontwikkelopdracht.Persistence;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "CREW")]
    public class Crewmember
    {
        [Identity(Column = "CREW_ID")]
        public int Id { get; set; }

        [DataMember(Column = "NAAM")]
        [DisplayName("Naam")]
        public string Name { get; set; }

        [DataMember(Column = "GEBOORTEDATUM")]
        [DisplayName("Geboortedatum")]
        [DataType(DataType.Date)]
        public DateTime Birth { get; set; }

        [DataMember(Column = "GESLACHT")]
        [DisplayName("Geslacht")]
        public Gender Gender { get; set; }

        [DataMember(Column = "BIO")]
        [DisplayName("Bio")]
        public string Bio { get; set; }
    }
}