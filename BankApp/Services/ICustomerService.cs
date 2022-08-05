using BankApp.Models;
using BankApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Services
{
    public interface ICustomerService
    {
        decimal CheckBalance(Account account);
        Customer CreateAccount(CustomerDTO customer);
        void LogOut(Account account);
        bool Login(string email, string password);
        bool Withdraw(Account account, decimal amount);
        void AddAnotherAccount(AccountDTO account);
        bool Transfer(Account from, Account to, decimal amount);
        public bool Deposit(Account account, decimal amount);
    }
}
