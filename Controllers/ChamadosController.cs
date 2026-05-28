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
        private static int contadorId = 2;

        public IActionResult Index()
        {
            return View(chamados);
        }

        public IActionResult Detalhes(int id)
        {
            // Recupera o chamado pelo ID da lista em memória.
            var chamadoRecuperado = chamados.FirstOrDefault(c => c.Id == id);

            if (chamadoRecuperado == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Detalhes do Chamado";

            return View(chamadoRecuperado);
        }

        public IActionResult Create()
        {
            return View(new Chamado());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Chamado chamado)
        {
            if (!ModelState.IsValid)
            {
                return View(chamado);
            }

            chamado.Id = contadorId++;
            chamado.Status = "Aberto";
            chamado.DataAbertura = DateTime.Now;
            chamado.DataFechamento = null;

            chamados.Add(chamado);

            return RedirectToAction(nameof(Index));
        }
    }
}
