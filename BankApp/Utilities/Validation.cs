using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankApp.Helpers
{
    public class Validation
    {
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            Regex ok = new(@"^\w+@\w+\.\w+");
            if (ok.IsMatch(email))
                return true;
            return false;
        }
        public static bool ValidatePassword(string password)
        {
            Regex ok = new(@"^(?=.*[@$!%*#?&])(?=.*[0-9])[\w\d@$!%*#?&]{6,}$");
            if (ok.IsMatch(password))
                return true;
            return false;
        }
    }
}
