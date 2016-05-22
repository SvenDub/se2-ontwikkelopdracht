using System;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "blog")]
    public class Blog
    {
        [Identity(Column = "blog_id")]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public Author Author { get; set; }
    }
}