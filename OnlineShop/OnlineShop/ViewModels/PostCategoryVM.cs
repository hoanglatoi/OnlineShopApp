using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineShop.Model.Abstract;

namespace OnlineShop.ViewModels
{
    public class PostCategoryVM
    {
        public long? ID { get; set; }

        [MaxLength(250)]
        [Display(Name = "Category_Name")]
        [Required(ErrorMessageResourceName = "Category_RequiredName")]
        public string? Name { get; set; }

        [MaxLength(250)]
        [Display(Name = "Category_MetaTitle")]
        public string?MetaTitle { get; set; }

        [Display(Name = "Category_ParentId")]
        public long? ParentID { get; set; }

        [Display(Name = "Category_DisplayOrder")]
        public int? DisplayOrder { get; set; }

        [MaxLength(250)]
        [Display(Name = "Category_SeoTitle")]
        public string? SeoTitle { get; set; }

        [Display(Name = "Category_ShowOnHome")]
        public bool? ShowOnHome { get; set; }

        public string? Language { set; get; }

        public DateTime? CreatedDate { set; get; }

        [MaxLength(256)]
        public string? CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        [MaxLength(256)]
        public string? UpdatedBy { set; get; }

        [MaxLength(256)]
        public string? MetaKeyword { set; get; }

        [MaxLength(256)]
        public string? MetaDescription { set; get; }

        public bool? Status { set; get; }
    }
}
