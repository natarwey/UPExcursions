using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPExcursions.Data
{
    internal class CurrentUser
    {
        public static User currentUser;

        public static bool IsClient => currentUser?.Role?.RoleName == "client";
        public static bool IsAdmin => currentUser?.Role?.RoleName == "admin";
    }
}
