using ControleDeBar.Dominio.AccountModule;
using ControleDeBar.Dominio.WaiterModule;
using ControleDeBar.Dominio.TableModule;
using ControleDeBar.WebApp.Models;

namespace ControleDeBar.WebApp.Extensions;

public static class AccountExtensions
{
    public static Account ToEntity(this OpenAccountViewModel openVM, List<Table> tables, List<Waiter> waiters)
    {
        Table? selectedTable = null;

        foreach (var table in tables)
        {
            if (table.Id == openVM.TableId)
                selectedTable = table;
        }

        Waiter? selectedWaiter = null;

        foreach (var waiter in waiters)
        {
            if (waiter.Id == openVM.WaiterId)
                selectedWaiter = waiter;
        }

        return new Account(openVM.Owner, selectedTable, selectedWaiter);
    }

    public static AccountDetailsViewModel ToDetailsVM(this Account account)
    {
        return new AccountDetailsViewModel(
                account.Id,
                account.Owner,
                account.Table.Number,
                account.Waiter.Name,
                account.IsOpen,
                account.CalculateTotalValue(),
                account.Orders
        );
    }
}
