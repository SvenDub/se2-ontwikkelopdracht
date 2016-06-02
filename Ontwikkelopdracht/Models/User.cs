using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ontwikkelopdracht.Persistence;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "GEBRUIKER")]
    public class User
    {
        [Identity(Column = "USER_ID")]
        public int Id { get; set; }

        [DataMember(Column = "EMAIL")]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataMember(Column = "NAAM")]
        [DisplayName("Naam")]
        public string Name { get; set; }

        [DataMember(Column = "WACHTWOORD")]
        [DisplayName("Wachtwoord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataMember(Column = "ADMIN")]
        [DisplayName("Admin")]
        public bool Admin { get; set; }
    }
}