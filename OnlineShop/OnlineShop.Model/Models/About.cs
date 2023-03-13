using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Model.Models
{
    [Table("About")]
    public partial class About
    {
        public long ID { get; set; }

        [StringLength(250)]
        public string? Name { get; set; }

        [StringLength(250)]
        public string? MetaTitle { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(250)]
        public string? Image { get; set; }

        [Column(TypeName = "ntext")]
        public string? Detail { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string? ModifiedBy { get; set; }

        [StringLength(250)]
        public string MetaKeywords { get; set; }

        [StringLength(250)]
        public string? MetaDescriptions { get; set; }

        public bool? Status { get; set; }
    }
}
