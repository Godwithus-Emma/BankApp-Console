using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Models.DTOs
{
    public class CustomerDTO
    {
        public string Name { get; set; }    
        public string Email { get; set; }
        public string Password { get; set; }
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
    }
}
