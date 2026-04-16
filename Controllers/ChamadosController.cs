using HelpDeskMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskMvc.Controllers
{
    public class ChamadosController : Controller
    {
        // Ação principal (lista de chamados)
        public IActionResult Index()
        {
             // Lista de exemplo (simulando dados)
            var chamados = new List<Chamado>
            {
                new Chamado
                {
                    Id = 1,
                    Titulo = "Computador não liga",
                    Descricao = "O computador da sala 2 não está ligando.",
                    Status = "Aberto"
                },
                new Chamado
                {
                    Id = 2,
                    Titulo = "Internet lenta",
                    Descricao = "A conexão está muito lenta no laboratório.",
                    Status = "Em andamento"
                }
            };
            
            return View(chamados);
        }
    }
}