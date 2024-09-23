namespace Vendas.DTO
{
    public class Produto
    {
        public Guid ProdutoId { get; set; }  // External Identity
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
