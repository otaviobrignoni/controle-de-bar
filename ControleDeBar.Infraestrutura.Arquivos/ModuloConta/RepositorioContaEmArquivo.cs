using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;

namespace ControleDeBar.Infraestrutura.Arquivos.ModuloConta;

public class RepositorioContaEmArquivo : IRepositorioConta
{
    private readonly ContextoDados contexto;
    protected readonly List<Conta> registros;

    public RepositorioContaEmArquivo(ContextoDados contexto)
    {
        this.contexto = contexto;
        registros = contexto.Contas;
    }

    public void CadastrarConta(Conta novaConta)
    {
        registros.Add(novaConta);

        contexto.Salvar();
    }

    public Conta? SelecionarPorId(Guid idRegistro)
    {
        return registros.Find(e => e.Id == idRegistro);
    }


    public List<Conta> SelecionarContas()
    {
        return registros;
    }

    public List<Conta> SelecionarContasAbertas()
    {
        return registros.Where(e => e.EstaAberta).ToList();
    }

    public List<Conta> SelecionarContasFechadas()
    {
        return registros.Where(e => !e.EstaAberta).ToList();
    }

    public List<Conta> SelecionarContasPorPeriodo(DateTime data)
    {
        return registros.Where(e => e.Fechamento.Date == data.Date).ToList();
    }
}