using System;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "film")]
    public class Film
    {
        [Identity(Column = "film_id")]
        public int Id { get; set; }

        [DataMember(Column = "titel")]
        public string Title { get; set; }

        [DataMember(Column = "duur")]
        public int Length { get; set; }

        [DataMember(Column = "release")]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime Release { get; set; }

        [DataMember(Column = "gesproken_taal")]
        public string Language { get; set; }

        [DataMember(Column = "ondertitel_taal")]
        public string Subtitles { get; set; }

        [DataMember(Column = "genre_id", Type = DataType.Entity)]
        public Genre Genre { get; set; }
    }
}