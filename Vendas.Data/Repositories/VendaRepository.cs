using Microsoft.EntityFrameworkCore;
using Vendas.Data.Interfaces;
using Vendas.DTO;

namespace Vendas.Data.Repositories
{
    public class VendaRepository(VendasContext context) : IVendaRepository
    {
        public async Task<IEnumerable<Venda>> ObterPaginadoAsync(int pagina, int tamanhoPagina)
            => await context.Vendas
                .Include(v => v.Itens)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

        public async Task<Venda> ObterPorIdAsync(Guid id)
            => await context.Vendas
                .Include(v => v.Itens)
                .FirstOrDefaultAsync(v => v.Id == id);

        public async Task<IEnumerable<Venda>> ObterTodasAsync()
            => await context.Vendas
                .Include(v => v.Itens)
                .ToListAsync();

        public async Task AdicionarAsync(Venda venda)
        {
            await context.Vendas.AddAsync(venda);
            await context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Venda venda)
        {
            context.Vendas.Update(venda);
            await context.SaveChangesAsync();
        }
    }
}
