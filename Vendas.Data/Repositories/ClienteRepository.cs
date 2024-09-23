using Microsoft.EntityFrameworkCore;
using Vendas.Data.Interfaces;
using Vendas.DTO;

namespace Vendas.Data.Repositories
{
    public class ClienteRepository(VendasContext context) : IClienteRepository
    {
        public async Task<Cliente> ObterPorIdAsync(Guid id)
            => await context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == id);
    }
}
