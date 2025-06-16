using ControleDeBar.Dominio.ProductModule;

namespace ControleDeBar.Dominio.AccountModule;

public class Order
{
    public Guid Id { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }

    public Order() { }

    public Order(Product product, int selectedQuantity) : this()
    {
        Id = Guid.NewGuid();
        Product = product;
        Quantity = selectedQuantity;
    }

    public decimal CalculatePartialTotal()
    {
        return Product.Price * Quantity;
    }

    public override string ToString()
    {
        return $"{Quantity}x {Product}";
    }
}
