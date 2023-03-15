using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class TagVM
    {
        public string? ID { get; set; }

        public string? Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string? Type { set; get; }
    }
}
