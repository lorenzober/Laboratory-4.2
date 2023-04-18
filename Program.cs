using System;
using System.Collections.Generic;
using System.Linq;

public class Client
{
    public string Name { get; set; }
    public int ClientId { get; set; }

    public Client(string name, int clientId)
    {
        Name = name;
        ClientId = clientId;
    }
}
public class Account
{
    public int AccountNumber { get; set; }
    public Client Owner { get; set; }
    public decimal Balance { get; private set; }
    public bool IsBlocked { get; private set; }

    public Account(int accountNumber, Client owner, decimal initialBalance = 0, bool isBlocked = false)
    {
        AccountNumber = accountNumber;
        Owner = owner;
        Balance = initialBalance;
        IsBlocked = isBlocked;
    }

    public void Block() => IsBlocked = true;
    public void Unblock() => IsBlocked = false;

    public bool Deposit(decimal amount)
    {
        if (IsBlocked || amount <= 0) return false;
        Balance += amount;
        return true;
    }

    public bool Withdraw(decimal amount)
    {
        if (IsBlocked || amount <= 0 || amount > Balance) return false;
        Balance -= amount;
        return true;
    }
}


public class Bank
{
    private List<Account> Accounts { get; set; }

    public Bank()
    {
        Accounts = new List<Account>();
    }

    public void AddAccount(Account account)
    {
        Accounts.Add(account);
    }

    public Account FindAccount(int accountNumber)
    {
        return Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
    }

    public IEnumerable<Account> GetSortedAccounts()
    {
        return Accounts.OrderBy(a => a.AccountNumber);
    }

    public decimal GetTotalBalance()
    {
        return Accounts.Sum(a => a.Balance);
    }

    public decimal GetTotalPositiveBalance()
    {
        return Accounts.Where(a => a.Balance > 0).Sum(a => a.Balance);
    }

    public decimal GetTotalNegativeBalance()
    {
        return Accounts.Where(a => a.Balance < 0).Sum(a => a.Balance);
    }
}

class Program
{
    static void Main()
    {
        Bank bank = new Bank();

        Client client1 = new Client("Lorenzo Beraudo", 1);
        Account account1 = new Account(101, client1, 1000);
        bank.AddAccount(account1);

        Client client2 = new Client("Hello Kitty", 2);
        Account account2 = new Account(102, client2, -500);
        bank.AddAccount(account2);

        Account foundAccount = bank.FindAccount(101);
        Console.WriteLine($"Account found: {foundAccount.AccountNumber}, owner: {foundAccount.Owner.Name}");

        Console.WriteLine("Sorted accounts:");
        foreach (Account account in bank.GetSortedAccounts())
        {
            Console.WriteLine($"Account: {account.AccountNumber}, owner: {account.Owner.Name}, balance: {account.Balance}");
        }

        Console.WriteLine($"Total balance: {bank.GetTotalBalance()}");
        Console.WriteLine($"Total positive balance: {bank.GetTotalPositiveBalance()}");
        Console.WriteLine($"Total negative balance: {bank.GetTotalNegativeBalance()}");

        Console.ReadLine();
    }
}