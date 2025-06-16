using System.Text.Json.Serialization;
using System.Text.Json;
using ControleDeBar.Dominio.TableModule;
using ControleDeBar.Dominio.WaiterModule;
using ControleDeBar.Dominio.ProductModule;
using ControleDeBar.Dominio.AccountModule;

namespace ControleDeBar.Infraestrura.Arquivos.Compartilhado;

public class ContextoDados
{
    private string pastaArmazenamento = "C:\\temp";
    private string arquivoArmazenamento = "dados-controle-bar.json";

    public List<Table> Tables { get; set; }
    public List<Waiter> Waiters { get; set; }
    public List<Product> Products { get; set; }
    public List<Account> Accounts { get; set; }

    public ContextoDados()
    {
        Tables = new List<Table>();
        Waiters = new List<Waiter>();
        Products = new List<Product>();
        Accounts = new List<Account>();
    }

    public ContextoDados(bool carregarDados) : this()
    {
        if (carregarDados)
            Carregar();
    }

    public void Salvar()
    {
        string caminhoCompleto = Path.Combine(pastaArmazenamento, arquivoArmazenamento);

        JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
        jsonOptions.WriteIndented = true;
        jsonOptions.ReferenceHandler = ReferenceHandler.Preserve;

        string json = JsonSerializer.Serialize(this, jsonOptions);

        if (!Directory.Exists(pastaArmazenamento))
            Directory.CreateDirectory(pastaArmazenamento);

        File.WriteAllText(caminhoCompleto, json);
    }

    public void Carregar()
    {
        string caminhoCompleto = Path.Combine(pastaArmazenamento, arquivoArmazenamento);

        if (!File.Exists(caminhoCompleto)) return;

        string json = File.ReadAllText(caminhoCompleto);

        if (string.IsNullOrWhiteSpace(json)) return;

        JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
        jsonOptions.ReferenceHandler = ReferenceHandler.Preserve;

        ContextoDados contextoArmazenado = JsonSerializer.Deserialize<ContextoDados>(
            json, 
            jsonOptions
        )!;

        if (contextoArmazenado == null) return;

        Tables = contextoArmazenado.Tables;
        Waiters = contextoArmazenado.Waiters;
        Products = contextoArmazenado.Products;
        Accounts = contextoArmazenado.Accounts;
    }
}
