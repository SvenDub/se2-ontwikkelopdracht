using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
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
        [EmailAddress(ErrorMessage = "Emailadres is ongeldig")]
        [Required]
        public string Email { get; set; }

        [DataMember(Column = "NAAM")]
        [Required]
        [DisplayName("Naam")]
        public string Name { get; set; }

        [DataMember(Column = "WACHTWOORD")]
        [Required]
        [MembershipPassword(MinRequiredPasswordLength = 8,
             MinPasswordLengthError =
                 "Wachtwoord moet minimaal 8 tekens zijn en een hoofdletter en kleine letter bevatten.",
             MinRequiredNonAlphanumericCharacters = 0)]
        [DisplayName("Wachtwoord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataMember(Column = "ADMIN")]
        [DisplayName("Admin")]
        public bool Admin { get; set; }
    }
}