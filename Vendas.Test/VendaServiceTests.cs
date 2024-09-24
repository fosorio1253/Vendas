using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Vendas.Data.Interfaces;
using Vendas.Domain.Implementations;
using Vendas.DTO;
using Xunit;

namespace Vendas.Test
{
    public class VendaServiceTests
    {
        private readonly Mock<IVendaRepository> _vendaRepositoryMock;
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly Mock<ILogger<VendaService>> _loggerMock;
        private readonly VendaService _vendaService;

        public VendaServiceTests()
        {
            _vendaRepositoryMock = new Mock<IVendaRepository>();
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _loggerMock = new Mock<ILogger<VendaService>>();

            _vendaService = new VendaService(
                _vendaRepositoryMock.Object,
                _clienteRepositoryMock.Object,
                _produtoRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CriarVendaAsync_DeveCriarVendaComSucesso()
        {
            // Arrange
            var cliente = new Cliente { ClienteId = Guid.NewGuid(), Nome = "Cliente Teste" };
            var produto = new Produto { ProdutoId = Guid.NewGuid(), Nome = "Produto Teste", Descricao = "Produto Teste" };
            var venda = new Venda
            {
                Id = Guid.NewGuid(),
                Cliente = cliente,
                Itens = new List<ItemVenda>
            {
                new ItemVenda { Produto = produto, Quantidade = 2, ValorUnitario = 100 }
            }
            };

            _clienteRepositoryMock.Setup(c => c.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(cliente);
            _produtoRepositoryMock.Setup(p => p.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(produto);

            // Act
            var result = await _vendaService.CriarVendaAsync(venda);

            // Assert
            result.Should().NotBeNull();
            result.ValorTotal.Should().Be(200);
            _vendaRepositoryMock.Verify(v => v.AdicionarAsync(It.IsAny<Venda>()), Times.Once);
            _loggerMock.Verify(log => log.LogInformation(It.Is<string>(msg => msg.Contains("Criando nova venda"))), Times.Once);
        }

        [Fact]
        public async Task AtualizarVendaAsync_DeveAtualizarVendaComSucesso()
        {
            // Arrange
            var vendaExistente = new Venda { Id = Guid.NewGuid(), Cliente = new Cliente(), Itens = new List<ItemVenda>(), ValorTotal = 100 };
            var vendaAtualizada = new Venda { Id = vendaExistente.Id, Cliente = new Cliente(), Itens = new List<ItemVenda>(), ValorTotal = 150 };

            _vendaRepositoryMock.Setup(v => v.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(vendaExistente);

            // Act
            var result = await _vendaService.AtualizarVendaAsync(vendaExistente.Id, vendaAtualizada);

            // Assert
            result.Should().NotBeNull();
            result.ValorTotal.Should().Be(150);
            _vendaRepositoryMock.Verify(v => v.AtualizarAsync(It.IsAny<Venda>()), Times.Once);
            _loggerMock.Verify(log => log.LogInformation(It.Is<string>(msg => msg.Contains("Atualizando venda"))), Times.Once);
        }

        [Fact]
        public async Task CancelarVendaAsync_DeveCancelarVendaComSucesso()
        {
            // Arrange
            var venda = new Venda { Id = Guid.NewGuid(), Cliente = new Cliente(), Itens = new List<ItemVenda>(), Cancelado = false };

            _vendaRepositoryMock.Setup(v => v.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(venda);

            // Act
            var result = await _vendaService.CancelarVendaAsync(venda.Id);

            // Assert
            result.Should().BeTrue();
            venda.Cancelado.Should().BeTrue();
            _vendaRepositoryMock.Verify(v => v.AtualizarAsync(It.IsAny<Venda>()), Times.Once);
            _loggerMock.Verify(log => log.LogInformation(It.Is<string>(msg => msg.Contains("Cancelando venda"))), Times.Once);
        }

        [Fact]
        public async Task ObterVendaAsync_DeveRetornarVendaQuandoExistente()
        {
            // Arrange
            var vendaExistente = new Venda { Id = Guid.NewGuid(), Cliente = new Cliente(), Itens = new List<ItemVenda>(), ValorTotal = 100 };
            _vendaRepositoryMock.Setup(v => v.ObterPorIdAsync(vendaExistente.Id)).ReturnsAsync(vendaExistente);

            // Act
            var result = await _vendaService.ObterVendaAsync(vendaExistente.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(vendaExistente.Id);
        }

        [Fact]
        public async Task ObterTodasVendasAsync_DeveRetornarListaDeVendas()
        {
            // Arrange
            var vendas = new List<Venda>
            {
                new Venda { Id = Guid.NewGuid(), Cliente = new Cliente(), Itens = new List<ItemVenda>(), ValorTotal = 100 },
                new Venda { Id = Guid.NewGuid(), Cliente = new Cliente(), Itens = new List<ItemVenda>(), ValorTotal = 200 }
            };
            _vendaRepositoryMock.Setup(v => v.ObterTodasAsync()).ReturnsAsync(vendas);

            // Act
            var result = await _vendaService.ObterTodasVendasAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(v => v.ValorTotal == 100);
            result.Should().Contain(v => v.ValorTotal == 200);
        }

        [Fact]
        public async Task ObterTodasVendasAsync_DeveRetornarListaVaziaQuandoNaoExistemVendas()
        {
            // Arrange
            _vendaRepositoryMock.Setup(v => v.ObterTodasAsync()).ReturnsAsync(new List<Venda>());

            // Act
            var result = await _vendaService.ObterTodasVendasAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task CriarVendaAsync_DeveLancarExcecao_SeClienteNaoEncontrado()
        {
            // Arrange
            var venda = new Venda
            {
                Cliente = new Cliente { ClienteId = Guid.NewGuid(), Nome = "Cliente Teste" },
                Itens = new List<ItemVenda> { new ItemVenda { Produto = new Produto(), Quantidade = 1, ValorUnitario = 100 } }
            };

            _clienteRepositoryMock.Setup(c => c.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Cliente)null);

            // Act
            Func<Task> act = async () => await _vendaService.CriarVendaAsync(venda);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Cliente não encontrado");
            _vendaRepositoryMock.Verify(v => v.AdicionarAsync(It.IsAny<Venda>()), Times.Never);
        }

        [Fact]
        public async Task AtualizarVendaAsync_DeveLancarExcecao_SeVendaNaoEncontrada()
        {
            // Arrange
            var vendaAtualizada = new Venda { Id = Guid.NewGuid(), Cliente = new Cliente(), Itens = new List<ItemVenda>(), ValorTotal = 150 };

            _vendaRepositoryMock.Setup(v => v.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Venda)null);

            // Act
            Func<Task> act = async () => await _vendaService.AtualizarVendaAsync(vendaAtualizada.Id, vendaAtualizada);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Venda não encontrada");
            _vendaRepositoryMock.Verify(v => v.AtualizarAsync(It.IsAny<Venda>()), Times.Never);
        }

        [Fact]
        public async Task CancelarVendaAsync_DeveRetornarFalso_SeVendaJaCancelada()
        {
            // Arrange
            var venda = new Venda { Id = Guid.NewGuid(), Cliente = new Cliente(), Itens = new List<ItemVenda>(), Cancelado = true };

            _vendaRepositoryMock.Setup(v => v.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(venda);

            // Act
            var result = await _vendaService.CancelarVendaAsync(venda.Id);

            // Assert
            result.Should().BeFalse();
            _vendaRepositoryMock.Verify(v => v.AtualizarAsync(It.IsAny<Venda>()), Times.Never);
        }

        [Fact]
        public async Task ObterVendaAsync_DeveRetornarNullQuandoNaoExistente()
        {
            // Arrange
            var vendaId = Guid.NewGuid();
            _vendaRepositoryMock.Setup(v => v.ObterPorIdAsync(vendaId)).ReturnsAsync((Venda)null);

            // Act
            var result = await _vendaService.ObterVendaAsync(vendaId);

            // Assert
            result.Should().BeNull();
        }

    }
}
