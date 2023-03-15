using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class ApplicationUserVM
    {
        public string? Id { set; get; }
        public string? FullName { set; get; }
        public DateTime BirthDay { set; get; }
        public string? Bio { set; get; }
        public string? Email { set; get; }
        public string? Password { set; get; }
        public string? UserName { set; get; }

        public string? PhoneNumber { set; get; }

        public IEnumerable<ApplicationGroupVM>? Groups { set; get; }
        //public string RefreshToken { get; set; } = string.Empty;
        //public DateTime RefreshTokenCreated { get; set; }
        //public DateTime RefreshTokenExpires { get; set; }
    }
}
