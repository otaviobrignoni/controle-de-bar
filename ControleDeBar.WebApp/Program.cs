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
using ControleDeBar.WebApp.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace ControleDeBar.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<ValidarModeloAttribute>();
        });

        builder.Services.AddScoped(_ => new ContextoDados(true));
        builder.Services.AddScoped<IRepositorioGarcom, RepositorioGarcomEmArquivo>();
        builder.Services.AddScoped<IRepositorioMesa, RepositorioMesaEmArquivo>();
        builder.Services.AddScoped<IRepositorioProduto, RepositorioProdutoEmArquivo>();
        builder.Services.AddScoped<IRepositorioConta, RepositorioContaEmArquivo>();

        builder.Services.AddSerilogConfig(builder.Logging);

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
            app.UseExceptionHandler("/erro");
        else
            app.UseDeveloperExceptionPage();

        app.UseAntiforgery();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapDefaultControllerRoute();

        app.Run();
    }
}
