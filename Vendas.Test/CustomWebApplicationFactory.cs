using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Vendas.Test
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            // Adiciona qualquer configuração ou serviço extra necessário para o teste aqui.
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Pode-se adicionar configurações adicionais para os testes aqui, se necessário
        }
    }
}
