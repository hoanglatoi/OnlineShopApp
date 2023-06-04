using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineShop.Areas.Identity.Pages.Account
{
    public class AccountErrorModel : PageModel
    {
        //[BindProperty]
        public string Title { get; set; } = string.Empty;
        //[BindProperty]
        public string ErrorMsg { get; set; } = string.Empty;
        public void OnGet(string title= "", string errormsg = "")
        {
            Title = title;
            foreach(var item in errormsg)
            {
                if(item == '-')
                {
                    ErrorMsg = ErrorMsg + " ";
                }
                else
                {
                    ErrorMsg = ErrorMsg + item;
                }
            }
        }
    }
}

