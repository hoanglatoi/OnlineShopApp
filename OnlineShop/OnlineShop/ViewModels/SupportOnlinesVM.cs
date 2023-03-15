using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OnlineShop.ViewModels
{
    public class SupportOnlineVM
    {
        public int? ID { set; get; }

        [Required]
        [MaxLength(50)]
        public string? Name { set; get; }

        [MaxLength(50)]
        public string? Department { set; get; }

        [MaxLength(50)]
        public string? Skype { set; get; }

        [MaxLength(50)]
        public string? Mobile { set; get; }

        [MaxLength(50)]
        public string? Email { set; get; }

        [MaxLength(50)]
        public string? Yahoo { set; get; }

        [MaxLength(50)]
        public string? Facebook { set; get; }

        public bool? Status { set; get; }

        public int? DisplayOrder { set; get; }
    }
}
