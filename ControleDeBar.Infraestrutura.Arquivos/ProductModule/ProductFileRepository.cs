using ControleDeBar.Dominio.ProductModule;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;

namespace ControleDeBar.Infraestrutura.Arquivos.ProductModule;

public class ProductFileRepository : RepositorioBaseEmArquivo<Product>, IProductRepository
{
    public ProductFileRepository(ContextoDados contexto) : base(contexto)
    {
    }

    protected override List<Product> ObterRegistros()
    {
        return contexto.Products;
    }
}
