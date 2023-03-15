using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class ApplicationGroupVM
    {
        public int? ID { set; get; }

        public string? Name { set; get; }

        public string? Description { set; get; }
    }
}
