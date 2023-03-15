using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class SystemConfigVM
    {
        [MaxLength(50)]
        public string? ID { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Type { get; set; }

        [MaxLength(250)]
        public string? Value { get; set; }

        public bool? Status { get; set; }
    }
}
