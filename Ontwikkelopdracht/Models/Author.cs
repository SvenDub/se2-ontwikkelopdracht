using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "auteur")]
    public class Author
    {
        [Identity(Column = "email")]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}