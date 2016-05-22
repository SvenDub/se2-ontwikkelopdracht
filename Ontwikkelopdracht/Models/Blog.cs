using System;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "blog")]
    public class Blog
    {
        [Identity(Column = "blog_id")]
        public int Id { get; set; }

        [DataMember(Column = "titel")]
        public string Title { get; set; }

        [DataMember(Column = "datum")]
        public DateTime Date { get; set; }

        [DataMember(Column = "body")]
        public string Body { get; set; }

        [DataMember(Column = "auteur", Type = DataType.Entity)]
        public Author Author { get; set; }
    }
}