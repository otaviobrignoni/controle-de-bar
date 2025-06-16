using ControleDeBar.Dominio.Compartilhado;

namespace ControleDeBar.Dominio.TableModule;

public class Table : EntidadeBase<Table>
{
    public int Number { get; set; }
    public int Capacity { get; set; }
    public bool IsOccupied { get; set; }

    public Table() { }

    public Table(int number, int seatCount) : this()
    {
        Id = Guid.NewGuid();
        Number = number;
        Capacity = seatCount;
        IsOccupied = false;
    }

    public void Occupy()
    {
        IsOccupied = true;
    }

    public void Vacate()
    {
        IsOccupied = false;
    }

    public override void AtualizarRegistro(Table editedRecord)
    {
        Number = editedRecord.Number;
        Capacity = editedRecord.Capacity;
    }
}
