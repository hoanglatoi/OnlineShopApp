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
    public class PostContentVM
    {
        public long? ID { get; set; }

        [MaxLength(250)]
        public string? Name { get; set; }

        [MaxLength(250)]
        public string? MetaTitle { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(250)]
        public string? Image { get; set; }

        public long? CategoryID { get; set; }

        public string? Detail { get; set; }

        public int? Warranty { get; set; }

        public DateTime? TopHot { get; set; }

        public int? ViewCount { get; set; }

        [StringLength(500)]
        public string? Tags { get; set; }

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
