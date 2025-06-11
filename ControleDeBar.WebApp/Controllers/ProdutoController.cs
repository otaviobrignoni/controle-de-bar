using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.ModuloProduto;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("produtos")]
public class ProdutoController : Controller
{
    private readonly ContextoDados contextoDados;
    private readonly IRepositorioProduto repositorioProduto;

    public ProdutoController()
    {
        contextoDados = new ContextoDados(true);
        repositorioProduto = new RepositorioProdutoEmArquivo(contextoDados);
    }

    public IActionResult Index()
    {
        var registros = repositorioProduto.SelecionarRegistros();

        var visualizarVM = new VisualizarProdutosViewModel(registros);

        return View(visualizarVM);
    }
}
