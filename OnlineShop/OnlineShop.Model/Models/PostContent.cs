using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineShop.Model.Abstract;

namespace OnlineShop.Model.Models
{
    [Table("PostContent")]
    public partial class PostContent : Auditable
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

        // ForeignKey
        public long? CategoryID { get; set; }

        [Column(TypeName = "text")]
        public string? Detail { get; set; }

        public int? Warranty { get; set; }

        public DateTime? TopHot { get; set; }

        public int? ViewCount { get; set; }

        [StringLength(500)]
        public string? Tags { get; set; }

        public string? Language { set; get; }
    }
}
