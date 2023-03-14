using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Model.Models
{
    [Table("PostTag")]
    public partial class PostTag
    {
        // ForeignKey
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ContentID { get; set; }

        // ForeignKey
        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string? TagID { get; set; }
    }
}
