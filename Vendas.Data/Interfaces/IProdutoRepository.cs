using Vendas.DTO;

namespace Vendas.Data.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto> ObterPorIdAsync(Guid id);
    }
}
