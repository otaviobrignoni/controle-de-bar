using ControleDeBar.Dominio.Compartilhado;

namespace ControleDeBar.Dominio.ProductModule;

public class Product : EntidadeBase<Product>
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product() { }

    public Product(string name, decimal price) : this()
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }

    public override void AtualizarRegistro(Product editedRecord)
    {
        Name = editedRecord.Name;
        Price = editedRecord.Price;
    }
}
