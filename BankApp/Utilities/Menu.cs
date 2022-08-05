using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Helpers
{
    public class Menu
    {
        public static List<string> GeneralMenu = new()
        {
            "Create Account",
            "Login",
            "Exit"
        };
        public static List<string> AccountMenu = new()
        {

            "Deposit",
            "Withdraw",
            "Transfer",
            "Check Balance",
            "Check Account Details",
            "Print Statement of Account",
            "Switch Account",
            "Add Another Account",
            "Logout",
            "Exit"
        };

    }
}
