using Vendas.DTO;

namespace Vendas.Data.Interfaces
{
    public interface IVendaRepository
    {
        Task<Venda> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Venda>> ObterTodasAsync();
        Task AdicionarAsync(Venda venda);
        Task AtualizarAsync(Venda venda);
        Task<IEnumerable<Venda>> ObterPaginadoAsync(int pagina, int tamanhoPagina);
    }
}
