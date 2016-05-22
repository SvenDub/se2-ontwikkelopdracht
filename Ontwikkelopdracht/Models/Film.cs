using System;

namespace Ontwikkelopdracht.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Length { get; set; }
        public DateTime Release { get; set; }
        public string Language { get; set; }
        public string Subtitles { get; set; }
        public Genre Genre { get; set; }
    }
}