using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LightOn.Models.Auth
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} is not valid")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long. Must contain one uppercase, one lower case letter, numbers and special symbols.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^(([A-za-zА-ЩЬЮЯҐЄІЇа-щьюяґєії]+[\s]{1}[A-za-zА-ЩЬЮЯҐЄІЇа-щьюяґєії]+)|([A-Za-zА-ЩЬЮЯҐЄІЇа-щьюяґєії]+))$",
               ErrorMessage = "The field must contain alphabetical characters")]
        [Display(Name = "FirstName")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^(([A-za-zА-ЩЬЮЯҐЄІЇа-щьюяґєії]+[\s]{1}[A-za-zА-ЩЬЮЯҐЄІЇа-щьюяґєії]+)|([A-Za-zА-ЩЬЮЯҐЄІЇа-щьюяґєії]+))$",
               ErrorMessage = "The field must contain alphabetical characters")]
        [Display(Name = "LastName")]
        public string? LastName { get; set; }

    }
}
