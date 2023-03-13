using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Model.Models
{
    [Table("ApplicationRoleGroups")]
    public class ApplicationRoleGroup
    {
        [Key]
        [Column(Order = 1)]
        public int GroupId { set; get; }

        [Column(Order = 2)]
        [StringLength(128)]
        [Key]
        public string? RoleId { set; get; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole? ApplicationRole { set; get; }

        [ForeignKey("GroupId")]
        public virtual ApplicationGroup? ApplicationGroup { set; get; }
    }
}
