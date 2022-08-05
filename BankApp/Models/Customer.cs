
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Models
{
    public class Customer
    {
        public List<Account> Account = new();
        public string AccountName { get; set; }
        public string Email { get; set; }
        public string Password { get;  set; }
        public bool IsLoggedIn { get; set; }    
    }
}
