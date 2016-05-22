using System;

namespace Ontwikkelopdracht.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public Author Author { get; set; }
    }
}