using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "GENRE")]
    public class Genre
    {
        [Identity(Column = "GENRE_ID")]
        public int Id { get; set; }

        [DataMember(Column = "NAAM")]
        public string Name { get; set; }
    }
}