using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class ProductTagsVM
    {

        public int? ProductID { set; get; }

        public string? TagID { set; get; }
    }
}
