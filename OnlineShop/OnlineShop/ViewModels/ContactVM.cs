using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class ContactVM
    {
        public int? ID { get; set; }

        public string? Content { get; set; }

        public bool? Status { get; set; }
    }
}
