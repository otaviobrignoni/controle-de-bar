using ControleDeBar.Dominio.ProductModule;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.ProductModule;
using ControleDeBar.WebApp.Extensions;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("products")]
public class ProductController : Controller
{
    private readonly ContextoDados contextoDados;
    private readonly IProductRepository productRepository;

    public ProductController()
    {
        contextoDados = new ContextoDados(true);
        productRepository = new ProductFileRepository(contextoDados);
    }

    [HttpGet]
    public IActionResult Index()
    {
        var registros = productRepository.SelecionarRegistros();

        var visualizarVM = new ProductListViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        var vm = new CreateProductViewModel();

        return View(vm);
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateProductViewModel vm)
    {
        var registros = productRepository.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (item.Name.Equals(vm.Name))
            {
                ModelState.AddModelError("Unique", "A product with this name already exists.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(vm);

        var entidade = vm.ToEntity();

        productRepository.CadastrarRegistro(entidade);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:guid}")]
    public IActionResult Edit(Guid id)
    {
        var registroSelecionado = productRepository.SelecionarRegistroPorId(id);

        var editarVM = new EditProductViewModel(
            id,
            registroSelecionado.Name,
            registroSelecionado.Price
        );

        return View(editarVM);
    }

    [HttpPost("edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, EditProductViewModel vm)
    {
        var registros = productRepository.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (!item.Id.Equals(id) && item.Name.Equals(vm.Name))
            {
                ModelState.AddModelError("Unique", "A product with this name already exists.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(vm);

        var entidadeEditada = vm.ToEntity();

        productRepository.EditarRegistro(id, entidadeEditada);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("delete/{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var registroSelecionado = productRepository.SelecionarRegistroPorId(id);

        var excluirVM = new DeleteProductViewModel(registroSelecionado.Id, registroSelecionado.Name);

        return View(excluirVM);
    }

    [HttpPost("delete/{id:guid}")]
    public IActionResult DeleteConfirmed(Guid id)
    {
        productRepository.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }
}
