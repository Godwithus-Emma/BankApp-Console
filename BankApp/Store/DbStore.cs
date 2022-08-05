using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Store
{
    public class DbStore
    {
        public static List<Customer> Customers { get; set; } = new List<Customer>();
    }
}
