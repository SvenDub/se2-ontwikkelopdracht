using System;
using System.ComponentModel.DataAnnotations;
using Ontwikkelopdracht.Persistence;
using DataType = Ontwikkelopdracht.Persistence.DataType;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "VOORSTELLING")]
    public class Show
    {
        [Identity(Column = "VOORSTELLING_ID")]
        public int Id { get; set; }

        [DataMember(Column = "FILM_ID", Type = DataType.Entity)]
        public Film Film { get; set; }

        [DataMember(Column = "DATUM")]
        [DisplayFormat(DataFormatString = "{0:D}")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime Date { get; set; }
    }
}