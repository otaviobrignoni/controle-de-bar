using ControleDeBar.Dominio.TableModule;
using ControleDeBar.WebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace ControleDeBar.WebApp.Models;

public abstract class TableFormViewModel
{
    [Required(ErrorMessage = "The field 'Number' is required.")]
    [Range(1, 100, ErrorMessage = "The field 'Number' must be between 1 and 100.")]
    public int Number { get; set; }

    [Required(ErrorMessage = "The field 'Capacity' is required.")]
    [Range(1, 100, ErrorMessage = "The field 'Capacity' must be between 1 and 100.")]
    public int Capacity { get; set; }
}

public class CreateTableViewModel : TableFormViewModel
{
    public CreateTableViewModel() { }

    public CreateTableViewModel(int number, int capacity) : this()
    {
        Number = number;
        Capacity = capacity;
    }
}

public class EditTableViewModel : TableFormViewModel
{
    public Guid Id { get; set; }

    public EditTableViewModel() { }

    public EditTableViewModel(Guid id, int number, int capacity) : this()
    {
        Id = id;
        Number = number;
        Capacity = capacity;
    }
}

public class DeleteTableViewModel
{
    public Guid Id { get; set; }
    public int Number { get; set; }

    public DeleteTableViewModel() { }

    public DeleteTableViewModel(Guid id, int number) : this()
    {
        Id = id;
        Number = number;
    }
}

public class TableListViewModel
{
    public List<TableDetailsViewModel> Records { get; }

    public TableListViewModel(List<Table> tables)
    {
        Records = [];

        foreach (var t in tables)
        {
            var detailsVM = t.ToDetailsVM();

            Records.Add(detailsVM);
        }
    }
}

public class TableDetailsViewModel
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public int Capacity { get; set; }

    public TableDetailsViewModel(Guid id, int number, int capacity)
    {
        Id = id;
        Number = number;
        Capacity = capacity;
    }
}

public class SelectTableViewModel
{
    public Guid Id { get; set; }
    public int Number { get; set; }

    public SelectTableViewModel(Guid id, int number)
    {
        Id = id;
        Number = number;
    }
}
