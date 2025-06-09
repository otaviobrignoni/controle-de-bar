using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrura.Arquivos.ModuloMesa;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("mesas")]
public class MesaController : Controller
{
    private readonly ContextoDados contextoDados;
    private readonly IRepositorioMesa repositorioMesa;

    public MesaController()
    {
        contextoDados = new ContextoDados(true);
        repositorioMesa = new RepositorioMesaEmArquivo(contextoDados);
    }

    [HttpGet]
    public IActionResult Index()
    {
        var registros = repositorioMesa.SelecionarRegistros();

        var visualizarVM = new VisualizarMesasViewModel(registros);

        return View(visualizarVM);
    }
}
