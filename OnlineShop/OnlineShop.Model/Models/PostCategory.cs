using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Model.Models
{
    [Table("PostCategory")]
    public partial class PostCategory
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

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string? ModifiedBy { get; set; }

        [StringLength(250)]
        [Display(Name = "Category_Metakeyword")]
        public string? MetaKeywords { get; set; }

        [StringLength(250)]
        [Display(Name = "Category_MetaDescription")]
        public string? MetaDescriptions { get; set; }

        [Display(Name = "Category_Status")]
        public bool? Status { get; set; }

        [Display(Name = "Category_ShowOnHome")]
        public bool? ShowOnHome { get; set; }

        public string? Language { set; get; }
    }
}
