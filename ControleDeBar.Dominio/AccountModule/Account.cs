using ControleDeBar.Dominio.Compartilhado;
using ControleDeBar.Dominio.WaiterModule;
using ControleDeBar.Dominio.TableModule;
using ControleDeBar.Dominio.ProductModule;

namespace ControleDeBar.Dominio.AccountModule;

public class Account : EntidadeBase<Account>
{
    public string Owner { get; set; }
    public Table Table { get; set; }
    public Waiter Waiter { get; set; }
    public DateTime Opened { get; set; }
    public DateTime Closed { get; set; }
    public bool IsOpen { get; set; }
    public List<Order> Orders { get; set; }

    public Account()
    {
        Orders = new List<Order>();
    }

    public Account(string owner, Table table, Waiter waiter) : this()
    {
        Id = Guid.NewGuid();
        Owner = owner;
        Table = table;
        Waiter = waiter;

        Open();
    }

    public void Open()
    {
        IsOpen = true;
        Opened = DateTime.Now;

        Table.Occupy();
    }

    public void Close()
    {
        IsOpen = false;
        Closed = DateTime.Now;

        Table.Vacate();
    }

    public Order RegisterOrder(Product product, int selectedQuantity)
    {
        Order newOrder = new Order(product, selectedQuantity);

        Orders.Add(newOrder);

        return newOrder;
    }

    public Order RemoveOrder(Order order)
    {
        Orders.Remove(order);

        return order;
    }

    public Order RemoveOrder(Guid orderId)
    {
        Order selectedOrder = null;

        foreach (var p in Orders)
        {
            if (p.Id == orderId)
                selectedOrder = p;
        }

        if (selectedOrder == null)
            return null;

        Orders.Remove(selectedOrder);

        return selectedOrder;
    }

    public decimal CalculateTotalValue()
    {
        decimal valorTotal = 0;

        foreach (var p in Orders)
            valorTotal += p.CalculatePartialTotal();

        return valorTotal;
    }

    public override void AtualizarRegistro(Account updatedRecord)
    {
        IsOpen = updatedRecord.IsOpen;
        Closed = updatedRecord.Closed;
    }
}
