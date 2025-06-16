using ControleDeBar.Dominio.WaiterModule;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.WaiterModule;
using ControleDeBar.WebApp.Extensions;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("waiters")]
public class WaiterController : Controller
{
    private readonly ContextoDados contextoDados;
    private readonly IWaiterRepository waiterRepository;

    public WaiterController()
    {
        contextoDados = new ContextoDados(true);
        waiterRepository = new WaiterFileRepository(contextoDados);
    }

    public IActionResult Index()
    {
        var registros = waiterRepository.SelecionarRegistros();

        var listarVM = new WaiterListViewModel(registros);

        return View(listarVM);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        var vm = new CreateWaiterViewModel();

        return View(vm);
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateWaiterViewModel vm)
    {
        var registros = waiterRepository.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (item.Name.Equals(vm.Name))
            {
                ModelState.AddModelError("Unique", "A waiter with this name already exists.");
                break;
            }

            if (item.Cpf.Equals(vm.Cpf))
            {
                ModelState.AddModelError("Unique", "A waiter with this CPF already exists.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(vm);

        var entidade = vm.ToEntity();

        waiterRepository.CadastrarRegistro(entidade);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:guid}")]
    public ActionResult Edit(Guid id)
    {
        var registroSelecionado = waiterRepository.SelecionarRegistroPorId(id);

        var editarVM = new EditWaiterViewModel(
            id,
            registroSelecionado.Name,
            registroSelecionado.Cpf
        );

        return View(editarVM);
    }

    [HttpPost("edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Guid id, EditWaiterViewModel vm)
    {
        var registros = waiterRepository.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (!item.Id.Equals(id) && item.Name.Equals(vm.Name))
            {
                ModelState.AddModelError("Unique", "A waiter with this name already exists.");
                break;
            }

            if (!item.Id.Equals(id) && item.Cpf.Equals(vm.Cpf))
            {
                ModelState.AddModelError("Unique", "A waiter with this CPF already exists.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(vm);

        var entidadeEditada = vm.ToEntity();

        waiterRepository.EditarRegistro(id, entidadeEditada);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("delete/{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var registroSelecionado = waiterRepository.SelecionarRegistroPorId(id);

        var excluirVM = new DeleteWaiterViewModel(registroSelecionado.Id, registroSelecionado.Name);

        return View(excluirVM);
    }

    [HttpPost("delete/{id:guid}")]
    public IActionResult DeleteConfirmed(Guid id)
    {
        waiterRepository.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id:guid}")]
    public IActionResult Details(Guid id)
    {
        var registroSelecionado = waiterRepository.SelecionarRegistroPorId(id);

        var detalhesVM = new WaiterDetailsViewModel(
            id,
            registroSelecionado.Name,
            registroSelecionado.Cpf
        );

        return View(detalhesVM);
    }
}
