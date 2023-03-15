using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class MenuVM
    {
        public int? ID { get; set; }

        [MaxLength(50)]
        public string? Text { get; set; }

        [MaxLength(250)]
        public string? Link { get; set; }

        public int? DisplayOrder { get; set; }

        [MaxLength(50)]
        public string? Target { get; set; }

        public bool? Status { get; set; }

        public int? TypeID { get; set; }

        [Required]
        public int? GroupID { set; get; }

        //public virtual MenuGroup MenuGroup { set; get; }
    }
}
