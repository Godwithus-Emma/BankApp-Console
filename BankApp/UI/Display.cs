using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace BankApp.UI
{
    public class Display
    {
        public static void PrintStatementOfAccount(List<Transaction> transactions, string accountNumber)
        {
            int tableWidth = 90;
            void PrintLine()
            {
                WriteLine(new string('-', tableWidth));
            }
            void PrintRow(params string[] columns)
            {
                int width = (tableWidth - columns.Length) / columns.Length;
                string row = "|";
                foreach (string column in columns)
                {
                    row += AlignCentre(column, width) + "|";
                }
                WriteLine(row);
            }
            string AlignCentre(string text, int width)
            {
                text = text.Length > width ? text[..(width-3)] + "..." : text;

                if (string.IsNullOrEmpty(text))
                {
                    return new string(' ', width);
                }
                else
                {
                    return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
                }
            }
            WriteLine($"ACCOUNT STATEMENT ON ACCOUNT NO {accountNumber}");
            PrintLine();
            PrintRow("DATE", "DESCRIPTION", "AMOUNT", "BALANCE");
            PrintLine();
            foreach (var transaction in transactions)
            {
                string[] statement = new string[4]
               {
                    transaction.Date.ToShortDateString(),
                    transaction.Description,
                    transaction.Amount.ToString(),  
                    transaction.Balance.ToString(), 
               };
                PrintLine();
                PrintRow(statement);
            }
            PrintLine();
        }
        public static void PrintAccountDetails(Customer customer)
        {
            int tableWidth = 90;
            void PrintLine()
            {
                WriteLine(new string('-', tableWidth));
            }
            void PrintRow(params string[] columns)
            {
                int width = (tableWidth - columns.Length) / columns.Length;
                string row = "|";
                foreach (string column in columns)
                {
                    row += AlignCentre(column, width) + "|";
                }
                WriteLine(row);
            }
            string AlignCentre(string text, int width)
            {
                text = text.Length > width ? text[..(width-3)] + "..." : text;

                if (string.IsNullOrEmpty(text))
                {
                    return new string(' ', width);
                }
                else
                {
                    return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
                }
            }
            WriteLine($"ACCOUNT DETAILS");
            PrintLine();
            PrintRow("FULL NAME", "ACCOUNT NUMBER", "ACCOUNT TYPE", "BALANCE");
            PrintLine();
            foreach (var account in customer.Account)
            {
                string[] statement = new string[4]
               {
                    customer.AccountName,
                    account.AccountNumber,
                    account.AccountType.ToString(),
                    account.AccountBalance.ToString(),
               };
                PrintLine();
                PrintRow(statement);
            }
            PrintLine();
        }

    }
}

