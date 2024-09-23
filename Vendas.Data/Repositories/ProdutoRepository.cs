using Microsoft.EntityFrameworkCore;
using Vendas.Data.Interfaces;
using Vendas.DTO;

namespace Vendas.Data.Repositories
{
    public class ProdutoRepository(VendasContext context) : IProdutoRepository
    {
        public async Task<Produto> ObterPorIdAsync(Guid id)
            => await context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
    }
}
