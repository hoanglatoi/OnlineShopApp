using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Model.Models
{
    [Table("SystemConfig")]
    public partial class SystemConfig
    {
        [StringLength(50)]
        public string? ID { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? Type { get; set; }

        [StringLength(250)]
        public string? Value { get; set; }

        public bool? Status { get; set; }
    }
}
