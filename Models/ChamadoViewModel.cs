namespace HelpDeskMvc.Models
{
    public class Chamado
    {
        // get -> permite ler o valor
        // set -> permite atribuir o valor
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Descricao { get; set; }
        public string Status { get; set; } = "Aberto";
        public DateTime DataAbertura { get; set; } = DateTime.Now;
        public DateTime? DataFechamento { get; set; }
    }
}