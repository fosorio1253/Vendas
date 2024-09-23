using Vendas.DTO;

namespace Vendas.Domain.Interfaces
{
    public interface IVendaService
    {
        Task<Venda> CriarVendaAsync(Venda venda);
        Task<Venda> AtualizarVendaAsync(Guid id, Venda venda);
        Task<bool> CancelarVendaAsync(Guid id);
        Task<Venda> ObterVendaAsync(Guid id);
        Task<IEnumerable<Venda>> ObterTodasVendasAsync();
    }
}
