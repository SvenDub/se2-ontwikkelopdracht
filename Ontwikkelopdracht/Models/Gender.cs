using System.ComponentModel.DataAnnotations;

namespace Ontwikkelopdracht.Models
{
    public enum Gender
    {
        [Display(Name = "Man")]
        Male = 0,
        [Display(Name = "Vrouw")]
        Female = 1
    }
}