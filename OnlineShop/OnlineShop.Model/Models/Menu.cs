using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Model.Models
{
    [Table("Menu")]
    public partial class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string? Text { get; set; }

        [StringLength(250)]
        public string? Link { get; set; }

        public int? DisplayOrder { get; set; }

        [StringLength(50)]
        public string? Target { get; set; }

        public bool? Status { get; set; }

        public int? TypeID { get; set; }
    }
}
