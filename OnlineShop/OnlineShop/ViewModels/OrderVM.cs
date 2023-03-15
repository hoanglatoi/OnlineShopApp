using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class OrderVM
    {
        public long? ID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? CustomerID { get; set; }

        public string? ShipName { get; set; }

        [MaxLength(50)]
        public string? ShipMobile { get; set; }

        public string? ShipAddress { get; set; }

        [MaxLength(50)]
        public string? ShipEmail { get; set; }

        public int? Status { get; set; }
    }
}
