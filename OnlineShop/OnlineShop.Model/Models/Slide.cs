using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Model.Models
{
    [Table("Slide")]
    public partial class Slide
    {
        public int ID { get; set; }

        [StringLength(250)]
        public string? Image { get; set; }

        public int? DisplayOrder { get; set; }

        [StringLength(250)]
        public string? Link { get; set; }

        [StringLength(50)]
        public string? Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string? ModifiedBy { get; set; }

        public bool? Status { get; set; }
    }
}
