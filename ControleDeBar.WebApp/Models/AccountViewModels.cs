using ControleDeBar.Dominio.WaiterModule;
using ControleDeBar.Dominio.TableModule;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ControleDeBar.Dominio.AccountModule;
using ControleDeBar.WebApp.Extensions;
using ControleDeBar.Dominio.ProductModule;

namespace ControleDeBar.WebApp.Models;

public class OpenAccountViewModel
{
    [Required(ErrorMessage = "The field 'Owner' is required.")]
    [MinLength(3, ErrorMessage = "The field 'Owner' must contain at least 3 characters.")]
    [MaxLength(100, ErrorMessage = "The field 'Owner' must contain at most 100 characters.")]
    public string Owner { get; set; }

    [Required(ErrorMessage = "The field 'Table' is required.")]
    public Guid TableId { get; set; }
    public List<SelectListItem> AvailableTables { get; set; }

    [Required(ErrorMessage = "The field 'Waiter' is required.")]
    public Guid WaiterId { get; set; }
    public List<SelectListItem> AvailableWaiters { get; set; }

    public OpenAccountViewModel()
    {
        AvailableTables = new List<SelectListItem>();
        AvailableWaiters = new List<SelectListItem>();
    }

    public OpenAccountViewModel(List<Table> tables, List<Waiter> waiters) : this()
    {
        foreach (var m in tables)
        {
            var available = new SelectListItem(m.Number.ToString(), m.Id.ToString());

            AvailableTables.Add(available);
        }

        foreach (var g in waiters)
        {
            var nameOption = new SelectListItem(g.Name.ToString(), g.Id.ToString());

            AvailableWaiters.Add(nameOption);
        }
    }
}

public class CloseAccountViewModel
{
    public Guid Id { get; set; }
    public string Owner { get; set; }
    public int Table { get; set; }
    public string Waiter { get; set; }
    public decimal TotalValue { get; set; }

    public CloseAccountViewModel(Guid id, string owner, int table, string waiter, decimal totalValue)
    {
        Id = id;
        Owner = owner;
        Table = table;
        Waiter = waiter;
        TotalValue = totalValue;
    }
}

public class AccountListViewModel
{
    public List<AccountDetailsViewModel> Records { get; set; }

    public AccountListViewModel(List<Account> accounts)
    {
        Records = new List<AccountDetailsViewModel>();

        foreach (var g in accounts)
            Records.Add(g.ToDetailsVM());
    }
}

public class AccountDetailsViewModel
{
    public Guid Id { get; set; }
    public string Owner { get; set; }
    public int Table { get; set; }
    public string Waiter { get; set; }
    public bool IsOpen { get; set; }
    public decimal TotalValue { get; set; }
    public List<AccountOrderViewModel> Orders { get; set; }

    public AccountDetailsViewModel(
        Guid id,
        string owner,
        int table,
        string waiter,
        bool isOpen,
        decimal totalValue,
        List<Order> orders
    )
    {
        Id = id;
        Owner = owner;
        Table = table;
        Waiter = waiter;
        IsOpen = isOpen;
        TotalValue = totalValue;

        Orders = new List<AccountOrderViewModel>();

        foreach (var item in orders)
        {
            var orderVM = new AccountOrderViewModel(
                item.Id,
                item.Product.Name,
                item.Quantity,
                item.CalculatePartialTotal()
            );

            Orders.Add(orderVM);
        }
    }
}

public class AccountOrderViewModel
{
    public Guid Id { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public decimal PartialTotal { get; set; }

    public AccountOrderViewModel(Guid id, string product, int quantity, decimal partialTotal)
    {
        Id = id;
        Product = product;
        Quantity = quantity;
        PartialTotal = partialTotal;
    }
}

public class ManageOrdersViewModel
{
    public AccountDetailsViewModel Account { get; set; }
    public List<SelectListItem> Products { get; set; }

    public ManageOrdersViewModel() { }

    public ManageOrdersViewModel(Account account, List<Product> products) : this()
    {
        Account = account.ToDetailsVM();

        Products = new List<SelectListItem>();

        foreach (var p in products)
        {
            var selectItem = new SelectListItem(p.Name, p.Id.ToString());

            Products.Add(selectItem);
        }
    }
}

public class AddOrderViewModel
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class BillingViewModel
{
    public List<AccountDetailsViewModel> Records { get; set; }
    public decimal Total { get; set; }

    public BillingViewModel(List<Account> accounts)
    {
        Records = new List<AccountDetailsViewModel>();

        foreach (var c in accounts)
        {
            Total += c.CalculateTotalValue();

            Records.Add(c.ToDetailsVM());
        }
    }
}
