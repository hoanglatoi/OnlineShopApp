using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class VisitorStatisticVM
    {
        public Guid ID { set; get; }

        [Required]
        public DateTime? VisitedDate { set; get; }

        [MaxLength(50)]
        public string? IPAddress { set; get; }
    }
}
