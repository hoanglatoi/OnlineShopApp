using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Model.Models
{
    [Table("MenuType")]
    public partial class MenuType
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }
    }
}
