using System;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "film")]
    public class Film
    {
        [Identity(Column = "film_id")]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Length { get; set; }
        public DateTime Release { get; set; }
        public string Language { get; set; }
        public string Subtitles { get; set; }
        public Genre Genre { get; set; }
    }
}