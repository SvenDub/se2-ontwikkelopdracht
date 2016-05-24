using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "AUTEUR")]
    public class Author
    {
        [Identity(Column = "AUTEUR_ID")]
        public int Id { get; set; }

        [DataMember(Column = "EMAIL")]
        public string Email { get; set; }

        [DataMember(Column = "NAAM")]
        public string Name { get; set; }

        [DataMember(Column = "WACHTWOORD")]
        public string Password { get; set; }
    }
}