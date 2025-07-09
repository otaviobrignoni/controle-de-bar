﻿using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrura.Arquivos.ModuloMesa;
using ControleDeBar.WebApp.Extensions;
using ControleDeBar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBar.WebApp.Controllers;

[Route("mesas")]
public class MesaController : Controller
{
    private readonly ContextoDados ContextoDados;
    private readonly IRepositorioMesa RepositorioMesa;

    public MesaController(ContextoDados contextoDados, IRepositorioMesa repositorioMesa)
    {
        ContextoDados = contextoDados;
        RepositorioMesa = repositorioMesa;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var registros = RepositorioMesa.SelecionarRegistros();

        var visualizarVM = new VisualizarMesasViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        var cadastrarVM = new CadastrarMesaViewModel();

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public ActionResult Cadastrar(CadastrarMesaViewModel cadastrarVM)
    {
        var registros = RepositorioMesa.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (item.Numero.Equals(cadastrarVM.Numero))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma mesa registrada com este número.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(cadastrarVM);

        var entidade = cadastrarVM.ParaEntidade();

        RepositorioMesa.CadastrarRegistro(entidade);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public ActionResult Editar(Guid id)
    {
        var registroSelecionado = RepositorioMesa.SelecionarRegistroPorId(id);

        var editarVM = new EditarMesaViewModel(
            id,
            registroSelecionado.Numero,
            registroSelecionado.Capacidade
        );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(Guid id, EditarMesaViewModel editarVM)
    {
        var registros = RepositorioMesa.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (!item.Id.Equals(id) && item.Numero.Equals(editarVM.Numero))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma mesa registrada com este número.");
                break;
            }
        }

        if (!ModelState.IsValid)
            return View(editarVM);

        var entidadeEditada = editarVM.ParaEntidade();

        RepositorioMesa.EditarRegistro(id, entidadeEditada);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public ActionResult Excluir(Guid id)
    {
        var registroSelecionado = RepositorioMesa.SelecionarRegistroPorId(id);

        var excluirVM = new ExcluirMesaViewModel(registroSelecionado.Id, registroSelecionado.Numero);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    public ActionResult ExcluirConfirmado(Guid id)
    {
        RepositorioMesa.ExcluirRegistro(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public ActionResult Detalhes(Guid id)
    {
        var registroSelecionado = RepositorioMesa.SelecionarRegistroPorId(id);

        var detalhesVM = new DetalhesMesaViewModel(
            id,
            registroSelecionado.Numero,
            registroSelecionado.Capacidade
        );

        return View(detalhesVM);
    }
}
