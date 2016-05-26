using System.ComponentModel;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "GEBRUIKER")]
    public class User
    {
        [Identity(Column = "USER_ID")]
        public int Id { get; set; }

        [DataMember(Column = "EMAIL")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DataMember(Column = "NAAM")]
        [DisplayName("Naam")]
        public string Name { get; set; }

        [DataMember(Column = "WACHTWOORD")]
        [DisplayName("Wachtwoord")]
        public string Password { get; set; }

        [DataMember(Column = "ADMIN")]
        [DisplayName("Admin")]
        public bool Admin { get; set; }
    }
}