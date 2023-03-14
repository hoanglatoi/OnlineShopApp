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

    [Table("ProductCategory")]
    public partial class ProductCategory : Auditable
    {
        public long ID { get; set; }

        [StringLength(250)]
        public string? Name { get; set; }

        [StringLength(250)]
        public string? MetaTitle { get; set; }

        public long? ParentID { get; set; }

        public int? DisplayOrder { get; set; }

        [StringLength(250)]
        public string? SeoTitle { get; set; }

        public bool? ShowOnHome { get; set; }
    }
}
