using System;
using System.ComponentModel;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "FILM")]
    public class Film
    {
        [Identity(Column = "FILM_ID")]
        public int Id { get; set; }

        [DataMember(Column = "TITEL")]
        [DisplayName("Titel")]
        public string Title { get; set; }

        [DataMember(Column = "DUUR")]
        [DisplayName("Duur")]
        public int Length { get; set; }

        [DataMember(Column = "RELEASE")]
        [DisplayName("Release")]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime Release { get; set; }

        [DataMember(Column = "GESPROKEN_TAAL")]
        [DisplayName("Gesproken taal")]
        public string Language { get; set; }

        [DataMember(Column = "ONDERTITEL_TAAL")]
        [DisplayName("Ondertiteling")]
        public string Subtitles { get; set; }

        [DataMember(Column = "GENRE_ID", Type = DataType.Entity)]
        public Genre Genre { get; set; }
    }
}