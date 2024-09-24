using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;
using Vendas.DTO;
using Xunit;

namespace Vendas.Test
{
    public class VendaControllerTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task Post_CriarVenda_DeveRetornarCreated()
        {
            // Arrange
            var venda = new Venda
            {
                Cliente = new Cliente { ClienteId = Guid.NewGuid(), Nome = "Cliente Teste" },
                Itens = new List<ItemVenda>
                {
                    new ItemVenda
                    {
                        Produto = new Produto { ProdutoId = Guid.NewGuid(), Nome = "Produto Teste", Descricao = "Produto Teste" },
                        Quantidade = 2,
                        ValorUnitario = 50
                    }
                },
                DataVenda = DateTime.Now
            };

            var content = new StringContent(JsonConvert.SerializeObject(venda), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/venda", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Get_ObterVenda_DeveRetornarVenda()
        {
            // Arrange
            var vendaId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/api/venda/{vendaId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Put_AtualizarVenda_DeveRetornarOk()
        {
            // Arrange
            var vendaId = Guid.NewGuid();
            var vendaAtualizada = new Venda
            {
                Cliente = new Cliente { ClienteId = Guid.NewGuid(), Nome = "Cliente Atualizado" },
                Itens = new List<ItemVenda>
        {
            new ItemVenda
            {
                Produto = new Produto { ProdutoId = Guid.NewGuid(), Nome = "Produto Atualizado", Descricao = "Produto Atualizado" },
                Quantidade = 1,
                ValorUnitario = 100
            }
        },
                DataVenda = DateTime.Now
            };

            var content = new StringContent(JsonConvert.SerializeObject(vendaAtualizada), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/venda/{vendaId}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_CancelarVenda_DeveRetornarNoContent()
        {
            // Arrange
            var vendaId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"/api/venda/{vendaId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Get_ObterTodasVendas_DeveRetornarOk()
        {
            // Act
            var response = await _client.GetAsync("/api/venda");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Post_CriarVenda_DeveRetornarBadRequest_SeVendaForNula()
        {
            // Arrange
            Venda vendaNula = null;
            var content = new StringContent(JsonConvert.SerializeObject(vendaNula), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/venda", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Put_AtualizarVenda_DeveRetornarBadRequest_SeVendaForNula()
        {
            // Arrange
            Venda vendaNula = null;
            var vendaId = Guid.NewGuid();
            var content = new StringContent(JsonConvert.SerializeObject(vendaNula), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/venda/{vendaId}", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Get_ObterVenda_DeveRetornarNotFound_SeVendaNaoExistir()
        {
            // Arrange
            var vendaId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/api/venda/{vendaId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_CancelarVenda_DeveRetornarNotFound_SeVendaNaoExistir()
        {
            // Arrange
            var vendaId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"/api/venda/{vendaId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Put_AtualizarVenda_DeveRetornarNotFound_SeVendaNaoExistir()
        {
            // Arrange
            var vendaId = Guid.NewGuid();
            var vendaAtualizada = new Venda
            {
                Cliente = new Cliente { ClienteId = Guid.NewGuid(), Nome = "Cliente Atualizado" },
                Itens = new List<ItemVenda>
                {
                    new ItemVenda
                    {
                        Produto = new Produto { ProdutoId = Guid.NewGuid(), Nome = "Produto Atualizado", Descricao = "Produto Atualizado" },
                        Quantidade = 1,
                        ValorUnitario = 100
                    }
                },
                DataVenda = DateTime.Now
            };

            var content = new StringContent(JsonConvert.SerializeObject(vendaAtualizada), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/venda/{vendaId}", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
