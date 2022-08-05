using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Models
{
    public class Transaction
    {
        public string Description { get; set; } 
        public DateTime Date { get; set; }
        public decimal Amount { get; set; } 
        public decimal Balance { get; set; } 

    }
}
