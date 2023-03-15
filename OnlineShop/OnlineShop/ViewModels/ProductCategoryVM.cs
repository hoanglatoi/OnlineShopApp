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
    public class ProductCategoryVM
    {
        public long? ID { get; set; }

        public string? Name { get; set; }

        public string? MetaTitle { get; set; }

        public long? ParentID { get; set; }

        public int? DisplayOrder { get; set; }

        public string? SeoTitle { get; set; }

        public bool? ShowOnHome { get; set; }

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
