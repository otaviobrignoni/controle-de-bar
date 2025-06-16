using ControleDeBar.Dominio.WaiterModule;
using ControleDeBar.WebApp.Models;

namespace ControleDeBar.WebApp.Extensions;

public static class WaiterExtensions
{
    public static Waiter ToEntity(this WaiterFormViewModel form)
    {
        return new Waiter(form.Name, form.Cpf);
    }

    public static WaiterDetailsViewModel ToDetailsVM(this Waiter waiter)
    {
        return new WaiterDetailsViewModel(
                waiter.Id,
                waiter.Name,
                waiter.Cpf
        );
    }
}
