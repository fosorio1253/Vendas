using Vendas.DTO;

namespace Vendas.Data.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> ObterPorIdAsync(Guid id);
    }
}
