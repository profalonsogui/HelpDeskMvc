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
    }
}