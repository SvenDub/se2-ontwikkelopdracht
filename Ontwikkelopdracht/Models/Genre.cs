using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "genre")]
    public class Genre
    {
        [Identity(Column = "genre_id")]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}