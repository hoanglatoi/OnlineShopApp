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
    [Table("Product")]
    public partial class Product : Auditable
    {
        public long ID { get; set; }

        [StringLength(250)]
        public string? Name { get; set; }

        [StringLength(10)]
        public string? Code { get; set; }

        [StringLength(250)]
        public string? MetaTitle { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(250)]
        public string? Image { get; set; }

        [Column(TypeName = "xml")]
        public string? MoreImages { get; set; }

        public decimal? Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        public bool? IncludedVAT { get; set; }

        public int Quantity { get; set; }

        public long? CategoryID { get; set; }

        [Column(TypeName = "ntext")]
        public string? Detail { get; set; }

        public int? Warranty { get; set; }

        public DateTime? TopHot { get; set; }

        public int? ViewCount { get; set; }
    }
}
