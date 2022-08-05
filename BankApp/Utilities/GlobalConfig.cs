
using BankApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Utilities
{
    public class GlobalConfig
    {
        public ICustomerService CustomerService { get; set; } 
        public void Instantiate()
        {
            ICustomerService customerService = new CustomerService();
            CustomerService = customerService;
        }
    }
}
