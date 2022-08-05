using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Models.DTOs
{
    public class AccountDTO
    {
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }    
    }
}
