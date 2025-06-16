using ControleDeBar.Dominio.TableModule;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;

namespace ControleDeBar.Infraestrura.Arquivos.TableModule;

public class TableFileRepository : RepositorioBaseEmArquivo<Table>, ITableRepository
{
    public TableFileRepository(ContextoDados contexto) : base(contexto) { }

    protected override List<Table> ObterRegistros()
    {
        return contexto.Tables;
    }
}
