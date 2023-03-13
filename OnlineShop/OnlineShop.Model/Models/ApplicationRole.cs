﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Model.Models
{
    [Table("Role")]
    public class Role : IdentityRole
    {
        public Role() : base()
        {

        }
        [StringLength(250)]
        public string? Description { set; get; }
    }
}
