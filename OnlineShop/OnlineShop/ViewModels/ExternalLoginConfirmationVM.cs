using System.ComponentModel.DataAnnotations;

namespace OnlineShop.ViewModels
{
    public class ExternalLoginListVM
    {
        public string? ReturnUrl { get; set; }
    }
    public class ExternalLoginConfirmationVM
    {
        [Required]
        [Display(Name = "Email")]
        public string? Email { get; set; }
    }
}
