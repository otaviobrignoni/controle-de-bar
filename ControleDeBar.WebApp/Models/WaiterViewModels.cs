using ControleDeBar.Dominio.WaiterModule;
using ControleDeBar.WebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace ControleDeBar.WebApp.Models;

public class WaiterFormViewModel
{
    [Required(ErrorMessage = "The field 'Name' is required.")]
    [MinLength(3, ErrorMessage = "The field 'Name' must contain at least 3 characters.")]
    [MaxLength(100, ErrorMessage = "The field 'Name' must contain at most 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The field 'CPF' is required.")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$",
        ErrorMessage = "The field 'CPF' must follow the format: 000.000.000-00.")]
    public string Cpf { get; set; }
}

public class CreateWaiterViewModel : WaiterFormViewModel
{
    public CreateWaiterViewModel() { }

    public CreateWaiterViewModel(string name, string cpf) : this()
    {
        Name = name;
        Cpf = cpf;
    }
}

public class EditWaiterViewModel : WaiterFormViewModel
{
    public Guid Id { get; set; }

    public EditWaiterViewModel() { }

    public EditWaiterViewModel(Guid id, string name, string cpf) : this()
    {
        Id = id;
        Name = name;
        Cpf = cpf;
    }
}

public class DeleteWaiterViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public DeleteWaiterViewModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class WaiterListViewModel
{
    public List<WaiterDetailsViewModel> Records { get; set; }

    public WaiterListViewModel(List<Waiter> waiters)
    {
        Records = new List<WaiterDetailsViewModel>();

        foreach (var w in waiters)
            Records.Add(w.ToDetailsVM());
    }
}

public class WaiterDetailsViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Cpf { get; set; }

    public WaiterDetailsViewModel(Guid id, string name, string cpf)
    {
        Id = id;
        Name = name;
        Cpf = cpf;
    }
}

public class SelectWaiterViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public SelectWaiterViewModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
