using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "auteur")]
    public class Author
    {
        [Identity(Column = "auteur_id")]
        public int Id { get; set; }

        [DataMember(Column = "email")]
        public string Email { get; set; }

        [DataMember(Column = "naam")]
        public string Name { get; set; }

        [DataMember(Column = "wachtwoord")]
        public string Password { get; set; }
    }
}