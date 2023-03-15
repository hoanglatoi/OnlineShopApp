using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class OrderDetailVM
    {
        public long ProductID { get; set; }

        public long OrderID { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
    }
}
