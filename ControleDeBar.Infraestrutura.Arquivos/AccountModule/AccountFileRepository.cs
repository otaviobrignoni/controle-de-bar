using ControleDeBar.Dominio.AccountModule;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;

namespace ControleDeBar.Infraestrutura.Arquivos.AccountModule;

public class AccountFileRepository : IAccountRepository
{
    private readonly ContextoDados contexto;
    protected readonly List<Account> registros;

    public AccountFileRepository(ContextoDados contexto)
    {
        this.contexto = contexto;
        registros = contexto.Accounts;
    }

    public void RegisterAccount(Account novaConta)
    {
        registros.Add(novaConta);

        contexto.Salvar();
    }

    public Account SelectById(Guid id)
    {
        foreach (var item in registros)
        {
            if (item.Id == id)
                return item;
        }

        return null;
    }

    public List<Account> SelectAccounts()
    {
        return registros;
    }

    public List<Account> SelectOpenAccounts()
    {
        var contasAbertas = new List<Account>();

        foreach (var item in registros)
        {
            if (item.IsOpen)
                contasAbertas.Add(item);
        }

        return contasAbertas;
    }

    public List<Account> SelectClosedAccounts()
    {
        var contasFechadas = new List<Account>();

        foreach (var item in registros)
        {
            if (!item.IsOpen)
                contasFechadas.Add(item);
        }

        return contasFechadas;
    }

    public List<Account> SelectAccountsByDate(DateTime data)
    {
        var contasDoPeriodo = new List<Account>();

        foreach (var item in registros)
        {
            if (item.Closed.Date == data.Date)
                contasDoPeriodo.Add(item);
        }

        return contasDoPeriodo;
    }
}
