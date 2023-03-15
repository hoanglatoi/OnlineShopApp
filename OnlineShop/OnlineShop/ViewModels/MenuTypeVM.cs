using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class MenuTypeVM
    {
        public int? ID { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }
    }
}
