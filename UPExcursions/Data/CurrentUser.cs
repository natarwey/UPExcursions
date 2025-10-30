using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPExcursions.Data;

namespace UPExcursions
{
    internal class CurrentUser
    {
        public static User currentUser;
        //public static Data.User? currentUser { get; set; }

        public static bool IsClient => currentUser?.Role?.RoleName == "client";
        public static bool IsAdmin => currentUser?.Role?.RoleName == "admin";

        public static bool CanViewExcursions => IsClient || IsAdmin;
        public static bool CanBookExcursions => IsClient;
        public static bool CanViewOwnBookings => IsClient;
        public static bool CanViewAndWriteReviews => IsClient;
        public static bool CanManageFavorites => IsClient || IsAdmin;

        public static bool CanViewOrderReport => IsAdmin;
        public static bool CanViewPopularityReport => IsAdmin;
        public static bool CanManageExcursions => IsAdmin;
        public static bool CanViewReviews => IsAdmin;
    }
}
