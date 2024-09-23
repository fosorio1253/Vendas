using System.ComponentModel.DataAnnotations;

namespace Vendas.DTO
{
    public class ItemVenda
    {
        [Required(ErrorMessage = "O produto é obrigatório.")]
        public Produto Produto { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "O valor unitário é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor unitário deve ser maior que zero.")]
        public decimal ValorUnitario { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O desconto não pode ser negativo.")]
        public decimal Desconto { get; set; }

        public decimal ValorTotal => Quantidade * ValorUnitario - Desconto;
    }
}
