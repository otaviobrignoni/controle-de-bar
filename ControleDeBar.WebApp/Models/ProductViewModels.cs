using ControleDeBar.Dominio.ProductModule;
using ControleDeBar.WebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace ControleDeBar.WebApp.Models;

public class ProductFormViewModel
{
    [Required(ErrorMessage = "The field 'Name' is required.")]
    [MinLength(3, ErrorMessage = "The field 'Name' must contain at least 3 characters.")]
    [MaxLength(100, ErrorMessage = "The field 'Name' must contain at most 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The field 'Price' is required.")]
    [DataType(DataType.Currency)]
    [Range(0, double.MaxValue,
        ErrorMessage = "The field 'Price' must contain a positive value.")]
    public decimal Price { get; set; }
}

public class CreateProductViewModel : ProductFormViewModel
{
    public CreateProductViewModel() { }

    public CreateProductViewModel(string name, decimal price) : this()
    {
        Name = name;
        Price = price;
    }
}

public class EditProductViewModel : ProductFormViewModel
{
    public Guid Id { get; set; }

    public EditProductViewModel() { }

    public EditProductViewModel(Guid id, string name, decimal price) : this()
    {
        Id = id;
        Name = name;
        Price = price;
    }
}

public class DeleteProductViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public DeleteProductViewModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class ProductListViewModel
{
    public List<ProductDetailsViewModel> Records { get; set; }

    public ProductListViewModel(List<Product> products)
    {
        Records = new List<ProductDetailsViewModel>();

        foreach (var p in products)
            Records.Add(p.ToDetailsVM());
    }
}

public class ProductDetailsViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public ProductDetailsViewModel(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}

public class SelectProductViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public SelectProductViewModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
