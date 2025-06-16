using ControleDeBar.Dominio.TableModule;
using ControleDeBar.WebApp.Models;

namespace ControleDeBar.WebApp.Extensions;

public static class TableExtensions
{
    public static Table ToEntity(this TableFormViewModel form)
    {
        return new Table(form.Number, form.Capacity);
    }

    public static TableDetailsViewModel ToDetailsVM(this Table table)
    {
        return new TableDetailsViewModel(
                table.Id,
                table.Number,
                table.Capacity
        );
    }
}
