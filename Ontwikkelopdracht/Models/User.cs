using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "GEBRUIKER")]
    public class User
    {
        [Identity(Column = "USER_ID")]
        public int Id { get; set; }

        [DataMember(Column = "EMAIL")]
        public string Email { get; set; }

        [DataMember(Column = "NAAM")]
        public string Name { get; set; }

        [DataMember(Column = "WACHTWOORD")]
        public string Password { get; set; }

        [DataMember(Column = "ADMIN")]
        public bool Admin { get; set; }
    }
}