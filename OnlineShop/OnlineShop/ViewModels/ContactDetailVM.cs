using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class ContactDetailVM
    {
        public int? ID { set; get; }

        [Required]
        public string? Name { set; get; }

        [MaxLength(50)]
        public string? Phone { set; get; }

        [MaxLength(250)]
        public string? Email { set; get; }

        [MaxLength(250)]
        public string? Website { set; get; }

        [MaxLength(250)]
        public string? Address { set; get; }

        public string? Other { set; get; }

        public double? Lat { set; get; }

        public double? Lng { set; get; }

        public bool? Status { set; get; }
    }
}
