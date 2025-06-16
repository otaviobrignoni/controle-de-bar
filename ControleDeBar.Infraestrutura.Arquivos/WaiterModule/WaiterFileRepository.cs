using ControleDeBar.Dominio.WaiterModule;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;

namespace ControleDeBar.Infraestrutura.Arquivos.WaiterModule;

public class WaiterFileRepository : RepositorioBaseEmArquivo<Waiter>, IWaiterRepository
{
    public WaiterFileRepository(ContextoDados contexto) : base(contexto) { }

    protected override List<Waiter> ObterRegistros()
    {
        return contexto.Waiters;
    }
}
