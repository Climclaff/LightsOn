using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LightOn.Models.Auth
{
    public class LoginModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }
}
