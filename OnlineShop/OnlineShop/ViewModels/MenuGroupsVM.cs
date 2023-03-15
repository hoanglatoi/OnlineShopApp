using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class MenuGroupsVM
    {
        public int? ID { set; get; }

        [Required]
        [MaxLength(50)]
        public string? Name { set; get; }

        //public virtual IEnumerable<MenuVM>? Menus { set; get; }
    }
}
