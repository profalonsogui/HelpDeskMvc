using System.ComponentModel.DataAnnotations;

namespace HelpDeskMvc.Models
{
    public class Chamado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [Display(Name = "Título")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public string? Status { get; set; } = "Aberto";
        public DateTime DataAbertura { get; set; } = DateTime.Now;
        public DateTime? DataFechamento { get; set; }
    }
}
