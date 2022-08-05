
using BankApp.Helpers;
using BankApp.Models;
using BankApp.Models.DTOs;
using BankApp.Services;
using BankApp.Store;
using BankApp.UI;
using BankApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace BankApp.Core
{
    public class DataManager
    {
        readonly ICustomerService customerService;
        public DataManager(GlobalConfig globalConfig )
        {
            globalConfig.Instantiate();
            customerService = globalConfig.CustomerService;
        }
        public void Processor(string command)
        {
            bool response = false;  
            switch (command)
            {
                case "login":
                    response = Login();
                    break;
                case "createaccount":
                    response = CreateAccount();
                    break;
                case "logout":
                    response = LogOut();
                    break;
                case "deposit":
                    response = Deposit();
                    break;
                case "checkbalance":
                    response = CheckBalance();
                    break;
                case "checkaccountdetails":
                    response = CheckAccountDetails();
                    break;
                case "printstatementofaccount":
                    response = PrintStatementOfAccount();
                    break;
                case "withdraw":
                    response = Withdraw();
                    break;
                case "transfer":
                    response = Transfer();
                    break;
                case "switchaccount":
                    response = SwitchAccount();
                    break;
                case "addanotheraccount":
                    response = AddAnotherAccount();
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
            if(response) Processor(AccountMenu());
            Processor(GeneralMenu());

        }
        bool CheckAccountDetails()
        {
            Display.PrintAccountDetails(GetCurrentCustomer());
            return true;
        }
        bool PrintStatementOfAccount()
        {
            List<Transaction> transactions = new();
            string accountNumber = string.Empty;
            Customer currentCustomer = GetCurrentCustomer();
            foreach (var account in currentCustomer.Account)
            {
                if (account.IsActive)
                {
                    accountNumber = account.AccountNumber;
                    foreach (var transaction in account.AccountStatement)
                    {
                        transactions.Add(transaction);
                    }
                }
            }
            Display.PrintStatementOfAccount(transactions, accountNumber);
            return true;// Processor(AccountMenu());
        }
        static Customer GetCurrentCustomer()
        {
            foreach (var customer in DbStore.Customers)
            {
                if (customer.IsLoggedIn) return customer;
            }
            return new();
        }
        bool AddAnotherAccount()
        {
            Waiting();
            var currentAccount = GetActiveAccount();
            currentAccount.IsActive = false;
            customerService.AddAnotherAccount(GetNewAccount());
            Notify("Congrats, account was created successfully", true);
            WelcomeWithDetails();
            return true; // Processor(AccountMenu());
        }
        bool CreateAccount()
        {
            Waiting();
            string fname;
            string lname;
            CustomerDTO customer = new();
            string email;
            string password;
            static string EditName(string name)
            {
                name = name.ToLower();
                if (name.Length < 3)
                {
                    Notify("Invalid, your name must be atleast 3 letter", false);
                    return "";
                }
                try
                {
                    for (int i = 0; i < name.Length; i++)
                    {
                        if (!char.IsLetter(name[i]))
                        {
                            Notify("Invalid, please enter only letters", false);
                            return "";
                        }
                    }
                    name = name[..1].ToUpper() + name[1..];
                }
                catch
                {

                }
                return name;
            }

            bool correct;
            do
            {
                fname = GetInput<string>("Enter your first name");
                fname = EditName(fname);
                correct = !string.IsNullOrWhiteSpace(fname);
            } while (!correct);
            do
            {
                lname = GetInput<string>("Enter your last name");
                lname = EditName(lname);
                correct = !string.IsNullOrWhiteSpace(lname);
            } while (!correct);
            do
            {
                email = GetInput<string>("Enter your email address");

                correct = Validation.ValidateEmail(email);
                if (!correct) Notify("Invalid email address", false);
                else
                {
                    if (CheckIfEmailExists(email))
                    {
                        Notify("email already used by another customer, you may try another email or login to add account", false);
                        correct = false;
                    }
                }
            } while (!correct);
            do
            {
                WriteLine("N.B - Password should be minimum 6 characters that include alphanumeric and at least one special characters (@$!%*#?&)");
                password = GetInput<string>("Enter your password");
                correct = Validation.ValidatePassword(password);
                if (!correct) Notify("Invalid  password", false);
            } while (!correct);

            customer.Name = $"{fname} {lname}";
            customer.Email = email;
            customer.Password = password;
            WriteLine("Choose your account type");
            AccountDTO accountDTO = GetNewAccount();
            customer.Balance = accountDTO.Balance;
            customer.AccountType = accountDTO.AccountType;
            Customer newCustomer = customerService.CreateAccount(customer);
            DbStore.Customers.Add(newCustomer);
            Notify("Congrats, account was created successfully", true);
            WelcomeWithDetails();
            return true; // Processor(AccountMenu());
        }
        static bool CheckIfEmailExists(string email)
        {
            foreach (var customer in DbStore.Customers)
            {
                if (customer.Email == email) return true;
            }
            return false;
        }
        static bool ConfirmPassword(string password)
        {
            foreach (var customer in DbStore.Customers)
            {
                if (customer.Password == password)
                {
                    //customer.IsLoggedIn = true;
                    return true;
                }
            }
            return false;
        }
        static void WelcomeWithDetails()
        {
            var activeAccount = GetActiveAccount();
            foreach (var customers in DbStore.Customers)
            {
                foreach (var account in customers.Account)
                {
                    if (account.AccountNumber == activeAccount.AccountNumber)
                    {
                        Notify($"Welcome, {customers.AccountName} - ({account.AccountNumber}) - {account.AccountType}.", true);
                    }
                }
            }
        }
        static AccountDTO GetNewAccount()
        {
            AccountDTO accountDto = new();
            List<string> options = Enum.GetNames(typeof(AccountType)).ToList();
            string type = GetMenu(options);
            accountDto.AccountType = Enum.Parse<AccountType>(type, true);
            do
            {
                accountDto.Balance = GetInput<decimal>("Enter amount to deposit, minimum of 1000");
            } while (accountDto.Balance < 1000);
            return accountDto;
        }
        bool LogOut()
        {
            Waiting();
            var account = GetActiveAccount();
            customerService.LogOut(account);
            Clear();
            Notify("Logout successful, thank you for banking with us", true);
            return false; // Processor(GeneralMenu());
        }
        bool Login()
        {
            Waiting();
            bool correct;
            string email;
            string password;
            do
            {
                email = GetInput<string>("Enter your email address");

                correct = Validation.ValidateEmail(email);
                if (!correct) Notify("Invalid email address", false);
                else
                {
                    if (!CheckIfEmailExists(email))
                    {
                        Notify("email address does not exist", false);
                        correct = false;
                    }
                }
                bool response = true;
                if (!correct) response = SayYes("Would you like to try try again? yes / no");
                if (response) continue;
                else return false; // Processor(GeneralMenu());
            } while (!correct);
            do
            {
                password = GetInput<string>("Enter your password");

                correct = Validation.ValidatePassword(password);
                if (!correct) Notify("Incorrect password", false);
                else
                {
                    if (!ConfirmPassword(password))
                    {
                        Notify("Incorrect password", false);
                        correct = false;
                    }
                }
                bool retry = false;
                if (!correct) retry = SayYes("Would you like to try again? yes / no");
                if (!retry && !correct) return false; // Processor(GeneralMenu());
            } while (!correct);

            correct = customerService.Login(email, password);
            if (correct)
            {
                Notify("Login Successful", true);

                var activeAccount = GetActiveAccount();
                foreach (var customers in DbStore.Customers)
                {
                    foreach (var account in customers.Account)
                    {
                        if (account.AccountNumber == activeAccount.AccountNumber)
                        {
                            Notify($"Welcome {customers.AccountName}, ({account.AccountNumber}) - {account.AccountType}", true);
                        }
                    }
                }

                return true; // Processor(AccountMenu());
            }
            return false;
        }
        bool SwitchAccount()
        {
            Waiting();
            Account newAccount = GetParticularAccount(false);
            if (newAccount.AccountNumber != null)
            {
                foreach (var customers in DbStore.Customers)
                {
                    foreach (var account in customers.Account)
                    {
                        if (account.AccountNumber == newAccount.AccountNumber)
                        {
                            var activeAccount = GetActiveAccount();
                            activeAccount.IsActive = false;
                            account.IsActive = true;
                            Notify($"Welcome, {customers.AccountName} - ({account.AccountNumber}) - {account.AccountType}", true);
                        }
                    }
                }
            }
            else
            {
                Notify("No other account", false);

            }
            return true; // Processor(AccountMenu());
        }
        static Account GetParticularAccount(bool isTransfer)
        {
            List<string> menus = new();
            Account theAccount = new();
            Account currentAccount = GetActiveAccount();
            foreach (var customer in DbStore.Customers)
            {
                if (isTransfer)
                {
                    foreach (var account in customer.Account)
                    {
                        if (account.AccountNumber != currentAccount.AccountNumber)
                            menus.Add(account.AccountNumber);
                    }
                }
                else
                {
                    if (customer.IsLoggedIn)
                    {
                        foreach (var account in customer.Account)
                        {
                            if (account.AccountNumber != currentAccount.AccountNumber)
                                menus.Add(account.AccountNumber);
                        }
                    }
                }
            }
            if (menus.Count > 0)
            {
                string newAccount = GetMenu(menus);
                foreach (var customers in DbStore.Customers)
                {
                    foreach (var account in customers.Account)
                    {
                        if (account.AccountNumber == newAccount)
                        {
                            theAccount = account;
                        }
                    }
                }
            }
            return theAccount;
        }
        bool Withdraw()
        {
            Waiting();
            bool response = customerService.Withdraw(GetActiveAccount(),
                GetInput<decimal>("Enter the amount you like to withdraw"));
            if (!response)
            {
                Notify("Insufficient fund", false);
            }
            else
            {
                Notify("Successful, thank you for banking with us", true);
                Notify($"Your new balance is N{customerService.CheckBalance(GetActiveAccount())}", true);
            }
            return true; // Processor(AccountMenu());
        }
        bool Transfer()
        {

            Account beneficiaryAccount = GetParticularAccount(true);
            if (beneficiaryAccount.AccountNumber != null)
            {
                Account debitorAccount = GetActiveAccount();
                decimal amount = GetInput<decimal>("Enter the amount to transfer");
                bool response = customerService.Transfer(debitorAccount, beneficiaryAccount, amount);
                if (response) Notify($"Tranfer successful, your new balance is N{debitorAccount.AccountBalance}", true);
                else Notify("Insufficient fund", false);
            }
            else Notify("Your beneficiary list is empty, try add another account or create a new account", false);
            return true; // Processor(AccountMenu());
        }
        bool Deposit()
        {
            Waiting();
            decimal amount = GetInput<decimal>("How much would you like to deposit");

            bool response = customerService.Deposit(GetActiveAccount(), amount);
            if (response)
            {
                Notify($"Your new balance is N{customerService.CheckBalance(GetActiveAccount())}", true);
                Notify("Successful, thank you for banking with us", true);
            }
            else Notify("Oops, transaction failed", false);
            return true; // Processor(AccountMenu());
        }
        bool CheckBalance()
        {
            Waiting();
            var res = customerService.CheckBalance(GetActiveAccount());
            Notify($"Your balance is : N{res}", true);
            return true; // Processor(AccountMenu());
        }
        public static dynamic GetInput<T>(string description)
        {
            dynamic response = "";

            bool fail;
            do
            {
                try
                {
                    do
                    {
                        fail = false;
                        Write(description + " : ");
                        string res = ReadLine().Trim();
                        response = (T)Convert.ChangeType(res, typeof(T));

                        if (typeof(T) == typeof(string))
                        {
                            if (string.IsNullOrWhiteSpace(res)) fail = true;
                        }
                        if (typeof(T) == typeof(double))
                        {
                            fail = !double.TryParse(res, out _);
                        }
                        if (typeof(T) == typeof(bool))
                        {

                        }
                        if (fail) Notify("Invalid input, please try again", false);
                    } while (fail);
                }
                catch
                {
                    fail = true;
                    Notify("Invalid input, please try again", false);

                }
            } while (fail);
            return response;
        }
        public static void Notify(string note, bool success)
        {
            ForegroundColor = ConsoleColor.Green;
            if (!success) ForegroundColor = ConsoleColor.Red;
            WriteLine(note);
            ResetColor();
        }
        static Account GetActiveAccount()
        {
            Customer currentCustomer = GetCurrentCustomer();
            foreach (var account in currentCustomer.Account)
            {
                if (account.IsActive) return account;
            }
            return new();
        }
        public static bool SayYes(string description)
        {
            BackgroundColor = ConsoleColor.Yellow;
            ForegroundColor = ConsoleColor.Black;
            Write(description + ": ");
            ResetColor();
            string response = ReadLine();
            if (!string.IsNullOrWhiteSpace(response))
            {
                response = response.Trim().ToLower();
                if (response == "y" || response == "ye" || response == "yes") return true;
            }
            return false;
        }
        static string GetMenu(List<string> menus)
        {
            bool valid;
            string response = string.Empty;
            int max = 0;
            do
            {
                for (int i = 0; i < menus.Count; i++)
                {
                    int j = i + 1;
                    WriteLine($"{j}: {menus[i]}");
                    if (max < j) max = j;
                }
                int res = GetInput<int>("Select from the above options to continue e.g 1 ");
                valid = res > 0 && res <= max;
                if (!valid)
                {
                    Notify("Invalid input, try again", false);
                    valid = false;
                }
                else
                {
                    response = menus[res - 1].ToLower().Replace(" ", string.Empty);
                }
            }
            while (!valid || string.IsNullOrWhiteSpace(response));
            return response;
        }
        public static string GeneralMenu()
        {
            return GetMenu(Menu.GeneralMenu);

        }
        public static string AccountMenu()
        {
            return GetMenu(Menu.AccountMenu);
        }
        void Waiting()
        {
            ForegroundColor = ConsoleColor.DarkBlue;
            WriteLine("Processing, please wait...");
            ResetColor();
        }
    }
}
