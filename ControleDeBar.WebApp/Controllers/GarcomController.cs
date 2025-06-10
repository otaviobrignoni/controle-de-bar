using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.ModuloGarcom;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("garcons")]
public class GarcomController : Controller
{
    private readonly ContextoDados contextoDados;
    private readonly IRepositorioGarcom repositorioGarcom;

    public GarcomController()
    {
        contextoDados = new ContextoDados(true);
        repositorioGarcom = new RepositorioGarcomEmArquivo(contextoDados);
    }

    public IActionResult Index()
    {
        var registros = repositorioGarcom.SelecionarRegistros();

        var visualizarVM = new VisualizarGarconsViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("excluir/{id:guid}")]
    public ActionResult Excluir(Guid id)
    {
        var registroSelecionado = repositorioGarcom.SelecionarRegistroPorId(id);

        var excluirVM = new ExcluirGarcomViewModel(registroSelecionado.Id, registroSelecionado.Nome);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    public ActionResult ExcluirConfirmado(Guid id)
    {
        repositorioGarcom.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public ActionResult Detalhes(Guid id)
    {
        var registroSelecionado = repositorioGarcom.SelecionarRegistroPorId(id);

        var detalhesVM = new DetalhesGarcomViewModel(
            id,
            registroSelecionado.Nome,
            registroSelecionado.Cpf
        );

        return View(detalhesVM);
    }
}
