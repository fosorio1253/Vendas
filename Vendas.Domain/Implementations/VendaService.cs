using Microsoft.Extensions.Logging;
using Vendas.Data.Interfaces;
using Vendas.Domain.Interfaces;
using Vendas.DTO;

namespace Vendas.Domain.Implementations
{
    public class VendaService : IVendaService
    {
        private readonly IVendaRepository _vendaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly ILogger<VendaService> _logger;

        public VendaService(IVendaRepository vendaRepository,
                            IClienteRepository clienteRepository,
                            IProdutoRepository produtoRepository,
                            ILogger<VendaService> logger)
        {
            _vendaRepository = vendaRepository;
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
            _logger = logger;
        }

        public async Task<Venda> CriarVendaAsync(Venda venda)
        {
            _logger.LogInformation("Criando nova venda...");

            await ValidarVendaAsync(venda);
            venda.ValorTotal = venda.Itens.Sum(item => item.ValorTotal);
            await _vendaRepository.AdicionarAsync(venda);

            LogEvento($"CompraCriada: Venda {venda.Id}");

            return venda;
        }

        public async Task<Venda> AtualizarVendaAsync(Guid id, Venda venda)
        {
            _logger.LogInformation($"Atualizando venda: {id}");

            var vendaExistente = await _vendaRepository.ObterPorIdAsync(id);
            if (vendaExistente == null)
                throw new KeyNotFoundException("Venda não encontrada.");

            await ValidarVendaAsync(venda);
            vendaExistente.DataVenda = venda.DataVenda;
            vendaExistente.Cliente = venda.Cliente;
            vendaExistente.Itens = venda.Itens;
            vendaExistente.ValorTotal = venda.Itens.Sum(item => item.ValorTotal);
            await _vendaRepository.AtualizarAsync(vendaExistente);

            LogEvento($"CompraAlterada: Venda {vendaExistente.Id}");

            return vendaExistente;
        }

        public async Task<bool> CancelarVendaAsync(Guid id)
        {
            _logger.LogInformation($"Cancelando venda: {id}");

            var venda = await _vendaRepository.ObterPorIdAsync(id);
            if (venda == null)
                throw new KeyNotFoundException("Venda não encontrada.");

            venda.Cancelado = true;
            await _vendaRepository.AtualizarAsync(venda);

            LogEvento($"CompraCancelada: Venda {id}");

            return true;
        }

        public async Task<Venda> ObterVendaAsync(Guid id)
        {
            _logger.LogInformation($"Obtendo venda por ID: {id}");

            var venda = await _vendaRepository.ObterPorIdAsync(id);
            if (venda == null)
                throw new KeyNotFoundException("Venda não encontrada.");

            return venda;
        }

        public async Task<IEnumerable<Venda>> ObterTodasVendasAsync()
        {
            _logger.LogInformation("Obtendo todas as vendas...");
            return await _vendaRepository.ObterTodasAsync();
        }

        private async Task ValidarVendaAsync(Venda venda)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(venda.Cliente.ClienteId);
            if (cliente == null)
                throw new KeyNotFoundException("Cliente não encontrado.");
            foreach (var item in venda.Itens)
            {
                var produto = await _produtoRepository.ObterPorIdAsync(item.Produto.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"Produto não encontrado: {item.Produto.ProdutoId}");
            }
        }

        private void LogEvento(string mensagem)
            => _logger.LogInformation(mensagem);
    }
}
