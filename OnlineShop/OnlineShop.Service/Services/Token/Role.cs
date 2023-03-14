using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Service.Services.Token
{
    public static class Role
    {
        // Maintainance role
        public static string Admin = "Admin";
        public static string Worker = "Worker";

        // Customer role
        public static string VipMember = "VipMember";
        public static string BasicMember = "BasicMember";
    }
}
