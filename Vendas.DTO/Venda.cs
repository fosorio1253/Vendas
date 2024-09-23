using System.ComponentModel.DataAnnotations;

namespace Vendas.DTO
{
    public class Venda
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O número da venda é obrigatório.")]
        [StringLength(20, ErrorMessage = "O número da venda não pode exceder 20 caracteres.")]
        public string NumeroVenda { get; set; }

        [Required(ErrorMessage = "A data da venda é obrigatória.")]
        public DateTime DataVenda { get; set; }

        [Required(ErrorMessage = "O cliente é obrigatório.")]
        public Cliente Cliente { get; set; }

        [Required(ErrorMessage = "O valor total é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor total deve ser maior que zero.")]
        public decimal ValorTotal { get; set; }

        [Required(ErrorMessage = "A filial é obrigatória.")]
        [StringLength(50, ErrorMessage = "O nome da filial não pode exceder 50 caracteres.")]
        public string Filial { get; set; }

        [Required(ErrorMessage = "A venda deve conter ao menos um item.")]
        [MinLength(1, ErrorMessage = "A venda deve conter ao menos um item.")]
        public List<ItemVenda> Itens { get; set; }

        public bool Cancelado { get; set; }
    }
}
