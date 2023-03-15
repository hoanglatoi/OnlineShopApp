using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class AboutVM
    {
        public long? ID { get; set; }

        [MaxLength(256)]
        public string? Name { get; set; }

        [MaxLength(256)]
        public string? MetaTitle { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public string? Detail { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescriptions { get; set; }

        public bool? Status { get; set; }
    }
}
