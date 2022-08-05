using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Models
{
    public class Account
    {
        public decimal MinimumBalance { get; set; }
        public string AccountNumber { get; set; }   
        public AccountType AccountType { get; set; }
        public decimal AccountBalance { get; set; } 
        public bool IsActive { get; set; }
        public List<Transaction> AccountStatement =new();

    }
}
