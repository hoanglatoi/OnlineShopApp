using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.ViewModels
{
    public class ErrorsVM
    {
        public int? ID { set; get; }

        public string? Message { set; get; }

        public string? StackTrace { set; get; }

        public DateTime? CreatedDate { set; get; }
    }
}
