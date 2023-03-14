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
    [Table("PostCategory")]
    public partial class PostCategory : Auditable
    {
        public long ID { get; set; }

        [StringLength(250)]
        [Display(Name = "Category_Name")]
        [Required(ErrorMessageResourceName = "Category_RequiredName")]
        public string? Name { get; set; }

        [StringLength(250)]
        [Display(Name = "Category_MetaTitle")]
        public string?MetaTitle { get; set; }

        // ForeignKey
        [Display(Name = "Category_ParentId")]
        public long? ParentID { get; set; }

        [Display(Name = "Category_DisplayOrder")]
        public int? DisplayOrder { get; set; }

        [StringLength(250)]
        [Display(Name = "Category_SeoTitle")]
        public string? SeoTitle { get; set; }

        [Display(Name = "Category_ShowOnHome")]
        public bool? ShowOnHome { get; set; }

        public string? Language { set; get; }
    }
}
