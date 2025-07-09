using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrura.Arquivos.ModuloMesa;
using ControleDeBar.Infraestrutura.Arquivos.ModuloConta;
using ControleDeBar.Infraestrutura.Arquivos.ModuloGarcom;
using ControleDeBar.Infraestrutura.Arquivos.ModuloProduto;
using ControleDeBar.WebApp.ActionFilters;
using ControleDeBar.WebApp.Extensions;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("contas")]
public class ContaController : Controller
{
    private readonly ContextoDados ContextoDados;
    private readonly IRepositorioConta RepositorioConta;
    private readonly IRepositorioMesa RepositorioMesa;
    private readonly IRepositorioGarcom RepositorioGarcom;
    private readonly IRepositorioProduto RepositorioProduto;

    public ContaController(ContextoDados contextoDados, IRepositorioConta repositorioConta, IRepositorioMesa repositorioMesa, IRepositorioGarcom repositorioGarcom, IRepositorioProduto repositorioProduto)
    {
        ContextoDados = contextoDados;
        RepositorioConta = repositorioConta;
        RepositorioMesa = repositorioMesa;
        RepositorioGarcom = repositorioGarcom;
        RepositorioProduto = repositorioProduto;
    }

    [HttpGet]
    public IActionResult Index(string status)
    {
        List<Conta> registros;

        switch (status)
        {
            case "abertas": registros = RepositorioConta.SelecionarContasAbertas(); break;
            case "fechadas": registros = RepositorioConta.SelecionarContasFechadas(); break;
            default: registros = RepositorioConta.SelecionarContas(); break;
        }

        var visualizarVM = new VisualizarContasViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("abrir")]
    public IActionResult Abrir()
    {
        var mesas = RepositorioMesa.SelecionarRegistros();
        var garcons = RepositorioGarcom.SelecionarRegistros();

        var abrirVM = new AbrirContaViewModel(mesas, garcons);

        return View(abrirVM);
    }

    [HttpPost("abrir")]
    [ValidateAntiForgeryToken]
    public IActionResult Abrir(AbrirContaViewModel abrirVM)
    {
        var registros = RepositorioConta.SelecionarContas();

        foreach (var conta in registros)
        {
            if (conta.Titular.Equals(abrirVM.Titular) && conta.EstaAberta)
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma conta aberta para este titular.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(abrirVM);

        var mesas = RepositorioMesa.SelecionarRegistros();
        var garcons = RepositorioGarcom.SelecionarRegistros();

        var entidade = abrirVM.ParaEntidade(mesas, garcons);

        RepositorioConta.CadastrarConta(entidade);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet, Route("/contas/{id:guid}/fechar")]
    public IActionResult Fechar(Guid id)
    {
        var registro = RepositorioConta.SelecionarPorId(id);

        var fecharContaVM = new FecharContaViewModel(
            registro.Id,
            registro.Titular,
            registro.Mesa.Numero,
            registro.Garcom.Nome,
            registro.CalcularValorTotal()
        );

        return View(fecharContaVM);
    }

    [HttpPost, Route("/contas/{id:guid}/fechar")]
    public IActionResult FecharConfirmado(Guid id)
    {
        var registroSelecionado = RepositorioConta.SelecionarPorId(id);

        registroSelecionado.Fechar();

        ContextoDados.Salvar();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet, Route("/contas/{id:guid}/gerenciar-pedidos")]
    public IActionResult GerenciarPedidos(Guid id)
    {
        var contaSelecionada = RepositorioConta.SelecionarPorId(id);
        var produtos = RepositorioProduto.SelecionarRegistros();

        var gerenciarPedidosVm = new GerenciarPedidosViewModel(contaSelecionada, produtos);

        return View(gerenciarPedidosVm);
    }

    [HttpPost, Route("/contas/{id:guid}/adicionar-pedido")]
    public IActionResult AdicionarPedido(Guid id, AdicionarPedidoViewModel adicionarPedidoVm)
    {
        var contaSelecionada = RepositorioConta.SelecionarPorId(id);
        var produtoSelecionado = RepositorioProduto.SelecionarRegistroPorId(adicionarPedidoVm.IdProduto);

        contaSelecionada.RegistrarPedido(
            produtoSelecionado,
            adicionarPedidoVm.QuantidadeSolicitada
        );

        ContextoDados.Salvar();

        var produtos = RepositorioProduto.SelecionarRegistros();

        var gerenciarPedidosVm = new GerenciarPedidosViewModel(contaSelecionada, produtos);

        return View("GerenciarPedidos", gerenciarPedidosVm);
    }

    [HttpPost, Route("/contas/{id:guid}/remover-pedido/{idPedido:guid}")]
    public IActionResult RemoverPedido(Guid id, Guid idPedido)
    {
        var contaSelecionada = RepositorioConta.SelecionarPorId(id);

        var pedidoRemovido = contaSelecionada.RemoverPedido(idPedido);

        ContextoDados.Salvar();

        var produtos = RepositorioProduto.SelecionarRegistros();

        var gerenciarPedidosVm = new GerenciarPedidosViewModel(contaSelecionada, produtos);

        return View("GerenciarPedidos", gerenciarPedidosVm);
    }

    [HttpGet("faturamento")]
    public IActionResult Faturamento(DateTime? data)
    {
        if (!data.HasValue)
            return View();

        var registros = RepositorioConta.SelecionarContasPorPeriodo(data.GetValueOrDefault());

        var faturamentoVM = new FaturamentoViewModel(registros);

        return View(faturamentoVM);
    }
}
