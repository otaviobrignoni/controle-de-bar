using ControleDeBar.Dominio.ProductModule;
using ControleDeBar.WebApp.Models;

namespace ControleDeBar.WebApp.Extensions;

public static class ProductExtensions
{
    public static Product ToEntity(this ProductFormViewModel form)
    {
        return new Product(form.Name, form.Price);
    }

    public static ProductDetailsViewModel ToDetailsVM(this Product product)
    {
        return new ProductDetailsViewModel(
                product.Id,
                product.Name,
                product.Price
        );
    }
}
