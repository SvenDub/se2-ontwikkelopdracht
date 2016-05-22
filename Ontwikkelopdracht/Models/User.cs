using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "gebruiker")]
    public class User
    {
        [Identity(Column = "email")]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
    }
}