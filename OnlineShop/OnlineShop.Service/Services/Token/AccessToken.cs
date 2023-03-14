using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using OnlineShop.Model.Models;

namespace OnlineShop.Service.Services.Token
{
    public class AccessToken
    {
        [Required]
        [RegularExpression(@"^[^<>""`&]*$", ErrorMessage = "UserName characters are not allowed."), StringLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[^<>""`&]*$", ErrorMessage = "UserID characters are not allowed."), StringLength(100)]
        public string UserID { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[^<>""`&]*$", ErrorMessage = "UserType characters are not allowed."), StringLength(100)]
        public string RoleID { get; set; } =  string.Empty;

        [Required]
        [RegularExpression(@"^[^<>""`&]*$", ErrorMessage = "UserType characters are not allowed."), StringLength(100)]
        public string RoleName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[^<>""`&]*$", ErrorMessage = "GroupName characters are not allowed."), StringLength(100)]
        public string GroupName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "GroupID characters are not allowed.")]
        public int GroupID { get; set; }

        [RegularExpression(@"^[^<>""`&]*$", ErrorMessage = "DisplayName characters are not allowed."), StringLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        public AccessToken()
        {

        }

        public AccessToken(ApplicationUser user, ApplicationRole role, ApplicationGroup group)
        {
            UserName = user.UserName;
            UserID = user.Id;
            RoleID = role.Id;
            RoleName = role.Name;
            GroupName = group!.Name!;
            GroupID = group.ID;
            DisplayName = user.FullName!;
        }
    }
}
