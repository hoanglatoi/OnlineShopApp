using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Model.Models
{
    [Table("Credential")]
    [Serializable]
    public class Credential
    {
        [Key]
        public string? Id { get; set; }
        
        [StringLength(20)]
        public string? UserGroupID { set; get; }

        [StringLength(50)]
        public string? RoleID { set; get; }
    }
}
