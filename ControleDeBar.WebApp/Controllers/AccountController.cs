using ControleDeBar.Dominio.AccountModule;
using ControleDeBar.Dominio.WaiterModule;
using ControleDeBar.Dominio.TableModule;
using ControleDeBar.Dominio.ProductModule;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrura.Arquivos.TableModule;
using ControleDeBar.Infraestrutura.Arquivos.AccountModule;
using ControleDeBar.Infraestrutura.Arquivos.WaiterModule;
using ControleDeBar.Infraestrutura.Arquivos.ProductModule;
using ControleDeBar.WebApp.Extensions;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("accounts")]
public class AccountController : Controller
{
    private readonly ContextoDados contextoDados;
    private readonly IAccountRepository accountRepository;
    private readonly ITableRepository tableRepository;
    private readonly IWaiterRepository waiterRepository;
    private readonly IProductRepository productRepository;

    public AccountController()
    {
        contextoDados = new ContextoDados(true);
        accountRepository = new AccountFileRepository(contextoDados);
        tableRepository = new TableFileRepository(contextoDados);
        waiterRepository = new WaiterFileRepository(contextoDados);
        productRepository = new ProductFileRepository(contextoDados);
    }

    [HttpGet]
    public IActionResult Index(string status)
    {
        List<Account> registros;

        switch (status)
        {
            case "open": registros = accountRepository.SelectOpenAccounts(); break;
            case "closed": registros = accountRepository.SelectClosedAccounts(); break;
            default: registros = accountRepository.SelectAccounts(); break;
        }

        var visualizarVM = new AccountListViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("open")]
    public IActionResult Open()
    {
        var tables = tableRepository.SelecionarRegistros();
        var waiters = waiterRepository.SelecionarRegistros();

        var openVM = new OpenAccountViewModel(tables, waiters);

        return View(openVM);
    }

    [HttpPost("open")]
    [ValidateAntiForgeryToken]
    public IActionResult Open(OpenAccountViewModel openVM)
    {
        var registros = accountRepository.SelectAccounts();

        foreach (var account in registros)
        {
            if (account.Owner.Equals(openVM.Owner) && account.IsOpen)
            {
                ModelState.AddModelError("Unique", "There is already an open account for this owner.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(openVM);

        var tables = tableRepository.SelecionarRegistros();
        var waiters = waiterRepository.SelecionarRegistros();

        var entidade = openVM.ToEntity(tables, waiters);

        accountRepository.RegisterAccount(entidade);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet, Route("/accounts/{id:guid}/close")]
    public IActionResult Close(Guid id)
    {
        var registro = accountRepository.SelectById(id);

        var closeAccountVM = new CloseAccountViewModel(
            registro.Id,
            registro.Owner,
            registro.Table.Number,
            registro.Waiter.Name,
            registro.CalculateTotalValue()
        );

        return View(closeAccountVM);
    }

    [HttpPost, Route("/accounts/{id:guid}/close")]
    public IActionResult CloseConfirmed(Guid id)
    {
        var registroSelecionado = accountRepository.SelectById(id);

        registroSelecionado.Close();

        contextoDados.Salvar();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet, Route("/accounts/{id:guid}/manage-orders")]
    public IActionResult ManageOrders(Guid id)
    {
        var accountSelected = accountRepository.SelectById(id);
        var products = productRepository.SelecionarRegistros();

        var manageOrdersVm = new ManageOrdersViewModel(accountSelected, products);

        return View(manageOrdersVm);
    }

    [HttpPost, Route("/accounts/{id:guid}/add-order")]
    public IActionResult AddOrder(Guid id, AddOrderViewModel addOrderVm)
    {
        var accountSelected = accountRepository.SelectById(id);
        var selectedProduct = productRepository.SelecionarRegistroPorId(addOrderVm.ProductId);

        accountSelected.RegisterOrder(
            selectedProduct,
            addOrderVm.Quantity
        );

        contextoDados.Salvar();

        var products = productRepository.SelecionarRegistros();

        var manageOrdersVm = new ManageOrdersViewModel(accountSelected, products);

        return View("ManageOrders", manageOrdersVm);
    }

    [HttpPost, Route("/accounts/{id:guid}/remove-order/{orderId:guid}")]
    public IActionResult RemoveOrder(Guid id, Guid orderId)
    {
        var accountSelected = accountRepository.SelectById(id);

        var removed = accountSelected.RemoveOrder(orderId);

        contextoDados.Salvar();

        var products = productRepository.SelecionarRegistros();

        var manageOrdersVm = new ManageOrdersViewModel(accountSelected, products);

        return View("ManageOrders", manageOrdersVm);
    }

    [HttpGet("billing")]
    public IActionResult Billing(DateTime? data)
    {
        if (!data.HasValue)
            return View();

        var registros = accountRepository.SelectAccountsByDate(data.GetValueOrDefault());

        var faturamentoVM = new BillingViewModel(registros);

        return View(faturamentoVM);
    }
}
