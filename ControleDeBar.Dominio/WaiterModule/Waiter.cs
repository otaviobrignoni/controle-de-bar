using ControleDeBar.Dominio.Compartilhado;

namespace ControleDeBar.Dominio.WaiterModule;

public class Waiter : EntidadeBase<Waiter>
{
    public string Name { get; set; }
    public string Cpf { get; set; }

    public Waiter() { }

    public Waiter(string name, string cpf) : this()
    {
        Id = Guid.NewGuid();
        Name = name;
        Cpf = cpf;
    }

    public override void AtualizarRegistro(Waiter editedRecord)
    {
        Name = editedRecord.Name;
        Cpf = editedRecord.Cpf;
    }
}
