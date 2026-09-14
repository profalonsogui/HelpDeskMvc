using HelpDeskMvc.Data;
using HelpDeskMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskMvc.Controllers
{
    public class ChamadosController : Controller
    {
        // O campo é readonly: recebemos o contexto uma vez e não trocamos depois.
        // Note que NÃO existe mais lista estática nem contadorId.
        private readonly AppDbContext _context;

        // Injeção de dependência.
        // Nós nunca damos "new AppDbContext()". Quem constrói é o ASP.NET Core,
        // usando o que foi registrado no Program.cs com AddDbContext.
        // O controller apenas declara o que precisa e recebe pronto.
        public ChamadosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Chamados
        public async Task<IActionResult> Index()
        {
            // ToListAsync() é o momento em que a consulta vai ao banco.
            // O OrderByDescending acontece no SQL, não em memória.
            var chamados = await _context.Chamados
                .OrderByDescending(c => c.DataAbertura)
                .ToListAsync();

            return View(chamados);
        }

        // GET: /Chamados/Detalhes/5
        public async Task<IActionResult> Detalhes(int id)
        {
            // Antes: chamados.FirstOrDefault(...)  -> procurava na lista em memória
            // Agora: _context.Chamados.FirstOrDefaultAsync(...) -> vira um SELECT com WHERE
            //
            // A sintaxe LINQ é praticamente a mesma. A diferença é para onde ela vai.
            var chamadoRecuperado = await _context.Chamados
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chamadoRecuperado == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Detalhes do Chamado";

            return View(chamadoRecuperado);
        }

        // GET: /Chamados/Create
        // Continua sincrono: só devolve o formulário vazio, não toca no banco.
        public IActionResult Create()
        {
            return View(new Chamado());
        }

        // POST: /Chamados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Chamado chamado)
        {
            if (!ModelState.IsValid)
            {
                return View(chamado);
            }

            // Zeramos o Id por segurança.
            // O model binding preenche o objeto com TUDO que veio no POST — inclusive
            // campos que o formulário não deveria mandar. Isso se chama overposting.
            // Com Id = 0, o EF Core entende "chave não definida" e deixa o banco gerar.
            chamado.Id = 0;

            // Campos controlados pelo servidor, não pelo usuário.
            chamado.Status = ChamadoStatus.Aberto;
            chamado.DataAbertura = DateTime.Now;
            chamado.DataFechamento = null;

            // Add() ainda não grava nada. Ele apenas marca a entidade como "a ser inserida"
            // no change tracker do EF Core.
            _context.Chamados.Add(chamado);

            // SaveChangesAsync() é quem realmente executa o INSERT.
            // Depois desta linha, chamado.Id já contém o valor gerado pelo banco.
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: /Chamados/IniciarAtendimento/5
        // Transição permitida: Aberto -> EmAndamento.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IniciarAtendimento(int id)
        {
            var chamado = await _context.Chamados.FirstOrDefaultAsync(c => c.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            if (chamado.Status != ChamadoStatus.Aberto)
            {
                TempData["MensagemTipo"] = "danger";
                TempData["Mensagem"] = "Só é possível iniciar o atendimento de um chamado Aberto.";
                return RedirectToAction(nameof(Detalhes), new { id });
            }

            chamado.Status = ChamadoStatus.EmAndamento;

            await _context.SaveChangesAsync();

            TempData["MensagemTipo"] = "success";
            TempData["Mensagem"] = "Atendimento iniciado com sucesso.";
            return RedirectToAction(nameof(Detalhes), new { id });
        }

        // POST: /Chamados/Resolver/5
        // Transição permitida: EmAndamento -> Resolvido.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resolver(int id)
        {
            var chamado = await _context.Chamados.FirstOrDefaultAsync(c => c.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            if (chamado.Status != ChamadoStatus.EmAndamento)
            {
                TempData["MensagemTipo"] = "danger";
                TempData["Mensagem"] = "Só é possível resolver um chamado que esteja Em Andamento.";
                return RedirectToAction(nameof(Detalhes), new { id });
            }

            chamado.Status = ChamadoStatus.Resolvido;
            chamado.DataFechamento = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["MensagemTipo"] = "success";
            TempData["Mensagem"] = "Chamado resolvido com sucesso.";
            return RedirectToAction(nameof(Detalhes), new { id });
        }

        // POST: /Chamados/Reabrir/5
        // Transição permitida: Resolvido -> EmAndamento.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reabrir(int id)
        {
            var chamado = await _context.Chamados.FirstOrDefaultAsync(c => c.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            if (chamado.Status != ChamadoStatus.Resolvido)
            {
                TempData["MensagemTipo"] = "danger";
                TempData["Mensagem"] = "Só é possível reabrir um chamado que esteja Resolvido.";
                return RedirectToAction(nameof(Detalhes), new { id });
            }

            chamado.Status = ChamadoStatus.EmAndamento;
            chamado.DataFechamento = null;

            await _context.SaveChangesAsync();

            TempData["MensagemTipo"] = "success";
            TempData["Mensagem"] = "Chamado reaberto com sucesso.";
            return RedirectToAction(nameof(Detalhes), new { id });
        }

        // POST: /Chamados/Cancelar/5
        // Transições permitidas: Aberto -> Cancelado ou EmAndamento -> Cancelado.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id)
        {
            var chamado = await _context.Chamados.FirstOrDefaultAsync(c => c.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            if (chamado.Status != ChamadoStatus.Aberto && chamado.Status != ChamadoStatus.EmAndamento)
            {
                TempData["MensagemTipo"] = "danger";
                TempData["Mensagem"] = "Este chamado não pode ser cancelado no status atual.";
                return RedirectToAction(nameof(Detalhes), new { id });
            }

            // O cancelamento não remove o registro do banco, apenas altera o status.
            chamado.Status = ChamadoStatus.Cancelado;

            await _context.SaveChangesAsync();

            TempData["MensagemTipo"] = "success";
            TempData["Mensagem"] = "Chamado cancelado.";
            return RedirectToAction(nameof(Detalhes), new { id });
        }
    }
}
