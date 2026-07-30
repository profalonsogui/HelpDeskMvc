using System.Diagnostics;
using HelpDeskMvc.Data;
using HelpDeskMvc.Models;
using HelpDeskMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        // Mesmo padrão do ChamadosController: o contexto entra pelo construtor.
        // O logger que já existia continua aqui — só ganhamos um segundo parâmetro.
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                // CountAsync() traduz para SELECT COUNT(*). Note que NÃO trazemos
                // os chamados para a memória para depois contar — a contagem
                // acontece dentro do banco e volta só um número.
                Total = await _context.Chamados.CountAsync(),

                // O predicado dentro do CountAsync vira o WHERE do SQL.
                Abertos = await _context.Chamados
                    .CountAsync(c => c.Status == "Aberto"),

                EmAndamento = await _context.Chamados
                    .CountAsync(c => c.Status == "Em andamento"),

                Resolvidos = await _context.Chamados
                    .CountAsync(c => c.Status == "Resolvido"),

                // Take(3) vira LIMIT 3 no SQLite. Só três linhas saem do banco,
                // não a tabela inteira.
                UltimosChamados = await _context.Chamados
                    .OrderByDescending(c => c.DataAbertura)
                    .Take(3)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}