using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineShop.ViewModels
{
    public class ApplicationRoleVM
    {
        public string? Id { set; get; } = String.Empty;
        public string? Name { set; get; } = String.Empty ;
        public string? Description { set; get; } = String.Empty;
    }
}
