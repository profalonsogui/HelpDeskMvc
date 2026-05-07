namespace HelpDeskMvc.Models
{
    public class Chamado
    {
        public int Id {get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? Status { get; set; } = "Aberto";
        public DateTime DataAbertura { get; set; } = DateTime.Now;
        public DateTime? DataFechamento { get; set; }
    }
}