using ControleDeBar.Dominio.TableModule;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrura.Arquivos.TableModule;
using ControleDeBar.WebApp.Extensions;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("tables")]
public class TableController : Controller
{
    private readonly ContextoDados contextoDados;
    private readonly ITableRepository tableRepository;

    public TableController()
    {
        contextoDados = new ContextoDados(true);
        tableRepository = new TableFileRepository(contextoDados);
    }

    [HttpGet]
    public IActionResult Index()
    {
        var registros = tableRepository.SelecionarRegistros();

        var visualizarVM = new TableListViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        var vm = new CreateTableViewModel();

        return View(vm);
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public ActionResult Create(CreateTableViewModel vm)
    {
        var registros = tableRepository.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (item.Number.Equals(vm.Number))
            {
                ModelState.AddModelError("Unique", "A table with this number already exists.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(vm);

        var entidade = vm.ToEntity();

        tableRepository.CadastrarRegistro(entidade);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:guid}")]
    public ActionResult Edit(Guid id)
    {
        var registroSelecionado = tableRepository.SelecionarRegistroPorId(id);

        var editarVM = new EditTableViewModel(
            id,
            registroSelecionado.Number,
            registroSelecionado.Capacity
        );

        return View(editarVM);
    }

    [HttpPost("edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Guid id, EditTableViewModel vm)
    {
        var registros = tableRepository.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (!item.Id.Equals(id) && item.Number.Equals(vm.Number))
            {
                ModelState.AddModelError("Unique", "A table with this number already exists.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(vm);

        var entidadeEditada = vm.ToEntity();

        tableRepository.EditarRegistro(id, entidadeEditada);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("delete/{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        var registroSelecionado = tableRepository.SelecionarRegistroPorId(id);

        var excluirVM = new DeleteTableViewModel(registroSelecionado.Id, registroSelecionado.Number);

        return View(excluirVM);
    }

    [HttpPost("delete/{id:guid}")]
    public ActionResult DeleteConfirmed(Guid id)
    {
        tableRepository.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id:guid}")]
    public ActionResult Details(Guid id)
    {
        var registroSelecionado = tableRepository.SelecionarRegistroPorId(id);

        var detalhesVM = new TableDetailsViewModel(
            id,
            registroSelecionado.Number,
            registroSelecionado.Capacity
        );

        return View(detalhesVM);
    }
}
