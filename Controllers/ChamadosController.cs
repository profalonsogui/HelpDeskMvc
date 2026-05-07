using HelpDeskMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskMvc.Controllers
{
    public class ChamadosController : Controller
    {
        // Ação principal (lista de chamados)
        public IActionResult Index()
        {
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
                },
                new Chamado
                {
                    Id = 3,
                    Titulo = "Monitor quebrado",
                    Descricao = "Monitor da sala 3 está quebrado",
                    Status = "Fechado"
                }
            };

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