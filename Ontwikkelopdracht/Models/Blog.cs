using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ontwikkelopdracht.Persistence;
using DataType = Ontwikkelopdracht.Persistence.DataType;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "BLOG")]
    public class Blog
    {
        [Identity(Column = "BLOG_ID")]
        public int Id { get; set; }

        [DataMember(Column = "TITEL")]
        public string Title { get; set; }

        [DataMember(Column = "DATUM")]
        [DisplayFormat(DataFormatString = "{0:D}")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime Date { get; set; }

        [DataMember(Column = "BODY")]
        public string Body { get; set; }

        [DataMember(Column = "AUTEUR", Type = DataType.Entity)]
        [DisplayName("Auteur")]
        public User Author { get; set; }
    }
}