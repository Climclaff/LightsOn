using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
#pragma warning disable CS8618 
namespace LightOn.Models.ClientModels
{
    public class ProfileModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^(([A-za-zА-ЩЬЮЯҐЄІЇа-щьюяґєії]+[\s]{1}[A-za-zА-ЩЬЮЯҐЄІЇа-щьюяґєії]+)|([A-Za-zА-ЩЬЮЯҐЄІЇа-щьюяґєії]+))$",
               ErrorMessage = "The field must contain alphabetical characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^(([A-za-zА-ЩЬЮЯҐЄІЇа-щьюяґєії]+[\s]{1}[A-za-zА-ЩЬЮЯҐЄІЇа-щьюяґєії]+)|([A-Za-zА-ЩЬЮЯҐЄІЇа-щьюяґєії]+))$",
               ErrorMessage = "The field must contain alphabetical characters")]
        [Display(Name = "Last Name")]

        public string LastName { get; set; }

        [Display(Name = "Image")]
        public byte[] ImgData { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Region id")]
        public int? regionId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "District id")]
        public int? districtId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Town id")]
        public int? townId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Street id")]
        public int? streetId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Building id")]
        public int? buildingId { get; set; }
    }
}
