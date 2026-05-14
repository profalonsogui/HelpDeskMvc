using Microsoft.AspNetCore.Mvc;
using HelpDeskMvc.Models;

namespace HelpDeskMvc.Controllers
{
    public class ChamadosController : Controller
    {
        // Lista em memória (simula o banco de dados)
        private static List<Chamado> chamados = new List<Chamado>
        {
            new Chamado
            {
                Id = 1,
                Titulo = "Sistema fora do ar",
                Descricao = "o sistema apresenta o error 500.",
                Status = "Aberto",
                DataAbertura = DateTime.Now,
                DataFechamento = null
            },
        };

        // Controle do ID automático
        //private static int contadorId = 2;

        // listagem de chamados
        public IActionResult Index()
        {
            return View(chamados);
        }

        public IActionResult Detalhes(int id)
        {
            var chamados = new List<Chamado>
            {
                new Chamado
                {
                    Id = 1,
                    Titulo = "Computador não liga",
                    Descricao = "PC da sala 2 não inicia",
                    Status = "Aberto"
                },
                new Chamado
                {
                    Id = 2,
                    Titulo = "Internet lenta",
                    Descricao = "Laboratório com lentidão",
                    Status = "Em andamento"
                },
                new Chamado
                {
                    Id = 3,
                    Titulo = "Monitor quebrado",
                    Descricao = "Monitor da sala 3 está quebrado",
                    Status = "Fechado"
                },
            };

            var chamadoRecuperado = chamados.FirstOrDefault(c => c.Id == id);

            if (chamadoRecuperado == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Detalhes do Chamado";
            ViewData["Message"] = "Detalhes do Chamado";

            return View(chamadoRecuperado);
        }
    }
}