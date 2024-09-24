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
    }
}
