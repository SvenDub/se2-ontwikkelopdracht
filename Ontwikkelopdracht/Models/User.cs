using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "gebruiker")]
    public class User
    {
        [Identity(Column = "user_id")]
        public int Id { get; set; }

        [DataMember(Column = "email")]
        public string Email { get; set; }

        [DataMember(Column = "naam")]
        public string Name { get; set; }

        [DataMember(Column = "wachtwoord")]
        public string Password { get; set; }

        [DataMember(Column = "admin")]
        public bool Admin { get; set; }
    }
}