using Microsoft.AspNetCore.Mvc;
using Vendas.Domain.Interfaces;
using Vendas.DTO;

namespace Vendas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendaController : ControllerBase
    {
        private readonly IVendaService _vendaService;

        public VendaController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarVenda([FromBody] Venda venda)
        {
            if (venda == null)
                return BadRequest("Venda não pode ser nula.");

            var result = await _vendaService.CriarVendaAsync(venda);
            return CreatedAtAction(nameof(ObterVenda), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterVenda(Guid id)
        {
            var venda = await _vendaService.ObterVendaAsync(id);
            return venda != null ?
                Ok(venda) :
                NotFound($"Venda com id {id} não encontrada.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarVenda(Guid id, [FromBody] Venda venda)
        {
            if (venda == null)
                return BadRequest("Venda não pode ser nula.");

            var result = await _vendaService.AtualizarVendaAsync(id, venda);
            return result != null ?
                Ok(result) :
                NotFound($"Venda com id {id} não encontrada.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarVenda(Guid id)
        {
            var result = await _vendaService.CancelarVendaAsync(id);
            return result ?
                NoContent() :
                NotFound($"Venda com id {id} não encontrada.");
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodasVendas()
        {
            var vendas = await _vendaService.ObterTodasVendasAsync();
            return Ok(vendas);
        }
    }
}
